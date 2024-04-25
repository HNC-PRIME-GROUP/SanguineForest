using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Sanguine_Forest
{

    ///// <summary>
    ///// Controller for parallax elements to create a smooth movement of background
    ///// </summary>

    internal class ParallaxManager
    {
        private List<ParallaxBackground> backgrounds;
        private int screenWidth = 1920;


        public ParallaxManager(ContentManager content)
        {
            backgrounds = new List<ParallaxBackground>();

            InitializeBackgrounds(content);
        }

        private void InitializeBackgrounds(ContentManager content)
        {
            // Start at the far left of the viewport
            Vector2 FOREposition = new Vector2(-1920 / 2, -1080 / 4);

            // Add backgrounds with appropriate offset for seamless looping
            AddBackground("Sprites/Background_day_01", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X += screenWidth + 60;
            AddBackground("Sprites/Background_day_02", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X += screenWidth + 60;
            AddBackground("Sprites/Background_day_03", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X += screenWidth + 60;
            AddBackground("Sprites/Background_day_04", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X = 0; // Reset for next layer, or manage differently if overlapping differently

            // Mid layer backgrounds
            Vector2 MIDposition = new Vector2(-1920 / 2, -1080 / 3);
            AddBackground("Sprites/Background_day_05", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X += screenWidth + 60;
            AddBackground("Sprites/Background_day_05", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X = 0; // Reset for next layer

            // Mid layer backgrounds
            Vector2 MIDposition_2 = new Vector2(-1920 / 2, -1080 / 2); 
            AddBackground("Sprites/Background_day_06", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X += screenWidth + 60;
            AddBackground("Sprites/Background_day_06", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X = 0; // Reset for next layer

            // Back layer background
            Vector2 BACKposition = new Vector2(-1920 / 2, -1080 / 2);
            AddBackground("Sprites/Background_day_07", Extentions.SpriteLayer.background_Back, 0.75f, BACKposition, content);
            BACKposition.X += screenWidth + 60;
            AddBackground("Sprites/Background_day_07", Extentions.SpriteLayer.background_Back, 0.75f, BACKposition, content);
            BACKposition.X = 0; // Reset for next layer


        }

        // Method to add a new background layer to the manager
        private void AddBackground(string texturePath, Extentions.SpriteLayer layer, float speed, Vector2 position, ContentManager content)
        {
            ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texturePath), layer, speed);
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
            deltaMovement.Y = 0;
            //foreach (var background in backgrounds)
            //{
            //    background.UpdatePosition(deltaMovement);
            //    //CheckAndLoopBackground(background);
            //}

            for(int i =0; i<backgrounds.Count;i++)
            {
                backgrounds[i].UpdateMe(deltaMovement);
                CheckAndLoopBackground(backgrounds[i]);
            }
        }

        private void CheckAndLoopBackground(ParallaxBackground background)
        {
            Vector2 position = background.GetPosition();

            // Check if the background has moved completely past the left edge
            if (position.X + screenWidth < 0)
            {
                // Find the farthest right position in the same layer
                float maxPosition = FindFarthestRightPosition(background.Layer);
                // Reset to the right of the farthest background
                background.SetPosition(new Vector2(maxPosition + screenWidth, position.Y));
            }
        }

        private float FindFarthestRightPosition(Extentions.SpriteLayer layer)
        {
            float maxPosition = 0;
            foreach (var bg in backgrounds)
            {
                if (bg.Layer == layer)
                {
                    float rightEdge = bg.GetPosition().X + screenWidth;
                    if (rightEdge > maxPosition)
                    {
                        maxPosition = rightEdge;
                    }
                }
            }
            return maxPosition;
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
