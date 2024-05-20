using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    static class AudioManager
    {
        //private static ListenerModule listener;

        // Songs collection and variables
        private static List<Song> songs = new List<Song>();
        private static Song currentSong;
        private static byte songId;
        private static float songPitch;
        public static float SongPitch
        {
            get => songPitch;
            set { songPitch = Math.Clamp(value, -1, 1); }  // Note: Song pitch range is different from SoundEffect
        }
        private static bool isMusicPlay;

        // Sound effects
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static List<SoundEffectInstance> playingSounds = new List<SoundEffectInstance>();

        // Volumes of sounds and music
        private static float musicVolume = 1.0f;
        public static float MusicVolume
        {
            get => musicVolume;
            set { musicVolume = Math.Clamp(value, 0, 1); }
        }

        private static float generalVolume = 1.0f;
        public static float GeneralVolume
        {
            get => generalVolume;
            set { generalVolume = Math.Clamp(value, 0, 1); }
        }

        private static float environmentVolume = 1.0f;
        public static float EnvironmentVolume
        {
            get => environmentVolume;
            set { environmentVolume = Math.Clamp(value, 0, 1); }
        }

        //// Initialize the audio manager with the listener module
        //public static void Initialize(ListenerModule listenerModule)
        //{
        //    listener = listenerModule;
        //}

        // Load content for audio manager
        public static void LoadContent(Game game)
        {
            //AddSoundEffect("jump", game.Content.Load<SoundEffect>("Sounds/jump"));
            //AddSoundEffect("walk", game.Content.Load<SoundEffect>("Sounds/walk"));
            //AddSoundEffect("death", game.Content.Load<SoundEffect>("Sounds/death"));
            //AddSoundEffect("dialogue", game.Content.Load<SoundEffect>("Sounds/dialogue"));
            //AddSoundEffect("slime", game.Content.Load<SoundEffect>("Sounds/slime"));
            //AddSoundEffect("bossVoice", game.Content.Load<SoundEffect>("Sounds/bossVoice"));
            //AddSoundEffect("bossAttack", game.Content.Load<SoundEffect>("Sounds/bossAttack"));

            AddSong(game.Content.Load<Song>("Sounds/Music/04-Meydan-Surreal-Forest(chosic.com)"));
            AddSong(game.Content.Load<Song>("Sounds/Music/Anonymous420_-_08_-_O_X_L2_X_-_O_R1(chosic.com)"));
            AddSong(game.Content.Load<Song>("Sounds/Music/Lurking-Evil(chosic.com)"));
        }

        // Add songs to the songs list
        public static void AddSong(Song song)
        {
            songs.Add(song);
        }

        // Add sound effects to the sound effects dictionary
        public static void AddSoundEffect(string name, SoundEffect soundEffect)
        {
            soundEffects[name] = soundEffect;
        }

        // Update the audio manager (call this in the game update loop)
        public static void Update(GameTime gameTime)
        {
            // Update music playback
            if (currentSong == null && songs.Count > 0)
            {
                PlaySong(0); // Start with the first song
            }

            if (isMusicPlay && MediaPlayer.State == MediaState.Stopped)
            {
                songId = (byte)((songId + 1) % songs.Count);
                PlaySong(songId);
            }

            MediaPlayer.Volume = MusicVolume;

            // Update positional audio
            //listener?.UpdateSounds(playingSounds, GeneralVolume);
        }

        // Play a song by index
        public static void PlaySong(int index)
        {
            if (index >= 0 && index < songs.Count)
            {
                currentSong = songs[index];
                MediaPlayer.Play(currentSong);
            }
        }

        // Play a sound effect by name
        public static void PlaySoundEffect(string name)
        {
            if (soundEffects.ContainsKey(name))
            {
                var soundInstance = soundEffects[name].CreateInstance();
                soundInstance.Volume = GeneralVolume;
                soundInstance.IsLooped = false;
                soundInstance.Play();
                playingSounds.Add(soundInstance);
            }
        }

        // Play a positional sound effect by name (for slimes and boss)
        public static void PlayPositionalSound(string name, Vector2 soundPosition)
        {
            if (soundEffects.ContainsKey(name))
            {
                var soundInstance = soundEffects[name].CreateInstance();
                //listener?.UpdateSoundPosition(soundInstance, soundPosition, GeneralVolume);
                soundInstance.IsLooped = false;
                soundInstance.Play();
                playingSounds.Add(soundInstance);
            }
        }

        // Control music playback
        public static void MusicTrigger(bool trigger)
        {
            isMusicPlay = trigger;
            if (trigger)
            {
                MediaPlayer.Resume();
            }
            else
            {
                MediaPlayer.Pause();
            }
        }

        // Play a sound effect instance directly
        public static void PlaySound(SoundEffectInstance sound)
        {
            if (sound.State == SoundState.Paused || sound.State == SoundState.Stopped)
            {
                sound.Volume = GeneralVolume;
                sound.IsLooped = false;
                sound.Play();
            }
        }
    }
}
