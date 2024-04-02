using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace Sanguine_Forest
{
    /// <summary>
    /// Calculate the distance between listener and all sound sources and change the volume and pan of every sound source according to disctance and their position to listener
    /// </summary>
    static class AudioManager
    {

        private static float maxDistance;
        private static ListenerModule listener;

        //songs collection and variables
        private static List<SoundEffect> songs;
        private static SoundEffectInstance currentSong;
        private static byte songId;
        private static float songPitch;
        public static float SongPitch
        {
            get => songPitch;
            set { songPitch = Math.Clamp(value, 0, 1); }
        }
        public static bool isMusicPlay;

        //Volumes of sounds and musics
        private static float musicVolume;
        public static float MusicVolume
        {
            get => musicVolume;
            set { musicVolume = Math.Clamp(value, 0, 1); }
        }

        private static float generalVolume;
        public static float GeneralVolume
        {
            get => generalVolume;
            set { generalVolume = Math.Clamp(value, 0, 1); }
        }

        private static float environmentVolume;
        public static float EnvironmentVolume
        {
            get => environmentVolume;
            set
            {
                environmentVolume= Math.Clamp(value, 0,1);
            }
        }


        public static void UpdateMe()
        {
            if(currentSong == null)
            {
                currentSong = songs[0].CreateInstance();
                currentSong.IsLooped = false;
                currentSong.Play();
            }

            if(currentSong.State == SoundState.Stopped)
            {
                if (isMusicPlay)
                {
                    if (songId < songs.Count)
                    {
                        songId++;
                        currentSong = songs[songId].CreateInstance();
                        currentSong.IsLooped = false;
                        currentSong.Play();
                    }
                    else
                    {
                        songId = 0;
                        currentSong = songs[songId].CreateInstance();
                        currentSong.IsLooped = false;
                        currentSong.Play();
                    }
                }
            }            
            currentSong.Pitch = SongPitch;
            currentSong.Volume = MusicVolume;
        }


        /// <summary>
        /// Method to call from audio module
        /// </summary>
        /// <param name="sound"></param>
        public static void PlaySound(SoundEffectInstance sound)
        {
            if (sound.State == SoundState.Paused || sound.State == SoundState.Stopped)
            {
                sound.Volume = GeneralVolume;
                sound.IsLooped = false;
                sound.Play();
            }
        }

        /// <summary>
        /// Music controller
        /// </summary>
        /// <param name="trigger">True - play, False - stop</param>
        public static void MusicTrigger(bool trigger)
        {
            isMusicPlay = trigger;
            if (trigger)
            {
                currentSong.Play();
            }
            else
            {
                currentSong.Stop();
            }
        }
    }
}
