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

        public Texture2D txr;

        private float gravity;
        private int ground;

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

            pos = position;

            //Collisions
            _collision = new PhysicModule(this, new Vector2(100, 100), new Vector2(140, 160));
            _feet = new PhysicModule(this, new Vector2(100, 190), new Vector2(140, 20));

            _walldetL = new PhysicModule(this, new Vector2(20, 100), new Vector2(10, 160));
            _walldetR = new PhysicModule(this, new Vector2(180, 100), new Vector2(10, 160));


            _collision.isPhysicActive = true;
            _feet.isPhysicActive =true;

            speed = 5f;
            vel = Vector2.Zero;

            gravity = 0.3f;
            ground = 400;

            _looking = looking.Right;
            _currAni = AniState.stand;
        }

        public void UpdateMe(KeyboardState curr, KeyboardState prev, List<Platform> plats)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                vel.X = speed;
                _looking = looking.Right;
                
                if (vel.Y != 0)
                {
                    _currAni = AniState.jump;
                }
                else
                    _currAni = AniState.walk;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                vel.X = -speed;
                _looking = looking.Left;
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

            if (prev.IsKeyUp(Keys.W)&&curr.IsKeyDown(Keys.W))
            {
                if (vel.Y == 0)
                {
                    vel.Y = -8;
                }
            }
            position += vel;

            //collision.X = (int)pos.X;
            //collision.Y = (int)pos.Y;

            if (_feet.GetPhysicRectangle().Bottom  < ground)
            {
                if(vel.Y < gravity * 35)
                {
                    vel.Y += gravity;
                }

                for (int i = 0; i < plats.Count; i++)
                {
                    if (plats[i].GetPlatformRectangle().Intersects(_feet.GetPhysicRectangle()))
                    {
                        if (vel.Y > 0)
                        {
                            vel.Y = 0;
                            pos.Y = plats[i].GetPlatformRectangle().Top - _collision.GetPhysicRectangle().Height + 1;
                        }
                    }

                }
            }
            else if(vel.Y>0)
            {
                vel.Y = 0;
                position.Y = ground - _collision.GetPhysicRectangle().Height - _feet.GetPhysicRectangle().Height*2;
            }

            

            //feet.X = collision.X + foot;
            //feet.Y = collision.Y + collision.Height - 2;

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
            _spriteModule.DrawMe(sp, _animationModule);
            DebugManager.DebugRectangle(_feet.GetPhysicRectangle());
            DebugManager.DebugRectangle(_collision.GetPhysicRectangle());
            DebugManager.DebugRectangle(_walldetL.GetPhysicRectangle());
            DebugManager.DebugRectangle(_walldetR.GetPhysicRectangle());
            DebugManager.DebugRectangle(new Rectangle(0, ground, 6000, 100));
            
        }

        public new void Collided(Collision collision)
        {
            base.Collided(collision);
            if(collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                platform.GetPlatformRectangle();

                // logic of staying on a platform
                // if we need a physic rectangle of platform here:
                // platform.GetPlatformRectangle();
            }
            if(collision.GetThisPhysicModule() == _collision && collision.GetCollidedPhysicModule().GetParent() is Obstacle)
            {

            }
        }
        






    }
}
