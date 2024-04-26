//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Extention;
//using Sanguine_Forest.Scripts.TestScripts;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework.Content;
//using SharpDX.DirectWrite;

//namespace Sanguine_Forest
//{
//    internal class CharacterStateMachineTemplate : GameObject
//    {


//        private Vector2 velocity;

//        //Our character states  (we also can use them for animation later)
//        private enum CharState
//        {
//            idle,
//            walk,
//            jump,
//            climb,
//            jumpAfterClimb
//        }

//        private CharState currentState;
 
//        //Modules
//        private SpriteModule spriteModule;
//        private AnimationModule animationModule;

//        private PhysicModule characterCollision;
//        private PhysicModule feet;
//        private PhysicModule leftClimb;
//        private PhysicModule rightClimb;

//        CharacterStateMachineTemplate (Vector2 position, float rotation, ContentManager content) : base (position, rotation)
//        {

//            //create the sprite module
//            //spriteModule = new SpriteModule();

//            //do some things for animation here (dictionary and spritesheetdata)

//            //animationModule = new AnimationModule();
//            //Initialise the animation 
//            spriteModule.AnimtaionInitialise(animationModule);


//            //Initialise the physic
//            //characterCollision = new PhysicModule();
//            //feet = new PhysicModule(); etc.

//        }

//        public void UpdateMe(KeyboardState curr, KeyboardState prev)
//        {
//            //update all modules
//            spriteModule.UpdateMe();
//            animationModule.UpdateMe();
//            characterCollision.UpdateMe();
//            feet.UpdateMe();
//            leftClimb.UpdateMe();
//            rightClimb.UpdateMe();

//            //Magic switch - if character in particular state - there is gonna execute only one Update.
//            //All these methods described lower for readability
//            switch(currentState)
//            {
//                case CharState.idle:
//                    IdleUpdate(curr, prev);
//                    break;
//                case CharState.walk:
//                    WalkUpdate(curr, prev);
//                    break;
//                case CharState.jump:
//                    JumpUpdate(curr, prev);
//                    break;
//                case CharState.climb:
//                    ClimbUpdate(curr, prev);
//                    break;
//                case CharState.jumpAfterClimb:
//                    JumpAfterClimbUpdate(curr, prev);
//                    break;
//            }

//        }

//        public void DrawMe(SpriteBatch sp)
//        {
//            spriteModule.DrawMe(sp);
//        }


//        public void IdleUpdate(KeyboardState curr, KeyboardState prev) 
//        {
//            //animationModule.Play("Idle");
//            //Here you can describe only that things that character can do from the Idle
//            //for example if climbing can be done only from the jump, just don't write here any 
//            //transition to climb

//            //transition to jump
//            if(curr.IsKeyDown(Keys.W)&&prev.IsKeyDown(Keys.W)) 
//            {
//                velocity.Y += -6;
//                currentState = CharState.jump;
                
//            }

//            //transitions to walk
//            if(curr.IsKeyDown(Keys.A))
//            {
//                //flip sprite here but velocity you can add in a walk state
//                currentState = CharState.walk;
//            }

//            if(curr.IsKeyDown(Keys.D))
//            {
//                //same stuff
//            }        
//        }

//        public void WalkUpdate(KeyboardState curr, KeyboardState prev)
//        {
//            //ye, some code should be repeated (or put in another method)
//            if (curr.IsKeyDown(Keys.W) && prev.IsKeyDown(Keys.W))
//            {
//                velocity.Y += -6;
//                currentState = CharState.jump;
//            }

            

//        }


//        public void ClimpState(KeyboardState curr, KeyboardState prev)
//        {
//            //fall slightly
            
//            //or condition to transiton between states to wall jump
//        }

//        public override void Collided(Collision collision)
//        {
//            if(collision.GetCollidedPhysicModule().GetParent() is Platform)
//            {
//                if(collision.GetThisPhysicModule() == feet && currentState==CharState.jump)
//                {
//                    // check if it's riht side of the platform
//                    currentState = CharState.idle;
//                }

//            }



//        }
















//    }
//}
