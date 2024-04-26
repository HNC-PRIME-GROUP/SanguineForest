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
//        private SpriteModule _spriteModule;
//        private AnimationModule _animationModule;

//        private enum CharState
//        {
//            idle,
//            walk,
//            jump,
//            cling,
//            jumpAfterCling
//        }

//        private enum Direction
//        {
//            Right,
//            Left
//        }

//        private CharState _currentState;
//        private Direction _currentDirection;


//        private PhysicModule _characterCollision;
//        private PhysicModule _feet;
//        private PhysicModule _leftCling;
//        private PhysicModule _rightCling;

//        public bool moveLeft;
//        public bool moveRight;
//        public bool isClinging;

//        private float _speed;
//        private Vector2 _velocity;
//        public Vector2 Position;

//        private float _gravity;
//        private int _groundLevel;

//        public Character(Vector2 position, float rotation, Texture2D texture) : base(position, rotation)
//        {
//            _spriteModule = new SpriteModule(this, Vector2.Zero, texture, Extentions.SpriteLayer.character1);
//            _spriteModule.SetScale(0.15f);

//            var animations = new Dictionary<string, AnimationSequence>
//            {
//                {"Idle", new AnimationSequence(Vector2.Zero, 3)},
//                {"Run", new AnimationSequence(new Vector2(0, 700), 3)},
//                {"Jump", new AnimationSequence(new Vector2(0, 1400), 5)},
//                {"Cling", new AnimationSequence(new Vector2(4200, 1400), 0)}
//            };



//            var spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);
//            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _spriteModule);
//            _spriteModule.AnimtaionInitialise(_animationModule);

//            Position = position;
//            _characterCollision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
//            _feet = new PhysicModule(this, new Vector2(100, 190), new Vector2(140, 20));
//            _leftCling = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
//            _rightCling = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));

//            _currentState = CharState.idle;
//            _currentDirection = Direction.Right;

//            moveLeft = true;
//            moveRight = true;

//            _speed = 10f;

//            _velocity = Vector2.Zero;
//            _gravity = 0.3f;
//            _groundLevel = 250;
//        }

//        public void UpdateMe(InputManager inputManager)
//        {
//            switch (_currentState)
//            {
//                case CharState.idle:
//                    IdleUpdate(inputManager);
//                    break;
//                case CharState.walk:
//                    WalkUpdate(inputManager);
//                    break;
//                case CharState.jump:
//                    JumpUpdate(inputManager);
//                    break;
//                case CharState.cling:
//                    ClingUpdate(inputManager);
//                    break;
//                case CharState.jumpAfterCling:
//                    JumpAfterClingUpdate(inputManager);
//                    break;
//            }
//            UpdateMovementPermissions();
//            ApplyPhysics();
//            _animationModule.UpdateMe();
//            _spriteModule.UpdateMe();
//            _characterCollision.UpdateMe();
//            _feet.UpdateMe();
//            _leftCling.UpdateMe();
//            _rightCling.UpdateMe();
//        }

//        public void IdleUpdate(InputManager inputManager)
//        {
//            _animationModule.SetAnimationSpeed(0.6f);
//            _animationModule.Play("Idle");

//            //Here you can describe only that things that character can do from the Idle
//            //for example if climbing can be done only from the jump, just don't write here any 
//            //transition to climb

//            bool isGrounded = _feet.GetPhysicRectangle().Bottom >= _groundLevel;

//            // Transition based on inputs
//            if (inputManager.IsKeyPressed(Keys.W))
//            {
//                _currentState = CharState.jump;

//            }
//            else if (inputManager.IsKeyDown(Keys.A))
//            {
//                _currentDirection = Direction.Left;
//                if (isGrounded) _currentState = CharState.walk;

//            }
//            else if (inputManager.IsKeyDown(Keys.D))
//            {
//                _currentDirection = Direction.Right;
//                if (isGrounded) _currentState = CharState.walk;

//            }

//        }

//        public void WalkUpdate(InputManager inputManager)
//        {
//            _animationModule.SetAnimationSpeed(0.2f);
//            _animationModule.Play("Run");

//            bool isGrounded = _feet.GetPhysicRectangle().Bottom >= _groundLevel;
//            Debug.WriteLine($"FEET RECT:  {_feet.GetPhysicRectangle().Bottom}");
//            Debug.WriteLine($"FEET RECT:  {_groundLevel}");


//            if (inputManager.IsKeyDown(Keys.A) && _currentDirection == Direction.Left)
//            {
//                _velocity.X = -_speed;

//            }
//            else if (inputManager.IsKeyDown(Keys.D) && _currentDirection == Direction.Right)
//            {
//                _velocity.X = _speed;
//            }

//            else if (inputManager.IsKeyPressed(Keys.W) && isGrounded)
//            {
//                _currentState = CharState.jump;
//            }
//            else
//            {
//                if (_currentState == CharState.walk && _feet.GetPhysicRectangle().Bottom >= _groundLevel)
//                {
//                    _velocity.X = 0;
//                }
//                if (isGrounded) _currentState = CharState.idle;
//            }

//            position += _velocity;

//            // Adjust sprite orientation based on direction
//            _spriteModule.SetSpriteEffects(_currentDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
//        }



//        public void JumpUpdate(InputManager inputManager)
//        {
//            bool isGrounded = _feet.GetPhysicRectangle().Bottom >= _groundLevel;

//            _animationModule.SetAnimationSpeed(0.1f);
//            _animationModule.Play("Jump");

