using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background_day_01" }, 0.45f, content);
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background_day_02" }, 0.45f, content);
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background_day_03" }, 0.45f, content);
            InitializeLayer(Extentions.SpriteLayer.background_Fore, new[] { "Sprites/Background_day_04" }, 0.45f, content);
            InitializeLayer(Extentions.SpriteLayer.background_Mid, new[] { "Sprites/Background_day_05" }, 0.25f, content);
            InitializeLayer(Extentions.SpriteLayer.background_Mid_Back, new[] { "Sprites/Background_day_06" }, 0.15f, content);
            InitializeLayer(Extentions.SpriteLayer.background_Back, new[] { "Sprites/Background_day_07" }, 0.05f, content);

        }

        private void InitializeLayer(Extentions.SpriteLayer layer, string[] textures, float speed, ContentManager content)
        {
            List<ParallaxBackground> backgrounds = new List<ParallaxBackground>();
            Vector2 position = new Vector2(-_camera.position.X, -_camera.position.Y);

            foreach (var texturePath in textures)
            {
                Texture2D texture = content.Load<Texture2D>(texturePath);
                backgrounds.Add(new ParallaxBackground(position, 0, texture, layer, speed));
                position.X += _screenWidth;  // Position each subsequent background immediately to the right of the last
            }

            layerBackgrounds[layer] = backgrounds;
        }


        public void UpdateMe(Vector2 deltaMovement)
        {
            foreach (var layer in layerBackgrounds.Values)
            {
                foreach (var background in layer)
                {
                    background.UpdateMe(deltaMovement);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
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