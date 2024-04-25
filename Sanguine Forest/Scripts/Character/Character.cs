using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using Sanguine_Forest.Scripts.GameState;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sanguine_Forest
{
    /// <summary>
    /// Main character
    /// </summary>
    internal class Character : GameObject
    {
        private SpriteModule _spriteModule;
        private AnimationModule _animationModule;

        private Dictionary<string, AnimationSequence> animations;
        private SpriteSheetData spriteSheetData;

        enum AniState
        {
            walk,
            stand,
            jump,
            hugWall,
            drink
        }
        enum looking
        {
            Right,
            Left
        }

        private AniState _currAni;
        private looking _looking;

        public Vector2 Position;

        //public Rectangle collision;
        private PhysicModule _collision;
        private PhysicModule _feet;

        private PhysicModule _walldetL;
        private PhysicModule _walldetR;

        private float speed;
        public Vector2 vel;

        public bool moveL;
        public bool moveR;

        public Texture2D txr;

        private float gravity;
        private int ground;

        public Character(Vector2 position, float rotation, Texture2D _txr) : base(position, rotation)
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, _txr,
                Extentions.SpriteLayer.character1);

            _spriteModule.SetScale(0.15f);

            animations = new Dictionary<string, AnimationSequence>();
            animations.Add("Idle", new AnimationSequence(Vector2.Zero, 3));
            animations.Add("Run", new AnimationSequence(new Vector2(0, 700), 3));
            animations.Add("Jump", new AnimationSequence(new Vector2(0, 1400), 5));
            animations.Add("hugWall", new AnimationSequence(new Vector2(4200, 1400), 0));

            spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);

            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _spriteModule);
            _spriteModule.AnimtaionInitialise(_animationModule);

            Position = position;

            //Collisions
            _collision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
            _feet = new PhysicModule(this, new Vector2(100, 190), new Vector2(140, 20));

            _walldetL = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
            _walldetR = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));


            _collision.isPhysicActive = true;
            _feet.isPhysicActive = true;
            _walldetL.isPhysicActive = true;
            _walldetR.isPhysicActive = true;

            speed = 10f;
            vel = Vector2.Zero;

            gravity = 0.3f;
            ground = 400;

            _looking = looking.Right;
            _currAni = AniState.stand;

            moveL = true;
            moveR = true;
        }

        public void UpdateMe(InputManager inputManager)
        {

            if (inputManager.IsKeyDown(Keys.D))
            {
                Debug.WriteLine("Key D is pressed.");

                if (moveR == true)
                {
                    vel.X = speed;
                    _looking = looking.Right;
                }

                if (vel.Y != 0)
                {
                    _currAni = AniState.jump;
                }
                else
                    _currAni = AniState.walk;
            }
            else if (inputManager.IsKeyDown(Keys.A))
            {
                Debug.WriteLine("Key A is pressed.");

                if (moveL == true)
                {
                    vel.X = -speed;
                    _looking = looking.Left;
                }

                if (vel.Y != 0)
                    _currAni = AniState.jump;
                else
                    _currAni = AniState.walk;
            }
            else if (_currAni == AniState.jump || _currAni == AniState.walk ||
                _currAni == AniState.hugWall || _currAni == AniState.drink)
            {
                vel.X = 0;
                _currAni = AniState.stand;
            }


            if (inputManager.IsKeyPressed(Keys.W))
            {
                Debug.WriteLine("Key W is pressed.");

                if (vel.Y == 0)
                {
                    vel.Y = -8;
                }
            }
            position += vel;

            //collision.X = (int)pos.X;
            //collision.Y = (int)pos.Y;

            if (_feet.GetPhysicRectangle().Bottom < ground)
            {
                if (vel.Y < gravity * 35)
                {
                    vel.Y += gravity;
                }
            }
            else if (vel.Y > 0)
            {
                vel.Y = 0;
                position.Y = ground - _collision.GetPhysicRectangle().Height - _feet.GetPhysicRectangle().Height * 2;
            }



            //feet.X = collision.X + foot;
            //feet.Y = collision.Y + collision.Height - 2;

            if (_looking == looking.Right)
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.None);
            }
            else if (_looking == looking.Left)
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
            }

            if (_currAni == AniState.stand)
            {
                _animationModule.SetAnimationSpeed(0.6f);
                _animationModule.Play("Idle");
            }
            else if (_currAni == AniState.walk)
            {
                _animationModule.SetAnimationSpeed(0.2f);
                _animationModule.Play("Run");
            }
            else if (_currAni == AniState.jump)
            {
                _animationModule.SetAnimationSpeed(0.1f);
                _animationModule.Play("Jump");
            }
            else if (_currAni == AniState.hugWall)
            {
                _animationModule.SetAnimationSpeed(0.1f);
                _animationModule.Play("hugWall");
            }

            _animationModule.UpdateMe();
            _spriteModule.UpdateMe();

            _collision.UpdateMe();
            _feet.UpdateMe();
            _walldetL.UpdateMe();
            _walldetR.UpdateMe();
        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
            DebugManager.DebugRectangle(_feet.GetPhysicRectangle());
            DebugManager.DebugRectangle(_collision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_walldetL.GetPhysicRectangle());
            DebugManager.DebugRectangle(_walldetR.GetPhysicRectangle());
            DebugManager.DebugRectangle(new Rectangle(0, ground, 6000, 100));

        }

        public override void Collided(Collision collision)
        {
            base.Collided(collision);
            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                platform.GetPlatformRectangle();

                if (collision.GetThisPhysicModule() == _feet)
                {
                    if (vel.Y > 0)
                    {
                        vel.Y = 0;
                        Position.Y = platform.GetPlatformRectangle().Top - _collision.GetPhysicRectangle().Height + 1;
                    }
                }

                if (collision.GetThisPhysicModule() == _walldetR)
                {
                    vel.X = 0;
                    Position.X = platform.GetPlatformRectangle().Left - _collision.GetPhysicRectangle().Width - 1;
                    moveR = false;
                    _currAni = AniState.hugWall;
                }
                else if (collision.GetThisPhysicModule() != _walldetR)
                {
                    moveR = true;
                }

                if (collision.GetThisPhysicModule() == _walldetL)
                {
                    vel.X = 0;
                    Position.X = platform.GetPlatformRectangle().Right;
                    moveL = false;
                    _currAni = AniState.hugWall;
                }
                else if (collision.GetThisPhysicModule() != _walldetL)
                {
                    moveL = true;
                }

                // if we need a physic rectangle of platform here:
            }
            if (collision.GetThisPhysicModule() == _collision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
            {

            }
        }


        public float GetVelocity()
        {
            return vel.X;
        }


    }
}






