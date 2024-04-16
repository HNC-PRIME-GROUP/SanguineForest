using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Extention;

namespace Sanguine_Forest
{
    /// <summary>
    /// Picture for any game object. 
    /// </summary>
    internal class SpriteModule : Module
    {
        private Texture2D texture;
        private Color color = Color.White;
        // default rectangle in case if this sprite is not under the animation control
        private Rectangle defaultFrameRectangle;
        //Picture scaling
        private float scale;
        //Sprite effect
        private SpriteEffects spriteEffect;
        //Layer
        private Extentions.SpriteLayer layer;

        public SpriteModule(GameObject parent, Vector2 shift, Texture2D texture, Extentions.SpriteLayer layer) : base(parent, shift) 
        {
            this.texture= texture;
            this.layer = layer;

            //default
            defaultFrameRectangle = new Rectangle( (int)Math.Round(GetPosition().X), (int)Math.Round(GetPosition().Y), texture.Width, texture.Height);
            scale = 1f;
            spriteEffect = SpriteEffects.None;

        }

        /// <summary>
        /// Draw if there is no animation module
        /// </summary>
        /// <param name="sp"></param>
        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, GetPosition(), defaultFrameRectangle, color, GetRotation(), Vector2.Zero, scale, spriteEffect, (int)layer); 
        }

        /// <summary>
        /// Draw if there is an animation module
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="animation">Animation module</param>
        public void DrawMe(SpriteBatch sp, AnimationModule animation)
        {
            sp.Draw(texture, GetPosition(), animation.GetFrameRectangle(), color, GetRotation(), Vector2.Zero, scale, spriteEffect, 0);
        }


        #region Get / Set of all parameters for Draw method

        public void SetColor(Color color) {this.color = color;}
        public Color GetColor(){return color;}
        public float GetScale() { return scale; }
        public void SetScale(float scale) {  this.scale = scale; }
        public SpriteEffects GetSpriteEffects() { return spriteEffect; }
        public void SetSpriteEffects(SpriteEffects spriteEffects) {  this.spriteEffect = spriteEffects; }

        #endregion


    }
}
