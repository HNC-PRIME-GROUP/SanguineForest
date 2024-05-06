using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace Sanguine_Forest
{
    /// <summary>
    /// Platform that move after death if the risk level is high
    /// </summary>
    internal class MoveblePlatform : Platform
    {
        private Vector2 startPoint;
        private Vector2 maxShift;

        private Random _rng;
        

        //Visual effects 
        public Color[] changedColor;
        public float speedToCrush;



        public MoveblePlatform(Vector2 position, 
                                float rotation,
                                Vector2 platformSize,
                                ContentManager content,
                                Dictionary<string, Rectangle> tileDictionary,
                                string[,] tileMap,
                                Vector2 maxShift
                                ) : 
                                base(position, rotation, platformSize, content, tileDictionary, tileMap) 
        {            
            
            
            this.startPoint = position;
            this.maxShift = maxShift;

            changedColor = new Color[3]
            {
                Color.Aqua,
                Color.Blue,
                Color.Green                
            };

            _rng = new Random();

            //_spriteModule.SetTexture(content.Load<Texture2D>("Spprites/Fungal - TileSet"));

        }

        public new void UpdateMe()
        {
            base.UpdateMe();

        }
        

        public new void DrawMe(SpriteBatch sp)
        {
            base.DrawMe(sp);
        }

        //Reset the position
        public void MoveMe(float riskLevel)
        {
            position = new Vector2(startPoint.X + (_rng.Next(-1, 1) * maxShift.X*riskLevel), startPoint.Y + (_rng.Next(-1, 1) * maxShift.Y* riskLevel));
            if(riskLevel>0)
            {
                _spriteModule.SetColor(changedColor[_rng.Next(changedColor.Length - 1)]);
            }
        }       

    }

    internal struct MovebalPlatformData 
    {
        public Vector2 position;
        public float rotation;
        public Vector2 platformSize;
        public Vector2 maxShift;

    }
}
