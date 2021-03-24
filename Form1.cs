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
namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        private AudioHelper.AudioPlayer player;
        public Form1()
        {
            InitializeComponent();
            fvDirectoryView.DoubleClick += FvDirectoryView_DoubleClick;
            fvPlayListView.DoubleClick += FvPlayListView_DoubleClick;
        }

        #region tsLeftButtons

        private void tsbStopPlaying_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.Stop();
        }

        private void tsbPlayAll_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.PlayAll();
        }

        private void tsbAddToList_Click(object sender, EventArgs e)
        {
            AudioHelper.Player.playList.Clear();
            foreach (ListViewItem item in fvDirectoryView.SelectedItems)
            {
                fvPlayListView.Items.Add(item.CloneSafe());
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
            AudioHelper.playSound((string)fvPlayListView.Items[fvPlayListView.SelectedIndex].Tag);
        }

        private void FvDirectoryView_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fvDirectoryView.SelectedItems)
            {
                fvPlayListView.Items.Add(item.CloneSafe());
                AudioHelper.Player.playList.Add((string)item.Tag);
            }

            if (fvDirectoryView.SelectedItems.Count > 0)
            {
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                fvPlayListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }


        #endregion

    }
}