//using Extention;
//using Microsoft.VisualBasic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Sanguine_Forest.Scripts.Environment.Obstacle;
//using Sanguine_Forest.Scripts.GameState;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.Metrics;

//namespace Sanguine_Forest
//{
//    internal class Character : GameObject
//    {
//        // Enums for state and direction
//        private enum CharState
//        {
//            Idle,
//            Walk,
//            Jump,
//            HugWall
//        }

//        private enum Direction
//        {
//            Right,
//            Left
//        }

//        // Animation and Physics
//        private SpriteModule _spriteModule;
//        private AnimationModule _animationModule;
//        private Dictionary<string, AnimationSequence> _animations;
//        private SpriteSheetData _spriteSheetData;

//        // Physics detection modules
//        private PhysicModule _collision;
//        private PhysicModule _feet;
//        private PhysicModule _wallDetectLeft;
//        private PhysicModule _wallDetectRight;

//        // Character properties
//        public Vector2 Position;
//        private Vector2 _velocity;
//        private CharState _currentState;
//        private Direction _currentDirection;
//        private float _speed;
//        private float _frictionFactor;
//        private float _gravity;
//        private int _groundLevel;

//        private bool _moveLeft;
//        private bool _moveRight;


//        // Constructor
//        public Character(Vector2 position, float rotation, Texture2D texture) : base(position, rotation)
//        {
//            Position = position;
//            _spriteModule = new SpriteModule(this, Vector2.Zero, texture, Extentions.SpriteLayer.character1);
//            _spriteModule.SetScale(0.15f);

