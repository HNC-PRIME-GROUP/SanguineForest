using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Collections.Generic;

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

        private Rectangle feet;
        private const int foot = 3;

        private Rectangle _walldetL;
        private Rectangle _walldetR;

        private Vector2 pos;
        private Rectangle collision;

        private float speed;
        private Vector2 vel;

        private Texture2D txr;

        private float gravity;
        private int ground;

        public Character(Vector2 position, float rotation, Texture2D _txr) : base(position, rotation)
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, txr, 
                Extentions.SpriteLayer.character1);

            txr = _txr;

            animations = new Dictionary<string, AnimationSequence>();
            animations.Add("Idle", new AnimationSequence(Vector2.Zero, 3));
            animations.Add("Run", new AnimationSequence(new Vector2(0, 700), 3));
            animations.Add("Jump", new AnimationSequence(new Vector2(0, 1400), 5));

            spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 700, 700), animations);

            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, _spriteModule);

            pos = position;
            collision = new Rectangle((int)pos.X, (int)pos.Y, txr.Width, txr.Height);

            feet = new Rectangle(collision.X + foot, collision.Y + collision.Height - 2,
                collision.Width - (foot * 2), 2);

            _walldetL = new Rectangle(collision.X - 1, collision.Y, 
                2, collision.Height);

            _walldetR = new Rectangle(collision.X + collision.Width, collision.Y,
                2, collision.Height);

            speed = 1f;
            vel = Vector2.Zero;

            gravity = 0.3f;
            ground = 223;

            _looking = looking.Right;
            _currAni = AniState.stand;
        }

        public override void UpdateMe()
        {
            _spriteModule.UpdateMe();
            _animationModule.UpdateMe();

            vel.X = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                vel.X = speed;
                _looking = looking.Right;
                if (vel.Y != 0)
                    _currAni = AniState.jump;
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
                _currAni = AniState.stand;
            }

            pos += vel;
            collision.X = (int)pos.X;
            collision.Y = (int)pos.Y;

            if (collision.Bottom < ground)
            {
                if(vel.Y < gravity * 15)
                {
                    vel.Y += gravity;
                }
                //platform collision
                //for ()
            }
            else
            {
                vel.Y = 0;
                pos.Y = ground - collision.Height;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (vel.Y == 0)
                {
                    vel.Y = -5;
                }
            }

            feet.X = collision.X + foot;
            feet.Y = collision.Y + collision.Height - 2;

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
        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
            
        }

        public new void Collided(Collision collision)
        {
            if(collision.GetCollidedPhysicModule().GetParent() is Platform)
            {
                Platform platform = (Platform)collision.GetCollidedPhysicModule().GetParent();
                // logic of staying on a platform
                // if we need a physic rectangle of platform here:
                // platform.GetPlatformRectangle();

            }
        }
        






    }
}
