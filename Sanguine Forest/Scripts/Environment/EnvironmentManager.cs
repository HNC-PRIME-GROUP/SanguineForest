using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Store data about environment on this scene and load them when needed from scene data
    /// </summary>
    internal class EnvironmentManager
    {

        //Content manaher
        private ContentManager content;

        public List<Platform> platforms;
        //private List<MoveblePlatform> movebles;
        //private List<FallingPlatform> falling;
        //private List<Decor> decors;


        //link to content manager
        public EnvironmentManager(ContentManager content) 
        { 
        this.content = content;
        }


        //Creating of all platforms and other environment from the scene
        public void Initialise(Scene scene)
        {
            platforms = new List<Platform>();

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


            for (int p =0; p<scene.simplePlatforms.Count; p++) 
            {
                string[,] tileMap = new string[(int)Math.Round(scene.simplePlatforms[p].platformSize.Y / 128), (int)Math.Round(scene.simplePlatforms[p].platformSize.X / 128)];
                for (int i = 0; i < tileMap.GetLength(0); i++)
                {
                    for (int j = 0; j < tileMap.GetLength(1); j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            tileMap[i, j] = "UpLeft";
                            continue;
                        }
                        if (i == 0 && j == tileMap.GetLength(1) - 1)
                        {
                            tileMap[i, j] = "UpRight";
                            continue;
                        }
                        if (i == 0 && j > 0)
                        {
                            tileMap[i, j] = "Up";
                            continue;
                        }
                        if (i == tileMap.GetLength(0) - 1 && j == 0)
                        {
                            tileMap[i, j] = "BottomLeft";
                            continue;
                        }
                        if (i == tileMap.GetLength(0) - 1 && j == tileMap.GetLength(1) - 1)
                        {
                            tileMap[i, j] = "BottomRight";
                            continue;
                        }
                        if (i == tileMap.GetLength(0) - 1 && j > 0)
                        {
                            tileMap[i, j] = "Bottom";
                            continue;
                        }
                        if (i > 0 && j == 0)
                        {
                            tileMap[i, j] = "MidLeft";
                            continue;
                        }
                        if (i > 0 && j == tileMap.GetLength(1) - 1)
                        {
                            tileMap[i, j] = "MidRight";
                            continue;
                        }
                        if (i > 0 && j > 0)
                        {
                            tileMap[i, j] = "Mid";
                            continue;
                        }
                    }
                }
                platforms.Add(new Platform(scene.simplePlatforms[p].position, scene.simplePlatforms[p].rotation, scene.simplePlatforms[p].platformSize, content, tileDictionary, tileMap));

            }
            

            //movebles = scene.moveblPlatforms;
            //falling = scene.fallingPlatforms;   
            //decors = scene.decors;
        }


        public void UpdateMe()
        {
            //Update for all platforms
            for (int i = 0; i < platforms.Count; i++){platforms[i].UpdateMe();}
            //for (int i = 0; i < movebles.Count; i++) { movebles[i].UpdateMe(); }
            //for (int i = 0; i < falling.Count; i++) falling[i].UpdateMe();

            ////update for decors
            //for(int i =0; i < decors.Count; i++) { decors[i].UpdateMe(); }
        
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < platforms.Count; i++) { platforms[i].DrawMe(spriteBatch); }
            //for (int i = 0;i < movebles.Count; i++) { movebles[i].DrawMe(spriteBatch); }
            //for(int i = 0;i<falling.Count; i++) { falling[i].DrawMe(spriteBatch); }

            //for(int i =0; i < decors.Count; i++) { decors[i].DrawMe(spriteBatch); }

        }

    }
}
