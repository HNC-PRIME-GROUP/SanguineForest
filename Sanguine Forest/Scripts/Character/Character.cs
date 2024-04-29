//using Extention;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Sanguine_Forest.Scripts.Environment.Obstacle;
//using Sanguine_Forest.Scripts.GameState;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;

//namespace Sanguine_Forest
//{
//    /// <summary>
//    /// Main character
//    /// </summary>
//    internal class Character : GameObject
//    {
//        private SpriteModule _spriteModule;
//        private AnimationModule _animationModule;

//        private Dictionary<string, AnimationSequence> animations;
//        private SpriteSheetData spriteSheetData;

//        enum AniState
//        {
//            walk,
//            stand,
//            jump,
//            hugWall,
//            drink
//        }
//        enum looking
//        {
//            Right,
//            Left
//        }

//        private AniState _currAni;
//        private looking _looking;

//        public Vector2 Position;

//        //public Rectangle collision;
//        private PhysicModule _collision;
//        private PhysicModule _feet;

//        private PhysicModule _walldetL;
//        private PhysicModule _walldetR;

//        private float speed;
//        public Vector2 vel;

//        public bool moveL;
//        public bool moveR;

//        public Texture2D txr;

//        private float gravity;
//        private int ground;

//        public Character(Vector2 position, float rotation, Texture2D _txr) : base(position, rotation)
//        {
//            _spriteModule = new SpriteModule(this, Vector2.Zero, _txr,
//                Extentions.SpriteLayer.character1);

//            _spriteModule.SetScale(0.15f);

//            animations = new Dictionary<string, AnimationSequence>();
//            animations.Add("Idle", new AnimationSequence(Vector2.Zero, 3));
//            animations.Add("Run", new AnimationSequence(new Vector2(0, 700), 3));
//            animations.Add("Jump", new AnimationSequence(new Vector2(0, 1400), 5));
//            animations.Add("hugWall", new AnimationSequence(new Vector2(4200, 1400), 0));

//            spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);

//            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _spriteModule);
//            _spriteModule.AnimtaionInitialise(_animationModule);

//            Position = position;

//            //Collisions
//            _collision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
//            _feet = new PhysicModule(this, new Vector2(100, 190), new Vector2(140, 20));

//            _walldetL = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
//            _walldetR = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));


//            _collision.isPhysicActive = true;
//            _feet.isPhysicActive = true;
//            _walldetL.isPhysicActive = true;
//            _walldetR.isPhysicActive = true;

//            speed = 10f;
//            vel = Vector2.Zero;

//            gravity = 0.3f;
//            ground = 400;

//            _looking = looking.Right;
//            _currAni = AniState.stand;

//            moveL = true;
//            moveR = true;
//        }

//        public void UpdateMe(InputManager inputManager)
//        {

//            if (inputManager.IsKeyDown(Keys.D))
//            {
//                Debug.WriteLine("Key D is pressed.");

//                if (moveR == true)
//                {
//                    vel.X = speed;
//                    _looking = looking.Right;
//                }

//                if (vel.Y != 0)
//                {
//                    _currAni = AniState.jump;
//                }
//                else
//                    _currAni = AniState.walk;
//            }
//            else if (inputManager.IsKeyDown(Keys.A))
//            {
//                Debug.WriteLine("Key A is pressed.");

//                if (moveL == true)
//                {
//                    vel.X = -speed;
//                    _looking = looking.Left;
//                }

//                if (vel.Y != 0)
//                    _currAni = AniState.jump;
//                else
//                    _currAni = AniState.walk;
//            }
//            else if (_currAni == AniState.jump || _currAni == AniState.walk ||
//                _currAni == AniState.hugWall || _currAni == AniState.drink)
//            {
//                vel.X = 0;
//                _currAni = AniState.stand;
//            }


//            if (inputManager.IsKeyPressed(Keys.W))
//            {
//                Debug.WriteLine("Key W is pressed.");

