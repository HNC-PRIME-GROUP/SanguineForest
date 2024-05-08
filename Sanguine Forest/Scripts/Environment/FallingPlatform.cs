using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System;

namespace Sanguine_Forest
{

    /// <summary>
    /// Platform that can fall if character step on it
    /// </summary>
    internal class FallingPlatform : Platform
    {
        private float timeToFall;
        private AnimationModule animationModule;

        //falling platform states
        private enum FallState
        {
            Stand,
            Falling,
            Pause,
            Restoring
        }

        private FallState state;

        //fall timers
        private float maxTimer;
        private float currTimer;


        public FallingPlatform(Vector2 position, 
                                float rotation, 
                                Vector2 platformSize, 
                                ContentManager content,                                 
                                Dictionary<string,Rectangle> tileDictionary,
                                string[,] tileMap,
                                float timeToFall
                                ): 
            base (position, rotation, platformSize, content, tileDictionary, tileMap ) 
        {
            _spriteModule.SetTexture(content.Load<Texture2D>("Sprites/Fungal - TileSet"));
            maxTimer=timeToFall;
            state = FallState.Stand;
            this.timeToFall=timeToFall;
        
        }

        public new  void UpdateMe()
        {
            base.UpdateMe();
            switch(state)
            {
                case FallState.Stand:

                    break;
                case FallState.Falling:
                    currTimer -= Extention.Extentions.globalTime;
                    Color currColor = _spriteModule.GetColor();
                    currColor.A -= (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    currColor.R -= (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    currColor.G -= (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    currColor.B -= (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    _spriteModule.SetColor(currColor);
                    if(currTimer<=0)
                    {
                        platformPhysic.isPhysicActive = false;
                        currColor = Color.Transparent;
                        _spriteModule.SetColor(currColor);
                        currTimer = timeToFall;
                        state= FallState.Pause;
                    }
                    break;
                case FallState.Pause:
                    currTimer -= Extention.Extentions.globalTime;
                    if(currTimer<=0)
                    {
                        currTimer= timeToFall;
                        state= FallState.Restoring;
                    }
                    break;
                case FallState.Restoring:
                    currTimer -=Extention.Extentions.globalTime;
                    Color currColorBack = _spriteModule.GetColor();
                    currColorBack.A += (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    //currColorBack.R += (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    currColorBack.G += (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    currColorBack.B += (byte)Math.Round(Math.Clamp((255 / timeToFall * Extention.Extentions.globalTime), 1, byte.MaxValue));
                    _spriteModule.SetColor(currColorBack);
                    if(currTimer<=0)
                    {
                        platformPhysic.isPhysicActive = true;
                        state = FallState.Stand;
                        currColorBack = Color.White;
                        _spriteModule.SetColor(currColorBack);
                    }
                    break;
            }


        }

        public override void Collided(Collision collision)
        {
            if(collision.GetCollidedPhysicModule().GetParent() is Character2&& state == FallState.Stand)
            {
                state = FallState.Falling;
                currTimer = timeToFall;

            }


        }
    }

    public struct FallingPlatformData
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 PlatformSize;
        public float TimeToFall;
    }
}
