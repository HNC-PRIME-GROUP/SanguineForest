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
            spriteModule.UpdateMe();

            // Adjust the background's position based on its parallax speed
            Vector2 adjustedMovement = cameraMovement * -ParallaxSpeed * 0.2f;
            Vector2 newPosition = GetPosition() + adjustedMovement;

            // Set the new position
            SetPosition(newPosition);
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            spriteModule.DrawMe(spriteBatch);
        }

    }
}
