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
        private ContextMenuStrip cmsMain;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItemOpen;
        private ToolStripSeparator toolStripSeparator1;
        private ContextMenuStrip cmsOpen;
        private ToolStripMenuItem toolStripMenuItemOpenFile;
        private ToolStripMenuItem toolStripMenuItemOpenFolder;
        private ToolStripMenuItem toolStripMenuItemRemoveFromList;
        private ToolStripMenuItem tsmiAddToList;

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

        public bool autoFillColumn { get; set; } = true;
        //public bool showingContextMenu { get; private set; } = false;
        public FileView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.EnableNotifyMessage, true);
            InitializeComponent();

            FullRowSelect = true;


            #region cmsMain Events

            cmsMain.Opening += ContextMenuStrip_Opening;
            //cmsMain.Closing += CmsMain_Closing;
            //toolStripMenuItemDelete.Click += ToolStripMenuItemDelete_Click;
            toolStripMenuItemRemoveFromList.Click += ToolStripMenuItemRemoveFromList_Click;

            #region cmsOpen Events
            toolStripMenuItemOpenFile.Click += ToolStripMenuItemOpenFile_Click;
            toolStripMenuItemOpenFolder.Click += ToolStripMenuItemOpenFolder_Click;
            #endregion

            #endregion

            MouseDoubleClick += MouseDoubleClick_Event;

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

        private bool supressIndexChangeEvent = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    supressIndexChangeEvent = false;
                    break;
                case MouseButtons.Right:
                    supressIndexChangeEvent = true;
                    break;
            }

            base.OnMouseDown(e);
        }

        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            if (!supressIndexChangeEvent)
            {
                base.OnItemSelectionChanged(e);
            }
        }


        #region cmsMain

        #region cmsOpen
        private void ToolStripMenuItemOpenFile_Click(object sender, EventArgs e)
        {
            if (SelectedIndex != -1)
            {
                if (File.Exists(Items[SelectedIndex].Tag.ToString()))
                {
                    PathHelper.OpenWithDefaultProgram(Items[SelectedIndex].Tag.ToString());
                }
                else
                {
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    Items.Remove(Items[SelectedIndex]);
                    SelectedIndex = -1;
                }
            }
        }

        private void ToolStripMenuItemOpenFolder_Click(object sender, EventArgs e)
        {
            if (SelectedIndex != -1)
            {
                if (File.Exists(Items[SelectedIndex].Tag.ToString()))
                {
                    PathHelper.OpenExplorerAtLocation(Items[SelectedIndex].Tag.ToString());
                }
                else if (Directory.Exists(Path.GetDirectoryName(Items[SelectedIndex].Tag.ToString())))
                {
                    PathHelper.OpenExplorerAtLocation(Path.GetDirectoryName(Items[SelectedIndex].Tag.ToString()));
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    Items.Remove(Items[SelectedIndex]);
                    SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    Items.Remove(Items[SelectedIndex]);
                    SelectedIndex = -1;
                }
            }
        }


        #endregion

        private async void ToolStripMenuItemRemoveFromList_Click(object sender, EventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in SelectedItems)
                {
                    this.Items.Remove(item);
                }
                await ListViewDumpAsync((ListViewItem[])this.Items.OfType<ListViewItem>().ToArray().Clone());
            }
        }
        #endregion

        public async Task ListViewDumpAsync(ListViewItem[] items)
        {
            if (File.Exists(PathHelper.loadedItemsPath))
            {
                await Task.Run(() =>
                {
                    System.IO.File.WriteAllText(PathHelper.loadedItemsPath, "");
                    using (StreamWriter w = File.AppendText(PathHelper.loadedItemsPath))
                    {
                        foreach (ListViewItem item in items.Reverse())
                        {
                            w.WriteLine(item.Tag.ToString());
                        }
                    }
                });
            }
        }

        public void AddItem(ListViewItem item)
        {
            using (StreamWriter w = File.AppendText(PathHelper.loadedItemsPath))
            {
                w.WriteLine(item.Tag.ToString());
            }
            this.Items.Add(item);
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void InsertItem(int index, ListViewItem item)
        {
            using (StreamWriter w = File.AppendText(PathHelper.loadedItemsPath))
            {
                w.WriteLine(item.Tag.ToString());
            }
            this.Items.Insert(index, item);
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void MouseDoubleClick_Event(object sender, MouseEventArgs e)
        {
            if (SelectedIndex != -1)
            {
                if (File.Exists(Items[SelectedIndex].Tag.ToString()))
                {
                    //PathHelper.OpenWithDefaultProgram(Items[SelectedIndex].Tag.ToString());
                }
                else
                {
                    MessageBox.Show("The file path has changed or the file has been deleted");
                    Items.Remove(Items[SelectedIndex]);
                    SelectedIndex = -1;
                }
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedIndex != -1)
            {
                foreach (ToolStripMenuItem a in this.cmsMain.Items.OfType<ToolStripMenuItem>())
                {
                    a.Enabled = true;
                }
            }
            else
            {
                foreach (ToolStripMenuItem a in this.cmsMain.Items.OfType<ToolStripMenuItem>())
                {
                    a.Enabled = false;
                }
            }
        }

        /*protected override void WndProc(ref Message m)
        {
            if (autoFillColumn && m.Msg == (int)WindowsMessages.PAINT && !DesignMode)
            {
                if (Columns.Count != 0) // sizes the columns to fill the rest of the list box
                {
                    this.Columns[this.Columns.Count - 1].Width = -2;
                }
            }
            base.WndProc(ref m);
        }*/

        public void UnselectAll()
        {
            if (MultiSelect)
            {
                SelectedItems.Clear();
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsOpen = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRemoveFromList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddToList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMain.SuspendLayout();
            this.cmsOpen.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsMain
            // 
            this.cmsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpen,
            this.tsmiAddToList,
            this.toolStripSeparator1,
            this.toolStripMenuItemRemoveFromList});
            this.cmsMain.Name = "cmsMain";
            this.cmsMain.Size = new System.Drawing.Size(234, 82);
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.DropDown = this.cmsOpen;
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(233, 24);
            this.toolStripMenuItemOpen.Text = "Open";
            // 
            // cmsOpen
            // 
            this.cmsOpen.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsOpen.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenFile,
            this.toolStripMenuItemOpenFolder});
            this.cmsOpen.Name = "cmsOpen";
            this.cmsOpen.OwnerItem = this.toolStripMenuItemOpen;
            this.cmsOpen.Size = new System.Drawing.Size(161, 52);
            // 
            // toolStripMenuItemOpenFile
            // 
            this.toolStripMenuItemOpenFile.Name = "toolStripMenuItemOpenFile";
            this.toolStripMenuItemOpenFile.Size = new System.Drawing.Size(160, 24);
            this.toolStripMenuItemOpenFile.Text = "Open File";
            // 
            // toolStripMenuItemOpenFolder
            // 
            this.toolStripMenuItemOpenFolder.Name = "toolStripMenuItemOpenFolder";
            this.toolStripMenuItemOpenFolder.Size = new System.Drawing.Size(160, 24);
            this.toolStripMenuItemOpenFolder.Text = "Open Folder";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(230, 6);
            // 
            // toolStripMenuItemRemoveFromList
            // 
            this.toolStripMenuItemRemoveFromList.Name = "toolStripMenuItemRemoveFromList";
            this.toolStripMenuItemRemoveFromList.Size = new System.Drawing.Size(233, 24);
            this.toolStripMenuItemRemoveFromList.Text = "Remove Selected Items";
            // 
            // tsmiAddToList
            // 
            this.tsmiAddToList.Name = "tsmiAddToList";
            this.tsmiAddToList.Size = new System.Drawing.Size(233, 24);
            this.tsmiAddToList.Text = "AddToList";
            // 
            // FileView
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.cmsMain;
            this.FullRowSelect = true;
            this.View = System.Windows.Forms.View.Details;
            this.cmsMain.ResumeLayout(false);
            this.cmsOpen.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}

