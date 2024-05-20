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

        private GameObject target;


        public ParallaxBackground(Vector2 position, float rotation, Texture2D texture, Extentions.SpriteLayer layer, float parallaxSpeed, GameObject target)
            : base(position, rotation)
        {
            spriteModule = new SpriteModule(this, Vector2.Zero, texture, layer);
            spriteModuleLeft = new SpriteModule(this, new Vector2(-texture.Width, 0),texture, layer);
            spriteModuleRight = new SpriteModule(this, new Vector2(texture.Width*2,0),texture, layer);

            this.target = target;
            position = new Vector2(target.GetPosition().X-texture.Width/2-100,target.GetPosition().Y-texture.Height/2);

        }

        public void UpdateMe(GameObject target)
        {
            curPos = target.GetPosition().X;

            spriteModule.UpdateMe();
            position.X += (curPos - prevPos) * parallaxSpeed*Extention.Extentions.globalTime;

            prevPos = curPos;
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            spriteModule.DrawMe(spriteBatch);
        }

    }
}
