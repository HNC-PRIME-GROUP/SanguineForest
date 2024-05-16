//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Sanguine_Forest.Scripts.Environment.Obstacle;
//using Sanguine_Forest.Scripts.GameState;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;

//namespace Sanguine_Forest
//{
//    /// <summary>
//    /// Store data about environment on this scene and load them when needed from scene data
//    /// </summary>
//    internal class EnvironmentManager
//    {

//        //Content manaher
//        private ContentManager content;

//        //Player state
//        private PlayerState playerState;

//        public List<Platform> platforms;
//        private List<MoveblePlatform> movebles;
//        private List<FallingPlatform> fallingPlatforms;
//        //private List<Decor> decors;

//        //Dangerous
//        public List<Thorns> thorns;

//        //Obstacles
//        public List<Obstacle> obstacles1;
//        public List<Obstacle> obstacles2;
//        public List<Obstacle> obstacles3;

//        //Decor Grass
//        public List<Decor> grassDecor;

//        //Cutscene option
//        public List<CutSceneObject> cutSceneObjects;
//        private List<CutSceneDialogue> cutSceneDialogues;
//        private SpriteFont font;
//        private Texture2D semiTransparentTexture;
//        public bool isCutScene;

//        private Camera _camera;

//        //Environment random
//        private Random _rng;



//        //link to content manager
//        public EnvironmentManager(ContentManager content, PlayerState playerState, Texture2D semiTransparentTexture)
//        {
//            this.content = content;
//            this.semiTransparentTexture = semiTransparentTexture;
//            this.playerState = playerState;
//            _rng = new Random();

//            font = content.Load<SpriteFont>("Fonts/TitleFontSmll");

//            if (font == null)
//            {
//                throw new Exception("Font could not be loaded.");
//            }

//            if (semiTransparentTexture == null)
//            {
//                throw new Exception("Semi-transparent texture could not be loaded.");
//            }

//        }


//        //Creating of all platforms and other environment from the scene
//        public void Initialise(Scene scene)
//        {
//            platforms = new List<Platform>();

//            //tiles for platforms
//            Dictionary<string, Rectangle> tileDictionary = new Dictionary<string, Rectangle>
//            {
//                {"UpLeft", new Rectangle(0,0,512,512) },
//                {"Up", new Rectangle(512,0,512,512) },
//                {"UpRight", new Rectangle(1024,0,512,512) },
//                {"MidLeft", new Rectangle(0,512,512,512) },
//                {"Mid", new Rectangle(512,512,512,512) },
//                {"MidRight", new Rectangle(1024,512,512,512) },
//                {"BottomLeft", new Rectangle(0,1024,512,512) },
//                {"Bottom", new Rectangle(512,1024,512,512) },
//                {"BottomRight", new Rectangle(1024,1024,512,512) }
//            };


//            for (int p = 0; p < scene.simplePlatforms.Count; p++)
//            {
//                string[,] tileMap = new string[(int)Math.Round(scene.simplePlatforms[p].platformSize.Y / 64), (int)Math.Round(scene.simplePlatforms[p].platformSize.X / 64)];
//                for (int i = 0; i < tileMap.GetLength(0); i++)
//                {
//                    for (int j = 0; j < tileMap.GetLength(1); j++)
//                    {
//                        if (i == 0 && j == 0)
//                        {
//                            tileMap[i, j] = "UpLeft";
//                            continue;
//                        }
//                        if (i == 0 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "UpRight";
//                            continue;
//                        }
//                        if (i == tileMap.GetLength(0) - 1 && j == 0)
//                        {
//                            tileMap[i, j] = "BottomLeft";
//                            continue;
//                        }
//                        if (i == tileMap.GetLength(0) - 1 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "BottomRight";
//                            continue;
//                        }
//                        if (i == 0 && j > 0)
//                        {
//                            tileMap[i, j] = "Up";
//                            continue;
//                        }


