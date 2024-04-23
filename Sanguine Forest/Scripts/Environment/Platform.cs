using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Sanguine_Forest
{

    /// <summary>
    /// Platform to jump on it
    /// </summary>
    internal class Platform : GameObject
    {


        //Sprite
        public SpriteModule _spriteModule;

        //Collision
        public PhysicModule platformPhysic;


        public Platform(Vector2 position, float rotation, Vector2 platformSize, ContentManager content ): base(position, rotation) 
        {

            //Sptire and graphic
            _spriteModule = new SpriteModule(this, Vector2.Zero, DebugManager.DebugTexture, Extention.Extentions.SpriteLayer.environment1);

            //Collision
            platformPhysic = new PhysicModule(this, Vector2.Zero, platformSize);
            platformPhysic.isPhysicActive = true;
            _spriteModule.SetDrawRectangle(platformPhysic.GetPhysicRectangle());


            //tiles
            Dictionary<string, Rectangle> tileDictionary = new Dictionary<string, Rectangle>
            {
                {"UpLeft", new Rectangle(0,0,512,512) },
                {"Up", new Rectangle(512,0,512,512) },
                {"UpRight", new Rectangle(1024,0,512,512) },
                {"MidLeft", new Rectangle(0,512,512,512) },
                {"Mid", new Rectangle(512,512,512,512) },
                {"MidRight", new Rectangle(1024,512,512,512) },
                {"BottomLeft", new Rectangle(0,1024,512,512) },
                {"Bottom", new Rectangle(512,1024,512,512) },
                {"BottomRight", new Rectangle(1024,1024,512,512) }
            };

            string[,] tileMap = new string[(int)Math.Round(platformSize.X/64), (int)Math.Round(platformSize.Y/64)];
            for(int i=0;i<tileMap.GetLength(0); i++)
            {
                for(int j=0; j<tileMap.GetLength(1); j++) 
                {
                    if(i==0&&j==0)
                    {
                        tileMap[i, j] = "UpLeft";
                        break;
                    }
                    if(i==0&&j==tileMap.GetLength(1)-1)
                    {
                        tileMap[i, j] = "UpRight";
                        break;
                    }
                    if(i==0&&j>0)
                    {
                        tileMap[i, j] = "Up";
                        break;
                    }
                    if(i==0&&j==0)
                    {
                        tileMap[i, j] = "MidLeft";
                        break;
                    }
                    if(i>0&&j<tileMap.GetLength(1)-1)
                    {
                        tileMap[i, j] = "Mid";
                        break;
                    }



                }
            }
            _spriteModule.TillingMe()

        }

        public new void UpdateMe()
        {   
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch) 
        {
            _spriteModule.DrawMe(spriteBatch);
           // DebugManager.DebugRectangle(platformPhysic.GetPhysicRectangle());
        }

        /// <summary>
        /// Return exactly rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPlatformRectangle()
        {
            return platformPhysic.GetPhysicRectangle();
        }

    }

    internal struct PlatformData
    {
        public Vector2 position;
        public float rotation;
        public Vector2 platformSize;
    }
}
