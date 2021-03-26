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
                // if the main thread is blocked it won't play another song until 
                // the next time start is played after stop, so using threading we can
                // ignore that
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

            public delegate void TrackFinished(int index);
            public event TrackFinished TrackEnd;

            public delegate void ProgressChanged(AudioTrack progress);
            public event ProgressChanged SongTimeChanged;

            public float Volume 
            {
                get 
                {
                    return volume;
                } 
                set
                {
                    volume = value.Clamp(0, 1);
                } 
            }
            public int songIndex { get; private set; } = 0;
            public bool isRunning { get; private set; } = false;
            public bool playingAll { get; private set; } = false;
            public bool paused { get; private set; } = false;

            private float volume = 0.5f;
            private CancellationTokenSource cts;


            private void OnTrackFinished()
            {
                if (TrackEnd != null)
                {
                    TrackEnd(songIndex);
                }
            }

            private void OnProgressChanged(AudioTrack percentage)
            {
                SongTimeChanged?.Invoke(percentage);
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
                    if (playList.Count - 1 >= songIndex)
                    {
                        playingAll = true;
                        Start(playList[songIndex]);
                    }
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

                    Progress<AudioTrack> progress = new Progress<AudioTrack>(OnProgressChanged);
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

                    OnTrackFinished();
                    isRunning = false;
                    GC.Collect();
                }

                if (playingAll)
                {
                    PlayNext();
                }
            }

            public void Resume()
            {
                if (isRunning)
                {
                    paused = false;
                }
            }

            public void Pause()
            {
                if (isRunning)
                {
                    paused = true;
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
                paused = false;
            }

            private void PlayAudioThread(string filePath, IProgress<AudioTrack> progress, CancellationToken ct)
            {
                try
                {
                    using (WaveOutEvent wout = new WaveOutEvent())
                    using (AudioFileReader afr = new AudioFileReader(filePath))
                    {
                        wout.Init(afr);
                        wout.Play();
                        wout.Volume = volume;
                        AudioTrack track = new AudioTrack()
                        {
                            currentTime = TimeSpan.Zero,
                            totalTime = afr.TotalTime,
                            progress = 0
                        };

                        while (wout.PlaybackState != PlaybackState.Stopped && !ct.IsCancellationRequested)
                        {
                            if (paused)
                            {
                                wout.Pause();
                                Thread.Sleep(1000);
                            }
                            else if (wout.PlaybackState == PlaybackState.Paused)
                            {
                                wout.Play();
                            }
                            else
                            {
                                wout.Volume = volume;
                                progress.Report(track);
                                track.currentTime = afr.CurrentTime;
                                track.progress = (int)Math.Round
                                    (
                                    (double)(afr.CurrentTime.TotalSeconds / afr.TotalTime.TotalSeconds) * 100d
                                    );
                                Thread.Sleep(200);
                            }
                        }
                        wout.Stop();
                        if (ct.IsCancellationRequested)
                        {
                            progress.Report(AudioTrack.empty);

                            ct.ThrowIfCancellationRequested();
                        }
                        else
                        {
                            progress.Report(track);
                        }
                    }
                }
                catch(Exception e)
                {
                    throw new OperationCanceledException(e.ToString());
                }
            }
        }


    }
}

