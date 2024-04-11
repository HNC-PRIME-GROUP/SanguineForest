using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Sanguine_Forest
{
 
    /// <summary>
    /// Controller for parallax elements to create a smooth movement of background
    /// </summary>
    internal class ParallaxManager
    {

        private List<ParallaxBackground> backgrounds;

        public ParallaxManager()
        {
            backgrounds = new List<ParallaxBackground>();
        }

        // Method to add a new background layer to the manager
        public void AddBackground(ParallaxBackground background)
        {
            backgrounds.Add(background);
        }
        public void AdjustLayerSpeed(Extentions.SpriteLayer layer, float newSpeed)
        {
            foreach (var background in backgrounds)
            {
                if (background.Layer == layer) 
                {
                    background.ParallaxSpeed = newSpeed;
                }
            }
        }

        // Update method to adjust the background positions based on player/camera movement
        public void UpdateMe(Vector2 deltaMovement)
        {
            //Debug.WriteLine($"Updating backgrounds with Delta: {deltaMovement}");
            //Debug.WriteLine($"Update ParManager");

            foreach (var background in backgrounds)
            {
                background.UpdatePosition(deltaMovement);
            }

        }
        // Draw all backgrounds in the correct order
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var background in backgrounds)
            {
                background.Draw(spriteBatch);
            }
        }
    }
}
