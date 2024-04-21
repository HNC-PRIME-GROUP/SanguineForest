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

        //Picture scaling
        private float scale;
        //Sprite effect
        private SpriteEffects spriteEffect;
        //Layer
        private Extentions.SpriteLayer layer;

        private static float testXPosition = 0; // Test variable for horizontal movement

        //Animation module to control the spritesheet
        private AnimationModule? animationModule;



        // default rectangle in case if this sprite is not under the animation control
        private Rectangle defaultFrameRectangle;
        // rectangle that actually used 
        private Rectangle drawRectangle;

        public SpriteModule(GameObject parent, Vector2 shift, Texture2D texture, Extentions.SpriteLayer layer) : base(parent, shift) 
        {
            this.texture= texture;
            this.layer = layer;

            //default
            defaultFrameRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            drawRectangle = defaultFrameRectangle;
            scale = 1f;
            spriteEffect = SpriteEffects.None;
        }

        /// <summary>
        /// Initialise the animation module
        /// </summary>
        /// <param name="animationModule"></param>
        public void AnimtaionInitialise (AnimationModule animationModule)
        {
            this.animationModule = animationModule;
            drawRectangle = new Rectangle(drawRectangle.Location,
                new Point((int)Math.Round((float)this.animationModule.GetFrameRectangle().Width * scale),
                (int)Math.Round((float)this.animationModule.GetFrameRectangle().Height * scale)));
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="sp"></param>
        public void DrawMe(SpriteBatch sp)
        {
            if (animationModule is null)
            {
                sp.Draw(texture, drawRectangle, null, color,
                GetRotation(), Vector2.Zero, spriteEffect,
                (float)layer / (float)Extentions.SpriteLayer.Length); // transformlayers to numbers between 0 and 1.
            }
            else
            {
                sp.Draw(texture, drawRectangle, animationModule.GetFrameRectangle(),
                color, GetRotation(), Vector2.Zero, spriteEffect,
                (float)layer / (float)Extentions.SpriteLayer.Length);
            }
               
        }

        /// <summary>
        /// Draw if there is an animation module
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="animation">Animation module</param>
        //public void DrawMe(SpriteBatch sp, AnimationModule animation)
        //{
        //    //terrible work around
        //    Rectangle temp= new Rectangle(drawRectangle.Location, 
        //        new Point((int)Math.Round((float)animation.GetFrameRectangle().Width*scale), 
        //        (int)Math.Round((float)animation.GetFrameRectangle().Height*scale)));

        //    sp.Draw(texture, drawRectangle, animationModule.GetFrameRectangle(), 
        //        color, GetRotation(), Vector2.Zero, spriteEffect, 
        //        (float)layer / (float)Extentions.SpriteLayer.Length);
        //}

        public new void UpdateMe()
        {
            base.UpdateMe();
            drawRectangle.Location = GetPosition().ToPoint();


        }


        #region Get / Set of all parameters for Draw method

        /// <summary>
        /// Set the colour of this spritemodule
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color) {this.color = color;}
        /// <summary>
        /// Get the colour of this spritemodule
        /// </summary>
        /// <returns></returns>
        public Color GetColor(){return color;}

        /// <summary>
        /// Set scale of this spritemodule
        /// </summary>
        /// <returns></returns>
        public float GetScale() { return scale; }
        /// <summary>
        /// Set scale of this spritemodule
        /// </summary>
        /// <param name="scale"></param>
        public void SetScale(float scale) { 
            this.scale = scale;
            drawRectangle.Height = (int)Math.Round((float)drawRectangle.Height * scale);
            drawRectangle.Width = (int)Math.Round((float)drawRectangle.Width * scale);
        }

        /// <summary>
        /// Get the effect for this spritemodule
        /// </summary>
        /// <returns></returns>
        public SpriteEffects GetSpriteEffects() { return spriteEffect; }

        /// <summary>
        /// Set the effect for this spritemodule
        /// </summary>
        /// <param name="spriteEffects"></param>
        public void SetSpriteEffects(SpriteEffects spriteEffects) {  this.spriteEffect = spriteEffects; }

        /// <summary>
        /// Set the draw rectangle size and width/height proportions
        /// </summary>
        /// <param name="drawRectangle"></param>
        public void SetDrawRectangle(Rectangle drawRectangle)
        {
            this.drawRectangle = drawRectangle;
        }

        /// <summary>
        /// Set the default draw rectangle
        /// </summary>
        public void SetDefaultDrawRectangle()
        {
            drawRectangle = defaultFrameRectangle;
        }


        #endregion


    }
}
