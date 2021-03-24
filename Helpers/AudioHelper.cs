using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio;
using System.Threading;
using NAudio.Wave;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MusicPlayer.Helpers
{
    public static class AudioHelper
    {
        public static AudioPlayer Player = new AudioPlayer();
        public static void playSound(string path)
        {
            if (Player == null)
            {
                Player = new AudioPlayer();
                Player.Start(path);
            }
            else if (!Player.isRunning)
            {
                Player.Start(path);
            }
            else
            {
                Player.Stop();
                Thread a = new Thread
                (
                    (x) =>
                    {
                        Thread.Sleep(500);
                        Player.Start((string)x);
                    }
                );
                a.Start(path);
            }
        }

        public class AudioPlayer
        {
            public List<string> playList = new List<string> { };

            public delegate void TrackFinished(string trackName);
            public event TrackFinished TrackEnd;

            public delegate void ProgressChanged(float progress);
            public event ProgressChanged FileCheckProgressChanged;

            public bool isRunning { get; private set; } = false;
            public bool playingAll { get; private set; } = false;

            private CancellationTokenSource cts; 
            private int songIndex = 0;
            
            private void OnTrackFinished(string track)
            {
                if(TrackEnd != null)
                {
                    TrackEnd(track);
                }
            }

            private void OnProgressChanged(float percentage)
            {
                FileCheckProgressChanged?.Invoke(percentage);
            }


            public void PlayAllFromIndex(int index)
            {
                if (playList.Count - 1 >= index)
                {
                    songIndex = index;
                    PlayAll();
                }
            }

            public void PlayAll()
            {
                if (!playingAll)
                {
                    playingAll = true;
                    if(playList.Count - 1 >= songIndex)
                    Start(playList[songIndex]);
                }
            }

            private void PlayNext()
            {
                songIndex++;

                if (playList.Count - 1 >= songIndex)
                {
                    Start(playList[songIndex]);
                }
                else
                {
                    playingAll = false;
                    songIndex = 0;
                }
            }

            public async void Start(string filePath)
            {
                if (!isRunning && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    isRunning = true;

                    Progress<float> progress = new Progress<float>(OnProgressChanged);
                    cts = new CancellationTokenSource();

                    await Task.Run(() =>
                    {
                        try
                        {
                            PlayAudioThread(filePath, progress, cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                        }
                    }, cts.Token);

                    OnTrackFinished(filePath);
                    isRunning = false;
                    GC.Collect();
                }

                if (playingAll)
                {
                    PlayNext();
                }
            }

            public void Stop()
            {
                if (cts != null)
                {
                    cts.Cancel();
                }
                songIndex = 0;
                playingAll = false;
            }

            private void PlayAudioThread(string filePath, IProgress<float> progress, CancellationToken ct)
            {
                using (WaveOutEvent wout = new WaveOutEvent())
                using (AudioFileReader afr = new AudioFileReader(filePath))
                {
                    wout.Init(afr);
                    wout.Play();
                    Stopwatch timer = Stopwatch.StartNew();
                    while (wout.PlaybackState != PlaybackState.Stopped && !ct.IsCancellationRequested)
                    {
                        if (timer.ElapsedMilliseconds > 200)
                        {
                            progress.Report((float)(afr.CurrentTime.TotalSeconds / afr.TotalTime.TotalSeconds) * 100f);

                            timer.Reset();
                            timer.Start();
                        }
                        
                        Thread.Sleep(200);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        progress.Report(0);

                        ct.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        progress.Report(100);
                    }
                }
            }
        }


    }
}

