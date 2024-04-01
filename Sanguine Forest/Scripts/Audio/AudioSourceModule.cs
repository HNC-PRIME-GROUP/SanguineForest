using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Module that create a sound
    /// </summary>
    internal class AudioSourceModule : Module
    {

        private Dictionary<string, SoundEffect> soundEffects;


        public AudioSourceModule(GameObject parent, Vector2 shift) : base(parent, shift) { }
    }
}
