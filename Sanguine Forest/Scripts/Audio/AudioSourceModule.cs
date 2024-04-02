using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Module that create a sound
    /// </summary>
    internal class AudioSourceModule : Module
    {

        private Dictionary<string, SoundEffectInstance> soundEffects;
        


        /// <summary>
        /// Audio module contain sounds effect dictionary to play
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="shift"></param>
        /// <param name="dictionary"></param>
        public AudioSourceModule(GameObject parent, Vector2 shift, Dictionary<string, SoundEffectInstance> dictionary) : base(parent, shift) 
        {
            soundEffects = dictionary;
            

        }

        public void PlaySoundOnce(string soundName)
        {
            
            AudioManager.PlaySound(soundEffects[soundName]);
        }

    }
}