//                if (vel.Y == 0)
//                {
//                    vel.Y = -8;
//                }
//            }
//            position += vel;

//            //collision.X = (int)pos.X;
//            //collision.Y = (int)pos.Y;

//            if (_feet.GetPhysicRectangle().Bottom < ground)
//            {
//                if (vel.Y < gravity * 35)
//                {
//                    vel.Y += gravity;
//                }
//            }
//            else if (vel.Y > 0)
//            {
//                vel.Y = 0;
//                position.Y = ground - _collision.GetPhysicRectangle().Height - _feet.GetPhysicRectangle().Height * 2;
//            }

//            //feet.X = collision.X + foot;
//            //feet.Y = collision.Y + collision.Height - 2;

//            if (_looking == looking.Right)
//            {
//                _spriteModule.SetSpriteEffects(SpriteEffects.None);
//            }
//            else if (_looking == looking.Left)
//            {
//                _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
//            }

//            if (_currAni == AniState.stand)
//            {
//                _animationModule.SetAnimationSpeed(0.6f);
//                _animationModule.Play("Idle");
//            }
//            else if (_currAni == AniState.walk)
//            {
//                _animationModule.SetAnimationSpeed(0.2f);
//                _animationModule.Play("Run");
//            }
//            else if (_currAni == AniState.jump)
//            {
//                _animationModule.SetAnimationSpeed(0.1f);
//                _animationModule.Play("Jump");
//            }
//            else if (_currAni == AniState.hugWall)
//            {
//                _animationModule.SetAnimationSpeed(0.1f);
//                _animationModule.Play("hugWall");
//            }

//            _animationModule.UpdateMe();
//            _spriteModule.UpdateMe();

//            _collision.UpdateMe();
//            _feet.UpdateMe();
//            _walldetL.UpdateMe();
//            _walldetR.UpdateMe();
//        }

//        public void DrawMe(SpriteBatch sp)
//        {
//            _spriteModule.DrawMe(sp);
//            DebugManager.DebugRectangle(_feet.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_collision.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_walldetL.GetPhysicRectangle());
//            DebugManager.DebugRectangle(_walldetR.GetPhysicRectangle());
//            DebugManager.DebugRectangle(new Rectangle(0, ground, 6000, 100));

//        }

//        public override void Collided(Collision collision)
//        {
//            base.Collided(collision);
//            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
//            {
//                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
//                platform.GetPlatformRectangle();

//                if (collision.GetThisPhysicModule() == _feet)
//                {
//                    if (vel.Y > 0)
//                    {
//                        vel.Y = 0;
//                        Position.Y = platform.GetPlatformRectangle().Top - _collision.GetPhysicRectangle().Height + 1;
//                    }
//                }

//                if (collision.GetThisPhysicModule() == _walldetR)
//                {
//                    vel.X = 0;
//                    Position.X = platform.GetPlatformRectangle().Left - _collision.GetPhysicRectangle().Width - 1;
//                    moveR = false;
//                    _currAni = AniState.hugWall;
//                }
//                else if (collision.GetThisPhysicModule() != _walldetR)
//                {
//                    moveR = true;
//                }

//                if (collision.GetThisPhysicModule() == _walldetL)
//                {
//                    vel.X = 0;
//                    Position.X = platform.GetPlatformRectangle().Right;
//                    moveL = false;
//                    _currAni = AniState.hugWall;
//                }
//                else if (collision.GetThisPhysicModule() != _walldetL)
//                {
//                    moveL = true;
//                }

//                // if we need a physic rectangle of platform here:
//            }
//            if (collision.GetThisPhysicModule() == _collision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
//            {

//            }
//        }


//        public float GetVelocity()
//        {
//            return vel.X;
//        }


//    }
//}






