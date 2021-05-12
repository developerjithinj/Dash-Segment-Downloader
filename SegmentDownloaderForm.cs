using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dash_Downloader
{
    public partial class SegmentDownloaderForm : Form
    {
        private DashManifest manifest;

        public SegmentDownloaderForm(DashManifest dashManifest)
        {

            manifest = dashManifest;
            InitializeComponent();
            generateUrls(dashManifest);
            //            Debug.WriteLine(manifest.ToString());
        }

        public static async Task generateUrls(DashManifest dashManifest)
        {
            await Task.Run(() =>
            {
                ArrayList urlList = new ArrayList();
                foreach (DashManifest.Track track in dashManifest.tracks)
                {
                    if (track.selected)
                    {
                        string initSegUrl = dashManifest.remoteUrl + dashManifest.path + track.initSegmentTemplate.Replace("$RepresentationID$", track.id);
                        urlList.Add(initSegUrl);
                        Debug.WriteLine(initSegUrl);
                        for (int i = track.segmentStartIndex; i <= track.segmentCount; i++)
                        {
                            string segUrl = dashManifest.remoteUrl + dashManifest.path + track.mediaSegmentTemplate.Replace("$RepresentationID$", track.id).Replace("$Number$", (i + ""));
                            urlList.Add(segUrl);
                            Debug.WriteLine(segUrl);
                        }
                    }

                }
            });
        }

    }
}