//                        if (i == tileMap.GetLength(0) - 1 && j > 0)
//                        {
//                            tileMap[i, j] = "Bottom";
//                            continue;
//                        }
//                        if (i > 0 && j == 0)
//                        {
//                            tileMap[i, j] = "MidLeft";
//                            continue;
//                        }
//                        if (i > 0 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "MidRight";
//                            continue;
//                        }
//                        if (i > 0 && j > 0)
//                        {
//                            tileMap[i, j] = "Mid";
//                            continue;
//                        }
//                    }
//                }
//                platforms.Add(new Platform(scene.simplePlatforms[p].position, scene.simplePlatforms[p].rotation,
//                    scene.simplePlatforms[p].platformSize, content, tileDictionary, tileMap));

//            }

//            //Initialise moveable platforms
//            movebles = new List<MoveblePlatform>();

//            for (int p = 0; p < scene.moveablPlatforms.Count; p++)
//            {
//                string[,] tileMap = new string[(int)Math.Round(scene.moveablPlatforms[p].platformSize.Y / 64), (int)Math.Round(scene.moveablPlatforms[p].platformSize.X / 64)];
//                for (int i = 0; i < tileMap.GetLength(0); i++)
//                {
//                    for (int j = 0; j < tileMap.GetLength(1); j++)
//                    {
//                        if (i == 0 && j == 0)
//                        {
//                            tileMap[i, j] = "UpLeft";
//                            continue;
//                        }
//                        if (i == 0 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "UpRight";
//                            continue;
//                        }
//                        if (i == tileMap.GetLength(0) - 1 && j == 0)
//                        {
//                            tileMap[i, j] = "BottomLeft";
//                            continue;
//                        }
//                        if (i == tileMap.GetLength(0) - 1 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "BottomRight";
//                            continue;
//                        }
//                        if (i == 0 && j > 0)
//                        {
//                            tileMap[i, j] = "Up";
//                            continue;
//                        }


//                        if (i == tileMap.GetLength(0) - 1 && j > 0)
//                        {
//                            tileMap[i, j] = "Bottom";
//                            continue;
//                        }
//                        if (i > 0 && j == 0)
//                        {
//                            tileMap[i, j] = "MidLeft";
//                            continue;
//                        }
//                        if (i > 0 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "MidRight";
//                            continue;
//                        }
//                        if (i > 0 && j > 0)
//                        {
//                            tileMap[i, j] = "Mid";
//                            continue;
//                        }
//                    }
//                }
//                movebles.Add(new MoveblePlatform(scene.moveablPlatforms[p].position, scene.moveablPlatforms[p].rotation,
//                    scene.moveablPlatforms[p].platformSize, content, tileDictionary, tileMap, scene.moveablPlatforms[p].maxShift));
//            }

//            //thorn loading
//            thorns = new List<Thorns>();
//            for (int i = 0; i < scene.thorns.Count; i++)
//            {
//                thorns.Add(new Thorns(scene.thorns[i].Position, scene.thorns[i].Rotation, content, scene.thorns[i].ThornsSize));
//            }

//            //Falling platforms
//            fallingPlatforms = new List<FallingPlatform>();
//            for (int p = 0; p < scene.fallingPlatforms.Count; p++)
//            {
//                string[,] tileMap = new string[(int)Math.Round(scene.fallingPlatforms[p].PlatformSize.Y / 64), (int)Math.Round(scene.fallingPlatforms[p].PlatformSize.X / 64)];
//                for (int i = 0; i < tileMap.GetLength(0); i++)
//                {
//                    for (int j = 0; j < tileMap.GetLength(1); j++)
//                    {
//                        if (i == 0 && j == 0)
//                        {
//                            tileMap[i, j] = "UpLeft";
//                            continue;
//                        }
//                        if (i == 0 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "UpRight";
//                            continue;
//                        }
//                        if (i == tileMap.GetLength(0) - 1 && j == 0)
//                        {
//                            tileMap[i, j] = "BottomLeft";
//                            continue;
//                        }
//                        if (i == tileMap.GetLength(0) - 1 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "BottomRight";
//                            continue;
//                        }
//                        if (i == 0 && j > 0)
//                        {
//                            tileMap[i, j] = "Up";
//                            continue;
//                        }