using Extention;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest;
using Sanguine_Forest.Scripts.GameState;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Sanguine_Forest
{
    internal class Character : GameObject
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

        private enum Direction
        {
            Right,
            Left
        }

        private CharState _currentState;
        private Direction _currentDirection;


        private PhysicModule _characterCollision;
        private PhysicModule _feet;
        private PhysicModule _leftCling;
        private PhysicModule _rightCling;

        public bool moveLeft;
        public bool moveRight;
        public bool isClinging;
        public bool isGrounded;

        private float _speed;
        private float _jumpSpeed;

        private Vector2 _velocity;
        public Vector2 Position;
        public Texture2D Texture;

        private float _gravity;
        private int _groundLevel;

        private float characterWidth;
        private float characterHeight;

        private float feetWidth;
        private float feetHeight;

        private float leftClingX;
        private float rightClingX;




        public Character(Vector2 position, float rotation, Texture2D texture) : base(position, rotation)
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



            float scale = _spriteModule.GetScale();
            // Adjust collision rectangles based on texture dimensions and scale
            feetHeight = 10 * scale; // Height of feet rectangle
            feetWidth = _animationModule.GetFrameRectangle().Width * scale * 1f; // Adjust as needed

            characterWidth = _animationModule.GetFrameRectangle().Width * scale * 0.8f; // Width of character
            characterHeight = _animationModule.GetFrameRectangle().Height * scale;

            leftClingX = (characterWidth / 2) - 5; // Adjust as needed
            rightClingX = (characterWidth / 2) + 25; // Adjust as needed

            // Collisions
            _characterCollision = new PhysicModule(this, new Vector2((characterWidth) / 2, (characterHeight) / 2), new Vector2(characterWidth, characterHeight));
            _feet = new PhysicModule(this, new Vector2((characterWidth) / 2, characterHeight - 6), new Vector2(feetWidth, feetHeight));
            _leftCling = new PhysicModule(this, new Vector2(leftClingX, ((characterHeight) / 2)), new Vector2(10, characterHeight - 10));
            _rightCling = new PhysicModule(this, new Vector2(rightClingX, ((characterHeight) / 2)), new Vector2(10, characterHeight - 10));

            _characterCollision.isPhysicActive = true;
            _leftCling.isPhysicActive = true;
            _rightCling.isPhysicActive = true;
            _feet.isPhysicActive = true;


            Debug.WriteLine($"Character Width {characterWidth}");
            Debug.WriteLine($"Character Height {characterHeight}");
            Debug.WriteLine($"feet Hight {feetHeight}");
            Debug.WriteLine($"feet Hight {feetWidth}");


            Position = position;

            _currentState = CharState.idle;
            _currentDirection = Direction.Right;

            moveLeft = true;
            moveRight = true;

            _speed = 10f;
            _jumpSpeed = -10f;

            _velocity = Vector2.Zero;
            _gravity = 0.3f;
            _groundLevel = 100;

            isClinging = false;

        }

        public void UpdateMe(InputManager inputManager)
        {
            // Handling input for jumping outside the state checks to ensure it's captured consistently
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _velocity.Y = -10f; // Set jump velocity
                _currentState = CharState.jump;d
            }

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
  //              case CharState.drink:
   //                 drinkUpdate(inputManager);
     //               break;
            }

            UpdateMovementPermissions();
            ApplyPhysics();
            _animationModule.UpdateMe();
            _spriteModule.UpdateMe();
            _characterCollision.UpdateMe();
            _feet.UpdateMe();
            _leftCling.UpdateMe();
            _rightCling.UpdateMe();


            //Debug.WriteLine($"Character Width {_characterCollision.GetPhysicRectangle().Width}");
            //Debug.WriteLine($"Texture Height {_characterCollision.GetPhysicRectangle().Height}");
            //Debug.WriteLine($"Feet Height {_feet.GetPhysicRectangle().Height}");
            //Debug.WriteLine($"Feet Width  {_feet.GetPhysicRectangle().Width}");

            //Debug.WriteLine($"FEET Bottom:  {_feet.GetPhysicRectangle().Bottom}");
            //Debug.WriteLine($"GroundLevel:  {_groundLevel}");


        }

        public void IdleUpdate(InputManager inputManager)
        {
            _animationModule.SetAnimationSpeed(0.6f);
            _animationModule.Play("Idle");
            // Transition based on inputs
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _currentState = CharState.jump;

            }
            else if (inputManager.IsKeyDown(Keys.A))
            {
                _currentDirection = Direction.Left;
                if (isGrounded) _currentState = CharState.walk;

            }
            else if (inputManager.IsKeyDown(Keys.D))
            {
                _currentDirection = Direction.Right;
                if (isGrounded) _currentState = CharState.walk;

            }
            CheckIfGrounded();

        }

        public void WalkUpdate(InputManager inputManager)
        {
            _animationModule.SetAnimationSpeed(0.2f);
            _animationModule.Play("Run");



            if (inputManager.IsKeyDown(Keys.A) && _currentDirection == Direction.Left)
            {
                _velocity.X = -_speed;

            }
            else if (inputManager.IsKeyDown(Keys.D) && _currentDirection == Direction.Right)
            {
                _velocity.X = _speed;
            }

            else if (inputManager.IsKeyPressed(Keys.W))
            {
                _currentState = CharState.jump;
            }
            else
            {
                if (_currentState == CharState.walk && _feet.GetPhysicRectangle().Bottom >= _groundLevel)
                {
                    _velocity.X = 0;
                }
                if (isGrounded) _currentState = CharState.idle;
            }

            Position.X += _velocity.X;
            CheckIfGrounded();

            // Adjust sprite orientation based on direction
            _spriteModule.SetSpriteEffects(_currentDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }


        public void JumpUpdate(InputManager inputManager)
        {
          //  Debug.WriteLine($"IsGrounded = {isGrounded}");

            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.Play("Jump");

            // Handling input for jumping outside the state checks to ensure it's captured consistently
            if (inputManager.IsKeyPressed(Keys.W))
            {
                _velocity.Y = -10f; // Set jump velocity
            }

            else if (isClinging)
            {
                _currentState = CharState.cling;
                _currentDirection = _currentDirection == Direction.Right ? Direction.Right : Direction.Left;
            }
            else
            {
                if (!isGrounded)
                {
                    _currentState = CharState.jump; // Ensure we stay in jump state if not grounded
                }
                else
                {
                    _currentState = CharState.idle; // Only reset to idle if grounded
                }
            }
            CheckIfGrounded();


            // Adjust sprite orientation based on direction
            _spriteModule.SetSpriteEffects(_currentDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }




        public void ClingUpdate(InputManager inputManager)
        {
            _animationModule.Play("Cling");

            if (_currentDirection == Direction.Left && inputManager.IsKeyPressed(Keys.W) && isClinging)
            {
                _velocity = new Vector2(-10, 10);
                isClinging = false;
                _currentState = CharState.jump;

            }
            else if (_currentDirection == Direction.Right && inputManager.IsKeyPressed(Keys.W) && isClinging)
            {
                _velocity = new Vector2(10, -10);
                isClinging = false;
                _currentState = CharState.jump;

            }

            else if (isClinging && _velocity.Y >= 0) ;
            {
                _currentState = CharState.idle;
            }

            Position += _velocity;
        }

        public void JumpAfterClingUpdate(InputManager inputManager)
        {
            //ye, some code should be repeated (or put in another method)
            HandleJump(inputManager);

        }


        public void ClingState(InputManager inputManager)
        {
            //fall slightly

            //or condition to transiton between states to wall jump
        }

        private void UpdateMovementPermissions()
        {
            // Assume movement is possible unless a wall is currently colliding
            moveLeft = true;
            moveRight = true;

            // Logic here to decide based on current collision state or other conditions
            if (_currentState != CharState.cling)
            {
                moveLeft = moveRight = true; // Reset if not currently hugging a wall
            }
        }

        private void ApplyPhysics()
        {
            //Debug.WriteLine($"Feet bottom: {_feet.GetPhysicRectangle().Bottom}, Ground level: {_groundLevel}");

            // Apply horizontal velocity
            position.X += _velocity.X;

            // Update the position based on velocity
            if (_feet.GetPhysicRectangle().Bottom < _groundLevel)
            {
                _velocity.Y += _gravity; // Continue applying gravity if not grounded
            }
            else if (_feet.GetPhysicRectangle().Bottom >= _groundLevel && _velocity.Y > 0)
            {
                // If moving downwards and feet are at or below ground level, stop
                _velocity.Y = 0;
                Position.Y = _groundLevel - _feet.GetPhysicRectangle().Height + 5; // Adjust to align exactly with ground
            }

            // Now apply vertical velocity after checking conditions
            position.Y += _velocity.Y;
        }

        public void CheckIfGrounded()
        {
            if (isGrounded = _feet.GetPhysicRectangle().Bottom >= _groundLevel)
            {
                isGrounded = true;

            }
            else { isGrounded = false; }
        }



        private void HandleJump(InputManager inputManager)
        {
            //if (inputManager.IsKeyPressed(Keys.W) && isGrounded)
            //{
            //    _velocity.Y = -10f; // Consider making this a constant for easier adjustments
            //    _currentState = CharState.jump;
            //}
        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
            DebugManager.DebugRectangle(_feet.GetPhysicRectangle());
            DebugManager.DebugRectangle(_characterCollision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_leftCling.GetPhysicRectangle());
            DebugManager.DebugRectangle(_rightCling.GetPhysicRectangle());
            DebugManager.DebugRectangle(new Rectangle(0, _groundLevel, 6000, 100));
        }

        // Include methods like IdleUpdate, WalkUpdate, JumpUpdate, ClingUpdate, JumpAfterClingUpdate here
        // Adapt the transitions and behavior from the original Character's UpdateMe method

        public override void Collided(Collision collision)
        {
           // Debug.WriteLine($"Collision detected with: {collision.GetCollidedPhysicModule().GetParent().GetType()}");
            base.Collided(collision);

            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                Rectangle platformRect = platform.GetPlatformRectangle();
                //Debug.WriteLine($"Platform rectangle: {platformRect}");

                // Handle collision with platform's feet
                if (collision.GetThisPhysicModule() == _feet)
                {
                   // Debug.WriteLine("Collided at feet");
                    if (_velocity.Y >= 0)
                    {
                        isGrounded = true;
                        _velocity.Y = 0;
                        Position = new Vector2(Position.X, platformRect.Top - characterHeight + 1);
                    }
                    else
                    {
                        isGrounded = false;
                    }
                }

                // Handle collision with platform's right side
                if (collision.GetThisPhysicModule() == _rightCling && isGrounded == false)
                {
                   // Debug.WriteLine("Collided at right cling");
                    // Stop horizontal movement and position character to the left of the platform
                    _velocity.X = 0;
                    Position = new Vector2(platformRect.Left - _characterCollision.GetPhysicRectangle().Width - 1, Position.Y);
                    moveRight = false;
                    _currentState = CharState.cling;
                    isClinging = true;
                }
                else if (collision.GetThisPhysicModule() != _rightCling)
                {
                    moveRight = true;
                    isClinging = false;
                }

                // Handle collision with platform's left side
                if (collision.GetThisPhysicModule() == _leftCling)
                {
                   // Debug.WriteLine("Collided at left cling");
                    // Stop horizontal movement and position character to the right of the platform
                    _velocity.X = 0;
                    Position = new Vector2(platformRect.Right + 1, Position.Y);
                    moveLeft = false;
                    _currentState = CharState.cling;
                    isClinging = true;
                }
                else if (collision.GetThisPhysicModule() != _leftCling)
                {
                    moveLeft = true;
                    isClinging = false;
                }
            }

            // Additional collision handling logic can be added here if needed

            // Handle collision with obstacles
            if (collision.GetThisPhysicModule() == _characterCollision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
            {
                // Handle collision with obstacles if needed
            }
        }

        public float GetVelocity()
        {
            return _velocity.X;
        }
    }
}