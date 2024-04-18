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
        public PhysicModule platformPhysic;


        public Platform(Vector2 position, float rotation, Vector2 platformSize, ContentManager content): base(position, rotation) 
        {

            //Sptire and graphic
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Extentions/DebugBounds"), Extention.Extentions.SpriteLayer.character1);
            _spriteModule.SetScale(10f);

           

            //Collision
            platformPhysic = new PhysicModule(this, Vector2.Zero, platformSize);
            platformPhysic.isPhysicActive = true;
        }

        public new void UpdateMe()
        {
            platformPhysic.UpdateMe();
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch) 
        {
            _spriteModule.DrawMe(spriteBatch);
            DebugManager.DebugRectangle(platformPhysic.physicRec);
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
