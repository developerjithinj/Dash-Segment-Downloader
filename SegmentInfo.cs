using System;
using System.Collections.Generic;
using System.Text;

namespace Dash_Downloader
{
    class SegmentInfo
    {
        public string remoteUri;
        public string localUri;

        public SegmentInfo(string remoteUri, string localUri)
        {
            this.remoteUri = remoteUri;
            this.localUri = localUri;
        }
    }
}
