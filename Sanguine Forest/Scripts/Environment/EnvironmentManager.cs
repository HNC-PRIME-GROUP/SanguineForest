using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using Sanguine_Forest.Scripts.GameState;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanguine_Forest
{
    /// <summary>
    /// Store data about environment on this scene and load them when needed from scene data
    /// </summary>
    internal class EnvironmentManager
    {

        //Content manaher
        private ContentManager content;

        //Player state
        private PlayerState playerState;

        public List<Platform> platforms;
        private List<MoveblePlatform> movebles;
        private List<FallingPlatform> fallingPlatforms;
        //private List<Decor> decors;

        //Dangerous
        public List<Thorns> thorns;

        //Obstacles
        public List<Obstacle> obstacles1;
        public List<Obstacle> obstacles2;
        public List<Obstacle> obstacles3;

        //Decor Grass
        public List<Decor> grassDecor;

        //Cutscene option
        public List<CutSceneObject> cutSceneObjects;
        private List<CutSceneDialogue> cutSceneDialogues;
        private SpriteFont font;
        private Texture2D semiTransparentTexture;
        public bool isCutScene;

        private Camera _camera;

        //Environment random
        private Random _rng;



        //link to content manager
        public EnvironmentManager(ContentManager content, PlayerState playerState, Texture2D semiTransparentTexture)
        {
            this.content = content;
            this.semiTransparentTexture = semiTransparentTexture;
            this.playerState = playerState;
            _rng = new Random();

            font = content.Load<SpriteFont>("Fonts/TitleFontSmll");

            if (font == null)
            {
                throw new Exception("Font could not be loaded.");
            }

            if (semiTransparentTexture == null)
            {
                throw new Exception("Semi-transparent texture could not be loaded.");
            }

        }


        //Creating of all platforms and other environment from the scene
        public void Initialise(Scene scene)
        {
            platforms = new List<Platform>();

            //tiles for platforms
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


            for (int p = 0; p < scene.simplePlatforms.Count; p++)
            {
                string[,] tileMap = new string[(int)Math.Round(scene.simplePlatforms[p].platformSize.Y / 64), (int)Math.Round(scene.simplePlatforms[p].platformSize.X / 64)];
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
                        if (i == 0 && j > 0)
                        {
                            tileMap[i, j] = "Up";
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
                platforms.Add(new Platform(scene.simplePlatforms[p].position, scene.simplePlatforms[p].rotation,
                    scene.simplePlatforms[p].platformSize, content, tileDictionary, tileMap));

            }

            //Initialise moveable platforms
            movebles = new List<MoveblePlatform>();

            for (int p = 0; p < scene.moveablPlatforms.Count; p++)
            {
                string[,] tileMap = new string[(int)Math.Round(scene.moveablPlatforms[p].platformSize.Y / 64), (int)Math.Round(scene.moveablPlatforms[p].platformSize.X / 64)];
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
                        if (i == 0 && j > 0)
                        {
                            tileMap[i, j] = "Up";
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
                movebles.Add(new MoveblePlatform(scene.moveablPlatforms[p].position, scene.moveablPlatforms[p].rotation,
                    scene.moveablPlatforms[p].platformSize, content, tileDictionary, tileMap, scene.moveablPlatforms[p].maxShift));
            }

            //thorn loading
            thorns = new List<Thorns>();
            for (int i = 0; i < scene.thorns.Count; i++)
            {
                thorns.Add(new Thorns(scene.thorns[i].Position, scene.thorns[i].Rotation, content, scene.thorns[i].ThornsSize));
            }

            //Falling platforms
            fallingPlatforms = new List<FallingPlatform>();
            for (int p = 0; p < scene.fallingPlatforms.Count; p++)
            {
                string[,] tileMap = new string[(int)Math.Round(scene.fallingPlatforms[p].PlatformSize.Y / 64), (int)Math.Round(scene.fallingPlatforms[p].PlatformSize.X / 64)];
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
                        if (i == 0 && j > 0)
                        {
                            tileMap[i, j] = "Up";
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
                fallingPlatforms.Add(new FallingPlatform(scene.fallingPlatforms[p].Position, scene.fallingPlatforms[p].Rotation,
                    scene.fallingPlatforms[p].PlatformSize, content, tileDictionary, tileMap, scene.fallingPlatforms[p].TimeToFall));
            }


            //Obstacles several lists to make them different levels of difficulty according to risk level 
            obstacles1 = new List<Obstacle>();
            for (int i = 0; i < scene.obstaclesData1.Count; i++)
            {
                obstacles1.Add(new Obstacle(scene.obstaclesData1[i].Position, scene.obstaclesData1[i].Rotation,
                    content, scene.obstaclesData1[i].slimeType, scene.obstaclesData1[i].finPosition,
                    scene.obstaclesData1[i].Speed, scene.obstaclesData1[i].Size));
            }
            obstacles2 = new List<Obstacle>();
            for (int i = 0; i < scene.obstaclesData2.Count; i++)
            {
                obstacles2.Add(new Obstacle(scene.obstaclesData2[i].Position, scene.obstaclesData2[i].Rotation,
                    content, scene.obstaclesData2[i].slimeType, scene.obstaclesData2[i].finPosition,
                    scene.obstaclesData2[i].Speed, scene.obstaclesData2[i].Size));
            }
            obstacles3 = new List<Obstacle>();
            for (int i = 0; i < scene.obstaclesData3.Count; i++)
            {
                obstacles3.Add(new Obstacle(scene.obstaclesData3[i].Position, scene.obstaclesData3[i].Rotation,
                    content, scene.obstaclesData3[i].slimeType, scene.obstaclesData3[i].finPosition,
                    scene.obstaclesData3[i].Speed, scene.obstaclesData3[i].Size));
            }


            //Grass Decor
            grassDecor = new List<Decor>();
            //decor tupes
            string[] Grasstupes = new string[5]
            {
                "Flower",
                "Grass1",
                "Grass2",
                "Grass3",
                "Grass4"
            };
            for (int i = 0; i < platforms.Count; i++)
            {
                Vector2 currGrassPos = platforms[i].GetPosition() - new Vector2(0, 25);
                do
                {
                    int randomTypeId = _rng.Next(0, 5);
                    grassDecor.Add(new Decor(currGrassPos, 0, content, Grasstupes[randomTypeId]));
                    grassDecor[grassDecor.Count - 1]._spriteModule.SetScale(0.5f);
                    grassDecor[grassDecor.Count - 1]._spriteModule.SetSpriteEffects((SpriteEffects)_rng.Next(0, 2));
                    currGrassPos.X += 128;
                }
                while (currGrassPos.X < platforms[i].GetPosition().X + platforms[i].GetPlatformRectangle().Width);


            }

            //for(int i =0;i<scene.decors.Count;i++)
            //{
            //    grassDecor.Add(new Decor(scene.decors[i].Position, scene.decors[i].Rotation, content, scene.decors[i].GrassType));
            //}


            cutSceneObjects = new List<CutSceneObject>();
            isCutScene = scene.isCutScene;
            cutSceneDialogues = scene.cutSceneDialogues ?? new List<CutSceneDialogue>(); // Ensure initialization


            if (scene.isCutScene)
            {
                for (int i = 0; i < scene.cutSceneObjects.Count; i++)
                {
                    var data = scene.cutSceneObjects[i];
                    cutSceneObjects.Add(new CutSceneObject(data.Position, data.Rotation, content, data.NPCType, string.Empty, font));
                }
            }



        }


        public void UpdateMe()
        {
            //Update for all platforms
            for (int i = 0; i < platforms.Count; i++) { platforms[i].UpdateMe(); }
            for (int i = 0; i < movebles.Count; i++) { movebles[i].UpdateMe(); }
            for (int i = 0; i < thorns.Count; i++) { thorns[i].UpdateMe(); }
            for (int i = 0; i < fallingPlatforms.Count; i++) fallingPlatforms[i].UpdateMe();

            //update for obstacles
            for (int i = 0; i < obstacles1.Count; i++) { obstacles1[i].UpdateMe(); }
            for (int i = 0; i < obstacles2.Count; i++) { obstacles2[i].UpdateMe(); }
            for (int i = 0; i < obstacles3.Count; i++) { obstacles3[i].UpdateMe(); }

            ////update for decors
            for (int i = 0; i < grassDecor.Count; i++) { grassDecor[i].UpdateMe(); }

            //Update for cut scene mode
            if (isCutScene)
            {
                for (int i = 0; i < cutSceneObjects.Count; i++)
                {
                    cutSceneObjects[i].UpdateMe();
                }
            }

        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            //platform draw
            for (int i = 0; i < platforms.Count; i++) { platforms[i].DrawMe(spriteBatch); }
            for (int i = 0; i < movebles.Count; i++) { movebles[i].DrawMe(spriteBatch); }
            for (int i = 0; i < fallingPlatforms.Count; i++) { fallingPlatforms[i].DrawMe(spriteBatch); }

            //thorns draw
            for (int i = 0; i < thorns.Count; i++) { thorns[i].DrawMe(spriteBatch); }

            //obstacles draw
            for (int i = 0; i < obstacles1.Count; i++) { obstacles1[i].Draw(spriteBatch); }
            for (int i = 0; i < obstacles2.Count; i++) { obstacles2[i].Draw(spriteBatch); }
            for (int i = 0; i < obstacles3.Count; i++) { obstacles3[i].Draw(spriteBatch); }

            //Decors drawning 
            for (int i = 0; i < grassDecor.Count; i++) { grassDecor[i].DrawMe(spriteBatch); }

            if (isCutScene)
            {
                for (int i = 0; i < cutSceneObjects.Count; i++)
                {
                    cutSceneObjects[i].DrawMe(spriteBatch);
                }

                if (cutSceneDialogues != null)
                {
                    // Draw semi-transparent rectangles behind text
                    foreach (var dialogue in cutSceneDialogues)
                    {
                        Vector2 textPosition = new Vector2(dialogue.Position.X, dialogue.Position.Y); // Ensure Y is non-negative
                        Vector2 textSize = font.MeasureString(dialogue.Text);

                        Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - 5, (int)textPosition.Y - 5, (int)textSize.X + 10, (int)textSize.Y + 10);
                        spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);

                        // Debug log for rectangle
                        Debug.WriteLine($"Drawing rectangle at {backgroundRectangle} for text: {dialogue.Text}");
                    }

                    // Draw the text on top of the rectangles
                    foreach (var dialogue in cutSceneDialogues)
                    {
                        Vector2 textPosition = new Vector2(dialogue.Position.X, dialogue.Position.Y); // Ensure Y is non-negative

                        spriteBatch.DrawString(font, dialogue.Text, textPosition, Color.White);

                        // Debug log for text
                        Debug.WriteLine($"Drawing text: {dialogue.Text} at position {textPosition}");
                    }
                }

                //if (cutSceneDialogues != null)
                //{
                //    // Draw semi-transparent rectangles behind text
                //    foreach (var dialogue in cutSceneDialogues)
                //    {
                //        Vector2 textPosition = new Vector2(dialogue.Position.X, dialogue.Position.Y);
                //        Vector2 textSize = font.MeasureString(dialogue.Text);

                //        Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - 5, (int)textPosition.Y - 5, (int)textSize.X + 10, (int)textSize.Y + 10);
                //        spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);

                //        Debug.WriteLine($"Drawing rectangle at {backgroundRectangle} for text: {dialogue.Text}");
                //    }

                //    // Draw the text on top of the rectangles
                //    foreach (var dialogue in cutSceneDialogues)
                //    {
                //        Vector2 textPosition = new Vector2(dialogue.Position.X, dialogue.Position.Y);
                //        spriteBatch.DrawString(font, dialogue.Text, textPosition, Color.White);

                //        Debug.WriteLine($"Drawing text: {dialogue.Text} at position {textPosition}");
                //    }
                //}
            }
        }






        public void DeathUpdate(object sender, EventArgs e)
        {
            for (int i = 0; i < movebles.Count; i++)
            {
                movebles[i].MoveMe(playerState.RiskLevel);
            }
            for (int i = 0; i < fallingPlatforms.Count; i++)
            {
                fallingPlatforms[i].RandomMyFallingTime(playerState.RiskLevel);
            }
        }

    }
}
