using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Dash_Downloader
{
    class DialogUtil
    {

        public static void showNoRetryError(string message) {
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                //this.Close();
            }
         
        }
    }
}