//                        if (i == tileMap.GetLength(0) - 1 && j > 0)
//                        {
//                            tileMap[i, j] = "Bottom";
//                            continue;
//                        }
//                        if (i > 0 && j == 0)
//                        {
//                            tileMap[i, j] = "MidLeft";
//                            continue;
//                        }
//                        if (i > 0 && j == tileMap.GetLength(1) - 1)
//                        {
//                            tileMap[i, j] = "MidRight";
//                            continue;
//                        }
//                        if (i > 0 && j > 0)
//                        {
//                            tileMap[i, j] = "Mid";
//                            continue;
//                        }
//                    }
//                }
//                fallingPlatforms.Add(new FallingPlatform(scene.fallingPlatforms[p].Position, scene.fallingPlatforms[p].Rotation,
//                    scene.fallingPlatforms[p].PlatformSize, content, tileDictionary, tileMap, scene.fallingPlatforms[p].TimeToFall));
//            }


//            //Obstacles several lists to make them different levels of difficulty according to risk level 
//            obstacles1 = new List<Obstacle>();
//            for (int i = 0; i < scene.obstaclesData1.Count; i++)
//            {
//                obstacles1.Add(new Obstacle(scene.obstaclesData1[i].Position, scene.obstaclesData1[i].Rotation,
//                    content, scene.obstaclesData1[i].slimeType, scene.obstaclesData1[i].finPosition,
//                    scene.obstaclesData1[i].Speed, scene.obstaclesData1[i].Size));
//            }
//            obstacles2 = new List<Obstacle>();
//            for (int i = 0; i < scene.obstaclesData2.Count; i++)
//            {
//                obstacles2.Add(new Obstacle(scene.obstaclesData2[i].Position, scene.obstaclesData2[i].Rotation,
//                    content, scene.obstaclesData2[i].slimeType, scene.obstaclesData2[i].finPosition,
//                    scene.obstaclesData2[i].Speed, scene.obstaclesData2[i].Size));
//            }
//            obstacles3 = new List<Obstacle>();
//            for (int i = 0; i < scene.obstaclesData3.Count; i++)
//            {
//                obstacles3.Add(new Obstacle(scene.obstaclesData3[i].Position, scene.obstaclesData3[i].Rotation,
//                    content, scene.obstaclesData3[i].slimeType, scene.obstaclesData3[i].finPosition,
//                    scene.obstaclesData3[i].Speed, scene.obstaclesData3[i].Size));
//            }


//            //Grass Decor
//            grassDecor = new List<Decor>();
//            //decor tupes
//            string[] Grasstupes = new string[5]
//            {
//                "Flower",
//                "Grass1",
//                "Grass2",
//                "Grass3",
//                "Grass4"
//            };
//            for (int i = 0; i < platforms.Count; i++)
//            {
//                Vector2 currGrassPos = platforms[i].GetPosition() - new Vector2(0, 25);
//                do
//                {
//                    int randomTypeId = _rng.Next(0, 5);
//                    grassDecor.Add(new Decor(currGrassPos, 0, content, Grasstupes[randomTypeId]));
//                    grassDecor[grassDecor.Count - 1]._spriteModule.SetScale(0.5f);
//                    grassDecor[grassDecor.Count - 1]._spriteModule.SetSpriteEffects((SpriteEffects)_rng.Next(0, 2));
//                    currGrassPos.X += 128;
//                }
//                while (currGrassPos.X < platforms[i].GetPosition().X + platforms[i].GetPlatformRectangle().Width);


//            }

//            //for(int i =0;i<scene.decors.Count;i++)
//            //{
//            //    grassDecor.Add(new Decor(scene.decors[i].Position, scene.decors[i].Rotation, content, scene.decors[i].GrassType));
//            //}


//            cutSceneObjects = new List<CutSceneObject>();
//            isCutScene = scene.isCutScene;
//            cutSceneDialogues = scene.cutSceneDialogues ?? new List<CutSceneDialogue>(); // Ensure initialization


//            if (scene.isCutScene)
//            {
//                for (int i = 0; i < scene.cutSceneObjects.Count; i++)
//                {
//                    var data = scene.cutSceneObjects[i];
//                    cutSceneObjects.Add(new CutSceneObject(data.Position, data.Rotation, content, data.NPCType, string.Empty, font));
//                }
//            }



