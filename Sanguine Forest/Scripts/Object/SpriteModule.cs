using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Extention;
using System.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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


        //Tilled stuff
        private bool isTilling=false;
        private Rectangle oneTileRectangle;
        private Dictionary<string,Rectangle> tilesDictionary;
        private string[,] TileMap;

        public SpriteModule(GameObject parent, Vector2 shift, Texture2D texture, Extentions.SpriteLayer layer) : base(parent, shift) 
        {
            this.texture= texture;
            this.layer = layer;

            //default
            defaultFrameRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            drawRectangle = defaultFrameRectangle;
            scale = 1f;
            spriteEffect = SpriteEffects.None;

            isTilling = false;
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

        public void TillingMe( Dictionary<string, Rectangle> tileDictionary, string[,] tileMap, Rectangle drawRectangle, Rectangle tileRectangle)
        {
            isTilling = true;            
            this.TileMap = tileMap;
            this.tilesDictionary = tileDictionary;
            this.drawRectangle = tileRectangle;
            this.drawRectangle.Location = GetPosition().ToPoint();
            oneTileRectangle = tileRectangle;

        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="sp"></param>
        public void DrawMe(SpriteBatch sp)
        {
            if (!isTilling)
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
            else
            {
                for(int i=0; i<TileMap.GetLength(0); i++)
                {
                    for(int j=0; j<TileMap.GetLength(1); j++)
                    {
                        

                        sp.Draw(texture, drawRectangle, tilesDictionary[TileMap[i,j]], color, 
                            GetRotation(),Vector2.Zero, spriteEffect, (float)layer/ (float)Extentions.SpriteLayer.Length);
                        //DebugManager.DebugRectangle(drawRectangle);
                        drawRectangle.Location = new Point(drawRectangle.Location.X+oneTileRectangle.Width, drawRectangle.Location.Y);
                        
                    }
                    drawRectangle.Location = new Point(GetPosition().ToPoint().X,drawRectangle.Location.Y+oneTileRectangle.Height);
                }
                drawRectangle.Location = GetPosition().ToPoint();
                
            }

               
              
            }

        

    
        public new void UpdateMe()
        {
            drawRectangle.Location = GetPosition().ToPoint();

            base.UpdateMe();

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
        public new void SetScale(float scale) { 
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
        /// Return the draw rectangle - to get a size and position of it
        /// </summary>
        /// <returns></returns>
        public Rectangle GetDrawRectangle()
        {
            return drawRectangle;
        }

        /// <summary>
        /// Set the default draw rectangle
        /// </summary>
        public void SetDefaultDrawRectangle()
        {
            drawRectangle = defaultFrameRectangle;
        }

        /// <summary>
        /// Set new texture for the 
        /// </summary>
        /// <param name="texture"></param>
        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;   
        }

        public Texture2D GetTexture()
        {
            return texture;
        }


        #endregion
    }

}
