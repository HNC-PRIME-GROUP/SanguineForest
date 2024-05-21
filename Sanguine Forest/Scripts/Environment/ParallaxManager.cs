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


        public ParallaxManager(ContentManager content, Camera target)
        {
            this.target = target;
            backgrounds = new List<ParallaxBackground>()
            {
                new ParallaxBackground(target.position, 0,content.Load<Texture2D>("Sprites/Background/background_day1"),
                0.1f,0,target),
                new ParallaxBackground(target.position, 0, content.Load<Texture2D>("Sprites/Background/background_day2"), 
                0.2f, 10,target),
                new ParallaxBackground(target.position,0,content.Load<Texture2D>("Sprites/Background/background_day3"),
                0.3f,20, target),
                new ParallaxBackground(target.position,0,content.Load<Texture2D>("Sprites/Background/background_day4A"),
                0.4f,40,target),
                new ParallaxBackground(target.position,0,content.Load<Texture2D>("Sprites/Background/background_day4B"),
                0.5f,50,target),
                new ParallaxBackground(target.position,0,content.Load<Texture2D>("Sprites/Background/background_day4C"),
                0.6f,60,target),
                new ParallaxBackground(target.position,0,content.Load<Texture2D>("Sprites/Background/background_day4D"),
                0.7f,70,target)

            };

        }

       

 
        public void UpdateMe()
        {
            for(int i =0;i<backgrounds.Count;i++)
            {
                backgrounds[i].UpdateMe();
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

