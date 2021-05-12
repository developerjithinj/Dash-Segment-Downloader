namespace Dash_Downloader
{
    partial class DashSegmentDownloaderParamsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBrowseManifest = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.labelManifestPath = new System.Windows.Forms.Label();
            this.textBoxManifestFile = new System.Windows.Forms.TextBox();
            this.labelTracks = new System.Windows.Forms.Label();
            this.textBoxOutFolder = new System.Windows.Forms.TextBox();
            this.labelOutputFolder = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkedListBoxTracks = new System.Windows.Forms.CheckedListBox();
            this.folderBrowserOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBrowseManifest
            // 
            this.buttonBrowseManifest.Location = new System.Drawing.Point(654, 28);
            this.buttonBrowseManifest.Name = "buttonBrowseManifest";
            this.buttonBrowseManifest.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseManifest.TabIndex = 0;
            this.buttonBrowseManifest.Text = "Fetch";
            this.buttonBrowseManifest.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonBrowseManifest.UseVisualStyleBackColor = true;
            this.buttonBrowseManifest.Click += new System.EventHandler(this.buttonBrowseManifest_Click);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(312, 387);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(144, 37);
            this.buttonDownload.TabIndex = 1;
            this.buttonDownload.Text = "Download Segments";
            this.buttonDownload.UseVisualStyleBackColor = false;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // labelManifestPath
            // 
            this.labelManifestPath.AutoSize = true;
            this.labelManifestPath.Location = new System.Drawing.Point(28, 32);
            this.labelManifestPath.Name = "labelManifestPath";
            this.labelManifestPath.Size = new System.Drawing.Size(77, 15);
            this.labelManifestPath.TabIndex = 2;
            this.labelManifestPath.Text = "Manifest File:";
            // 
            // textBoxManifestFile
            // 
            this.textBoxManifestFile.AllowDrop = true;
            this.textBoxManifestFile.Location = new System.Drawing.Point(118, 28);
            this.textBoxManifestFile.Name = "textBoxManifestFile";
            this.textBoxManifestFile.Size = new System.Drawing.Size(518, 23);
            this.textBoxManifestFile.TabIndex = 3;
            // 
            // labelTracks
            // 
            this.labelTracks.AutoSize = true;
            this.labelTracks.Location = new System.Drawing.Point(29, 115);
            this.labelTracks.Name = "labelTracks";
            this.labelTracks.Size = new System.Drawing.Size(76, 15);
            this.labelTracks.TabIndex = 4;
            this.labelTracks.Text = "Select Tracks:";
            // 
            // textBoxOutFolder
            // 
            this.textBoxOutFolder.Enabled = false;
            this.textBoxOutFolder.Location = new System.Drawing.Point(118, 71);
            this.textBoxOutFolder.Name = "textBoxOutFolder";
            this.textBoxOutFolder.Size = new System.Drawing.Size(518, 23);
            this.textBoxOutFolder.TabIndex = 7;
            // 
            // labelOutputFolder
            // 
            this.labelOutputFolder.AutoSize = true;
            this.labelOutputFolder.Location = new System.Drawing.Point(28, 75);
            this.labelOutputFolder.Name = "labelOutputFolder";
            this.labelOutputFolder.Size = new System.Drawing.Size(84, 15);
            this.labelOutputFolder.TabIndex = 6;
            this.labelOutputFolder.Text = "Output Folder:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(654, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Browse";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkedListBoxTracks
            // 
            this.checkedListBoxTracks.FormattingEnabled = true;
            this.checkedListBoxTracks.Location = new System.Drawing.Point(118, 115);
            this.checkedListBoxTracks.Name = "checkedListBoxTracks";
            this.checkedListBoxTracks.Size = new System.Drawing.Size(518, 256);
            this.checkedListBoxTracks.TabIndex = 8;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(654, 115);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 9;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // DashSegmentDownloaderParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 450);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.checkedListBoxTracks);
            this.Controls.Add(this.textBoxOutFolder);
            this.Controls.Add(this.labelOutputFolder);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelTracks);
            this.Controls.Add(this.textBoxManifestFile);
            this.Controls.Add(this.labelManifestPath);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.buttonBrowseManifest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "DashSegmentDownloaderParamsForm";
            this.Text = "Dash Segment Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBrowseManifest;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label labelManifestPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelTracks;
        private System.Windows.Forms.TextBox textBoxOutFolder;
        private System.Windows.Forms.Label labelOutputFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxManifestFile;
        private System.Windows.Forms.CheckedListBox checkedListBoxTracks;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonSelectAll;
    }
}

