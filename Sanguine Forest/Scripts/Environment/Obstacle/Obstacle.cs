using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.Environment.Obstacle
{
    internal class Obstacle : GameObject
    {
        private SpriteModule spriteModule; 
        private int alcoholLevelThreshold; // Alcohol level at which this obstacle will be visible

        public Obstacle(Vector2 position, float rotation, Texture2D texture, int alcoholLevelThreshold) : base(position, rotation)
        {
            this.spriteModule = new SpriteModule(this, Vector2.Zero, texture, Extentions.SpriteLayer.obstacles);
            this.alcoholLevelThreshold = alcoholLevelThreshold;
        }
        public void Draw(SpriteBatch spriteBatch, int currentAlcoholLevel)
        {
            if (currentAlcoholLevel >= alcoholLevelThreshold)
            {
                spriteModule.DrawMe(spriteBatch);
            }
        }
    }
}