//        }


//        public void UpdateMe()
//        {
//            //Update for all platforms
//            for (int i = 0; i < platforms.Count; i++) { platforms[i].UpdateMe(); }
//            for (int i = 0; i < movebles.Count; i++) { movebles[i].UpdateMe(); }
//            for (int i = 0; i < thorns.Count; i++) { thorns[i].UpdateMe(); }
//            for (int i = 0; i < fallingPlatforms.Count; i++) fallingPlatforms[i].UpdateMe();

//            //update for obstacles
//            for (int i = 0; i < obstacles1.Count; i++) { obstacles1[i].UpdateMe(); }
//            for (int i = 0; i < obstacles2.Count; i++) { obstacles2[i].UpdateMe(); }
//            for (int i = 0; i < obstacles3.Count; i++) { obstacles3[i].UpdateMe(); }

//            ////update for decors
//            for (int i = 0; i < grassDecor.Count; i++) { grassDecor[i].UpdateMe(); }

//            //Update for cut scene mode
//            if (isCutScene)
//            {
//                for (int i = 0; i < cutSceneObjects.Count; i++)
//                {
//                    cutSceneObjects[i].UpdateMe();
//                }
//            }

//        }

//        public void DrawMe(SpriteBatch spriteBatch, Matrix cameraTransform)
//        {
//            //platform draw
//            for (int i = 0; i < platforms.Count; i++) { platforms[i].DrawMe(spriteBatch); }
//            for (int i = 0; i < movebles.Count; i++) { movebles[i].DrawMe(spriteBatch); }
//            for (int i = 0; i < fallingPlatforms.Count; i++) { fallingPlatforms[i].DrawMe(spriteBatch); }

//            //thorns draw
//            for (int i = 0; i < thorns.Count; i++) { thorns[i].DrawMe(spriteBatch); }

//            //obstacles draw
//            for (int i = 0; i < obstacles1.Count; i++) { obstacles1[i].Draw(spriteBatch); }
//            for (int i = 0; i < obstacles2.Count; i++) { obstacles2[i].Draw(spriteBatch); }
//            for (int i = 0; i < obstacles3.Count; i++) { obstacles3[i].Draw(spriteBatch); }

//            //Decors drawning 
//            for (int i = 0; i < grassDecor.Count; i++) { grassDecor[i].DrawMe(spriteBatch); }

//            if (isCutScene)
//            {
//                for (int i = 0; i < cutSceneObjects.Count; i++)
//                {
//                    cutSceneObjects[i].DrawMe(spriteBatch);
//                }

//                if (cutSceneDialogues != null)
//                {
//                    // Draw semi-transparent rectangles behind text
//                    foreach (var dialogue in cutSceneDialogues)
//                    {
//                        Vector2 textPosition = Vector2.Transform(new Vector2(dialogue.Position.X, Math.Max(0, dialogue.Position.Y)), cameraTransform);
//                        Vector2 textSize = font.MeasureString(dialogue.Text);

//                        Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - 5, (int)textPosition.Y - 5, (int)textSize.X + 10, (int)textSize.Y + 10);
//                        spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);

//                        // Debug log for rectangle
//                        Debug.WriteLine($"Drawing rectangle at {backgroundRectangle} for text: {dialogue.Text}");
//                    }

//                    // Draw the text on top of the rectangles
//                    foreach (var dialogue in cutSceneDialogues)
//                    {
//                        Vector2 textPosition = Vector2.Transform(new Vector2(dialogue.Position.X, Math.Max(0, dialogue.Position.Y)), cameraTransform);
//                        spriteBatch.DrawString(font, dialogue.Text, textPosition, Color.White);

//                        // Debug log for text
//                        Debug.WriteLine($"Drawing text: {dialogue.Text} at position {textPosition}");
//                    }
//                }

