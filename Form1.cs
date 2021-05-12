using System;
using System.Diagnostics;
using System.Windows.Forms;


namespace Dash_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {

            string manifest = "C:\\Users\\Jithin Jose\\Downloads\\manifest.mpd";
            DashManifest dashManifest = DashManifest.parseManifest(manifest);
            Debug.WriteLine(dashManifest.ToString());
        }

    }

}
