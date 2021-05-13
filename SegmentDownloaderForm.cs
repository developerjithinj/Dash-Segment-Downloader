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
        private bool completed;
        public SegmentDownloaderForm(DashManifest dashManifest, string folderPath, int threadCount)
        {
            manifest = dashManifest;
            InitializeComponent();
            cancelDownloads = false;
            completed = false;
            successCount = 0;
            failureCount = 0;
            dashManifest.manifestLocalUri = folderPath + "\\manifest.mpd";
            string folderUri = createAndGetLocalFolderUri(folderPath, manifest.path);
            initiateDownload(dashManifest, folderUri, threadCount);
        }

        private async void initiateDownload(DashManifest dashManifest, string folderPath, int threadCount)
        {
            ArrayList segmentInfoList = await generateUrls(dashManifest, folderPath);
            totalCount = segmentInfoList.Count - 1; // -1 for ignoring manifest info

            //Split download list for multiple threads
            int segmentListSize = segmentInfoList.Count;
            int downloadsPerThread = segmentListSize / threadCount;

            ArrayList segmentInfoLListofLists = new ArrayList(threadCount);

            for (int i = 0; i < threadCount; i++)
            {
                segmentInfoLListofLists.Add(new ArrayList());
            }

            int threadIndex = 0;
            for (int i = 0; i < segmentListSize; i++)
            {
                if (threadIndex >= threadCount)
                {
                    threadIndex = 0;
                }
               ((ArrayList)segmentInfoLListofLists[threadIndex++]).Add(segmentInfoList[i]);

            }
            for (int i = 0; i < threadCount; i++)
            {
                downloadFiles(i == 0, (ArrayList)segmentInfoLListofLists[i], ProgressCallback);
            }

            //Alternative simple method for multi-threading. Issue with this aproach is that the bigger segments may endup at the last list on a single thread
            /* for (int i = 0; i < threadCount; i++)
             {
                 int index = i * downloadsPerThread;
                 int count = downloadsPerThread;

                 //When it's the last set, add the remaining items as weel 
                 if (i == threadCount - 1)
                 {
                     count = segmentListSize - (index - downloadsPerThread + count);
                 }
                 downloadFiles(i == 0, new ArrayList(segmentInfoList.GetRange(index, count)), ProgressCallback);
            }*/
        }


        public async Task downloadFiles(bool includesManifest, ArrayList segmentInfoList, Action<string, bool> progressTick)
        {
            await Task.Run(() =>
            {
                var client = new WebClient();
                if (includesManifest)
                {
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
                }
                foreach (SegmentInfo segmentInfo in segmentInfoList)
                {
                    if (cancelDownloads)
                    {
                        progressTick(segmentInfo.remoteUri, cancelDownloads);
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
                    progressTick(segmentInfo.remoteUri, cancelDownloads);
                }
            });
        }
        delegate void ProgressCallbackDelegate(string url, bool hasCancelled);
        private void ProgressCallback(string url, bool hasCancelled)
        {
            int progress = successCount + failureCount;

            if (this.progressBarDownload.InvokeRequired)
            {
                try
                {
                    ProgressCallbackDelegate d = new ProgressCallbackDelegate(ProgressCallback);
                    this.Invoke(d, new object[] { url, hasCancelled });
                }
                catch (Exception e) { }
            }
            else
            {
                if (!cancelDownloads)
                {
                    progressBarDownload.Value = progress;
                    progressBarDownload.Maximum = totalCount;
                    labelProgress.Text = "Downloading segments in progress (" + progress + "/" + totalCount + ")";
                    labelUrl.Text = hasCancelled ? "Cancelling downloads..." : url;
                }
                if (progress == totalCount || cancelDownloads)
                {
                    progressCompleted();
                }
            }
        }

        private void progressCompleted()
        {
            if (completed)
            {
                return;
            }
            completed = true;
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

        public static async Task<ArrayList> generateUrls(DashManifest dashManifest, string folderPath)
        {
            ArrayList urlList = new ArrayList();
            await Task.Run(() =>
            {
                //Add manifest as the first item
                urlList.Add(new SegmentInfo(dashManifest.manifestRemoteUri, dashManifest.manifestLocalUri));

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