//            // Load animations
//            _animations = new Dictionary<string, AnimationSequence>
//            {
//                {"Idle", new AnimationSequence(Vector2.Zero, 3)},
//                {"Run", new AnimationSequence(new Vector2(0, 700), 3)},
//                {"Jump", new AnimationSequence(new Vector2(0, 1400), 5)},
//                {"HugWall", new AnimationSequence(new Vector2(4200, 1400), 0)}
//            };
//            _spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), _animations);
//            _animationModule = new AnimationModule(this, Vector2.Zero, _spriteSheetData, _spriteModule);
//            _spriteModule.AnimtaionInitialise(_animationModule);

//            // Initialize physics modules
//            _collision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
//            _feet = new PhysicModule(this, new Vector2(100, 190), new Vector2(140, 20));
//            _wallDetectLeft = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
//            _wallDetectRight = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));

//            _speed = 10f;
//            _frictionFactor = 0;
//            _gravity = 0.3f;
//            _groundLevel = 200;
//            _currentState = CharState.Idle;
//            _currentDirection = Direction.Right;
//            _moveLeft = true;
//            _moveRight = true;
//        }

//        public void UpdateMe(InputManager inputManager)
//        {
//            UpdateMovementPermissions();
//            UpdateState(inputManager);
//            // Update modules
//            _spriteModule.UpdateMe();
//            _animationModule.UpdateMe();
//            _collision.UpdateMe();
//            _feet.UpdateMe();
//            Debug.WriteLine($"Feet bottom after update: {_feet.GetPhysicRectangle().Bottom}");
//            _wallDetectLeft.UpdateMe();
//            _wallDetectRight.UpdateMe();
//            Debug.WriteLine($"Before physics - Pos: {Position}, Vel: {_velocity}, State: {_currentState}");
//            // Apply physics
//            ApplyPhysics();
//            Debug.WriteLine($"After physics - Pos: {Position}, Vel: {_velocity}, State: {_currentState}");
//            // Update animation based on state
//            UpdateAnimation();
//        }

//        private void UpdateState(InputManager inputManager)
//        {
//            bool isGrounded = _feet.GetPhysicRectangle().Bottom >= _groundLevel;

//            // Transition based on inputs
//            if (inputManager.IsKeyDown(Keys.W) && isGrounded)
//            {
//                _currentState = CharState.Jump;
//                _velocity.Y = -8; // Upward initial velocity for the jump

//            }
//            else if (inputManager.IsKeyDown(Keys.A))
//            {
//                _currentDirection = Direction.Left;
//                _velocity.X = -_speed;
//                if (isGrounded) _currentState = CharState.Walk;

//            }
//            else if (inputManager.IsKeyDown(Keys.D))
//            {
//                _currentDirection = Direction.Right;
//                _velocity.X = _speed;
//                if (isGrounded) _currentState = CharState.Walk;

//            }
//            else
//            {
//                if (_currentState == CharState.Idle && _feet.GetPhysicRectangle().Bottom >= _groundLevel)
//                {
//                    _velocity.X = 0;
//                }
//                if (isGrounded) _currentState = CharState.Idle;
//            }

//            position += _velocity;

//            // Adjust sprite orientation based on direction
//            _spriteModule.SetSpriteEffects(_currentDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
//        }

//        public override void Collided(Collision collision)
//        {
//            base.Collided(collision);
//            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
//            {
//                Debug.WriteLine("Collision with platform detected.");
//                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
//                platform.GetPlatformRectangle();
//                Debug.WriteLine($"Platform Rect: {platform}");

