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
using Extention;

namespace Sanguine_Forest.Scripts.TestScripts
{
    internal class IuriiTestGameObject : GameObject
    {
        //Sprites and animation
        public SpriteModule _SpriteModule;
        public AnimationModule _AnimationModule;
        private Dictionary<string, AnimationSequence> animations;
        private SpriteSheetData spriteSheetData;


        //Audio
        public AudioSourceModule AudioSourceModule;
        public Dictionary<string, SoundEffectInstance> sounds;

        //Collision
        public PhysicModule _PhysicsModule;

        public IuriiTestGameObject(Vector2 position, float rotation, ContentManager content) : base(position, rotation) 
        {

            //Setting graphic and animation
            _SpriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Sprites_Character_v1"), Extention.Extentions.SpriteLayer.character1);

            animations = new Dictionary<string, AnimationSequence>();
            animations.Add("Idle", new AnimationSequence(Vector2.Zero, 3));
            animations.Add("Run", new AnimationSequence(new Vector2(0, 700), 3));
            animations.Add("Jump", new AnimationSequence(new Vector2(0, 1400), 5));

            spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);

            _AnimationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _SpriteModule);


            //Setting audio Audio
            sounds = new Dictionary<string, SoundEffectInstance>();
            sounds.Add("Run", content.Load<SoundEffect>("Sounds/Sound_Char_Run").CreateInstance());
            sounds.Add("Jump", content.Load<SoundEffect>("Sounds/Sound_Char_Jump").CreateInstance());
            sounds.Add("Climb", content.Load<SoundEffect>("Sounds/Sound_Char_Climb").CreateInstance());
            AudioSourceModule = new AudioSourceModule(this, Vector2.Zero, sounds);

            //Physic
            _PhysicsModule = new PhysicModule(this, new Vector2(50, 50), new Vector2(20, 20));
            _PhysicsModule.isPhysicActive = true;
        }


        public void UpdateMe(KeyboardState currKeyboard, KeyboardState oldKeyboard)
        {
            base.UpdateMe();
            

            if (currKeyboard.GetPressedKeyCount()==0)
            {
                _AnimationModule.SetAnimationSpeed(0.6f);
                _AnimationModule.Play("Idle");
            }

            if (currKeyboard.IsKeyDown(Keys.W) )
            {
                _AnimationModule.SetAnimationSpeed(0.1f);
                AudioSourceModule.PlaySoundOnce("Jump");
                _AnimationModule.PlayOnce("Jump");
                SetPosition(new Vector2(GetPosition().X, GetPosition().Y-1));
            }

            if(currKeyboard.IsKeyDown(Keys.D))
            {
                _AnimationModule.SetAnimationSpeed(0.2f);
                AudioSourceModule.PlaySoundOnce("Run");
                _AnimationModule.Play("Run");
                _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                SetPosition(new Vector2(GetPosition().X-1, GetPosition().Y ));
            }

            if(currKeyboard.IsKeyDown(Keys.A) ) 
            {
                _AnimationModule.SetAnimationSpeed(0.2f);
                AudioSourceModule.PlaySoundOnce("Run");
                _AnimationModule.Play("Run");
                _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                SetPosition(new Vector2(GetPosition().X + 1, GetPosition().Y));
            }

            if( currKeyboard.IsKeyDown(Keys.S))
            {
                AudioSourceModule.PlaySoundOnce("Climb");
                SetPosition(new Vector2(GetPosition().X , GetPosition().Y+1));

            }

            _SpriteModule.UpdateMe();
            AudioSourceModule.UpdateMe();
            _AnimationModule.UpdateMe();
            _PhysicsModule.UpdateMe();

        }

        public new void UpdateMe()
        {
            base.UpdateMe();
            _AnimationModule.SetAnimationSpeed(0.6f);
            _AnimationModule.Play("Idle");
            _SpriteModule.UpdateMe();
            AudioSourceModule.UpdateMe();
            _AnimationModule.UpdateMe();
            _PhysicsModule.UpdateMe();

        }

        public override void Collided(Collision collision)
        {
            base.Collided(collision);
            if(collision.GetCollidedPhysicModule().GetParent() is IuriiTestGameObject)
            {
                IuriiTestGameObject obj = (IuriiTestGameObject)collision.GetCollidedPhysicModule().GetParent();
                obj._SpriteModule.SetColor(Color.Red);                
            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            _SpriteModule.DrawMe(sp,_AnimationModule);
            DebugManager.DebugRectangle(new Rectangle((int)Math.Round(GetPosition().X),(int)Math.Round(GetPosition().Y), 10,10));
            DebugManager.DebugRectangle(new Rectangle((int)Math.Round(_SpriteModule.GetPosition().X), (int)Math.Round(_SpriteModule.GetPosition().Y), 50, 50));
            DebugManager.DebugRectangle(_PhysicsModule.GetPhysicRectangle());
        }
    }
}
