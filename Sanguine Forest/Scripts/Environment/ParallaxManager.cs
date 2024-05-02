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


        public ParallaxManager(ContentManager content, Camera camera)
        {

            _screenWidth = Extentions.ScreenWidth;
            _screenHeight = Extentions.ScreenHeight;
            _camera = camera;

            layerBackgrounds = new Dictionary<Extentions.SpriteLayer, List<ParallaxBackground>>();
            InitializeBackgrounds(content);
        }

        private void InitializeBackgrounds(ContentManager content)
        {
            // Initialize each layer individually
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background_day_01", "Sprites/Background_day_02", "Sprites/Background_day_01" }, 0.55f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Fore_Mid, new[] { "Sprites/Background_day_03", "Sprites/Background_day_04", "Sprites/Background_day_03" }, 0.35f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Mid, new[] { "Sprites/Background_day_05", "Sprites/Background_day_05", "Sprites/Background_day_05" }, 0.25f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Mid_Back, new[] { "Sprites/Background_day_06", "Sprites/Background_day_06", "Sprites/Background_day_06" }, 0.15f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Back, new[] { "Sprites/Background_day_07", "Sprites/Background_day_07", "Sprites/Background_day_07" }, 0.05f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
        }
        private void InitializeLayer(Extentions.SpriteLayer layer, string[] textures, float speed, Vector2 initialPosition, ContentManager content, Camera camera2)
        {
            List<ParallaxBackground> backgrounds = new List<ParallaxBackground>();

            Vector2 position = initialPosition;

            foreach (var texture in textures)

            {
                ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texture), layer, speed, camera2);
                background.spriteModule.SetDrawRectangle(new Rectangle(_camera.position.ToPoint(), new Vector2(_screenWidth,_screenHeight).ToPoint()));
                backgrounds.Add(background);
                position.X += _screenWidth;  // Position each subsequent background immediately to the right of the last
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
                    float currentRightEdge = background.GetPosition().X + _screenWidth;
                    float currentLeftEdge = background.GetPosition().X;

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
                    if (background.GetPosition().X + _screenWidth * _camera.zoom < -_camera.position.X)
                    {
                        background.SetPosition(new Vector2(maxRight , background.GetPosition().Y));
                        //Debug.WriteLine($"Repositioning from {originalX} to {background.GetPosition().X} (right loop)");
                    }
                    // Reposition to the left if the background has moved out of the visible area on the right
                    else if (background.GetPosition().X > -_camera.position.X + _screenWidth * _camera.zoom)
                    {
                        background.SetPosition(new Vector2(minLeft - _screenWidth * _camera.zoom, background.GetPosition().Y));
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
    }
}

