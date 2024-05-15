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
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background/Background_day4C", "Sprites/Background/Background_day4D", "Sprites/Background/Background_day4C" }, 0.55f, 0.0f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Fore_Mid, new[] { "Sprites/Background/background_day4A", "Sprites/Background/background_day4B", "Sprites/Background/background_day4A" }, 0.35f, 0.0f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Mid, new[] { "Sprites/Background/background_day3", "Sprites/Background/background_day3", "Sprites/Background/background_day3" }, 0.25f, 0.025f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Mid_Back, new[] { "Sprites/Background/background_day2", "Sprites/Background/background_day2", "Sprites/Background/background_day2" }, 0.15f, 0.015f, new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.background_Back, new[] { "Sprites/Background/background_day1", "Sprites/Background/background_day1", "Sprites/Background/background_day1" }, 0.05f, 0.01f ,new Vector2(-_camera.position.X, -_camera.position.Y), content, _camera);

            InitializeLayer(Extentions.SpriteLayer.underground_Back, new[] { "Sprites/CaveBackground/Background1", "Sprites/CaveBackground/Background1", "Sprites/CaveBackground/Background1" }, 0.25f, 0.025f, new Vector2(-_camera.position.X, -_camera.position.Y + 1080), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.underground_Mid, new[] { "Sprites/CaveBackground/Background2", "Sprites/CaveBackground/Background2", "Sprites/CaveBackground/Background2" }, 0.45f, 0.015f, new Vector2(-_camera.position.X, -_camera.position.Y + 1080), content, _camera);
            InitializeLayer(Extentions.SpriteLayer.underground_Fore, new[] { "Sprites/CaveBackground/Background3", "Sprites/CaveBackground/Background3", "Sprites/CaveBackground/Background3" }, 0.75f, 0.01f, new Vector2(-_camera.position.X, -_camera.position.Y + 1080), content, _camera);

        }
        private void InitializeLayer(Extentions.SpriteLayer layer, string[] textures, float speedX, float speedY, Vector2 initialPosition, ContentManager content, Camera camera2)
        {
            //List<ParallaxBackground> backgrounds = new List<ParallaxBackground>();

            //Vector2 position = initialPosition;

            //foreach (var texture in textures)

            //{
            //    ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texture), layer, speedX, speedY, camera2);
            //    background.spriteModule.SetDrawRectangle(new Rectangle(new Vector2(_camera.position.X, _camera.position.Y).ToPoint(), new Vector2(_screenWidth,_screenHeight).ToPoint()));
            //    backgrounds.Add(background);
            //    position.X += _screenWidth;  // Position each subsequent background immediately to the right of the last
            //}

            //layerBackgrounds[layer] = backgrounds;

            List<ParallaxBackground> backgrounds = new List<ParallaxBackground>();

            Vector2 position = initialPosition;

            foreach (var texture in textures)
            {
                ParallaxBackground background = new ParallaxBackground(position, 0, content.Load<Texture2D>(texture), layer, speedX, speedY, camera2);
                background.spriteModule.SetDrawRectangle(new Rectangle(
                    (int)position.X,
                    (int)position.Y - background.spriteModule.GetDrawRectangle().Height,
                    _screenWidth,
                    _screenHeight 
                ));
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
                    //background.SetPosition(new Vector2(background.GetPosition().X, _camera.GetCameraTarget().GetPosition().Y - _screenHeight / 2 - 100));
                    background.SetPosition(new Vector2(background.GetPosition().X, background.GetPosition().Y));

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