//                //if (cutSceneDialogues != null)
//                //{
//                //    // Draw semi-transparent rectangles behind text
//                //    foreach (var dialogue in cutSceneDialogues)
//                //    {
//                //        Vector2 textPosition = new Vector2(dialogue.Position.X, dialogue.Position.Y);
//                //        Vector2 textSize = font.MeasureString(dialogue.Text);

//                //        Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - 5, (int)textPosition.Y - 5, (int)textSize.X + 10, (int)textSize.Y + 10);
//                //        spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);

//                //        Debug.WriteLine($"Drawing rectangle at {backgroundRectangle} for text: {dialogue.Text}");
//                //    }

//                //    // Draw the text on top of the rectangles
//                //    foreach (var dialogue in cutSceneDialogues)
//                //    {
//                //        Vector2 textPosition = new Vector2(dialogue.Position.X, dialogue.Position.Y);
//                //        spriteBatch.DrawString(font, dialogue.Text, textPosition, Color.White);

//                //        Debug.WriteLine($"Drawing text: {dialogue.Text} at position {textPosition}");
//                //    }
//                //}
//            }
//        }






//        public void DeathUpdate(object sender, EventArgs e)
//        {
//            for (int i = 0; i < movebles.Count; i++)
//            {
//                movebles[i].MoveMe(playerState.RiskLevel);
//            }
//            for (int i = 0; i < fallingPlatforms.Count; i++)
//            {
//                fallingPlatforms[i].RandomMyFallingTime(playerState.RiskLevel);
//            }
//        }

