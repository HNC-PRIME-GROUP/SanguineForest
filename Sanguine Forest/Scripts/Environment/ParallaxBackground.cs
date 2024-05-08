using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Sanguine_Forest
{
    internal class ParallaxBackground : GameObject
    {
        public SpriteModule spriteModule;
        public float ParallaxSpeed { get; private set; }
        public float ParallaxSpeedX { get; set; } // Existing property for horizontal speed
        public float ParallaxSpeedY { get; set; } // New property for vertical speed


        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, Extentions.SpriteLayer layer, float parallaxSpeedX, float parallaxSpeedY, Camera camera)
            : base(position, rotation)
        {
            this.spriteModule = new SpriteModule(this, Vector2.Zero, texture, layer);
            this.ParallaxSpeedX = parallaxSpeedX;
            this.ParallaxSpeedY = parallaxSpeedY;

        }

        public void UpdateMe(Vector2 cameraMovement)
        {
            spriteModule.UpdateMe();

            //Debug.WriteLine($"Before Update - Position: {GetPosition()}");

            // Adjust the background's position based on its parallax speed
            Vector2 adjustedMovement = cameraMovement * -ParallaxSpeedX * 0.2f;
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
