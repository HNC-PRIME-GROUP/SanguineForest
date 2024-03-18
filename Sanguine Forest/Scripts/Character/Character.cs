using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{
    /// <summary>
    /// Main character
    /// </summary>
    internal class Character : GameObject
    {
        private SpriteModule _spriteModule;
        private AnimationModule _animationModule;



        public Character(Vector2 position, float rotation) : base(position, rotation)
        {

        }

        public override void UpdateMe()
        {
            _spriteModule.UpdateMe();
            _animationModule.UpdateMe();
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
