using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class LevelTrigger : GameObject
    {
        //visual 
        public SpriteModule SpriteModule;
        public AnimationModule AnimationModule;

        //Trigger zone
        public PhysicModule PhysicModule;

        //trigger state
        public enum TriggerState
        {
            wait,
            triggered
        };

        public TriggerState currentState;

        //trigger timers

        public event EventHandler LevelEnd;
        public float EndTimer;
        public float MaxTimer=4;



        public LevelTrigger(Vector2 position, ContentManager content, SpriteEffects spriteEffect) : base(position, 0)
        {

            SpriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Boss"), Extention.Extentions.SpriteLayer.character2);
            Dictionary<string, AnimationSequence> dictionary = new Dictionary<string, AnimationSequence>()
            {
                {"Idle", new AnimationSequence(new Vector2(0,0),3) },
                {"FlyBack", new AnimationSequence(new Vector2(0,512),0) },
                {"FlyForward", new AnimationSequence(new Vector2(0,1024),0)}
            };
            SpriteSheetData spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 512, 512), dictionary);
            AnimationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, SpriteModule);
            currentState= TriggerState.wait;
            SpriteModule.SetSpriteEffects(spriteEffect);
            SpriteModule.SetScale(0.4f);
            SpriteModule.AnimtaionInitialise(AnimationModule);
            AnimationModule.SetAnimationSpeed(0.5f);
            PhysicModule = new PhysicModule(this, Vector2.Zero, new Vector2(100, 100));

            EndTimer = MaxTimer;
        }

        public new void UpdateMe()
        {
            SpriteModule.UpdateMe();
            AnimationModule.UpdateMe();
            PhysicModule.UpdateMe();

            switch (currentState)
            {
                case TriggerState.wait:
                    AnimationModule.Play("Idle");
                    break;
                case TriggerState.triggered:
                    AnimationModule.Play("FlyBack");
                    position.Y -= 10;
                    position.X -= SpriteModule.GetSpriteEffects() == SpriteEffects.None ? 10 : -10;
                    EndTimer -= 1 * Extention.Extentions.globalTime;
                    if(EndTimer<=0)
                    {
                        LevelEnd?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    break;
            }
            

        }


        public void DrawMe(SpriteBatch sp)
        {
            SpriteModule.DrawMe(sp);
        }

        public override void Collided(Collision collision)
        {
            if(collision.GetCollidedPhysicModule().GetParent() is Character2)
            {
                currentState = TriggerState.triggered;
            }

        }

        public void DeleteMe()
        {
            PhysicManager.RemoveObject(PhysicModule);
        }
    }

    public struct LevelTriggerData
    {
        public Vector2 position;
        public int SpriteEffect;

    }
}
