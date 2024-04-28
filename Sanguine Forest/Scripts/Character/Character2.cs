using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectWrite;

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


        //Character state
        private enum CharState
        {
            idle,
            walk,
            jump,
            cling,
            wallJump
        }
        private CharState _currentState;

        //Movement
        private float _speed = 10f;
        private float _jumpHigh = 30f;

        private Vector2 _velocity = Vector2.Zero;
        private float _gravityRate = 0.3f;
        private float _gravityEffect = 0;


        public Character2(Vector2 position, float rotation, ContentManager content) : base(position, rotation)
        {

            //Animation and graphic
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Sprites_Character_v1"),
                Extentions.SpriteLayer.character1);

            _spriteModule.SetScale(0.2f);

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
            _feetCollision = new PhysicModule(this, new Vector2(100, 190), new Vector2(50, 20));

            _leftCollision = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
            _rightCollision = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));


            _currentState = CharState.jump;

        }


        public void UpdateMe(KeyboardState prev, KeyboardState curr)
        {

            _animationModule.UpdateMe();
            _spriteModule.UpdateMe();
            //_characterCollision.UpdateMe();
            _feetCollision.UpdateMe();
            _leftCollision.UpdateMe();
            _rightCollision.UpdateMe();

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
            if (prev.IsKeyDown(Keys.W) && curr.IsKeyDown(Keys.W))
            {
                _velocity.Y = -_jumpHigh;                
            }

            //transition to walk
            if (prev.IsKeyDown(Keys.A)||prev.IsKeyDown(Keys.D))
            {
                _currentState = CharState.walk;
            }

            //transition to fall or jump
            if(_velocity.Y!=0)
            {
                _currentState = CharState.jump;
                return;
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
            if (prev.IsKeyDown(Keys.W) && curr.IsKeyDown(Keys.W))
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
            if(curr.IsKeyUp(Keys.W)&& prev.IsKeyDown(Keys.W)) 
            {
                if(_spriteModule.GetSpriteEffects()==SpriteEffects.None)
                {
                    _velocity.X = -_speed;
                    _velocity.Y = -_jumpHigh;
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    _currentState = CharState.jump;
                    return;
                }
                else
                {
                    _velocity.X = _speed;
                    _velocity.Y = -_jumpHigh;
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
                    _currentState = CharState.jump;
                    return;
                }
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

                if (_currentState == CharState.jump && collision.GetThisPhysicModule() == _feetCollision && _velocity.Y > 0)
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
                    position.X += 2;
                    _currentState = CharState.idle;
                    return;
                }
                if (_currentState == CharState.walk && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    position.X -= 2;
                    _currentState = CharState.idle;
                    return;
                }
                if (_currentState == CharState.jump && collision.GetThisPhysicModule() == _leftCollision)
                {
                    _velocity.X = 0;
                    _velocity.Y = 0;
                    position.X = platform.GetPlatformRectangle().Right;
                    _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    _gravityEffect = 0f;
                    _currentState = CharState.cling;

                    return;
                }
                if (_currentState == CharState.jump && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    _velocity.Y = 0;
                    position.X = platform.GetPlatformRectangle().Left-collision.GetThisPhysicModule().GetShiftPosition().X-collision.GetThisPhysicModule().GetPhysicRectangle().Width;  
                    _gravityEffect = 0f;
                    _spriteModule.SetSpriteEffects(SpriteEffects.None);
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
                
            }

        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
            DebugManager.DebugRectangle(_feetCollision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_rightCollision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_leftCollision.GetPhysicRectangle());
        }

        public void SetCharacterScale(float scale)
        {
            _spriteModule.SetScale(scale);
            _leftCollision.SetScale(scale);
            _rightCollision.SetScale(scale);
            _feetCollision.SetScale(scale);
        }



    }
}
