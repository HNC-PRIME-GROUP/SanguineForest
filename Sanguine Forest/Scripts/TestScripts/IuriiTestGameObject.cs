using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.TestScripts
{
    internal class IuriiTestGameObject : GameObject
    {
        public AudioSourceModule AudioSourceModule;

        public Dictionary<string, SoundEffectInstance> sounds;

        public IuriiTestGameObject(Vector2 position, float rotation, ContentManager content) : base(position, rotation) 
        {
            sounds = new Dictionary<string, SoundEffectInstance>();
            sounds.Add("Run", content.Load<SoundEffect>("Sounds/Sound_Char_Run").CreateInstance());
            sounds.Add("Jump", content.Load<SoundEffect>("Sounds/Sound_Char_Jump").CreateInstance());
            sounds.Add("Climb", content.Load<SoundEffect>("Sounds/Sound_Char_Climb").CreateInstance());

            AudioSourceModule = new AudioSourceModule(this, Vector2.Zero, sounds);
        }


        public void UpdateMe(KeyboardState currKeyboard, KeyboardState oldKeyboard)
        {
            if(currKeyboard.IsKeyDown(Keys.W) )
            {
                AudioSourceModule.PlaySoundOnce("Jump");
            }

            if(currKeyboard.IsKeyDown(Keys.D))
            {
                AudioSourceModule.PlaySoundOnce("Run");
            }

            if( currKeyboard.IsKeyDown(Keys.S))
            {
                AudioSourceModule.PlaySoundOnce("Climb");
            }

        }
    }
}
