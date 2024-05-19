using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Sanguine_Forest
{
    internal class ParallaxManager
    {
        private Dictionary<Extentions.SpriteLayer, List<ParallaxBackground>> layerBackgrounds;
        private int _screenWidth;
        private int _screenHeight;
        private Camera _camera;


        public ParallaxManager(ContentManager content, Camera camera, Scene scene)
        {

            _screenWidth = Extentions.ScreenWidth;
            _screenHeight = Extentions.ScreenHeight;
            _camera = camera;

            layerBackgrounds = new Dictionary<Extentions.SpriteLayer, List<ParallaxBackground>>();
            InitializeBackgrounds(content, scene);
        }


        private void InitializeBackgrounds(ContentManager content, Scene scene)
        {

            // Define the textures and speeds for each layer
            var layerConfigurations = new Dictionary<Extentions.SpriteLayer, (string[] Textures, float SpeedX, float SpeedY)>
            {
                { Extentions.SpriteLayer.background_Fore, (new[] { "Sprites/Background/Background_day4A", "Sprites/Background/Background_day4D", "Sprites/Background/Background_day4A" },     0.7f, 0.1f) },
                { Extentions.SpriteLayer.background_Fore_Mid, (new[] { "Sprites/Background/Background_day4B", "Sprites/Background/Background_day4C", "Sprites/Background/Background_day4B" }, 0.5f, 0.05f) },
                { Extentions.SpriteLayer.background_Mid, (new[] { "Sprites/Background/Background_day3", "Sprites/Background/Background_day3", "Sprites/Background/Background_day3" },         0.3f, 0.04f) },
                { Extentions.SpriteLayer.background_Mid_Back, (new[] { "Sprites/Background/Background_day2", "Sprites/Background/Background_day2", "Sprites/Background/Background_day2" },    0.25f, 0.01f) },
                { Extentions.SpriteLayer.background_Back, (new[] { "Sprites/Background/Background_day1", "Sprites/Background/Background_day1", "Sprites/Background/Background_day1" },        0.2f, 0.005f) },


                { Extentions.SpriteLayer.underground_Back, (new[] { "Sprites/CaveBackground/Background1", "Sprites/CaveBackground/Background1", "Sprites/CaveBackground/Background1" }, 0.25f, 0.025f) },
                { Extentions.SpriteLayer.underground_Mid, (new[] { "Sprites/CaveBackground/Background2", "Sprites/CaveBackground/Background2", "Sprites/CaveBackground/Background2" }, 0.45f, 0.015f) },
                { Extentions.SpriteLayer.underground_Fore, (new[] { "Sprites/CaveBackground/Background3", "Sprites/CaveBackground/Background3", "Sprites/CaveBackground/Background3" }, 0.75f, 0.01f) }
            };

            foreach (var layerPosition in scene.ParallaxLayerPositions)
            {
                var layer = layerPosition.Key;
                var initialPosition = layerPosition.Value.InitialPosition;

                if (layerConfigurations.TryGetValue(layer, out var config))
                {
                    InitializeLayer(layer, config.Textures, config.SpeedX, config.SpeedY, initialPosition, content);
                }
                else
                {
                    Debug.WriteLine($"No configuration found for layer: {layer}");
                }
            }
        }


    private void InitializeLayer(Extentions.SpriteLayer layer, string[] textures, float speedX, float speedY, Vector2 initialPosition, ContentManager content)
        {
            List<ParallaxBackground> backgrounds = new List<ParallaxBackground>();

            Vector2 position = initialPosition;

            foreach (var texture in textures)

            {
                ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texture), layer, speedX, speedY);

                //background.spriteModule.SetDrawRectangle(new Rectangle(new Vector2(_camera.position.X, _camera.position.Y-100).ToPoint(), new Vector2(_screenWidth,_screenHeight+200).ToPoint()));

                background.spriteModule.SetDrawRectangle(new Rectangle(Vector2.Zero.ToPoint(), new Vector2(_screenWidth, _screenHeight).ToPoint()));
                backgrounds.Add(background);
                position.X += _screenWidth ;  // Position each subsequent background immediately to the right of the last
            }

            layerBackgrounds[layer] = backgrounds;

        }

        public void UpdateMe(Vector2 deltaMovement)
        {

            foreach (var layer in layerBackgrounds)
            {
                List<ParallaxBackground> backgrounds = layer.Value;

                //backgrounds.ForEach(bg => Debug.WriteLine($"Layer {layer.Key} - Before Update - Position: {bg.GetPosition()}"));

                // Determine the current rightmost and leftmost background positions
                float maxRight = float.MinValue;
                float minLeft = float.MaxValue;

                // Update positions based on parallax speed and calculate new edges
                foreach (var background in backgrounds)
                {
                    background.UpdateMe(deltaMovement);
                    //background.SetPosition(new Vector2(background.GetPosition().X, _camera.GetCameraTarget().GetPosition().Y - _screenHeight / 2 - 100));
                    //background.SetPosition(new Vector2(background.GetPosition().X, background.GetPosition().Y));

                    //// Calculate the parallax effect on Y position
                    //float parallaxEffectY = (_camera.GetCameraTarget().GetPosition().Y - _camera.position.Y) * background.ParallaxSpeedY;

                    //// Set new position with parallax effect on Y-axis
                    //background.SetPosition(new Vector2(background.GetPosition().X, -_screenHeight / 2 - parallaxEffectY));

                    float currentRightEdge = background.GetPosition().X + _screenWidth -1 ;
                    float currentLeftEdge = background.GetPosition().X + 1;

                    if (currentRightEdge > maxRight)
                    {
                        maxRight = currentRightEdge;
                    }
                    if (currentLeftEdge < minLeft)
                    {
                        minLeft = currentLeftEdge;
                    }
                }

                // Reposition backgrounds if they move off-screen
                foreach (var background in backgrounds)
                {
                    float originalX = background.GetPosition().X;  // Store original position for logging
                    // Reposition to the right if the background has moved out of the visible area on the left
                    if (background.GetPosition().X + (_screenWidth - 1) * _camera.zoom < -_camera.position.X)
                    {
                        background.SetPosition(new Vector2(maxRight -1, background.GetPosition().Y));
                        //Debug.WriteLine($"Repositioning from {originalX} to {background.GetPosition().X} (right loop)");
                    }
                    // Reposition to the left if the background has moved out of the visible area on the right
                    else if (background.GetPosition().X > -_camera.position.X + (_screenWidth + 1) * _camera.zoom)
                    {
                        background.SetPosition(new Vector2(minLeft - _screenWidth + 1 * _camera.zoom, background.GetPosition().Y));
                        //Debug.WriteLine($"Repositioning from {originalX} to {background.GetPosition().X} (left loop)");
                    }
                    
                }
                
                
                //backgrounds.ForEach(bg => Debug.WriteLine($"Layer {layer.Key} - After Update - Position: {bg.GetPosition()}"));
            }
        }


        public void DrawMe(SpriteBatch spriteBatch)
        {
            foreach (var layer in layerBackgrounds.Values)
            {
                foreach (var background in layer)
                {

                    background.DrawMe(spriteBatch);
                }
            }
        }

        public struct LayerPositionData
        {
            public Vector2 InitialPosition { get; set; }
        }
    }
}

