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
        public MainForm()
        {
            InitializeComponent();
            fvDirectoryView.DoubleClick += FvDirectoryView_DoubleClick;
            fvPlayListView.DoubleClick += FvPlayListView_DoubleClick;
            AudioHelper.Player.SongTimeChanged += Player_SongTimeChanged;
            AudioHelper.Player.TrackEnd += Player_TrackEnd;

            pbSongTime.ForeColor = Color.CornflowerBlue;
            psVolume.ValueChange += ProgressSlider11_ValueChange;

            shuffleRateLimit = Stopwatch.StartNew();
        }

        private void ProgressSlider11_ValueChange(int value)
        {
            if (preventOverflow)
                return;

            preventOverflow = true;

            AudioHelper.Player.Volume = (float)(value / 100f);
            nudVolume.Value = value;

            preventOverflow = false;
        }

        private void nudVolume_ValueChanged(object sender, EventArgs e)
        {
            if (preventOverflow)
                return;

            preventOverflow = true;

            AudioHelper.Player.Volume = (float)((float)nudVolume.Value / 100f);
            psVolume.Value = (int)nudVolume.Value;

            preventOverflow = false;
        }

        private void Player_TrackEnd(int index)
        {
            if (!AudioHelper.Player.playingAll)
            {
                lblCurrentlyPlaying.Text = "Last Played:";
                return;
            }
                

            if(fvPlayListView.Items.Count > index + 1)
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

        private void tsbShuffle_Click(object sender, EventArgs e)
        {
            if(shuffleRateLimit.ElapsedMilliseconds > 2000 && fvPlayListView.Items.Count > 0)
            {
                shuffleRateLimit.Restart();
                Random rnd = new Random();

                AudioHelper.Player.playList.Clear();
                List< ListViewItem> items = fvPlayListView.Items.OfType<ListViewItem>().OrderBy(x => rnd.Next()).ToList();
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

            for(int i = 0; i < count; i++)
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

            if(fvDirectoryView.SelectedItems.Count > 0)
            {
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }


        #endregion

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (AudioHelper.Player.paused)
            {
                AudioHelper.Player.Resume();
                btnPause.Text = "Pause";
            }
            else if(AudioHelper.Player.isRunning)
            {
                AudioHelper.Player.Pause();
                btnPause.Text = "Resume";
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.Next();
        }
    }
}
