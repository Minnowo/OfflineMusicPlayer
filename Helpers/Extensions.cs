using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Helpers
{
    public static class Extensions
    {
        public static T CloneSafe<T>(this T obj) where T : class, ICloneable
        {
            try
            {
                if (obj != null)
                {
                    return obj.Clone() as T;
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
