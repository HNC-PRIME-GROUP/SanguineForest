using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    internal class AudioSourceModule : Module
    {
        private Dictionary<string, SoundEffectInstance> soundEffects;

        public AudioSourceModule(GameObject parent, Vector2 shift, Dictionary<string, SoundEffectInstance> dictionary) : base(parent, shift)
        {
            soundEffects = dictionary;
        }

        public void PlaySoundOnce(string soundName, float volume = 1.0f)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                var soundInstance = soundEffects[soundName];
                soundInstance.Volume = volume;
                AudioManager.PlaySound(soundInstance);
            }
        }

        public void StopSound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                var soundInstance = soundEffects[soundName];
                if (soundInstance.State == SoundState.Playing)
                {
                    soundInstance.Stop();
                }
            }
        }

        public void PlayPositionalSound(string soundName, Vector2 position, float volume = 1.0f)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                var soundInstance = soundEffects[soundName];
                soundInstance.Volume = volume;
                AudioManager.PlayPositionalSound(soundName, position);
            }
        }
    }
}