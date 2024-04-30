using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sanguine_Forest
{
    internal class ParallaxBackground : GameObject
    {
        private SpriteModule spriteModule;
        public float ParallaxSpeed { get; private set; }

        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, Extentions.SpriteLayer layer, float parallaxSpeed)
            : base(position, rotation)
        {
            this.spriteModule = new SpriteModule(this, Vector2.Zero, texture, layer);
            this.ParallaxSpeed = parallaxSpeed;
        }

        public void UpdateMe(Vector2 cameraMovement)
        {
            // Calculate the adjusted movement based on the parallax speed
            Vector2 adjustedMovement = cameraMovement * ParallaxSpeed;

            // Directly update position using the base GameObject's capabilities
            this.position -= adjustedMovement;
            spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            spriteModule.DrawMe(spriteBatch);
        }
    }
}