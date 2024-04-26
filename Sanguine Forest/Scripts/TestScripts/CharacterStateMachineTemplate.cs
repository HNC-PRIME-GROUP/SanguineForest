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
using System;

namespace Sanguine_Forest
{
    internal class CharacterStateMachine : GameObject
    {
        private SpriteModule _spriteModule;
        private AnimationModule _animationModule;

        private enum CharState
        {
            idle,
            walk,
            jump,
            cling,
            jumpAfterCling
        }

        private CharState _currentState;

        private PhysicModule _characterCollision;
        private PhysicModule _feet;
        private PhysicModule _leftCling;
        private PhysicModule _rightCling;

        public bool moveL;
        public bool moveR;
        public bool isClinging;

        private Vector2 _velocity;
        private Vector2 _position;

        public CharacterStateMachine(Vector2 position, float rotation, Texture2D texture) : base(position, rotation)
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, texture, Extentions.SpriteLayer.character1);
            _spriteModule.SetScale(0.15f);

            var animations = new Dictionary<string, AnimationSequence>
            {
                {"Idle", new AnimationSequence(Vector2.Zero, 3)},
                {"Run", new AnimationSequence(new Vector2(0, 700), 3)},
                {"Jump", new AnimationSequence(new Vector2(0, 1400), 5)},
                {"Cling", new AnimationSequence(new Vector2(4200, 1400), 0)}
            };

            var spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);
            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _spriteModule);
            _spriteModule.AnimtaionInitialise(_animationModule);

            _position = position;
            _characterCollision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
            _feet = new PhysicModule(this, new Vector2(100, 190), new Vector2(140, 20));
            _leftCling = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
            _rightCling = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));

            _currentState = CharState.idle;
            moveL = true;
            moveR = true;
        }

        public void UpdateMe(InputManager inputManager)
        {
            switch (_currentState)
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

            _animationModule.UpdateMe();
            _spriteModule.UpdateMe();
            _characterCollision.UpdateMe();
            _feet.UpdateMe();
            _leftCling.UpdateMe();
            _rightCling.UpdateMe();
        }

        public void IdleUpdate(InputManager inputManager)
        {
            //animationModule.Play("Idle");
            //Here you can describe only that things that character can do from the Idle
            //for example if climbing can be done only from the jump, just don't write here any 
            //transition to climb

            //transition to jump
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _velocity.Y += -6;
                _currentState = CharState.jump;

            }

            //transitions to walk
            if (inputManager.IsKeyDown(Keys.A))
            {
                //Rotate here the sprite but velocity you can add in a walk state
                _currentState = CharState.walk;
            }

            if (inputManager.IsKeyDown(Keys.D))
            {
                //same stuff
            }
        }

        public void WalkUpdate(InputManager inputManager)
        {
            //ye, some code should be repeated (or put in another method)
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _velocity.Y += -6;
                _currentState = CharState.jump;
            }


        }
        public void JumpUpdate(InputManager inputManager)
        {
            HandleJump(inputManager);

        }

        public void ClingUpdate(InputManager inputManager)
        {
            //ye, some code should be repeated (or put in another method)
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _velocity.Y += 0;

                _currentState = CharState.cling;
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
                _velocity.Y -= 10; // Consider making this a constant for easier adjustments
                _currentState = CharState.jump;
            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
        }

        // Include methods like IdleUpdate, WalkUpdate, JumpUpdate, ClingUpdate, JumpAfterClingUpdate here
        // Adapt the transitions and behavior from the original Character's UpdateMe method

        public override void Collided(Collision collision)
        {
            base.Collided(collision);
            // Handle collisions as in the original Character's Collided method
            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                platform.GetPlatformRectangle();

                if (collision.GetThisPhysicModule() == _feet)
                {
                    if (_velocity.Y > 0)
                    {
                        _velocity.Y = 0;
                        position.Y = platform.GetPlatformRectangle().Top - _characterCollision.GetPhysicRectangle().Height + 1;
                    }
                }

                if (collision.GetThisPhysicModule() == _rightCling)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Left - _characterCollision.GetPhysicRectangle().Width - 1;
                    moveR = false;
                    _currentState = CharState.cling;
                }
                else if (collision.GetThisPhysicModule() != _rightCling)
                {
                    moveR = true;
                }

                if (collision.GetThisPhysicModule() == _leftCling)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Right;
                    moveL = false;
                    _currentState = CharState.cling;
                }
                else if (collision.GetThisPhysicModule() != _leftCling)
                {
                    moveL = true;
                }

                // if we need a physic rectangle of platform here:
            }
            if (collision.GetThisPhysicModule() == _characterCollision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
            {

            }
        }
    

        public float GetVelocity()
        {
            return _velocity.X;
        }
    }
}