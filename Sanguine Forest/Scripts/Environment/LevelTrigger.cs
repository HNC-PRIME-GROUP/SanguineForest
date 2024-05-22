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
        // Visual
        public SpriteModule SpriteModule;
        public AnimationModule AnimationModule;

        // Trigger zone
        public PhysicModule PhysicModule;

        // Trigger state
        public enum TriggerState
        {
            phase1,
            dialogue,
            option,
            phase2,
            triggered
        };

        public TriggerState currentState;

        // Trigger timers
        public event EventHandler LevelEnd;
        public float EndTimer;
        public float MaxTimer = 4;

        private Vector2 startPosition;
        private Vector2 endPosition;

        private SpriteFont spriteFont;
        private Texture2D semiTransparentTexture;



        public LevelTrigger(Vector2 startPosition, Vector2 endPosition, ContentManager content, SpriteEffects spriteEffect, SpriteFont font, Texture2D semiTransparentTexture) : base(startPosition, 0)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.spriteFont = font;
            this.semiTransparentTexture = semiTransparentTexture;

            SpriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Boss"), Extention.Extentions.SpriteLayer.character2);
            Dictionary<string, AnimationSequence> dictionary = new Dictionary<string, AnimationSequence>()
        {
            {"Idle", new AnimationSequence(new Vector2(0,0),3) },
            {"FlyBack", new AnimationSequence(new Vector2(0,512),0) },
            {"FlyForward", new AnimationSequence(new Vector2(0,1024),0)}
        };
            SpriteSheetData spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 512, 512), dictionary);
            AnimationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, SpriteModule);
            currentState = TriggerState.phase1;
            SpriteModule.SetSpriteEffects(spriteEffect);
            SpriteModule.SetScale(0.4f);
            SpriteModule.AnimtaionInitialise(AnimationModule);
            AnimationModule.SetAnimationSpeed(0.5f);
            PhysicModule = new PhysicModule(this, new Vector2(100,50), new Vector2(100, 300));

            EndTimer = MaxTimer;
        }

        public new void UpdateMe()
        {
            SpriteModule.UpdateMe();
            AnimationModule.UpdateMe();
            PhysicModule.UpdateMe();

            switch (currentState)
            {
                case TriggerState.phase1:
                    AnimationModule.Play("Idle");
                    break;
                case TriggerState.dialogue:
                    AnimationModule.Play("Idle");
                    break;
                case TriggerState.option:
                    AnimationModule.Play("Idle");
                    break;
                case TriggerState.phase2:
                    AnimationModule.Play("Idle");
                    position = endPosition;
                    break;
                case TriggerState.triggered:
                    AnimationModule.Play("FlyBack");
                    position.Y -= 10;
                    position.X -= SpriteModule.GetSpriteEffects() == SpriteEffects.None ? 10 : -10;
                    EndTimer -= 1 * Extention.Extentions.globalTime;
                    if (EndTimer <= 0)
                    {
                        LevelEnd?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    break;
            }
        }

        public void DrawMe(SpriteBatch sp, bool showPressE)
        {
            SpriteModule.DrawMe(sp);
            DebugManager.DebugRectangle(PhysicModule.GetPhysicRectangle());

        }
    

        public override void Collided(Collision collision)
        {
            if (currentState == TriggerState.phase2)
            {
                if (collision.GetCollidedPhysicModule().GetParent() is Character2)
                {
                    currentState = TriggerState.triggered;
                }
            }
           
        }

        public void StartDialogue()
        {
            if (currentState == TriggerState.phase1)
            {
                currentState = TriggerState.dialogue;
            }
        }

        public void ShowOptions()
        {
            currentState = TriggerState.option;
        }

        public void MoveToEnd()
        {
            currentState = TriggerState.phase2;
        }



        public void DeleteMe()
        {
            PhysicManager.RemoveObject(PhysicModule);
        }
    }



    public struct LevelTriggerData
    {
        public Vector2 startPosition;
        public Vector2 endPosition;
        public int SpriteEffect;
        public List<string> Dialogue; 
    }

    public struct LevelDialogueData
    {
        public string Text;
        public Vector2 Position;
    }
}
