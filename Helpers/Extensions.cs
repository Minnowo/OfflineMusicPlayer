using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Helpers
{
    public static class Extensions
    {
        public static T TClamp<T>(T num, T min, T max) where T : IComparable<T>
        {
            if (num.CompareTo(min) <= 0) return min;
            if (num.CompareTo(max) >= 0) return max;
            return num;
        }

        public static T Clamp<T>(this T input, T min, T max) where T : IComparable<T>
        {
            return TClamp(input, min, max);
        }

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
