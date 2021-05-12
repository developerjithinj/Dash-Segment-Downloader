using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
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

            Debug.WriteLine(manifest.ToString());
        }
    }
}
