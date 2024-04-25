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
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/TileSet_Mossy"), Extention.Extentions.SpriteLayer.environment1);

            
            //_spriteModule.SetDrawRectangle(platformPhysic.GetPhysicRectangle());


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

            string[,] tileMap = new string[(int)Math.Round(platformSize.Y / 128), (int)Math.Round(platformSize.X / 128)];
            for(int i=0;i<tileMap.GetLength(0); i++)
            {
                for(int j=0; j<tileMap.GetLength(1); j++) 
                {
                    if(i==0&&j==0)
                    {
                        tileMap[i, j] = "UpLeft";
                        continue;
                    }
                    if(i==0&&j==tileMap.GetLength(1)-1)
                    {
                        tileMap[i, j] = "UpRight";
                        continue;
                    }
                    if(i==0&&j>0)
                    {
                        tileMap[i, j] = "Up";
                        continue;
                    }
                    if(i==tileMap.GetLength(0)-1&&j==0)
                    {
                        tileMap[i, j] = "BottomLeft";
                        continue;
                    }                    
                    if(i==tileMap.GetLength(0)-1&&j==tileMap.GetLength(1)-1)
                    {
                        tileMap[i, j] = "BottomRight";
                        continue;
                    }
                    if(i==tileMap.GetLength(0)-1&& j>0 )
                    {
                        tileMap[i, j] = "Bottom";
                        continue;
                    }
                    if(i>0&&j==0)
                    {
                        tileMap[i, j] = "MidLeft";
                        continue;
                    }
                    if(i>0&&j==tileMap.GetLength(1)-1)
                    {
                        tileMap[i, j] = "MidRight";
                        continue;
                    }
                    if(i>0&&j>0)
                    {
                        tileMap[i, j] = "Mid";
                        continue;
                    }
                }
            }
            _spriteModule.TillingMe(tileDictionary,tileMap,new Rectangle((int)Math.Round(GetPosition().X), (int)Math.Round(GetPosition().Y), 
                (int)Math.Round(platformSize.X / 128)*128, (int)Math.Round(platformSize.Y / 128)*128), new Rectangle(0,0,128,128));

            //Collision
            platformPhysic = new PhysicModule(this, Vector2.Zero, new Vector2((int)Math.Round(platformSize.X / 128)*128, (int)Math.Round(platformSize.Y / 128) * 128));
            platformPhysic.isPhysicActive = true;


        }

        public new void UpdateMe()
        {   
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch) 
        {
            _spriteModule.DrawMe(spriteBatch);
            DebugManager.DebugRectangle(platformPhysic.GetPhysicRectangle());
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
