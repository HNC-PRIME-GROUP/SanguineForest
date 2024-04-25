using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using Sanguine_Forest.Scripts.TestScripts;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using SharpDX.DirectWrite;
using Sanguine_Forest.Scripts.GameState;
using Microsoft.VisualBasic;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using System.Diagnostics.Metrics;

namespace Sanguine_Forest
{
    internal class CharacterStateMachineTemplate : GameObject
    {


        private Vector2 velocity;
        private Vector2 pos;

        //Our character states  (we also can use them for animation later)
        private enum CharState
        {
            idle,
            walk,
            jump,
            cling,
            jumpAfterCling
        }

        private CharState currentState;

        //Modules
        private SpriteModule spriteModule;
        private AnimationModule animationModule;
        private PhysicModule characterCollision;
        private PhysicModule feet;
        private PhysicModule leftCling;
        private PhysicModule rightCling;

        public bool moveL;
        public bool moveR;
        public bool isClinging;

        CharacterStateMachineTemplate(Vector2 position, float rotation, ContentManager content) : base (position, rotation)
        {

            //create the sprite module
            //spriteModule = new SpriteModule();

            //do some things for animation here (dictionary and spritesheetdata)

            //animationModule = new AnimationModule();

            //Initialise the animation 
            spriteModule.AnimtaionInitialise(animationModule);


            //Initialise the physic
            //characterCollision = new PhysicModule();
            //feet = new PhysicModule(); etc.

        }

        public void UpdateMe(InputManager inputManager)
        {
            //update all modules
            spriteModule.UpdateMe();
            animationModule.UpdateMe();
            characterCollision.UpdateMe();
            feet.UpdateMe();
            leftCling.UpdateMe();
            rightCling.UpdateMe();

            //Magic switch - if character in particular state - there is gonna execute only one Update.
            //All these methods described lower for readability
            switch(currentState)
            {
                case CharState.idle:
                    IdleUpdate(inputManager);
                    break;
                case CharState.walk:
                    WalkUpdate(inputManager);
                    break;
                case CharState.jump:
                    JumpUpdate(inputManager);
                    break;
                case CharState.cling:
                    ClingUpdate(inputManager);
                    break;
                case CharState.jumpAfterCling:
                    JumpAfterClingUpdate(inputManager);
                    break;
            }

        }

        public void DrawMe(SpriteBatch sp)
        {
            //spriteModule.DrawMe(sp);
        }


        public void IdleUpdate(InputManager inputManager) 
        {
            //animationModule.Play("Idle");
            //Here you can describe only that things that character can do from the Idle
            //for example if climbing can be done only from the jump, just don't write here any 
            //transition to climb

            //transition to jump
            if(inputManager.IsKeyPressed(Keys.W)) 
            {
                velocity.Y += -6;
                currentState = CharState.jump;
                
            }

            //transitions to walk
            if(inputManager.IsKeyDown(Keys.A))
            {
                //Rotate here the sprite but velocity you can add in a walk state
                currentState = CharState.walk;
            }

            if(inputManager.IsKeyDown(Keys.D))
            {
                //same stuff
            }        
        }

        public void WalkUpdate(InputManager inputManager)
        {
            //ye, some code should be repeated (or put in another method)
            if (inputManager.IsKeyPressed(Keys.W))
            {
                velocity.Y += -6;
                currentState = CharState.jump;
            }
        

        }
        public void JumpUpdate(InputManager inputManager)
        {
            HandleJump(inputManager);

        }

        public void ClingUpdate (InputManager inputManager)
        {
            //ye, some code should be repeated (or put in another method)
            if (inputManager.IsKeyPressed(Keys.W))
            {
                velocity.Y += 0;

                currentState = CharState.cling;
            }

        }

        public void JumpAfterClingUpdate(InputManager inputManager)
        {
            //ye, some code should be repeated (or put in another method)
            HandleJump(inputManager);

        }


        public void ClingState(KeyboardState curr, KeyboardState prev)
        {
            //fall slightly
            
            //or condition to transiton between states to wall jump
        }

        private void HandleJump(InputManager inputManager)
        {
            if (inputManager.IsKeyPressed(Keys.W))
            {
                velocity.Y -= 10; // Consider making this a constant for easier adjustments
                currentState = CharState.jump;
            }
        }


        public override void Collided(Collision collision)
        {
            base.Collided(collision);
            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                platform.GetPlatformRectangle();

                if (collision.GetThisPhysicModule() == feet)
                {
                    if (velocity.Y > 0)
                    {
                        velocity.Y = 0;
                        pos.Y = platform.GetPlatformRectangle().Top - characterCollision.GetPhysicRectangle().Height + 1;
                    }
                }

                if (collision.GetThisPhysicModule() == rightCling)
                {
                    velocity.X = 0;
                    pos.X = platform.GetPlatformRectangle().Left - characterCollision.GetPhysicRectangle().Width - 1;
                    moveR = false;
                    currentState = CharState.cling;
                }
                else if (collision.GetThisPhysicModule() != rightCling)
                {
                    moveR = true;
                }

                if (collision.GetThisPhysicModule() == leftCling)
                {
                    velocity.X = 0;
                    pos.X = platform.GetPlatformRectangle().Right;
                    moveL = false;
                    currentState = CharState.cling;
                }
                else if (collision.GetThisPhysicModule() != leftCling)
                {
                    moveL = true;
                }

                // if we need a physic rectangle of platform here:
            }
            if (collision.GetThisPhysicModule() == characterCollision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
            {

            }
        }

        public float GetVelocity()
        {
            return velocity.X;
        }



    }

}

