using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Extention;
using System.Diagnostics;

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

        private static float testXPosition = 0; // Test variable for horizontal movement

        public SpriteModule(GameObject parent, Vector2 shift, Texture2D texture, Extentions.SpriteLayer layer) : base(parent, shift) 
        {
            this.texture= texture;
            this.layer = layer;

            //default
            defaultFrameRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            scale = 1f;
            spriteEffect = SpriteEffects.None;

        }

        /// <summary>
        /// Draw if there is no animation module
        /// </summary>
        /// <param name="sp"></param>
        public void DrawMe(SpriteBatch sp)
        {            

            Vector2 currentPosition = this.GetParent().GetPosition();
            var tmp = (float)layer / (float)Extentions.SpriteLayer.Length;
            sp.Draw(texture, GetPosition(), null, color,
                GetRotation(), Vector2.Zero, scale, spriteEffect, tmp);

            //// Update the position for testing
            //testXPosition -= 0f;
            //// Use the updated test position for drawing
            //Vector2 testPosition = new Vector2(testXPosition, 0);
            //// Draw the texture at the new test position
            //sp.Draw(texture, testPosition, null, color, GetRotation(), Vector2.Zero, scale, spriteEffect, (float)layer);            

        }

        /// <summary>
        /// Draw if there is an animation module
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="animation">Animation module</param>
        public void DrawMe(SpriteBatch sp, AnimationModule animation)
        {

            sp.Draw(texture, GetPosition(), animation.GetFrameRectangle(), color, GetRotation(), Vector2.Zero, scale, spriteEffect, (float)layer / (float)Extentions.SpriteLayer.Length);
        
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
