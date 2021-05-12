using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;


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


                for (int i = 0; i < checkedListBoxTracks.Items.Count; i++)
                {
                    CheckState st = checkedListBoxTracks.GetItemCheckState(checkedListBoxTracks.Items.IndexOf(i));
                    //dashManifest.tracks[i].selected = st == CheckState.Checked;

                }
            }
        }

        //Basic Validations before downloading
        private bool validateInputParameters()
        {
            if (dashManifest == null)
            {
                return false;
            }
            if (textBoxManifestFile.Text.Length <= 0)
            {
                DialogUtil.showNoRetryError("Please select the manifest file first");
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
            if (openFileManifest.ShowDialog() == DialogResult.OK)
            {
                string manifestFile = openFileManifest.FileName;
                textBoxManifestFile.Text = manifestFile;
                populateManifest(manifestFile);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (folderBrowserOutput.ShowDialog() == DialogResult.OK)
            {
                textBoxOutFolder.Text = folderBrowserOutput.SelectedPath;
            }
        }

        private void populateManifest(String manifestFile)
        {
            checkedListBoxTracks.Items.Clear();
            dashManifest = DashManifest.parseManifest(manifestFile);
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
            Debug.WriteLine(dashManifest.ToString());
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
    }

}