//    }
//}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using Sanguine_Forest.Scripts.GameState;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Sanguine_Forest
{
    internal class EnvironmentManager
    {
        // Content manager
        private ContentManager content;

        // Player state
        private PlayerState playerState;

        public List<Platform> platforms;
        private List<MoveblePlatform> movebles;
        private List<FallingPlatform> fallingPlatforms;
        public List<Thorns> thorns;
        public List<Obstacle> obstacles1;
        public List<Obstacle> obstacles2;
        public List<Obstacle> obstacles3;
        public List<Decor> grassDecor;

        // Cutscene option
        public List<CutSceneObject> cutSceneObjects;
        private List<CutSceneDialogue> cutSceneDialogues;
        private SpriteFont font;
        private Texture2D semiTransparentTexture;
        public bool isCutScene;

        private int currentDialogueIndex = 0;
        private bool isDialogueActive = false;

        private float proximityRange = 200f; // Adjust this value as needed

        public bool IsDialogueActive => isDialogueActive;

        private bool isMovingToNPC = false;
        private Vector2 targetPosition;
        private CutSceneObject nearestNPC;


        private Random _rng;

        public EnvironmentManager(ContentManager content, PlayerState playerState, Texture2D semiTransparentTexture)
        {
            this.content = content;
            this.semiTransparentTexture = semiTransparentTexture;
            this.playerState = playerState;
            _rng = new Random();

            font = content.Load<SpriteFont>("Fonts/TitleFontSmll");

        }

        // Initialize all platforms and other environment from the scene
        public void Initialise(Scene scene)
        {
            platforms = new List<Platform>();

            // Tiles for platforms
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

            foreach (var platform in scene.simplePlatforms)
            {
                string[,] tileMap = CreateTileMap(platform.platformSize);
                platforms.Add(new Platform(platform.position, platform.rotation, platform.platformSize, content, tileDictionary, tileMap));
            }

            movebles = new List<MoveblePlatform>();
            foreach (var platform in scene.moveablPlatforms)
            {
                string[,] tileMap = CreateTileMap(platform.platformSize);
                movebles.Add(new MoveblePlatform(platform.position, platform.rotation, platform.platformSize, content, tileDictionary, tileMap, platform.maxShift));
            }

            thorns = new List<Thorns>();
            foreach (var thorn in scene.thorns)
            {
                thorns.Add(new Thorns(thorn.Position, thorn.Rotation, content, thorn.ThornsSize));
            }

            fallingPlatforms = new List<FallingPlatform>();
            foreach (var platform in scene.fallingPlatforms)
            {
                string[,] tileMap = CreateTileMap(platform.PlatformSize);
                fallingPlatforms.Add(new FallingPlatform(platform.Position, platform.Rotation, platform.PlatformSize, content, tileDictionary, tileMap, platform.TimeToFall));
            }

            obstacles1 = new List<Obstacle>();
            foreach (var obstacle in scene.obstaclesData1)
            {
                obstacles1.Add(new Obstacle(obstacle.Position, obstacle.Rotation, content, obstacle.slimeType, obstacle.finPosition, obstacle.Speed, obstacle.Size));
            }

            obstacles2 = new List<Obstacle>();
            foreach (var obstacle in scene.obstaclesData2)
            {
                obstacles2.Add(new Obstacle(obstacle.Position, obstacle.Rotation, content, obstacle.slimeType, obstacle.finPosition, obstacle.Speed, obstacle.Size));
            }

            obstacles3 = new List<Obstacle>();
            foreach (var obstacle in scene.obstaclesData3)
            {
                obstacles3.Add(new Obstacle(obstacle.Position, obstacle.Rotation, content, obstacle.slimeType, obstacle.finPosition, obstacle.Speed, obstacle.Size));
            }

            //grassDecor = new List<Decor>();
            //foreach (var platform in platforms)
            //{
            //    Vector2 currGrassPos = platform.GetPosition() - new Vector2(0, 25);
            //    while (currGrassPos.X < platform.GetPosition().X + platform.GetPlatformRectangle().Width)
            //    {
            //        int randomTypeId = _rng.Next(0, 5);
            //        grassDecor.Add(new Decor(currGrassPos, 0, content, GetGrassType(randomTypeId)));
            //        grassDecor[^1]._spriteModule.SetScale(0.5f);
            //        grassDecor[^1]._spriteModule.SetSpriteEffects((SpriteEffects)_rng.Next(0, 2));
            //        currGrassPos.X += 128;
            //    }
            //}

            grassDecor = new List<Decor>();
            foreach (var platform in platforms)
            {
                float platformWidth = platform.GetPlatformRectangle().Width;
                float platformStartX = platform.GetPosition().X;
                float platformEndX = platformStartX + platformWidth;

                // Adjust density factor to control the amount of decor
                float densityFactor = 0.1f; // Lower values increase density

                while (true)
                {
                    float randomX = (float)_rng.NextDouble() * platformWidth + platformStartX;
                    int randomTypeId = _rng.Next(0, 5);

                    // Ensure the grass is within the platform's bounds and there's a minimum gap between decorations
                    if (randomX > platformEndX)
                        break;

                    Vector2 randomPosition = new Vector2(randomX, platform.GetPosition().Y - 25);
                    grassDecor.Add(new Decor(randomPosition, 0, content, GetGrassType(randomTypeId)));
                    grassDecor[^1]._spriteModule.SetScale(0.5f);
                    grassDecor[^1]._spriteModule.SetSpriteEffects((SpriteEffects)_rng.Next(0, 2));

                    // Adjust the gap to control the density
                    float minGap = 32f * densityFactor; // Minimum gap between decorations
                    float maxGap = 128f * densityFactor; // Maximum gap between decoration
                    float randomGap = minGap + (float)_rng.NextDouble() * (maxGap - minGap);
                    platformStartX += randomGap;
                }
            }

            // Initialize Cutscene
            cutSceneObjects = new List<CutSceneObject>();
            isCutScene = scene.isCutScene;
            cutSceneDialogues = scene.cutSceneDialogues ?? new List<CutSceneDialogue>();

            if (scene.isCutScene)
            {

                foreach (var data in scene.cutSceneObjects)
                {
                    cutSceneObjects.Add(new CutSceneObject(data.Position, data.Rotation, content, data.NPCType, font, semiTransparentTexture));
                }
            }
        }

        private string[,] CreateTileMap(Vector2 platformSize)
        {
            string[,] tileMap = new string[(int)Math.Round(platformSize.Y / 64), (int)Math.Round(platformSize.X / 64)];
            for (int i = 0; i < tileMap.GetLength(0); i++)
            {
                for (int j = 0; j < tileMap.GetLength(1); j++)
                {
                    if (i == 0 && j == 0) tileMap[i, j] = "UpLeft";
                    else if (i == 0 && j == tileMap.GetLength(1) - 1) tileMap[i, j] = "UpRight";
                    else if (i == tileMap.GetLength(0) - 1 && j == 0) tileMap[i, j] = "BottomLeft";
                    else if (i == tileMap.GetLength(0) - 1 && j == tileMap.GetLength(1) - 1) tileMap[i, j] = "BottomRight";
                    else if (i == 0) tileMap[i, j] = "Up";
                    else if (i == tileMap.GetLength(0) - 1) tileMap[i, j] = "Bottom";
                    else if (j == 0) tileMap[i, j] = "MidLeft";
                    else if (j == tileMap.GetLength(1) - 1) tileMap[i, j] = "MidRight";
                    else tileMap[i, j] = "Mid";
                }
            }
            return tileMap;
        }

        private string GetGrassType(int randomTypeId)
        {
            return randomTypeId switch
            {
                0 => "Flower",
                1 => "Grass1",
                2 => "Grass2",
                3 => "Grass3",
                4 => "Grass4",
                _ => "Grass1"
            };
        }

        public void UpdateMe()
        {
            // Update all platforms
            foreach (var platform in platforms) { platform.UpdateMe(); }
            foreach (var moveble in movebles) { moveble.UpdateMe(); }
            foreach (var thorn in thorns) { thorn.UpdateMe(); }
            foreach (var platform in fallingPlatforms) { platform.UpdateMe(); }

            // Update obstacles
            foreach (var obstacle in obstacles1) { obstacle.UpdateMe(); }
            foreach (var obstacle in obstacles2) { obstacle.UpdateMe(); }
            foreach (var obstacle in obstacles3) { obstacle.UpdateMe(); }

            // Update decors
            foreach (var decor in grassDecor) { decor.UpdateMe(); }

            // Update cut scene mode
            if (isCutScene)
            {
                foreach (var cutSceneObject in cutSceneObjects)
                {
                    cutSceneObject.UpdateMe();
                }

            }
        }

        public void DrawMe(SpriteBatch spriteBatch, Vector2 characterPosition)
        {
            // Draw platforms
            foreach (var platform in platforms) { platform.DrawMe(spriteBatch); }
            foreach (var moveble in movebles) { moveble.DrawMe(spriteBatch); }
            foreach (var platform in fallingPlatforms) { platform.DrawMe(spriteBatch); }

            // Draw thorns
            foreach (var thorn in thorns) { thorn.DrawMe(spriteBatch); }

            // Draw obstacles
            foreach (var obstacle in obstacles1) { obstacle.Draw(spriteBatch); }
            foreach (var obstacle in obstacles2) { obstacle.Draw(spriteBatch); }
            foreach (var obstacle in obstacles3) { obstacle.Draw(spriteBatch); }

            // Draw decors
            foreach (var decor in grassDecor) { decor.DrawMe(spriteBatch); }

            if (isCutScene)
            {
                foreach (var cutSceneObject in cutSceneObjects)
                {
                    cutSceneObject.DrawMe(spriteBatch, false);
                }
            }
        }

        public void DrawCutSceneDialogues(SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            if (isCutScene && cutSceneDialogues != null && cutSceneDialogues.Count > 0 && isDialogueActive)
            {
                foreach (var cutSceneObject in cutSceneObjects)
                {
                    if (!string.IsNullOrEmpty(cutSceneObject.CutsceneText))
                    {
                        var textPosition = Vector2.Transform(cutSceneObject.TextPosition, cameraTransform);
                        var textSize = font.MeasureString(cutSceneObject.CutsceneText);

                        Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - 5, (int)textPosition.Y - 5, (int)textSize.X + 10, (int)textSize.Y + 10);
                        spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);
                        spriteBatch.DrawString(font, cutSceneObject.CutsceneText, textPosition, Color.White);
                    }
                }
            }
        }





        public void DeathUpdate(object sender, EventArgs e)
        {
            foreach (var moveble in movebles)
            {
                moveble.MoveMe(playerState.RiskLevel);
            }
            foreach (var platform in fallingPlatforms)
            {
                platform.RandomMyFallingTime(playerState.RiskLevel);
            }
        }



        public void UpdateCutscene(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Character2 character)
        {
            if (isCutScene && cutSceneDialogues != null && cutSceneDialogues.Count > 0)
            {
                if (currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    if (!isDialogueActive && IsCharacterCloseToNPC(character.GetPosition()))
                    {
                        nearestNPC = GetNearestNPC(character.GetPosition());
                        if (nearestNPC != null)
                        {
                            targetPosition = GetTargetPositionInFrontOfNPC(nearestNPC);
                            isMovingToNPC = true;
                            character.SetTargetPosition(targetPosition);
                            character._currentState = Character2.CharState.walkToTarget; // Set state to walkToTarget
                        }
                    }
                    else if (isDialogueActive || (character.GetCharacterState() == Character2.CharState.idle && !isMovingToNPC))
                    {
                        DisplayNextDialogue();
                    }
                }

                if (isMovingToNPC)
                {
                    if (character.GetCharacterState() != Character2.CharState.walkToTarget)
                    {
                        isMovingToNPC = false;
                        isDialogueActive = true;
                        DisplayNextDialogue();
                    }
                }
            }
        }


        private CutSceneObject GetNearestNPC(Vector2 characterPosition)
        {
            CutSceneObject nearestNPC = null;
            float minDistance = float.MaxValue;

            foreach (var cutSceneObject in cutSceneObjects)
            {
                float distance = Vector2.Distance(characterPosition, cutSceneObject.GetPosition());
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNPC = cutSceneObject;
                }
            }

            return nearestNPC;
        }

        private bool IsCharacterCloseToNPC(Vector2 characterPosition)
        {
            foreach (var cutSceneObject in cutSceneObjects)
            {
                float distance = Vector2.Distance(characterPosition, cutSceneObject.GetPosition());
                if (distance <= proximityRange)
                {
                    return true;
                }
            }
            return false;
        }

        private Vector2 GetTargetPositionInFrontOfNPC(CutSceneObject npc)
        {
            Vector2 npcPosition = npc.GetPosition();
            Vector2 offset = new Vector2(-180, 40); // Adjust the offset as needed to place the character in front of the NPC
            return npcPosition + offset;
        }

        private void DisplayNextDialogue()
        {
            // Clear previous text from all cutscene objects
            foreach (var cutSceneObject in cutSceneObjects)
            {
                cutSceneObject.SetCutsceneText(string.Empty, Vector2.Zero);
            }

            // Set the current dialogue text to the appropriate cutscene object
            if (currentDialogueIndex < cutSceneDialogues.Count)
            {
                var dialogue = cutSceneDialogues[currentDialogueIndex];
                var cutSceneObject = cutSceneObjects[currentDialogueIndex % cutSceneObjects.Count];
                cutSceneObject.SetCutsceneText(dialogue.Text, dialogue.Position);
                currentDialogueIndex++;
            }
            else
            {
                isDialogueActive = false;
                currentDialogueIndex = 0; // Reset index for potential restart
            }
        }

        public void DrawPressEPrompts(SpriteBatch spriteBatch, Matrix cameraTransform, Vector2 characterPosition)
        {
            if (isCutScene && !isDialogueActive)
            {
                foreach (var cutSceneObject in cutSceneObjects)
                {
                    if (IsCharacterCloseToNPC(characterPosition))
                    {
                        string pressEText = "Press E";
                        Vector2 pressETextSize = font.MeasureString(pressEText);
                        var npcScreenPosition = Vector2.Transform(cutSceneObject.GetPosition(), cameraTransform);
                        Vector2 pressETextPosition = new Vector2(npcScreenPosition.X, npcScreenPosition.Y - 30); // Adjust as needed

                        Rectangle backgroundRectangle = new Rectangle((int)pressETextPosition.X - 5, (int)pressETextPosition.Y - 5, (int)pressETextSize.X + 10, (int)pressETextSize.Y + 10);
                        spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);
                        spriteBatch.DrawString(font, pressEText, pressETextPosition, Color.White);
                    }
                }
            }
        }

    }
}
