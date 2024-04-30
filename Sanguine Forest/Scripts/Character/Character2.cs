﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class Character2 : GameObject
    {

        //graphic
        private SpriteModule _spriteModule;
        private AnimationModule _animationModule;
        private Dictionary<string, AnimationSequence> _animations;
        private SpriteSheetData _spriteSheetData;

        //Collisions
        private PhysicModule _characterCollision;
        private PhysicModule _feetCollision;
        private PhysicModule _leftCollision;
        private PhysicModule _rightCollision;
        private PhysicModule _headCollision;


        //Character state
        private enum CharState
        {
            idle,
            walk,
            jump,
            cling,
            wallJump,
            falling
        }
        private CharState _currentState;

        //Movement
        private float _speed = 10f;
        private float _jumpHigh = 30f;

        private Vector2 _velocity = Vector2.Zero;
        private float _gravityRate = 0.3f;
        private float _gravityEffect = 0;

        //work around for cling update
        private Rectangle currClingRectangle;


        public Character2(Vector2 position, float rotation, ContentManager content) : base(position, rotation)
        {

            //Animation and graphic
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Sprites_Character_v1"),
                Extentions.SpriteLayer.character1);

            _spriteModule.SetScale(0.15f);

            _animations = new Dictionary<string, AnimationSequence>();
            _animations.Add("Idle", new AnimationSequence(Vector2.Zero, 3));
            _animations.Add("Run", new AnimationSequence(new Vector2(0, 700), 3));
            _animations.Add("Jump", new AnimationSequence(new Vector2(0, 1400), 5));
            _animations.Add("hugWall", new AnimationSequence(new Vector2(4200, 1400), 0));

            _spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), _animations);

            _animationModule = new AnimationModule(this, Vector2.Zero, _spriteSheetData, _spriteModule);
            _spriteModule.AnimtaionInitialise(_animationModule);

            //Collisions
           // _characterCollision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
            _feetCollision = new PhysicModule(this, new Vector2(50, 95), new Vector2(35, 10));
            _headCollision = new PhysicModule(this, new Vector2(50, 0), new Vector2(35, 10));

            _leftCollision = new PhysicModule(this, new Vector2(30, 50), new Vector2(10, 60));
            _rightCollision = new PhysicModule(this, new Vector2(70, 50), new Vector2(10, 60));


            _currentState = CharState.jump;

            _spriteModule.SetColor(Color.Yellow);

        }


        public void UpdateMe(KeyboardState prev, KeyboardState curr)
        {

            _animationModule.UpdateMe();
            _spriteModule.UpdateMe();
            //_characterCollision.UpdateMe();
            _feetCollision.UpdateMe();
            _leftCollision.UpdateMe();
            _rightCollision.UpdateMe();
            _headCollision.UpdateMe();

            //Made separated _gravity effect to make possible stop it
            if (_gravityEffect < 1)
            {
                _gravityEffect += _gravityRate;
            }

            switch (_currentState)
            {
                case CharState.idle:
                    IdleUpdate(prev, curr);
                    break;
                case CharState.walk:
                    WalkUpdate(prev, curr);
                    break;
                case CharState.jump:
                    JumpUpdate(prev, curr);
                    break;
                case CharState.cling:
                    ClingUpdate(prev, curr);
                    break;
                case CharState.wallJump:
                    WallJumpUpdate(prev, curr);
                    break;
                case CharState.falling:
                    FallingUpdate(prev, curr);
                    break;
            }


            _velocity.Y += _gravityEffect;
            position += _velocity;



        }

        #region State updates

        public void IdleUpdate(KeyboardState prev, KeyboardState curr)
        {
            _animationModule.SetAnimationSpeed(0.5f);
            _animationModule.Play("Idle");

            //transition to jump
            if (curr.IsKeyDown(Keys.W))
            {
                _velocity.Y = -_jumpHigh;                
            }

            //transition to walk
            if (prev.IsKeyDown(Keys.A)||prev.IsKeyDown(Keys.D))
            {
                _currentState = CharState.walk;
            }

            //transition to fall or jump
            if(_velocity.Y<0)
            {
                _currentState = CharState.jump;
                return;
            }
            if(_velocity.Y>0)
            {
                _currentState = CharState.wallJump;
            }

            if(curr.GetPressedKeys().Length==0)
            {
                _velocity.X = 0;
            }
            //_gravityEffect = 0f;
            //_velocity.Y = 0f;
        }

        public void WalkUpdate(KeyboardState prev, KeyboardState curr)
        {
            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.Play("Run");
            if (curr.IsKeyDown(Keys.W))
            {
                _velocity.Y = -_jumpHigh;
                _currentState = CharState.jump;
                return;
                
            }

            if(curr.GetPressedKeys().Length==0)
            {
                _velocity.X = 0;
                _currentState = CharState.idle;
                return;
            }
            if (prev.IsKeyDown(Keys.A))
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                _velocity.X = -_speed;
            }
            if (prev.IsKeyDown(Keys.D))
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.None);
                _velocity.X = _speed;
            }

            //transition to fall or jump
            //transition to fall or jump
            if (_velocity.Y < 0)
            {
                _currentState = CharState.jump;
                return;
            }
            if (_velocity.Y > 0)
            {
                _currentState = CharState.wallJump;
            }
            // _gravityEffect = 0f;
            //_velocity.Y = 0f;
        }

        public void JumpUpdate(KeyboardState prev, KeyboardState curr)
        {
            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.PlayOnce("Jump");
            if (curr.IsKeyDown(Keys.A))
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                _velocity.X = -(_speed*2/3);
            }
            if (curr.IsKeyDown(Keys.D))
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.None);
                _velocity.X = (_speed*2/3);
            }
            
        }

        public void ClingUpdate(KeyboardState prev, KeyboardState curr)
        {

            _animationModule.Play("hugWall");
            _gravityEffect = 0; //Make slower gravity effect
            _velocity.X = 0;
            _velocity.Y = _gravityRate;
            if(curr.IsKeyDown(Keys.W)&& prev.IsKeyUp(Keys.W)) 
            {
                if(_spriteModule.GetSpriteEffects()==SpriteEffects.None)
                {
                    _velocity.X = -_speed;
                    _velocity.Y = -_jumpHigh;
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    _currentState = CharState.wallJump;
                    return;
                }
                else
                {
                    _velocity.X = _speed;
                    _velocity.Y = -_jumpHigh;
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
                    _currentState = CharState.wallJump;
                    return;
                }
            }
            if(_rightCollision.physicRec.Y>currClingRectangle.Y+currClingRectangle.Height) 
            {
                _currentState = CharState.falling;
            }
        }

        public void WallJumpUpdate(KeyboardState prev, KeyboardState curr)
        {
            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.PlayOnce("Jump");
            if (_velocity.Y > 0)
            {
                if (curr.IsKeyDown(Keys.A))
                {
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    _velocity.X = -(_speed * 2 / 3);
                }
                if (curr.IsKeyDown(Keys.D))
                {
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
                    _velocity.X = (_speed * 2 / 3);
                }
            }
        }

        public void FallingUpdate(KeyboardState prev, KeyboardState curr)
        {
            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.PlayOnce("Jump");
            if (curr.IsKeyDown(Keys.A))
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                _velocity.X = -(_speed * 1 / 5);
            }
            if (curr.IsKeyDown(Keys.D))
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.None);
                _velocity.X = (_speed * 1 / 5);
            }
        }

        #endregion



        public override void Collided (Collision collision)
        {
            base.Collided (collision);

            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                if ((_currentState == CharState.idle || _currentState == CharState.walk) && collision.GetThisPhysicModule() == _feetCollision)
                {
                    _gravityEffect = 0f;
                    _velocity.Y = 0f;
                    position.Y = collision.GetCollidedPhysicModule().GetPhysicRectangle().Top - _feetCollision.GetShiftPosition().Y;
                    return;
                }

                if ((_currentState == CharState.jump|| _currentState==CharState.wallJump || _currentState==CharState.falling) && collision.GetThisPhysicModule() == _feetCollision && _velocity.Y > 0)
                {
                    _gravityEffect = 0f;
                    _velocity.Y = 0f;
                    position.Y = collision.GetCollidedPhysicModule().GetPhysicRectangle().Top - _feetCollision.GetShiftPosition().Y;
                    _currentState = CharState.idle;
                    return;
                }
                if (_currentState == CharState.walk && collision.GetThisPhysicModule() == _leftCollision)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Right - collision.GetThisPhysicModule().GetShiftPosition().X + collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    return;
                }
                if (_currentState == CharState.walk && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Left - collision.GetThisPhysicModule().GetShiftPosition().X - collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
                    return;
                }
                if ((_currentState == CharState.jump || _currentState == CharState.wallJump) && collision.GetThisPhysicModule() == _leftCollision)
                {
                    _velocity.X = 0;
                    _velocity.Y = 0;
                    position.X = platform.GetPlatformRectangle().Right-collision.GetThisPhysicModule().GetShiftPosition().X+collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    currClingRectangle = platform.GetPlatformRectangle();
                    _gravityEffect = 0f;
                    _currentState = CharState.cling;

                    return;
                }
                if ((_currentState == CharState.jump || _currentState == CharState.wallJump) && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    _velocity.Y = 0;
                    position.X = platform.GetPlatformRectangle().Left-collision.GetThisPhysicModule().GetShiftPosition().X-collision.GetThisPhysicModule().GetPhysicRectangle().Width;  
                    _gravityEffect = 0f;
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
                    currClingRectangle = platform.GetPlatformRectangle();
                    _currentState = CharState.cling;
                    return;
                }
                if (_currentState==CharState.cling&&collision.GetThisPhysicModule()==_feetCollision)
                {
                    _currentState = CharState.idle;
                    if(_spriteModule.GetSpriteEffects() == SpriteEffects.None)
                    {
                        position.X -= 1;
                    }
                    else
                    {
                        position.X += 1;
                    }
                    _gravityEffect = 0;
                    return;
                } 
                if(_currentState==CharState.falling&&collision.GetThisPhysicModule() == _leftCollision)
                {
                    _velocity.X = 0;                    
                    position.X = platform.GetPlatformRectangle().Right - collision.GetThisPhysicModule().GetShiftPosition().X + collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    return;
                }
                if(_currentState==CharState.falling&&collision.GetThisPhysicModule()==_rightCollision) 
                {
                    _velocity.X = 0;                    
                    position.X = platform.GetPlatformRectangle().Left - collision.GetThisPhysicModule().GetShiftPosition().X - collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
                    return;
                }
                if(collision.GetThisPhysicModule()==_headCollision)
                {
                    _velocity.Y = 0;
                    _velocity.X = 0;
                    position.Y = platform.GetPlatformRectangle().Bottom+_headCollision.GetShiftPosition().Y+10;
                    
                   // _currentState = CharState.falling; 
                    return;
                }    
              
            }

        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
            DebugManager.DebugRectangle(_feetCollision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_rightCollision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_leftCollision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_headCollision.GetPhysicRectangle());
        }


        //try to change scale of cahracter - failed
        //public void SetCharacterScale(float scale)
        //{
        //    _spriteModule.SetScale(scale);
        //    _leftCollision.SetScale(scale);
        //    _rightCollision.SetScale(scale);
        //    _feetCollision.SetScale(scale);
        //}

        public float GetVelocity()
        {
            return _velocity.X;
        }



    }
}
