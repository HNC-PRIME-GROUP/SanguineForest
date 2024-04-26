using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Sanguine_Forest
{
    /// <summary>
    /// Object of background that can be moved by parallax manager
    /// </summary>
    internal class ParallaxBackground : GameObject
    {
        private SpriteModule spriteModule; // Composition instead of inheritance
        public float ParallaxSpeed { get; set; } // Multiplier to control speed
        public Extentions.SpriteLayer Layer { get; private set; }

        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, Extentions.SpriteLayer layer, float parallaxSpeed) 
            : base (position, rotation)
        
        {
            // Initialize the SpriteModule with the position, rotation, and layer specified
            this.Layer = layer; // Set the layer property
            this.spriteModule = new SpriteModule(this, Vector2.Zero, texture, layer);
            
            this.ParallaxSpeed = parallaxSpeed;
        }

        public void UpdateMe(Vector2 deltaMovement)
        {

            spriteModule.UpdateMe();
          

            //Debug.WriteLine($"Update ParBackground");

            //// Adjust the background's position based on its parallax speed
            Vector2 adjustedMovement = deltaMovement * ParallaxSpeed;
            Vector2 newPosition = GetPosition() + adjustedMovement;

            // Set the new position
            SetPosition(newPosition);
        }

        // Method to draw the background
        public void Draw(SpriteBatch spriteBatch)
        {
            //Debug.WriteLine($"Draw ParBackground");
            // Delegate drawing to the spriteModule
            spriteModule.DrawMe(spriteBatch);
        }
    }
}
