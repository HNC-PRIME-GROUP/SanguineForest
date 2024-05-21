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


        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, float layer, float parallaxSpeed,  ref Camera target)
            : base(position, rotation)
        {
            spriteModule = new SpriteModule(this, Vector2.Zero, texture, (Extentions.SpriteLayer)layer);
            spriteModuleLeft = new SpriteModule(this, new Vector2(0, 0), texture, (Extentions.SpriteLayer)layer);
            spriteModuleRight = new SpriteModule(this, new Vector2(texture.Width,0),texture, (Extentions.SpriteLayer)layer);
            //spriteModule.SetPosition(new Vector2(-texture.Width,0));
            this.target = target;
            //this.position = position;
            //this.position = new Vector2(target.position.X,target.position.Y);
            this.position = Vector2.Zero;
            this.parallaxSpeed = parallaxSpeed;

        }

        public void UpdateMe(Camera target)
        {
            curPos = target.position.X;

            spriteModule.UpdateMe();
            spriteModuleLeft.UpdateMe();
            //spriteModuleLeft.SetDrawRectangle(new Rectangle(new Point(-spriteModuleLeft.GetTexture().Width, 0), spriteModuleLeft.GetDrawRectangle().Size));
            spriteModuleRight.UpdateMe();
            if(curPos!=prevPos)
            {

            }
            shift += (curPos - prevPos) * parallaxSpeed * Extention.Extentions.globalTime;
            position.X += (curPos - prevPos) * parallaxSpeed*Extention.Extentions.globalTime;
            if(shift>spriteModule.GetTexture().Width)
            {
                position.X -= shift ;
                shift = 0;
            }
            if(shift<-spriteModule.GetTexture().Width)
            {
                position.X -= shift ;
                shift = 0;
            }

            prevPos = curPos;
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            //spriteModule.DrawMe(spriteBatch);
            spriteModuleLeft.DrawMe(spriteBatch);
            //spriteModuleRight.DrawMe(spriteBatch);
            
            DebugManager.DebugRectangle(spriteModuleLeft.GetDrawRectangle());
        }

    }
}
