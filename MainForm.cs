using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicPlayer.Controls;
using MusicPlayer.Helpers;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace MusicPlayer
{
    public partial class MainForm : Form
    {
        private bool preventOverflow = false;
        private Stopwatch shuffleRateLimit;
        private bool forceClose = false;
        public MainForm()
        {
            InitializeComponent();
            fvDirectoryView.DoubleClick += FvDirectoryView_DoubleClick;
            fvPlayListView.DoubleClick += FvPlayListView_DoubleClick;
            AudioHelper.Player.SongTimeChanged += Player_SongTimeChanged;
            AudioHelper.Player.TrackEnd += Player_TrackEnd;
            psVolume.ValueChange += ProgressSlider11_ValueChange;
            tsmiOpenWindowTray.Click += TsmiOpenWindowTray_Click;
            tsmiCloseTray.Click += TsmiCloseTray_Click;
            this.FormClosing += MainForm_FormClosing;

            pbSongTime.ForeColor = Color.CornflowerBlue;

            shuffleRateLimit = Stopwatch.StartNew();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !forceClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void TsmiCloseTray_Click(object sender, EventArgs e)
        {
            forceClose = true;
            this.Close();
        }

        private void TsmiOpenWindowTray_Click(object sender, EventArgs e)
        {
            if (!this.IsDisposed)
            {
                if (!this.Visible)
                {
                    this.Show();
                }

                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }

                this.BringToFront();
                this.Activate();
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        private void ProgressSlider11_ValueChange(int value)
        {
            if (preventOverflow)
                return;

            preventOverflow = true;

            AudioHelper.Player.Volume = (float)(value / 100f);
            nudVolume.Value = value;
            nudVolumeTray.Value = value;

            preventOverflow = false;
        }

        private void nudVolume_ValueChanged(object sender, EventArgs e)
        {
            if (preventOverflow)
                return;

            preventOverflow = true;

            AudioHelper.Player.Volume = (float)((float)((NumericUpDown)sender).Value / 100f);
            psVolume.Value = (int)((NumericUpDown)sender).Value;
            nudVolume.Value = (int)((NumericUpDown)sender).Value;
            nudVolumeTray.Value = (int)((NumericUpDown)sender).Value;

            preventOverflow = false;
        }

        private void Player_TrackEnd(int index)
        {
            if (!AudioHelper.Player.playingAll)
            {
                lblCurrentlyPlaying.Text = "Last Played:";
                return;
            }


            if (fvPlayListView.Items.Count > index + 1)
            {
                fvPlayListView.SelectedIndex = index + 1;
                lblCurrentSong.Text = Path.GetFileName(AudioHelper.Player.playList[index + 1]);
                lblCurrentlyPlaying.Text = "Currently Playing:";
                //Console.WriteLine($"now playing {AudioHelper.Player.playList[index + 1]}, index of {index + 1}");
            }
            else if (fvPlayListView.Items.Count > index)
            {
                fvPlayListView.SelectedIndex = index;
                lblCurrentSong.Text = Path.GetFileName(AudioHelper.Player.playList[index]);
                lblCurrentlyPlaying.Text = "Last Played:";
                //Console.WriteLine($"last song {AudioHelper.Player.playList[index]}, index of {index}");
            }
        }

        private void Player_SongTimeChanged(AudioTrack progress)
        {
            // to prevent cross thread exceptions 
            MethodInvoker m = new MethodInvoker(() => pbSongTime.Value = progress.progress);
            MethodInvoker m1 = new MethodInvoker(() => lblCurrentTime.Text = progress.currentTime.ToString("g"));
            MethodInvoker m2 = new MethodInvoker(() => lblTotalTime.Text = progress.totalTime.ToString("g"));
            pbSongTime.Invoke(m);
            lblCurrentTime.Invoke(m1);
            lblTotalTime.Invoke(m2);
        }

        #region tsLeftButtons

        private void tsbSavePlaylist_Click(object sender, EventArgs e)
        {
            if (fvPlayListView.Items.Count < 1)
                return;


            string fileName = PathHelper.AskSaveImage();

            if (string.IsNullOrEmpty(fileName))
                return;

            File.WriteAllText(fileName, "");
            using (StreamWriter w = File.AppendText(fileName))
            {
                foreach (ListViewItem item in fvPlayListView.Items)
                {
                    w.WriteLine(item.Tag.ToString());
                }
            }
        }

        private void tsbLoadPlaylist_Click(object sender, EventArgs e)
        {
            string file = PathHelper.AskChooseFile();

            if (string.IsNullOrEmpty(file))
                return;

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(file))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                FileInfo info;

                fvPlayListView.Items.Clear();
                AudioHelper.Player.playList.Clear();

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (File.Exists(line))
                    {
                        info = new FileInfo(line);

                        ListViewItem newItem = new ListViewItem(info.Name);
                        newItem.Tag = info.FullName;
                        newItem.Name = info.FullName;

                        fvPlayListView.Items.Add(newItem);
                        AudioHelper.Player.playList.Add(info.FullName);
                    }
                }
            }
            fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void tsbShuffle_Click(object sender, EventArgs e)
        {
            if (shuffleRateLimit.ElapsedMilliseconds > 2000 && fvPlayListView.Items.Count > 0)
            {
                shuffleRateLimit.Restart();
                Random rnd = new Random();

                AudioHelper.Player.playList.Clear();
                List<ListViewItem> items = fvPlayListView.Items.OfType<ListViewItem>().OrderBy(x => rnd.Next()).ToList();
                fvPlayListView.Items.Clear();
                fvPlayListView.Items.AddRange(items.ToArray());

                foreach (ListViewItem item in fvPlayListView.Items)
                {
                    AudioHelper.Player.playList.Add((string)item.Tag);
                }
            }

        }

        private void tsbRemoveFromList_Click(object sender, EventArgs e)
        {
            int count = fvPlayListView.SelectedItems.Count;
            if (count == 0 || AudioHelper.Player.playingAll)
                return;

            int index = fvPlayListView.SelectedIndices[0];

            for (int i = 0; i < count; i++)
            {
                fvPlayListView.Items.RemoveAt(index);
                AudioHelper.Player.playList.RemoveAt(index);
            }
        }

        private void tsbPlayFromIndex_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.PlayAllFromIndex(fvPlayListView.SelectedIndex);
        }

        private void tsbStopPlaying_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.Stop();
        }

        private void tsbPlayAll_Click(object sender, EventArgs e)
        {
            if (AudioHelper.Player.playList.Count < 1)
                return;

            lblCurrentlyPlaying.Text = "Currently Playing:";
            fvPlayListView.SelectedIndex = AudioHelper.Player.songIndex;
            lblCurrentSong.Text = Path.GetFileName(AudioHelper.Player.playList[AudioHelper.Player.songIndex]);
            AudioHelper.Player.PlayAll();
        }

        private void tsbAddToList_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fvDirectoryView.SelectedItems)
            {
                ListViewItem newItem = new ListViewItem(item.SubItems[1].Text);
                newItem.Tag = item.Tag;
                newItem.Name = item.Name;

                fvPlayListView.Items.Add(newItem);
                AudioHelper.Player.playList.Add((string)item.Tag);
            }

            if (fvDirectoryView.SelectedItems.Count > 0)
            {
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void tsbAddDirectory_Click(object sender, EventArgs e)
        {

            string dir = PathHelper.AskChooseDirectory();
            if (!string.IsNullOrEmpty(dir))
            {
                fvDirectoryView.OpenDirectory(dir);

                fvDirectoryView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                fvDirectoryView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        #endregion

        #region folder view control

        private void FvPlayListView_DoubleClick(object sender, EventArgs e)
        {
            if (AudioHelper.Player.playingAll)
                return;

            lblCurrentlyPlaying.Text = "Currently Playing:";
            string path = (string)fvPlayListView.Items[fvPlayListView.SelectedIndex].Tag;
            lblCurrentSong.Text = Path.GetFileName(path);
            AudioHelper.playSound(path);
        }

        private void FvDirectoryView_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fvDirectoryView.SelectedItems)
            {
                ListViewItem newItem = new ListViewItem(item.SubItems[1].Text);
                newItem.Tag = item.Tag;
                newItem.Name = item.Name;

                fvPlayListView.Items.Add(newItem);
                AudioHelper.Player.playList.Add((string)item.Tag);
            }

            if (fvDirectoryView.SelectedItems.Count > 0)
            {
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }


        #endregion

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (AudioHelper.Player.paused)
            {
                AudioHelper.Player.Resume();
                btnPause.Text = "Pause";
                tsmiPauseTray.Text = "Pause";
            }
            else if (AudioHelper.Player.isRunning)
            {
                AudioHelper.Player.Pause();
                btnPause.Text = "Resume"; 
                tsmiPauseTray.Text = "Resume";
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.Next();
        }

        #region context menu strip


        private void fvDirectoryView_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    tsmiRemoveFromList.Visible = false;
                    tsmiAddToList.Visible = true;

                    if (fvDirectoryView.SelectedIndex != -1)
                    {
                        foreach (ToolStripMenuItem a in this.cmsFVPlayListMain.Items.OfType<ToolStripMenuItem>())
                        {
                            a.Enabled = true;
                        }
                    }
                    else
                    {
                        foreach (ToolStripMenuItem a in this.cmsFVPlayListMain.Items.OfType<ToolStripMenuItem>())
                        {
                            a.Enabled = false;
                        }
                    }

                    cmsFVPlayListMain.Show(Cursor.Position);
                    break;
            }
        }

        private void fvPlayListView_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    tsmiAddToList.Visible = false;
                    tsmiRemoveFromList.Visible = true;

                    if (fvPlayListView.SelectedIndex != -1)
                    {
                        foreach (ToolStripMenuItem a in this.cmsFVPlayListMain.Items.OfType<ToolStripMenuItem>())
                        {
                            a.Enabled = true;
                        }
                    }
                    else
                    {
                        foreach (ToolStripMenuItem a in this.cmsFVPlayListMain.Items.OfType<ToolStripMenuItem>())
                        {
                            a.Enabled = false;
                        }
                    }

                    cmsFVPlayListMain.Show(Cursor.Position);
                    break;
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fvPlayListView.SelectedIndex != -1)
            {
                int index = fvPlayListView.SelectedIndex;
                if (File.Exists(fvPlayListView.Items[index].Tag.ToString()))
                {
                    PathHelper.OpenExplorerAtLocation(fvPlayListView.Items[index].Tag.ToString());
                }
                else if (Directory.Exists(Path.GetDirectoryName(fvPlayListView.Items[index].Tag.ToString())))
                {
                    PathHelper.OpenExplorerAtLocation(Path.GetDirectoryName(fvPlayListView.Items[index].Tag.ToString()));
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    fvPlayListView.Items.RemoveAt(index);
                    AudioHelper.Player.playList.RemoveAt(index);
                    fvPlayListView.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    fvPlayListView.Items.RemoveAt(index);
                    AudioHelper.Player.playList.RemoveAt(index);
                    fvPlayListView.SelectedIndex = -1;
                }
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fvPlayListView.SelectedIndex != -1)
            {
                int index = fvPlayListView.SelectedIndex;
                if (File.Exists(fvPlayListView.Items[index].Tag.ToString()))
                {
                    PathHelper.OpenWithDefaultProgram(fvPlayListView.Items[index].Tag.ToString());
                }
                else
                {
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    fvPlayListView.Items.RemoveAt(index);
                    AudioHelper.Player.playList.RemoveAt(index);
                    fvPlayListView.SelectedIndex = -1;
                }
            }
        }

        private void removeFromListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tsbRemoveFromList_Click(null, EventArgs.Empty);
        }

        private void tsmiAddToList_Click(object sender, EventArgs e)
        {
            tsbAddToList_Click(null, EventArgs.Empty);
        }

        #endregion

        
    }
}
