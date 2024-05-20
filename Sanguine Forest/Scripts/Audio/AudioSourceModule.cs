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

        public void PlaySoundOnce(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                AudioManager.PlaySound(soundEffects[soundName]);
            }
        }

        public void PlayPositionalSound(string soundName, Vector2 position)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                AudioManager.PlayPositionalSound(soundName, position);
            }
        }
    }
}