using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using SharpDX.Direct3D9;
using System.Collections.Generic;
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
            wallJump,
            drink
        }
        enum looking
        {
            Right,
            Left
        }

        private AniState _currAni;
        private looking _looking;

        public Vector2 pos;

        //public Rectangle collision;
        private PhysicModule _collision;
        private PhysicModule _feet;

        private PhysicModule _walldetL;
        private PhysicModule _walldetR;

        private float speed;
        private Vector2 vel;

        private bool moveL;
        private bool moveR;

        public Texture2D txr;

        private float gravity;
        private int ground;

        public bool hugwall;

        public Character(Vector2 position, float rotation, Texture2D _txr) : base(position, rotation)
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, _txr, 
                Extentions.SpriteLayer.character1);

            _spriteModule.SetScale(0.3f);

            animations = new Dictionary<string, AnimationSequence>();
            animations.Add("Idle", new AnimationSequence(Vector2.Zero, 3));
            animations.Add("Run", new AnimationSequence(new Vector2(0, 700), 3));
            animations.Add("Jump", new AnimationSequence(new Vector2(0, 1400), 5));
            animations.Add("hugWall", new AnimationSequence(new Vector2(4200, 1400), 0));

            spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);

            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _spriteModule);
            _spriteModule.AnimtaionInitialise(_animationModule);

            pos = position;

            //Collisions
            _collision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
            _feet = new PhysicModule(this, new Vector2(100, 180), new Vector2(130, 20));

            _walldetL = new PhysicModule(this, new Vector2(35, 100), new Vector2(10, 130));
            _walldetR = new PhysicModule(this, new Vector2(165, 100), new Vector2(10, 130));


            _collision.isPhysicActive = true;
            _feet.isPhysicActive =true;
            _walldetL.isPhysicActive = true;
            _walldetR.isPhysicActive = true;

            speed = 5f;
            vel = Vector2.Zero;

            gravity = 0.3f;
            ground = 800;

            _looking = looking.Right;
            _currAni = AniState.stand;

            moveL = true;
            moveR = true;
        }

        public void UpdateMe(KeyboardState curr, KeyboardState prev)
        {
            _animationModule.UpdateMe();
            _spriteModule.UpdateMe();

            _collision.UpdateMe();
            _feet.UpdateMe();
            _walldetL.UpdateMe();
            _walldetR.UpdateMe();

            switch (_currAni)
            {
                case AniState.stand:
                    IdleUpdate(curr, prev);
                    break;
                case _currAni.walk:
                    WalkUpdate(curr, prev);
                    break;
                case CharState.jump:
                    JumpUpdate(curr, prev);
                    break;
                case CharState.climb:
                    ClimbUpdate(curr, prev);
                    break;
                case CharState.jumpAfterClimb:
                    JumpAfterClimbUpdate(curr, prev);
                    break;





                    if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (moveR == true)
                {
                    vel.X = speed;
                    _looking = looking.Right;
                    moveL = true;
                }

                if (vel.Y != 0)
                {
                    _currAni = AniState.jump;
                }
                else if (_currAni != AniState.hugWall || vel.X >= speed)
                {
                    _currAni = AniState.walk;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (moveL == true)
                {
                    vel.X = -speed;
                    _looking = looking.Left;
                    moveR = true;
                }

                if (vel.Y != 0)
                    _currAni = AniState.jump;
                else if (_currAni != AniState.hugWall || vel.X <= -speed)
                {
                    _currAni = AniState.walk;
                }
            }
            else if (_currAni == AniState.jump || _currAni == AniState.walk ||
                  _currAni == AniState.drink)
            {
                vel.X = 0;
                _currAni = AniState.stand;
            }

            if (prev.IsKeyUp(Keys.W)&&curr.IsKeyDown(Keys.W))
            {
                
                if (vel.Y == 0)
                {
                    vel.Y = -8;
                    moveL = true;
                    moveR = true;
                }

                if (_currAni == AniState.hugWall)
                {
                    vel.Y = -9;
                    if (_looking == looking.Right)
                    {
                        vel.X = -6;
                        _looking = looking.Left;
                    }
                    else if (_looking == looking.Left)
                    {
                        vel.X = 6;
                        _looking = looking.Right;
                    }
                }
                hugwall = false;
            }
            position += vel;


            if (_feet.GetPhysicRectangle().Bottom  < ground)
            {
                if(vel.Y < gravity * 35)
                {
                    vel.Y += gravity;
                }
            }
            else if(vel.Y>0)
            {
                vel.Y = 0;
                position.Y = ground - _collision.GetPhysicRectangle().Height - _feet.GetPhysicRectangle().Height*2;
            }


            if (_looking == looking.Right)
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.None);
            }
            else if( _looking == looking.Left)
            {
                _spriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);
            }

            if (_currAni == AniState.stand)
            {
                _animationModule.SetAnimationSpeed(0.6f);
                _animationModule.Play("Idle");
            }
            else if (_currAni == AniState.hugWall)
            {
                _animationModule.SetAnimationSpeed(0.1f);
                _animationModule.Play("hugWall");
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
            if(collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                platform.GetPlatformRectangle();

                if (collision.GetThisPhysicModule() == _feet)
                {
                    if (vel.Y > 0)
                    {
                        vel.Y = 0;
                        pos.Y = platform.GetPlatformRectangle().Top - _collision.GetPhysicRectangle().Height + 1;
                    }
                }
                
                if (collision.GetThisPhysicModule() == _walldetR)
                {
                    if (_looking == looking.Right)
                    {
                        vel.X = 0;
                        pos.X = platform.GetPlatformRectangle().Left - _collision.GetPhysicRectangle().Width - 1;
                        moveR = false;
                        hugwall = true;
                        _currAni = AniState.hugWall;
                    }
                }
                else
                {
                    moveR = true;
                    if (vel.Y < 0)
                    {
                        _currAni = AniState.jump;
                    }
                }
                
                if (collision.GetThisPhysicModule() == _walldetL)
                {
                    if (_looking == looking.Left)
                    {
                        vel.X = 0;
                        pos.X = platform.GetPlatformRectangle().Right;
                        moveL = false;
                        hugwall = true;
                        _currAni = AniState.hugWall;
                    }
                }
                else
                {
                    moveL = true;
                    if (vel.Y < 0)
                    {
                        _currAni = AniState.jump;
                    }
                }

                // if we need a physic rectangle of platform here:
            }
            if(collision.GetThisPhysicModule() == _collision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
            {

            }
        }


        public Vector2 GetVelocity()
        {
            return vel;
        }
        






    }
}