//            if (inputManager.IsKeyPressed(Keys.W) && isGrounded)
//            {
//                _velocity.Y = -10; // Upward initial velocity for the jump
//            }

//            if (isClinging)
//            {
//                _currentState = CharState.cling;
//                _currentDirection = _currentDirection == Direction.Right ? Direction.Right : Direction.Left;
//            }
//            else
//            {
//                if (!isGrounded)
//                {
//                    _currentState = CharState.jump; // Ensure we stay in jump state if not grounded
//                }
//                else
//                {
//                    _currentState = CharState.idle; // Only reset to idle if grounded
//                }
//            }

//            position += _velocity;

//            // Adjust sprite orientation based on direction
//            _spriteModule.SetSpriteEffects(_currentDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
//        }




//        public void ClingUpdate(InputManager inputManager)
//        {                
//            _animationModule.Play("Cling");

//            if (_currentDirection == Direction.Left && inputManager.IsKeyPressed(Keys.W) && isClinging && _velocity.Y != 0)
//            {
//                _velocity = new Vector2(-10, -10);
//                isClinging = false;
//                _currentState = CharState.jump;

//            }
//            else if (_currentDirection == Direction.Right && inputManager.IsKeyPressed(Keys.W) && isClinging && _velocity.Y != 0)
//            {
//                _velocity = new Vector2(10, 10);
//                isClinging = false;
//                _currentState = CharState.jump;


//            }

//            else if (isClinging && _velocity.Y == 0);
//            {
//                _currentState = CharState.idle;
//            }

//        }

//        public void JumpAfterClingUpdate(InputManager inputManager)
//        {
//            //ye, some code should be repeated (or put in another method)
//            HandleJump(inputManager);

//        }


//        public void ClingState(InputManager inputManager)
//        {
//            //fall slightly

//            //or condition to transiton between states to wall jump
//        }

//        private void UpdateMovementPermissions()
//        {
//            // Assume movement is possible unless a wall is currently colliding
//            moveLeft = true;
//            moveRight = true;

//            // Logic here to decide based on current collision state or other conditions
//            if (_currentState != CharState.cling)
//            {
//                moveLeft = moveRight = true; // Reset if not currently hugging a wall
//            }
//        }

//        private void ApplyPhysics()
//        {
//            Debug.WriteLine($"Feet bottom: {_feet.GetPhysicRectangle().Bottom}, Ground level: {_groundLevel}");

//                Position += _velocity; // Update the position based on velocity

//            // Check if the character's feet are not touching the ground
//            if (_feet.GetPhysicRectangle().Bottom < _groundLevel)
//            {
//                _velocity.Y += _gravity; // Continue applying gravity
//            }
//            else
//            {
//                // Correct the position if the character has reached or exceeded the ground level
//                Position.Y = _groundLevel - (_characterCollision.GetPhysicRectangle().Height - _feet.GetPhysicRectangle().Height);
//                _velocity.Y = 0; // Stop vertical movement
//            }
//        }


//        private void HandleJump(InputManager inputManager)
//        {
//            if (inputManager.IsKeyPressed(Keys.W))
//            {
//                _velocity.Y -= 10; // Consider making this a constant for easier adjustments
//                _currentState = CharState.jump;
//            }
//        }

//        public void DrawMe(SpriteBatch sp)
//        {
//            _spriteModule.DrawMe(sp);
//            DebugManager.DebugRectangle(_feet.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_characterCollision.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_leftCling.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_rightCling.GetPhysicRectangle());
//            DebugManager.DebugRectangle(new Rectangle(0, _groundLevel, 6000, 100));
//        }

//        // Include methods like IdleUpdate, WalkUpdate, JumpUpdate, ClingUpdate, JumpAfterClingUpdate here
//        // Adapt the transitions and behavior from the original Character's UpdateMe method

//        public override void Collided(Collision collision)
//        {
//            base.Collided(collision);
//            // Handle collisions as in the original Character's Collided method
//            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
//            {
//                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
//                platform.GetPlatformRectangle();

//                if (collision.GetThisPhysicModule() == _feet)
//                {
//                    if (_velocity.Y > 0)
//                    {
//                        _velocity.Y = 0;
//                            Position.Y = platform.GetPlatformRectangle().Top - _characterCollision.GetPhysicRectangle().Height + 1;
//                    }
//                }

//                if (collision.GetThisPhysicModule() == _rightCling)
//                {
//                    _velocity.X = 0;
//                    Position.X = platform.GetPlatformRectangle().Left - _characterCollision.GetPhysicRectangle().Width - 1;
//                    moveRight = false;
//                    _currentState = CharState.cling;
//                    isClinging = true;
//                }
//                else if (collision.GetThisPhysicModule() != _rightCling)
//                {
//                    moveRight = true;
//                    isClinging = false;
//                }

//                if (collision.GetThisPhysicModule() == _leftCling)
//                {
//                    _velocity.X = 0;
//                    Position.X = platform.GetPlatformRectangle().Right;
//                    moveLeft = false;
//                    _currentState = CharState.cling;
//                    isClinging = true;

//                }

//                else if (collision.GetThisPhysicModule() != _leftCling)
//                {
//                    moveLeft = true;
//                    isClinging = false;
//                }

//                // if we need a physic rectangle of platform here:
//            }
//            if (collision.GetThisPhysicModule() == _characterCollision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
//            {

//            }
//        }


//        public float GetVelocity()
//        {
//            return _velocity.X;
//        }
//    }
//}