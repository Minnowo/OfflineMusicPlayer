using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace MusicPlayer.Helpers
{
    public static class PathHelper
    {
        public static string loadedItemsPath = "";
        public static readonly string[] SizeSuffixes =
           { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public static bool OpenExplorerAtLocation(string path)
        {
            if (File.Exists(path))
            {
                Process fileopener = new Process();
                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = string.Format("/select,\"{0}\"", path);
                fileopener.Start();
                return true;
            }
            else if (Directory.Exists(path))
            {
                Process fileopener = new Process();
                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = path;
                fileopener.Start();
                return true;
            }
            return false;
        }

        public static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        public static string AskChooseDirectory(string dir = "")
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.ValidateNames = false;
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;

                dialog.FileName = "Folder Selection.";

                if (!string.IsNullOrEmpty(dir))
                    dialog.InitialDirectory = dir;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return Path.GetDirectoryName(dialog.FileName);
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                if (File.Exists(path))
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
    }
}
