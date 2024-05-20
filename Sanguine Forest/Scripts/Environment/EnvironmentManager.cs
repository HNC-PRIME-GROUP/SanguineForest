using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using Sanguine_Forest.Scripts.GameState;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanguine_Forest
{
    internal class EnvironmentManager
    {
        // Content manager
        private ContentManager content;

        // Player state
        private PlayerState playerState;

        //Platforms
        public List<Platform> platforms;
        private List<MoveblePlatform> movebles;
        private List<FallingPlatform> fallingPlatforms;
        //Obstacles
        public List<Thorns> thorns;
        public List<Obstacle> obstacles1;
        public List<Obstacle> obstacles2;
        public List<Obstacle> obstacles3;
        //Decor
        public List<Decor> grassDecor;
        public List<PatchDecor> patchDecors;
        //Level end trigger
        public LevelTrigger trigger;

        //Level end trigger
        public event EventHandler LevelEndTrigger;


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
            if (platforms != null)
            {
                if (platforms.Count > 0)
                {
                    for (int i = 0; i < platforms.Count; i++)
                    {
                        platforms[i].DeleteMe();
                    }
                }
            }
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

            if(movebles!=null)
            {
                if (platforms.Count > 0)
                {
                    for (int i = 0; i < movebles.Count; i++)
                    {
                        movebles[i].DeleteMe();
                    }
                }
            }
            movebles = new List<MoveblePlatform>();
            foreach (var platform in scene.moveablPlatforms)
            {
                string[,] tileMap = CreateTileMap(platform.platformSize);
                movebles.Add(new MoveblePlatform(platform.position, platform.rotation, platform.platformSize, content, tileDictionary, tileMap, platform.maxShift));
            }

            if (thorns != null)
            {
                if (thorns.Count>0)
                {
                    for(int i =0;i<thorns.Count;i++)
                    {
                        thorns[i].DeleteMe();
                    }
                }

            }
            thorns = new List<Thorns>();
            foreach (var thorn in scene.thorns)
            {
                thorns.Add(new Thorns(thorn.Position, thorn.Rotation, content, thorn.ThornsSize));
            }

            if(fallingPlatforms != null)
            {
                if(fallingPlatforms.Count>0)
                {
                    for(int i =0; i<fallingPlatforms.Count;i++)
                    {
                        fallingPlatforms[i].DeleteMe();
                    }
                }
            }
            fallingPlatforms = new List<FallingPlatform>();
            foreach (var platform in scene.fallingPlatforms)
            {
                string[,] tileMap = CreateTileMap(platform.PlatformSize);
                fallingPlatforms.Add(new FallingPlatform(platform.Position, platform.Rotation, platform.PlatformSize, content, tileDictionary, tileMap, platform.TimeToFall));
            }

            if(obstacles1 != null)
            {
                if(obstacles1.Count>0)
                {
                    for(int i = 0; i<obstacles1.Count;i++)
                    {
                        obstacles1[i].DeleteMe();
                    }
                }
            }
            obstacles1 = new List<Obstacle>();
            foreach (var obstacle in scene.obstaclesData1)
            {
                obstacles1.Add(new Obstacle(obstacle.Position, obstacle.Rotation, content, obstacle.slimeType, obstacle.finPosition, obstacle.Speed, obstacle.Size));
            }
            if (obstacles2 != null)
            {
                if (obstacles2.Count > 0)
                {
                    for (int i = 0; i < obstacles1.Count; i++)
                    {
                        obstacles2[i].DeleteMe();
                    }
                }
            }
            obstacles2 = new List<Obstacle>();
            foreach (var obstacle in scene.obstaclesData2)
            {
                obstacles2.Add(new Obstacle(obstacle.Position, obstacle.Rotation, content, obstacle.slimeType, obstacle.finPosition, obstacle.Speed, obstacle.Size));
            }
            if (obstacles3 != null)
            {
                if (obstacles3.Count > 0)
                {
                    for (int i = 0; i < obstacles1.Count; i++)
                    {
                        obstacles3[i].DeleteMe();
                    }
                }
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
                float densityFactor = 0.09f; // Lower values increase density

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

            // Initialize patch decors from JSON
            patchDecors = new List<PatchDecor>();
            foreach (var patchData in scene.patchDecors)
            {
                patchDecors.Add(new PatchDecor(patchData.Position, content, patchData.PatchType));
            }

            // Initialize Cutscene
            currentDialogueIndex = 0;
            cutSceneObjects = new List<CutSceneObject>();
            isCutScene = scene.isCutScene;
            cutSceneDialogues = new List<CutSceneDialogue>();
            cutSceneDialogues = scene.cutSceneDialogues ?? new List<CutSceneDialogue>();

            if (scene.isCutScene)
            {

                foreach (var data in scene.cutSceneObjects)
                {
                    cutSceneObjects.Add(new CutSceneObject(data.Position, data.Rotation, content, data.NPCType, font, semiTransparentTexture));
                }
            }

            //initialise the level end trigger
            if (!scene.isCutScene)
            {
             
                trigger = new LevelTrigger(scene.levelTrigger.position, content, (SpriteEffects)scene.levelTrigger.SpriteEffect);
                trigger.LevelEnd += LevelEnd;
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
            //foreach (var platform in platforms) { platform.UpdateMe(); }
            for(int i =0; i<platforms.Count; i++) { platforms[i].UpdateMe(); }
            foreach (var moveble in movebles) { moveble.UpdateMe(); }
            foreach (var thorn in thorns) { thorn.UpdateMe(); }
            foreach (var platform in fallingPlatforms) { platform.UpdateMe(); }

            // Update obstacles
            foreach (var obstacle in obstacles1) { obstacle.UpdateMe(); }
            foreach (var obstacle in obstacles2) { obstacle.UpdateMe(); }
            foreach (var obstacle in obstacles3) { obstacle.UpdateMe(); }

            // Update decors
            foreach (var decor in grassDecor) { decor.UpdateMe(); }

            //Update level triger
            if (!isCutScene) {trigger.UpdateMe();}


            // Update patch decors
            foreach (var decor in patchDecors) { decor.UpdateMe(); }

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

            // Draw patch decors
            foreach (var patchdecor in patchDecors) { patchdecor.DrawMe(spriteBatch); }


            //Draw trigger
            if (!isCutScene) {trigger.DrawMe(spriteBatch);}


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
            Vector2 offset = new Vector2(-180, 45); // Adjust the offset as needed to place the character in front of the NPC
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
                LevelEnd(this, EventArgs.Empty);
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

        //Level end triggers
        public void LevelEnd(object sender, EventArgs e)
        {
            LevelEndTrigger?.Invoke(sender, e);
        }

    }
}
