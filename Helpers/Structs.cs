using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Helpers
{
    public struct AudioTrack
    {
        public static readonly AudioTrack empty;
        public TimeSpan totalTime;
        public TimeSpan currentTime;
        public int progress;
    }
}