//                if (collision.GetThisPhysicModule() == _feet)
//                {
//                    if (_velocity.Y > 0)
//                    {
//                        _velocity.Y = 0;
//                        Position.Y = platform.GetPlatformRectangle().Top - _collision.GetPhysicRectangle().Height + 1;
//                        _currentState = CharState.Idle; // Assuming character transitions to idle upon landing
//                    }
//                }

//                if (collision.GetThisPhysicModule() == _wallDetectRight)
//                {
//                    _velocity.X = 0;
//                    Position.X = platform.GetPlatformRectangle().Left - _collision.GetPhysicRectangle().Width - 1;
//                    _moveRight = false;
//                    _currentState = CharState.HugWall; // Change state if needed
//                }
//                else if (collision.GetThisPhysicModule() != _wallDetectRight)
//                {
//                    _moveRight = true;
//                }

//                if (collision.GetThisPhysicModule() == _wallDetectLeft)
//                {
//                    _velocity.X = 0;
//                    Position.X = platform.GetPlatformRectangle().Right;
//                    _moveLeft = false;
//                    _currentState = CharState.HugWall; // Change state if needed
//                }
//                else if (collision.GetThisPhysicModule() != _wallDetectLeft)
//                {
//                    _moveLeft = true;
//                }
//            }

//            if (collision.GetThisPhysicModule() == _collision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
//            {
//                // Handle specific obstacle collisions here if needed
//            }
//        }


//        // In your update or tick method, reset movement based on state
//        private void UpdateMovementPermissions()
//        {
//            // Assume movement is possible unless a wall is currently colliding
//            _moveLeft = true;
//            _moveRight = true;

//            // Logic here to decide based on current collision state or other conditions
//            if (_currentState != CharState.HugWall)
//            {
//                _moveLeft = _moveRight = true; // Reset if not currently hugging a wall
//            }
//        }

//        private void ApplyPhysics()
//        {
//            Debug.WriteLine($"Feet bottom: {_feet.GetPhysicRectangle().Bottom}, Ground level: {_groundLevel}");

//            Position += _velocity; // Update the position based on velocity

//            // Check if the character's feet are not touching the ground
//            if (_feet.GetPhysicRectangle().Bottom < _groundLevel)
//            {
//                _velocity.Y += _gravity; // Continue applying gravity
//            }
//            else
//            {
//                // Correct the position if the character has reached or exceeded the ground level
//                Position.Y = _groundLevel - (_collision.GetPhysicRectangle().Height + _feet.GetPhysicRectangle().Height);
//                _velocity.Y = 0; // Stop vertical movement
//            }
//        }

//        private void UpdateAnimation()
//        {
//            switch (_currentState)
//            {
//                case CharState.Idle:
//                    _animationModule.SetAnimationSpeed(0.6f);
//                    _animationModule.Play("Idle");
//                    break;
//                case CharState.Walk:
//                    _animationModule.SetAnimationSpeed(0.2f);
//                    _animationModule.Play("Run");
//                    break;
//                case CharState.Jump:
//                    _animationModule.SetAnimationSpeed(0.1f);
//                    _animationModule.Play("Jump");
//                    break;
//                case CharState.HugWall:
//                    _animationModule.SetAnimationSpeed(0.1f);
//                    _animationModule.Play("HugWall");
//                    break;
//            }
//        }

//        public void DrawMe(SpriteBatch spriteBatch)
//        {
//            _spriteModule.DrawMe(spriteBatch);
//            DebugManager.DebugRectangle(_feet.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_collision.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_wallDetectLeft.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_wallDetectRight.GetPhysicRectangle());
//            DebugManager.DebugRectangle(new Rectangle(0, _groundLevel, 6000, 100));
//            DebugManager.DebugRectangle(new Rectangle(800, 0, 100, 100));

//        }

//        public float GetVelocity()
//        {
//            return _velocity.X;
//        }
//    }
//}
