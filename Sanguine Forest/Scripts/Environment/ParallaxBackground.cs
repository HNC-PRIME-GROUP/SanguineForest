using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Sanguine_Forest
{
    internal class ParallaxBackground : GameObject
    {
        public SpriteModule spriteModule;
        public SpriteModule spriteModuleLeft;
        public SpriteModule spriteModuleRight;

        //parallax movement
        float curPos;
        float prevPos;
        private float parallaxSpeed;

        private float shift; 

        private Camera target;


        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, float layer, float parallaxSpeed, Camera target)
            : base(position, rotation)
        {
            spriteModule = new SpriteModule(this, Vector2.Zero, texture, (Extentions.SpriteLayer)layer);
            spriteModuleLeft = new SpriteModule(this, new Vector2(-texture.Width, 0),texture, (Extentions.SpriteLayer)layer);
            spriteModuleRight = new SpriteModule(this, new Vector2(texture.Width*2,0),texture, (Extentions.SpriteLayer)layer);

            this.target = target;
            position = new Vector2(target.position.X-texture.Width/2-100,target.position.Y-texture.Height/2);

        }

        public void UpdateMe()
        {
            curPos = target.position.X;

            spriteModule.UpdateMe();
            shift += (curPos - prevPos) * parallaxSpeed * Extention.Extentions.globalTime;
            position.X += (curPos - prevPos) * parallaxSpeed*Extention.Extentions.globalTime;
            if(shift>spriteModule.GetTexture().Width/2)
            {
                position.X = shift * 2;
                shift = 0;
            }

            prevPos = curPos;
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            spriteModule.DrawMe(spriteBatch);
        }

    }
}
