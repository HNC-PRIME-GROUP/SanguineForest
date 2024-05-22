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
using Sanguine_Forest.Scripts.Environment.Obstacle;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace Sanguine_Forest
{
    internal class Character2 : GameObject
    {

        //graphic
        public SpriteModule _SpriteModule;
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
        public enum CharState
        {
            idle,
            walk,
            jump,
            cling,
            wallJump,
            falling,
            Death,
            walkToTarget,
            dialogue

        }
        public CharState _currentState;

        //Movement
        private float _speed = 9.5f;
        private float _jumpHigh = 24f;
        private float walkSpeed = 3f; // Set a fixed walk speed
        private Vector2 _velocity = Vector2.Zero;
        private float _gravityRate = 0.3f;
        private float _gravityEffect = 0;
        //start position saved to restore the character here
        private Vector2 startPos;

        //work around for cling update
        private PhysicModule currClingRectangle;

        //Death event
        public event EventHandler DeathEvent;

        private Vector2 _targetPosition;
        private bool isWalkingToTarget;
        private bool isJumping = false; // Track if the character is jumping

        //Audio
        public AudioSourceModule AudioSourceModule;
        public Dictionary<string, SoundEffectInstance> sounds;


        //checkpoints
        public Vector2 SavePoint;

        public delegate void CharacterSaveEvent(Character2 sender, SaveCharacterDataArgs e);

        public event CharacterSaveEvent savePosMoment;

        public Character2(Vector2 position, float rotation, ContentManager content) : base(position, rotation)
        {

            //Save start pos
            startPos = position;
            SavePoint = startPos;

            //Animation and graphic
            _SpriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Sprites_Character_v1"),
                Extentions.SpriteLayer.character1);

            _SpriteModule.SetScale(0.15f);

            _animations = new Dictionary<string, AnimationSequence>
            {
                { "Idle", new AnimationSequence(Vector2.Zero, 3) },
                { "Run", new AnimationSequence(new Vector2(0, 700), 3) },
                { "Jump", new AnimationSequence(new Vector2(0, 1400), 5) },
                { "hugWall", new AnimationSequence(new Vector2(4200, 1400), 0) },
                { "Death", new AnimationSequence(new Vector2(0, 2100), 2) }
            };

            _spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), _animations);
            _animationModule = new AnimationModule(this, Vector2.Zero, _spriteSheetData, _SpriteModule);
            _SpriteModule.AnimtaionInitialise(_animationModule);

            //Setting audio Audio
            sounds = new Dictionary<string, SoundEffectInstance>();
            sounds.Add("Run", content.Load<SoundEffect>("Sounds/Char_Run").CreateInstance());
            sounds.Add("Jump", content.Load<SoundEffect>("Sounds/Char_Jump").CreateInstance());
            sounds.Add("Death", content.Load<SoundEffect>("Sounds/Char_Death").CreateInstance());
            AudioSourceModule = new AudioSourceModule(this, Vector2.Zero, sounds);

            //Collisions
            // _characterCollision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
            _feetCollision = new PhysicModule(this, new Vector2(50, 95), new Vector2(35, 10));
            _headCollision = new PhysicModule(this, new Vector2(50, 0), new Vector2(35, 10));

            _leftCollision = new PhysicModule(this, new Vector2(15, 50), new Vector2(10, 60));
            _rightCollision = new PhysicModule(this, new Vector2(85, 50), new Vector2(10, 60));


            _currentState = CharState.jump;

            this.isWalkingToTarget = false;
            this._currentState = CharState.idle;


        }


        public void UpdateMe(KeyboardState prev, KeyboardState curr)
        {
            AudioSourceModule.UpdateMe();
            _animationModule.UpdateMe();
            _SpriteModule.UpdateMe();
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
                case CharState.Death:
                    DeathUpdate();
                    break;
                case CharState.walkToTarget:
                    WalkToTargetUpdate();
                    break;
                case CharState.dialogue:
                    DialogueUpdate();
                    break;
            }


            _velocity.Y += _gravityEffect;
            position += _velocity;

            //Debug.WriteLine($"Character Position: {position}, Velocity: {_velocity}, State: {_currentState}");

        }

        #region State updates
        private void WalkToTargetUpdate()
        {
            AudioSourceModule.PlaySoundOnce("Run", 1f);
            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.Play("Run");

            if (Vector2.Distance(position, _targetPosition) > walkSpeed)
            {
                Vector2 direction = Vector2.Normalize(_targetPosition - position);
                _velocity.X = direction.X * walkSpeed;

                // Set the character's sprite effect based on the direction
                _SpriteModule.SetSpriteEffects(direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

                // Debug log for walking to target
                //Debug.WriteLine($"Walking to target. Direction: {direction}, Velocity: {_velocity}, Target Position: {_targetPosition}");
            }
            else
            {
                // If the character is close enough to the target, set position directly
                position = _targetPosition;
                isWalkingToTarget = false;
                _currentState = CharState.dialogue;
                AudioSourceModule.StopSound("Run");
                _velocity = Vector2.Zero;

                // Face towards the NPC after reaching the target
                Vector2 directionToNPC = Vector2.Normalize(_targetPosition - GetPosition());
                _SpriteModule.SetSpriteEffects(directionToNPC.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                // Debug log for reaching the target
                // Debug.WriteLine($"Reached target position: {_targetPosition}");


            }
        }

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
            if (prev.IsKeyDown(Keys.A) || prev.IsKeyDown(Keys.D))
            {
                _currentState = CharState.walk;
            }

            //transition to fall or jump
            if (_velocity.Y < 0)
            {
                isJumping = true; // Set jumping state              
                _currentState = CharState.jump;
                return;
            }
            if (_velocity.Y > 0)
            {
                _currentState = CharState.wallJump;
            }

            if (curr.GetPressedKeys().Length == 0)
            {
                _velocity.X = 0;
            }
            //_gravityEffect = 0f;
            //_velocity.Y = 0f;
        }

        public void WalkUpdate(KeyboardState prev, KeyboardState curr)
        {
            AudioSourceModule.PlaySoundOnce("Run");

            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.Play("Run");
            if (curr.IsKeyDown(Keys.W) && prev.IsKeyUp(Keys.W))
            {
                isJumping = true; // Set jumping state
                _velocity.Y = -_jumpHigh;               
                _currentState = CharState.jump;
                return;

            }

            if (curr.GetPressedKeys().Length == 0)
            {
                _velocity.X = 0;
                _currentState = CharState.idle;
                AudioSourceModule.StopSound("Run");
                return;
            }
            if (prev.IsKeyDown(Keys.A))
            {
                _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                _velocity.X = -_speed;
            }
            if (prev.IsKeyDown(Keys.D))
            {
                _SpriteModule.SetSpriteEffects(SpriteEffects.None);
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

            //Debug.WriteLine($"Walk Update. Position: {position}, Velocity: {_velocity}");
        }

        public void JumpUpdate(KeyboardState prev, KeyboardState curr)
        {
            AudioSourceModule.StopSound("Run");

            if (isJumping)
            {
                AudioSourceModule.PlaySoundOnce("Jump");
                isJumping = false;
            }

            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.PlayOnce("Jump");
            if (curr.IsKeyDown(Keys.A))
            {
                _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                _velocity.X = -_speed;
            }
            if (curr.IsKeyDown(Keys.D))
            {
                _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                _velocity.X = _speed;
            }

            if (curr.GetPressedKeys().Length == 0)
            {
                AudioSourceModule.StopSound("Run");
                AudioSourceModule.StopSound("Jump");
                return;
            }



        }

        public void ClingUpdate(KeyboardState prev, KeyboardState curr)
        {

            _animationModule.Play("hugWall");
            _gravityEffect = 0; //Make slower gravity effect
            _velocity.X = 0;
            _velocity.Y = _gravityRate;

            if (prev.IsKeyDown(Keys.S))
            {
                _velocity.X = 0;
                _currentState = CharState.idle;
            }


            if (curr.IsKeyDown(Keys.W) && prev.IsKeyUp(Keys.W))
            {
                if (_SpriteModule.GetSpriteEffects() == SpriteEffects.None)
                {
                    _velocity.X = -_speed;
                    _velocity.Y = -_jumpHigh;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    _currentState = CharState.wallJump;
                    return;
                }
                else
                {
                    _velocity.X = _speed;
                    _velocity.Y = -_jumpHigh;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                    _currentState = CharState.wallJump;
                    return;
                }
            }
            if (_rightCollision.physicRec.Y > currClingRectangle.GetPhysicRectangle().Y + currClingRectangle.GetPhysicRectangle().Height || !currClingRectangle.isPhysicActive)
            {
                _currentState = CharState.falling;
            }
        }

        public void WallJumpUpdate(KeyboardState prev, KeyboardState curr)
        {
            if (isJumping)
            {
                AudioSourceModule.PlaySoundOnce("Jump");
                isJumping = false;
            }

            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.PlayOnce("Jump");
            if (_velocity.Y > 0)
            {
                if (curr.IsKeyDown(Keys.A))
                {
                    _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    _velocity.X = -(_speed * 2 / 3);
                }
                if (curr.IsKeyDown(Keys.D))
                {
                    _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                    _velocity.X = (_speed * 2 / 3);
                }
            }

            if (curr.GetPressedKeys().Length == 0)
            {
                AudioSourceModule.StopSound("Run");
                AudioSourceModule.StopSound("Jump");
                return;
            }
        }

        public void FallingUpdate(KeyboardState prev, KeyboardState curr)
        {
            _animationModule.SetAnimationSpeed(0.1f);
            _animationModule.PlayOnce("Jump");
            if (curr.IsKeyDown(Keys.A))
            {
                _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                _velocity.X = -(_speed * 1 / 5);
            }
            if (curr.IsKeyDown(Keys.D))
            {
                _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                _velocity.X = (_speed * 1 / 5);
            }
        }

        public void DeathUpdate()
        {
            AudioSourceModule.PlaySoundOnce("Death");
            _animationModule.SetAnimationSpeed(0.6f);
            _animationModule.PlayOnce("Death");
            _velocity.X = 0;
            _velocity.Y = 0;
            _gravityEffect = 0;
            _animationModule.AnimationEnd += CharacterRestore;


        }

        public void DialogueUpdate()
        {
            _animationModule.SetAnimationSpeed(0.5f);
            _animationModule.Play("Idle");
        }


        public void CharacterRestore(object obj, EventArgs e)
        {
            _currentState = CharState.jump;
            position = SavePoint ;
            _animationModule.AnimationEnd -= CharacterRestore;
        }

        public void CharacterRestore()
        {
            _currentState = CharState.jump;
            position = startPos;
        }

        public void CharacterEndDialogue(object sender, EventArgs e)
        {
            _currentState = CharState.idle;
        }




        #endregion



        public override void Collided(Collision collision)
        {
            base.Collided(collision);

            if (collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform prevPlatform = null;

                

                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();

                if (collision.GetCollidedPhysicModule().GetParent() is Thorns)
                {
                    _velocity.Y = 0;
                    _velocity.X = 0;
                    _gravityEffect = 0;
                    return;
                }
                if ((_currentState == CharState.idle || _currentState == CharState.walk || _currentState == CharState.walkToTarget || _currentState==CharState.dialogue) && collision.GetThisPhysicModule() == _feetCollision)
                {
                    _gravityEffect = 0f;
                    _velocity.Y = 0f;
                    position.Y = collision.GetCollidedPhysicModule().GetPhysicRectangle().Top - _feetCollision.GetShiftPosition().Y;
                    if(platform is not FallingPlatform&& platform is not MoveblePlatform )
                    {
                        SavePoint = new Vector2(position.X, position.Y);
                        savePosMoment?.Invoke(this, new SaveCharacterDataArgs(SavePoint));
                        

                    }
                    return;
                }

                if ((_currentState == CharState.jump || _currentState == CharState.wallJump || _currentState == CharState.falling) && collision.GetThisPhysicModule() == _feetCollision && _velocity.Y > 0)
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
                    _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    return;
                }
                if (_currentState == CharState.walk && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Left - collision.GetThisPhysicModule().GetShiftPosition().X - collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                    return;
                }
                if ((_currentState == CharState.jump || _currentState == CharState.wallJump) && collision.GetThisPhysicModule() == _leftCollision)
                {
                    _velocity.X = 0;
                    _velocity.Y = 0;
                    position.X = platform.GetPlatformRectangle().Right - collision.GetThisPhysicModule().GetShiftPosition().X + collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    currClingRectangle = platform.GetPhysicModule();
                    _gravityEffect = 0f;
                    _currentState = CharState.cling;

                    return;
                }
                if ((_currentState == CharState.jump || _currentState == CharState.wallJump) && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    _velocity.Y = 0;
                    position.X = platform.GetPlatformRectangle().Left - collision.GetThisPhysicModule().GetShiftPosition().X - collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _gravityEffect = 0f;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                    currClingRectangle = platform.GetPhysicModule();
                    _currentState = CharState.cling;
                    return;
                }
                if (_currentState == CharState.cling && collision.GetThisPhysicModule() == _feetCollision)
                {
                    _currentState = CharState.idle;
                    if (_SpriteModule.GetSpriteEffects() == SpriteEffects.None)
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
                if (_currentState == CharState.falling && collision.GetThisPhysicModule() == _leftCollision)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Right - collision.GetThisPhysicModule().GetShiftPosition().X + collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
                    return;
                }
                if (_currentState == CharState.falling && collision.GetThisPhysicModule() == _rightCollision)
                {
                    _velocity.X = 0;
                    position.X = platform.GetPlatformRectangle().Left - collision.GetThisPhysicModule().GetShiftPosition().X - collision.GetThisPhysicModule().GetPhysicRectangle().Width;
                    _SpriteModule.SetSpriteEffects(SpriteEffects.None);
                    return;
                }
                if (collision.GetThisPhysicModule() == _headCollision)
                {
                    _velocity.Y = 0;
                    //_velocity.X = 0;
                    position.Y = platform.GetPlatformRectangle().Bottom + _headCollision.GetShiftPosition().Y + 10;

                    // _currentState = CharState.falling; 
                    return;
                }

            }
            if (collision.GetCollidedPhysicModule().GetParent() is Thorns)
            {
                _velocity.X = 0;
                _velocity.Y = 0;
                _gravityEffect = 0;
            }

        }

        public void Death()
        {
            _currentState = CharState.Death;
            DeathEvent?.Invoke(this, EventArgs.Empty);
        }

        public void DrawMe(SpriteBatch sp)
        {
            _SpriteModule.DrawMe(sp);
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

        public float GetVelocityX()
        {
            return _velocity.X;
        }

        public float GetVelocityY()
        {
            return _velocity.Y;
        }


        public CharState GetCharacterState()
        {
            return _currentState;
        }

        public void SetTargetPosition(Vector2 targetPosition)
        {
            this._targetPosition = targetPosition;
            this.isWalkingToTarget = true;
            this._currentState = CharState.walkToTarget;

            //Debug.WriteLine($"Set target position: {targetPosition}");
        }




    }

    public class SaveCharacterDataArgs : EventArgs
    {
        public SaveCharacterDataArgs(Vector2 pos)
        {
            savePoint = pos;
        }
        public Vector2 savePoint;
    }


}
