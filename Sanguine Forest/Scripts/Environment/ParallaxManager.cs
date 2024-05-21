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
        
        private int _screenWidth;
        private int _screenHeight;
        private Camera target;

        //parallax backgrounds
        private List<ParallaxBackground> backgrounds;


        public ParallaxManager(ContentManager content, ref Camera target)
        {
            this.target = target;
            backgrounds = new List<ParallaxBackground>()
            {
                new ParallaxBackground(new Vector2(target.position.X - 2880,0), 0,content.Load<Texture2D>("Sprites/Background/background_day1"),
                7f,0,ref target),
                new ParallaxBackground(new Vector2(target.position.X - 2880,0), 0, content.Load<Texture2D>("Sprites/Background/background_day2"),
                6f, 5,ref target),
                new ParallaxBackground(new Vector2(target.position.X - 2880,0),0,content.Load<Texture2D>("Sprites/Background/background_day3"),
                5f,7,ref target),
                new ParallaxBackground(new Vector2(target.position.X - 2880, 0),0,content.Load<Texture2D>("Sprites/Background/background_day4A"),
                4f,12,ref target),
                new ParallaxBackground(new Vector2(target.position.X - 2880, 0),0,content.Load<Texture2D>("Sprites/Background/background_day4B"),
                3f,14,ref target),
                new ParallaxBackground(new Vector2(target.position.X - 2880, 0),0,content.Load<Texture2D>("Sprites/Background/background_day4C"),
                2f,16,ref target),
                new ParallaxBackground(new Vector2(target.position.X - 2880, 0),0,content.Load<Texture2D>("Sprites/Background/background_day4D"),
                1f,18,ref target)

            };

        }

       

 
        public void UpdateMe(Camera target)
        {
            for(int i =0;i<backgrounds.Count;i++)
            {
                backgrounds[i].UpdateMe(target);
            }
        
        }


        public void DrawMe(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgrounds.Count; i++)
            {
                backgrounds[i].DrawMe(spriteBatch);
            }
        }

        public struct LayerPositionData
        {
            
        }
    }
}

