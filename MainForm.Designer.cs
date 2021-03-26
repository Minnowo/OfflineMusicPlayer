namespace MusicPlayer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tsLeftButtons = new System.Windows.Forms.ToolStrip();
            this.tsbAddDirectory = new System.Windows.Forms.ToolStripButton();
            this.tsbAddToList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPlayAll = new System.Windows.Forms.ToolStripButton();
            this.tsbPlayFromIndex = new System.Windows.Forms.ToolStripButton();
            this.tsbStopPlaying = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrentSong = new System.Windows.Forms.Label();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.nudVolume = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tsbRemoveFromList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.psVolume = new MusicPlayer.Controls.ProgressSlider1();
            this.pbSongTime = new MusicPlayer.Controls.ColorProgressBar();
            this.fvDirectoryView = new MusicPlayer.Controls.FileView();
            this.chFolderViewFolderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDirectoryViewFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFolderViewFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDirectoryViewFileLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fvPlayListView = new MusicPlayer.Controls.FileView();
            this.chPlayListFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tsLeftButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // tsLeftButtons
            // 
            this.tsLeftButtons.AutoSize = false;
            this.tsLeftButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.tsLeftButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsLeftButtons.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsLeftButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddDirectory,
            this.tsbAddToList,
            this.toolStripSeparator2,
            this.tsbRemoveFromList,
            this.toolStripSeparator1,
            this.tsbPlayAll,
            this.tsbPlayFromIndex,
            this.tsbStopPlaying});
            this.tsLeftButtons.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsLeftButtons.Location = new System.Drawing.Point(0, 0);
            this.tsLeftButtons.Name = "tsLeftButtons";
            this.tsLeftButtons.Size = new System.Drawing.Size(142, 481);
            this.tsLeftButtons.TabIndex = 2;
            this.tsLeftButtons.Text = "toolStrip1";
            // 
            // tsbAddDirectory
            // 
            this.tsbAddDirectory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddDirectory.Name = "tsbAddDirectory";
            this.tsbAddDirectory.Size = new System.Drawing.Size(140, 24);
            this.tsbAddDirectory.Text = "AddDirectory";
            this.tsbAddDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbAddDirectory.Click += new System.EventHandler(this.tsbAddDirectory_Click);
            // 
            // tsbAddToList
            // 
            this.tsbAddToList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddToList.Name = "tsbAddToList";
            this.tsbAddToList.Size = new System.Drawing.Size(140, 24);
            this.tsbAddToList.Text = "AddToList";
            this.tsbAddToList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbAddToList.Click += new System.EventHandler(this.tsbAddToList_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // tsbPlayAll
            // 
            this.tsbPlayAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlayAll.Name = "tsbPlayAll";
            this.tsbPlayAll.Size = new System.Drawing.Size(140, 24);
            this.tsbPlayAll.Text = "PlayAll";
            this.tsbPlayAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbPlayAll.Click += new System.EventHandler(this.tsbPlayAll_Click);
            // 
            // tsbPlayFromIndex
            // 
            this.tsbPlayFromIndex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlayFromIndex.Name = "tsbPlayFromIndex";
            this.tsbPlayFromIndex.Size = new System.Drawing.Size(140, 24);
            this.tsbPlayFromIndex.Text = "StartFromSelection";
            this.tsbPlayFromIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbPlayFromIndex.Click += new System.EventHandler(this.tsbPlayFromIndex_Click);
            // 
            // tsbStopPlaying
            // 
            this.tsbStopPlaying.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStopPlaying.Name = "tsbStopPlaying";
            this.tsbStopPlaying.Size = new System.Drawing.Size(140, 24);
            this.tsbStopPlaying.Text = "Stop";
            this.tsbStopPlaying.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbStopPlaying.ToolTipText = "Stop";
            this.tsbStopPlaying.Click += new System.EventHandler(this.tsbStopPlaying_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(145, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fvDirectoryView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fvPlayListView);
            this.splitContainer1.Size = new System.Drawing.Size(652, 402);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 405);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Song:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 422);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Time:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 439);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Total Time:";
            // 
            // lblCurrentSong
            // 
            this.lblCurrentSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentSong.AutoSize = true;
            this.lblCurrentSong.Location = new System.Drawing.Point(247, 405);
            this.lblCurrentSong.Name = "lblCurrentSong";
            this.lblCurrentSong.Size = new System.Drawing.Size(42, 17);
            this.lblCurrentSong.TabIndex = 8;
            this.lblCurrentSong.Text = "None";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(247, 422);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(16, 17);
            this.lblCurrentTime.TabIndex = 9;
            this.lblCurrentTime.Text = "0";
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Location = new System.Drawing.Point(247, 439);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(16, 17);
            this.lblTotalTime.TabIndex = 10;
            this.lblTotalTime.Text = "0";
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Location = new System.Drawing.Point(722, 430);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 11;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // nudVolume
            // 
            this.nudVolume.Location = new System.Drawing.Point(12, 416);
            this.nudVolume.Name = "nudVolume";
            this.nudVolume.Size = new System.Drawing.Size(120, 22);
            this.nudVolume.TabIndex = 15;
            this.nudVolume.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudVolume.ValueChanged += new System.EventHandler(this.nudVolume_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 396);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Volume:";
            // 
            // tsbRemoveFromList
            // 
            this.tsbRemoveFromList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveFromList.Name = "tsbRemoveFromList";
            this.tsbRemoveFromList.Size = new System.Drawing.Size(140, 24);
            this.tsbRemoveFromList.Text = "RemoveFromList";
            this.tsbRemoveFromList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbRemoveFromList.Click += new System.EventHandler(this.tsbRemoveFromList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // psVolume
            // 
            this.psVolume.BackColor = System.Drawing.Color.Black;
            this.psVolume.BorderSize = 2;
            this.psVolume.Location = new System.Drawing.Point(12, 444);
            this.psVolume.Name = "psVolume";
            this.psVolume.Size = new System.Drawing.Size(120, 25);
            this.psVolume.SliderColor = System.Drawing.Color.BlueViolet;
            this.psVolume.TabIndex = 14;
            this.psVolume.Value = 50;
            // 
            // pbSongTime
            // 
            this.pbSongTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSongTime.Location = new System.Drawing.Point(145, 459);
            this.pbSongTime.Name = "pbSongTime";
            this.pbSongTime.Size = new System.Drawing.Size(652, 10);
            this.pbSongTime.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbSongTime.TabIndex = 4;
            // 
            // fvDirectoryView
            // 
            this.fvDirectoryView.autoFillColumn = true;
            this.fvDirectoryView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fvDirectoryView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFolderViewFolderName,
            this.chDirectoryViewFileName,
            this.chFolderViewFileSize,
            this.chDirectoryViewFileLength});
            this.fvDirectoryView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fvDirectoryView.FullRowSelect = true;
            this.fvDirectoryView.HideSelection = false;
            this.fvDirectoryView.Location = new System.Drawing.Point(0, 0);
            this.fvDirectoryView.Name = "fvDirectoryView";
            this.fvDirectoryView.Size = new System.Drawing.Size(320, 402);
            this.fvDirectoryView.TabIndex = 0;
            this.fvDirectoryView.UseCompatibleStateImageBehavior = false;
            this.fvDirectoryView.View = System.Windows.Forms.View.Details;
            // 
            // chFolderViewFolderName
            // 
            this.chFolderViewFolderName.Text = "Folder";
            // 
            // chDirectoryViewFileName
            // 
            this.chDirectoryViewFileName.Text = "Name";
            // 
            // chFolderViewFileSize
            // 
            this.chFolderViewFileSize.Text = "Size";
            // 
            // chDirectoryViewFileLength
            // 
            this.chDirectoryViewFileLength.Text = "Length";
            // 
            // fvPlayListView
            // 
            this.fvPlayListView.autoFillColumn = true;
            this.fvPlayListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fvPlayListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPlayListFileName});
            this.fvPlayListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fvPlayListView.FullRowSelect = true;
            this.fvPlayListView.HideSelection = false;
            this.fvPlayListView.Location = new System.Drawing.Point(0, 0);
            this.fvPlayListView.Name = "fvPlayListView";
            this.fvPlayListView.Size = new System.Drawing.Size(328, 402);
            this.fvPlayListView.TabIndex = 0;
            this.fvPlayListView.UseCompatibleStateImageBehavior = false;
            this.fvPlayListView.View = System.Windows.Forms.View.Details;
            // 
            // chPlayListFileName
            // 
            this.chPlayListFileName.Text = "Name";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 481);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudVolume);
            this.Controls.Add(this.psVolume);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.lblTotalTime);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.lblCurrentSong);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbSongTime);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tsLeftButtons);
            this.Name = "MainForm";
            this.Text = "CustomTrackBar Enabled";
            this.tsLeftButtons.ResumeLayout(false);
            this.tsLeftButtons.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip tsLeftButtons;
        private System.Windows.Forms.ToolStripButton tsbAddDirectory;
        private System.Windows.Forms.ToolStripButton tsbPlayAll;
        private System.Windows.Forms.ToolStripButton tsbAddToList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.FileView fvDirectoryView;
        private System.Windows.Forms.ColumnHeader chFolderViewFolderName;
        private System.Windows.Forms.ColumnHeader chFolderViewFileSize;
        private System.Windows.Forms.ColumnHeader chDirectoryViewFileName;
        private Controls.FileView fvPlayListView;
        private System.Windows.Forms.ColumnHeader chPlayListFileName;
        private System.Windows.Forms.ColumnHeader chDirectoryViewFileLength;
        private System.Windows.Forms.ToolStripButton tsbPlayFromIndex;
        private System.Windows.Forms.ToolStripButton tsbStopPlaying;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrentSong;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblTotalTime;
        private Controls.ColorProgressBar pbSongTime;
        private System.Windows.Forms.Button btnPause;
        private Controls.ProgressSlider1 psVolume;
        private System.Windows.Forms.NumericUpDown nudVolume;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbRemoveFromList;
    }
}

