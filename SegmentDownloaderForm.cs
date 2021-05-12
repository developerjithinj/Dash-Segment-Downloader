using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dash_Downloader
{
    public partial class SegmentDownloaderForm : Form
    {
        private DashManifest manifest;
        private static volatile bool cancelDownloads;
        private static int successCount;
        private static int totalCount;
        private static int failureCount;

        public SegmentDownloaderForm(DashManifest dashManifest, string folderPath)
        {
            manifest = dashManifest;
            InitializeComponent();
            cancelDownloads = false;
            successCount = 0;
            failureCount = 0;
            string folderUri = createAndGetLocalFolderUri(folderPath, manifest.path);
            initiateDownload(dashManifest, folderUri);
        }

        private async void initiateDownload(DashManifest dashManifest, string folderPath)
        {
            ArrayList segmentInfoList = await generateUrls(dashManifest, folderPath);
            totalCount = segmentInfoList.Count - 1; // -1 for ignoring manifest info
            await downloadFiles(segmentInfoList, ProgressCallback);
            if (successCount == totalCount)
            {
                DialogUtil.showNoRetryError("All segements downloaded successfully");
            }
            else
            {
                DialogUtil.showNoRetryError("Download operation " + (cancelDownloads ? "cancelled" : "completed") + ". "
                    + successCount + "/" + totalCount + " downloads successful");
            }

            this.Close();
        }


        public static async Task downloadFiles(ArrayList segmentInfoList, Action<string> progressTick)
        {

            await Task.Run(() =>
            {
                Debug.WriteLine("segmentInfoList: " + segmentInfoList.Count);
                var client = new WebClient();
                try
                {
                    SegmentInfo manifestInfo = (SegmentInfo)segmentInfoList[0];
                    client.DownloadFile(new Uri(manifestInfo.remoteUri), manifestInfo.localUri);
                }
                catch (Exception e)
                {
                    //ignore
                }
                segmentInfoList.RemoveAt(0);
                foreach (SegmentInfo segmentInfo in segmentInfoList)
                {
                    if (cancelDownloads)
                    {
                        break;
                    }
                    try
                    {
                        client.DownloadFile(new Uri(segmentInfo.remoteUri), segmentInfo.localUri);
                        successCount++;
                    }
                    catch (Exception e)
                    {
                        failureCount++;
                    }
                    progressTick(segmentInfo.remoteUri);
                }
            });
        }
        delegate void ProgressCallbackDelegate(string url);
        private void ProgressCallback(string url)
        {
            int progress = successCount + failureCount;

            if (this.progressBarDownload.InvokeRequired)
            {
                ProgressCallbackDelegate d = new ProgressCallbackDelegate(ProgressCallback);
                this.Invoke(d, new object[] { url });
            }
            else
            {

                progressBarDownload.Value = progress;
                progressBarDownload.Maximum = totalCount;
                labelProgress.Text = "Downloading segments in progress (" + progress + "/" + totalCount + ")";
                labelUrl.Text = url;
            }
        }

        public static async Task<ArrayList> generateUrls(DashManifest dashManifest, string folderPath)
        {
            ArrayList urlList = new ArrayList();
            await Task.Run(() =>
            {
                //Add manifest as the first item
                urlList.Add(new SegmentInfo(dashManifest.remoteUri, folderPath + "manifest.mpd"));

                //Loop through each segment and create URLs
                foreach (DashManifest.Track track in dashManifest.tracks)
                {
                    if (track.selected)
                    {
                        string fileName = track.initSegmentTemplate.Replace("$RepresentationID$", track.id);
                        string initSegUrl = dashManifest.remoteUrlBase + dashManifest.path + fileName;
                        string localUrl = folderPath + fileName;

                        urlList.Add(new SegmentInfo(initSegUrl, localUrl));

                        for (int i = track.segmentStartIndex; i <= track.segmentCount; i++)
                        {
                            fileName = track.mediaSegmentTemplate.Replace("$RepresentationID$", track.id).Replace("$Number$", (i + ""));
                            localUrl = folderPath + fileName;
                            string segUrl = dashManifest.remoteUrlBase + dashManifest.path + fileName;
                            urlList.Add(new SegmentInfo(segUrl, localUrl));
                        }
                    }
                }
            });
            return urlList;
        }

        private string createAndGetLocalFolderUri(string folderPath, string manifestPath)
        {
            //Remove first backward slash
            if (manifestPath.ToCharArray()[0] != '/')
            {
                manifestPath = "/" + manifestPath;
            }
            //convert to windows format
            manifestPath = manifestPath.Replace("/", "\\");

            //Add missing slash at the end
            if (manifestPath.ToCharArray()[manifestPath.Length - 1] != '\\')
            {
                manifestPath += "\\";

            }
            string savePath = folderPath + manifestPath;
            //Create missing directory structure
            Directory.CreateDirectory(savePath);

            return savePath;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancelDownloads = true;
        }
    }
}
