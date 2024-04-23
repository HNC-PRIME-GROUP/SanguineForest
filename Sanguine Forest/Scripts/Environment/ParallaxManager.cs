using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanguine_Forest
{

    ///// <summary>
    ///// Controller for parallax elements to create a smooth movement of background
    ///// </summary>
    //internal class ParallaxManager
    //{
    //    private GraphicsDevice graphicsDevice; // Assuming you have access to GraphicsDevice
    //    private List<ParallaxBackground> backgrounds;


    //    public ParallaxManager(ContentManager Content, GraphicsDevice graphicsDevice)
    //    {
    //        this.graphicsDevice = graphicsDevice;

    //        backgrounds = new List<ParallaxBackground>();

    //        // Assuming each background image is the same width as the screen width
    //        int screenWidth = graphicsDevice.Viewport.Width;
    //        Vector2 position = Vector2.Zero; // Start at the far left

    //        // Centralized position calculation
    //        Vector2 startParallaxPos = new Vector2(-1920 / 2, -1080 / 2);

    //        // Helper method to add backgrounds
    //        AddBackground("Sprites/Background_day_01", Extentions.SpriteLayer.background_Fore, 0.25f, startParallaxPos, Content);
    //        AddBackground("Sprites/Background_day_02", Extentions.SpriteLayer.background_Fore, 0.25f, startParallaxPos, Content);
    //        AddBackground("Sprites/Background_day_03", Extentions.SpriteLayer.background_Fore, 0.25f, startParallaxPos, Content);
    //        AddBackground("Sprites/Background_day_04", Extentions.SpriteLayer.background_Fore, 0.25f, startParallaxPos, Content);
    //        AddBackground("Sprites/Background_day_05", Extentions.SpriteLayer.background_Mid, 0.5f, startParallaxPos, Content);
    //        AddBackground("Sprites/Background_day_06", Extentions.SpriteLayer.background_Mid, 0.5f, startParallaxPos, Content);
    //        AddBackground("Sprites/Background_day_07", Extentions.SpriteLayer.background_Back, 0.75f, startParallaxPos, Content);
    //    }


    //    // Method to add a new background layer to the manager
    //    private void AddBackground(string texturePath, Extentions.SpriteLayer layer, float speed, Vector2 position, ContentManager content)
    //    {
    //        ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texturePath), layer, speed);
    //        backgrounds.Add(background);
    //    }
    //    public void AdjustLayerSpeed(Extentions.SpriteLayer layer, float newSpeed)
    //    {
    //        foreach (var background in backgrounds)
    //        {
    //            if (background.Layer == layer) 
    //            {
    //                background.ParallaxSpeed = newSpeed;
    //            }
    //        }

    //    }

    //    // Update method to adjust the background positions based on player/camera movement
    //    public void UpdateMe(Vector2 deltaMovement)
    //    {
    //        //Debug.WriteLine($"Updating backgrounds with Delta: {deltaMovement}");
    //        //Debug.WriteLine($"Update ParManager");

    //        foreach (var background in backgrounds)
    //        {
    //            background.UpdatePosition(deltaMovement);
    //        }


    //    }
    //    // Draw all backgrounds in the correct order
    //    public void Draw(SpriteBatch spriteBatch)
    //    {

    //        foreach (var background in backgrounds)
    //        {
    //            background.Draw(spriteBatch);
    //        }
    //    }
    //}
    internal class ParallaxManager
    {
        private GraphicsDevice graphicsDevice;
        private List<ParallaxBackground> backgrounds;

        public ParallaxManager(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            backgrounds = new List<ParallaxBackground>();

            InitializeBackgrounds(content);
        }

        private void InitializeBackgrounds(ContentManager content)
        {
            // Start at the far left of the viewport
            int screenWidth = graphicsDevice.Viewport.Width;
            Vector2 FOREposition = new Vector2(-1920 / 2, -1080 / 4); ; // Start at the top-left corner of the viewport

            // Add backgrounds with appropriate offset for seamless looping
            AddBackground("Sprites/Background_day_01", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X += screenWidth;
            AddBackground("Sprites/Background_day_02", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X += screenWidth;
            AddBackground("Sprites/Background_day_03", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X += screenWidth;
            AddBackground("Sprites/Background_day_04", Extentions.SpriteLayer.background_Fore, 0.25f, FOREposition, content);
            FOREposition.X = 0; // Reset for next layer, or manage differently if overlapping differently

            // Mid layer backgrounds
            Vector2 MIDposition = new Vector2(-1920 / 2, -1080 / 3); ; // Start at the top-left corner of the viewport
            AddBackground("Sprites/Background_day_05", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X += screenWidth;
            AddBackground("Sprites/Background_day_05", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X = 0; // Reset for next layer

            // Mid layer backgrounds
            Vector2 MIDposition_2 = new Vector2(-1920 / 2, -1080 / 2); ; // Start at the top-left corner of the viewport
            AddBackground("Sprites/Background_day_06", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X += screenWidth;
            AddBackground("Sprites/Background_day_06", Extentions.SpriteLayer.background_Mid, 0.5f, MIDposition, content);
            MIDposition.X = 0; // Reset for next layer

            // Back layer background
            Vector2 BACKposition = new Vector2(-1920 / 2, -1080 / 2); ; // Start at the top-left corner of the viewport
            AddBackground("Sprites/Background_day_07", Extentions.SpriteLayer.background_Back, 0.75f, BACKposition, content);
            BACKposition.X += screenWidth;
            AddBackground("Sprites/Background_day_07", Extentions.SpriteLayer.background_Back, 0.75f, BACKposition, content);
            BACKposition.X = 0; // Reset for next layer


            // Assuming you loop the backgrounds, this setup assumes each layer has its own independent loop logic if needed
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
            foreach (var background in backgrounds)
            {
                background.UpdatePosition(deltaMovement);
                CheckAndLoopBackground(background);
            }
        }

        private void CheckAndLoopBackground(ParallaxBackground background)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
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
                    float rightEdge = bg.GetPosition().X + graphicsDevice.Viewport.Width;
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
