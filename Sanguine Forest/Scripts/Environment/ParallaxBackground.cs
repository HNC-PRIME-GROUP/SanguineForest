using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Sanguine_Forest
{
    internal class ParallaxBackground : GameObject
    {
        private SpriteModule spriteModule;
        public float ParallaxSpeed { get; private set; }


        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, Extentions.SpriteLayer layer, float parallaxSpeed, Camera camera)
            : base(position, rotation)
        {
            this.spriteModule = new SpriteModule(this, Vector2.Zero, texture, layer);
            this.ParallaxSpeed = parallaxSpeed;
        }

        public void UpdateMe(Vector2 cameraMovement)
        {
            spriteModule.UpdateMe();

            //Debug.WriteLine($"Before Update - Position: {GetPosition()}");

            // Adjust the background's position based on its parallax speed
            Vector2 adjustedMovement = cameraMovement * -ParallaxSpeed;
            Vector2 newPosition = GetPosition() + adjustedMovement;

            // Round positions to the nearest whole number
            newPosition.X = (float)Math.Round(newPosition.X);
            newPosition.Y = (float)Math.Round(newPosition.Y);

            // Set the new position
            SetPosition(newPosition);

            //Debug.WriteLine($"After Update - Position: {GetPosition()}");
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            spriteModule.DrawMe(spriteBatch);
        }

    }
}
