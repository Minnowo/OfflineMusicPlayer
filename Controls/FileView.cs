using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using MusicPlayer.Helpers;
namespace MusicPlayer.Controls
{
    public partial class FileView : ListView
    {
        private IContainer components;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                if (SelectedIndices.Count > 0)
                {
                    return SelectedIndices[0];
                }

                return -1;
            }
            set
            {
                UnselectAll();

                if (value > -1 && value < Items.Count)
                {
                    ListViewItem lvi = Items[value];
                    lvi.EnsureVisible();
                    lvi.Selected = true;
                }
            }
        }

        public FileView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.EnableNotifyMessage, true);
            InitializeComponent();

            FullRowSelect = true;

        }

        public void OpenDirectory(string dir)
        {
            OpenDirectory(new DirectoryInfo(dir));
        }

        public void OpenDirectory(DirectoryInfo directoryInfo)
        {
            ListViewGroup dirItem;
            string[] dirRow = { directoryInfo.Name };
            string[] itemRow;

            dirItem = this.Groups[directoryInfo.FullName];
            if (dirItem == null)
            {
                dirItem = new ListViewGroup() 
                { 
                    Header = directoryInfo.Name, 
                    Tag = directoryInfo.FullName, 
                    Name = directoryInfo.FullName 
                };

                this.Groups.Add(dirItem);
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (this.Items[file.FullName] == null)
                {
                    itemRow = new string[]
                    {
                    file.Name,
                    PathHelper.SizeSuffix(file.Length)
                    };

                    ListViewItem item = new ListViewItem()
                    {
                        Name = file.FullName,
                        Text = "", 
                        Tag = file.FullName 
                    };
                    item.SubItems.AddRange(itemRow);
                    item.Group = dirItem;
                    this.Items.Add(item);
                }
            }
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories())
            {
                try
                {
                    OpenDirectory(subdir);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        public void UnselectAll()
        {
            if (MultiSelect)
            {
                SelectedItems.Clear();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FileView
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FullRowSelect = true;
            this.View = System.Windows.Forms.View.Details;
            this.ResumeLayout(false);

        }
    }
}

