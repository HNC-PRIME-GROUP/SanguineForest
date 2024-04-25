
using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanguine_Forest
{
    //    internal class ParallaxManager
    //    {
    //        private List<ParallaxBackground> backgrounds;
    //        private int screenWidth = 1920;

    //        public ParallaxManager(ContentManager content)
    //        {
    //            backgrounds = new List<ParallaxBackground>();
    //            InitializeBackgrounds(content);
    //        }

    //        private void InitializeBackgrounds(ContentManager content)
    //        {
    //            AddLayer(new[] { "Sprites/Background_day_01", "Sprites/Background_day_02", "Sprites/Background_day_03", "Sprites/Background_day_04" },
    //                Extentions.SpriteLayer.background_Fore, 0.25f, new Vector2(-1920 / 2, -1080 / 3.9f), content);
    //            AddLayer(new[] { "Sprites/Background_day_05", "Sprites/Background_day_05" },
    //                Extentions.SpriteLayer.background_Mid, 0.4f, new Vector2(-1920 / 2, -1080 / 3), content);
    //            AddLayer(new[] { "Sprites/Background_day_06", "Sprites/Background_day_06" },
    //                Extentions.SpriteLayer.background_Mid_Back, 0.6f, new Vector2(-1920 / 2, -1080 / 2.3f), content);
    //            AddLayer(new[] { "Sprites/Background_day_07", "Sprites/Background_day_07" },
    //                Extentions.SpriteLayer.background_Back, 0.75f, new Vector2(-1920 / 2, -1080 / 2f), content);
    //        }

    //        private void AddLayer(string[] textures, Extentions.SpriteLayer layer, float speed, Vector2 initialPosition, ContentManager content)
    //        {
    //            Vector2 position = initialPosition;
    //            foreach (var texture in textures)
    //            {
    //                backgrounds.Add(new ParallaxBackground(position, 0, content.Load<Texture2D>(texture), layer, speed));
    //                position.X += screenWidth + 60;  // Position each subsequent background immediately to the right of the last
    //            }
    //        }

    //        public void UpdateMe(Vector2 deltaMovement)
    //        {
    //            deltaMovement.Y = 0;  // Ignore vertical movement for parallax
    //            foreach (var background in backgrounds)
    //            {
    //                background.UpdateMe(deltaMovement);
    //                CheckAndLoopBackground(background);
    //            }
    //        }

    //        private void CheckAndLoopBackground(ParallaxBackground background)
    //        {
    //            if (background.GetPosition().X + screenWidth + 60 < 0)  // Check if the background is fully out of the left view
    //            {
    //                float maxRight = FindFarthestRightPosition(background.Layer);
    //                background.SetPosition(new Vector2(maxRight + screenWidth - 1, background.GetPosition().Y));
    //            }
    //        }

    //        private float FindFarthestRightPosition(Extentions.SpriteLayer layer)
    //        {
    //            float maxPosition = float.MinValue;
    //            foreach (var bg in backgrounds)
    //            {
    //                if (bg.Layer == layer)
    //                {
    //                    float rightEdge = bg.GetPosition().X + screenWidth;
    //                    maxPosition = Math.Max(maxPosition, rightEdge);
    //                }
    //            }
    //            return maxPosition;
    //        }

    //        public void Draw(SpriteBatch spriteBatch)
    //        {
    //            foreach (var background in backgrounds)
    //            {
    //                background.Draw(spriteBatch);
    //            }
    //        }
    //    }
    //}
    internal class ParallaxManager
    {
        private Dictionary<Extentions.SpriteLayer, List<ParallaxBackground>> layerBackgrounds;
        private int screenWidth = 1920;

        public ParallaxManager(ContentManager content)
        {
            layerBackgrounds = new Dictionary<Extentions.SpriteLayer, List<ParallaxBackground>>();
            InitializeBackgrounds(content);
        }

        private void InitializeBackgrounds(ContentManager content)
        {
            // Initialize each layer individually
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background_day_01", "Sprites/Background_day_02", "Sprites/Background_day_03", "Sprites/Background_day_04" }, 0.25f, new Vector2(-1920 / 2, -1080 / 4), content);
            InitializeLayer(Extentions.SpriteLayer.background_Mid, new[] { "Sprites/Background_day_05", "Sprites/Background_day_05" }, 0.4f, new Vector2(-1920 / 2, -1080 / 2.9f), content);
            InitializeLayer(Extentions.SpriteLayer.background_Mid_Back, new[] { "Sprites/Background_day_06", "Sprites/Background_day_06" }, 0.6f, new Vector2(-1920 / 2, -1080 / 2), content);
            InitializeLayer(Extentions.SpriteLayer.background_Back, new[] { "Sprites/Background_day_07", "Sprites/Background_day_07" }, 0.75f, new Vector2(-1920 / 2, -1080 / 2.2f), content);
        }

        private void InitializeLayer(Extentions.SpriteLayer layer, string[] textures, float speed, Vector2 initialPosition, ContentManager content)
        {
            List<ParallaxBackground> backgrounds = new List<ParallaxBackground>();
            Vector2 position = initialPosition;
            foreach (var texture in textures)
            {
                ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texture), layer, speed);
                backgrounds.Add(background);
                position.X += screenWidth;  // Position each subsequent background immediately to the right of the last
            }
            layerBackgrounds[layer] = backgrounds;
            Debug.WriteLine($"Initializing {layer} with initial position {initialPosition} and speed {speed}");
        }

        public void UpdateMe(Vector2 deltaMovement)
        {
            Debug.WriteLine($"Received DeltaMovement: {deltaMovement}");
            deltaMovement.Y = 0;  // Ignore vertical movement for parallax
            Debug.WriteLine($"Updating Backgrounds with DeltaMovement: {deltaMovement}");

            // First update all background positions
            foreach (var layer in layerBackgrounds.Values)
            {
                foreach (var background in layer)
                {
                    background.UpdateMe(deltaMovement);
                }
            }

            // Then check and adjust for looping separately to avoid immediate repositioning problems
            foreach (var layer in layerBackgrounds.Keys)
            {
                CheckAndLoopBackgrounds(layer, deltaMovement);
            }
        }

        private void CheckAndLoopBackgrounds(Extentions.SpriteLayer layer, Vector2 deltaMovement)
        {
            List<ParallaxBackground> backgrounds = layerBackgrounds[layer];
            float maxRight = float.MinValue;
            float minLeft = float.MaxValue;

            // Find maximum right and minimum left edges
            foreach (var background in backgrounds)
            {
                float rightEdge = background.GetPosition().X + screenWidth;
                float leftEdge = background.GetPosition().X;
                maxRight = Math.Max(maxRight, rightEdge);
                minLeft = Math.Min(minLeft, leftEdge);
            }

            Debug.WriteLine($"Layer {layer}: Max Right = {maxRight}, Min Left = {minLeft}");

            if (deltaMovement != Vector2.Zero)  // Add this check if looping should depend on actual movement
            {
                // Adjust positions for looping
                foreach (var background in backgrounds)
                {
                    if (background.GetPosition().X + screenWidth < 0)  // Background has moved past the left boundary
                    {
                        Debug.WriteLine($"Background at {background.GetPosition()} moved past left boundary. Looping to right.");
                        background.SetPosition(new Vector2(maxRight, background.GetPosition().Y));  // Loop to right
                    }
                    else if (background.GetPosition().X > screenWidth)  // Background has moved past the right boundary
                    {
                        Debug.WriteLine($"Background at {background.GetPosition()} moved past right boundary. Looping to left.");
                        background.SetPosition(new Vector2(minLeft - screenWidth, background.GetPosition().Y));  // Loop to left
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in layerBackgrounds.Values)
            {
                foreach (var background in layer)
                {
                    background.Draw(spriteBatch);
                }
            }
        }
    }
}