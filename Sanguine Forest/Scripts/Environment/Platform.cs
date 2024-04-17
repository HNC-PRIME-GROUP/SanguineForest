using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Sanguine_Forest
{

    /// <summary>
    /// Platform to jump on it
    /// </summary>
    internal class Platform : GameObject
    {


        //Sprite
        private SpriteModule _spriteModule;

        //Collision
        private PhysicModule platformPhysic;


        public Platform(Vector2 position, float rotation, Vector2 platformSize, ContentManager content): base(position, rotation) 
        {

            //Sptire and graphic
            _spriteModule = new SpriteModule(this, Vector2.Zero, DebugManager.DebugTexture, Extention.Extentions.SpriteLayer.environment1);

            //Collision
            platformPhysic = new PhysicModule(this, Vector2.Zero, platformSize);
        }

        public new void UpdateMe()
        {
            
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch) 
        {
            _spriteModule.DrawMe(spriteBatch);
        }





        /// <summary>
        /// Return exactly rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPlatformRectangle()
        {
            return platformPhysic.GetPhysicRectangle();
        }

    }
}
