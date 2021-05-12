using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Dash_Downloader
{
    public partial class DashSegmentDownloaderParamsForm : Form
    {
        DashManifest dashManifest;
        public DashSegmentDownloaderParamsForm()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (validateInputParameters())
            {

                for (int i = 0; i < (checkedListBoxTracks.Items.Count); i++)
                {
                    ((DashManifest.Track)dashManifest.tracks[i]).selected = checkedListBoxTracks.GetItemChecked(i);
                }

                SegmentDownloaderForm segmentDownloader = new SegmentDownloaderForm(dashManifest,textBoxOutFolder.Text);
                segmentDownloader.ShowDialog();
            }
        }

        //Basic Validations before downloading
        private bool validateInputParameters()
        {

            if (textBoxManifestFile.Text.Length <= 0)
            {
                DialogUtil.showNoRetryError("Please select the manifest file first");
                return false;

            }
            if (dashManifest == null)
            {
                DialogUtil.showNoRetryError("Manifest unavailable. Fetch manifest and try again");
                return false;
            }
            if (dashManifest.isLocal)
            {
                DialogUtil.showNoRetryError("Segment download unavailable for local files");
                return false;
            }
            if (textBoxOutFolder.Text.Length <= 0)
            {
                DialogUtil.showNoRetryError("Please select the output folder");
                return false;

            }
            if (checkedListBoxTracks.CheckedItems.Count <= 0)
            {
                DialogUtil.showNoRetryError("Please select atleast one track to download");
                return false;
            }

            return true;
        }

        private void buttonBrowseManifest_Click(object sender, EventArgs e)
        {
            string xmlData = "";
            bool isLocal = true;
            string uri = textBoxManifestFile.Text;
            if (Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                using (var client = new WebClient())
                {
                    xmlData = (client).DownloadString(uri);
                    isLocal = false;
                }

            }
            else if (File.Exists(uri))
            {
                xmlData = File.ReadAllText(uri);
                DialogUtil.showNoRetryError("You've selected a local file. The tracks will be listed but, segment download won't work.");
            }
            else
            {
                DialogUtil.showNoRetryError("Invalid manifest file path");
                return;
            }
            if (xmlData.Length <= 0)
            {
                DialogUtil.showNoRetryError("Failed to fetch manifest file");
                return;
            }
            populateManifest(xmlData, uri, isLocal);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (folderBrowserOutput.ShowDialog() == DialogResult.OK)
            {
                textBoxOutFolder.Text = folderBrowserOutput.SelectedPath;
            }
        }

        private void populateManifest(String manifestData, String uri, bool isLocal)
        {
            checkedListBoxTracks.Items.Clear();
            XmlDocument doc = new XmlDocument();
            try
            {
                //Load xml file
                doc.LoadXml(manifestData);
            }
            catch (Exception e)
            {
                DialogUtil.showNoRetryError("Invalid manifest file");
                return;
            }

            dashManifest = DashManifest.parseManifestData(doc, uri, isLocal);
            if (dashManifest == null)
            {
                textBoxManifestFile.Text = "";
                DialogUtil.showNoRetryError("Unsupported Type");
                return;

            }
            ArrayList tracks = dashManifest.tracks;
            foreach (DashManifest.Track track in tracks)
            {
                checkedListBoxTracks.Items.Add(getTrackText(track));
            }
        }

        private string getTrackText(DashManifest.Track track)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[").Append(track.type.ToUpper()).Append("] ").Append(track.id)
                .Append(" (");
            if (track.width > 0 && track.height > 0)
            {
                sb.Append(track.width + "").Append("x").Append(track.height + "");
            }
            if (track.bandwidth > 0)
            {
                sb.Append(" @" + track.bandwidth + "bps");
            }
            if (track.frameRate > 0)
            {
                sb.Append(", " + track.frameRate + "fps");
            }
            if (track.language != null && track.language.Length > 0)
            {
                sb.Append(", lang=").Append(track.language);
            }
            sb.Append(")");
            return sb.ToString();
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxTracks.Items.Count; i++)
            {
                checkedListBoxTracks.SetItemChecked(i, true);
            }
        }
    }

}
