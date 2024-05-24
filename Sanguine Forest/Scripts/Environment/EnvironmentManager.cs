using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public event EventHandler YesOptionSelected; // New event
        public event EventHandler NoOptionSelected; // New event

        //Checkpoints
        public List<CheckPoint> checkPoints;

        private List<LevelDialogueData> levelDialogues; // New list for level dialogues
        public bool ShowOptions = false;
        private bool isMovingToTrigger = false;


        // Cutscene option
        public List<CutSceneObject> cutSceneObjects;
        private List<CutSceneDialogue> cutSceneDialogues;
        private SpriteFont font;
        private Texture2D semiTransparentTexture;
        public bool isCutScene;


        //Dialogue
        private int currentDialogueIndex = 0;
        private bool isDialogueActive = false;
        private float proximityRange = 300f; // Adjust this value as needed
        public bool IsDialogueActive => isDialogueActive;
        public event EventHandler DialogueEnd;


        private bool isMovingToNPC = false;
        private Vector2 targetPosition;
        private CutSceneObject nearestNPC;

        //Audio
        public AudioSourceModule AudioSourceModule;
        private List<SoundEffectInstance> dialogueSounds;
        private Random _rng;

        //scene
        public Scene currentScene;
        public bool isYesOption;
        public bool isNoOption;


        public EnvironmentManager(ContentManager content, PlayerState playerState, Texture2D semiTransparentTexture)
        {
            this.content = content;
            this.semiTransparentTexture = semiTransparentTexture;
            this.playerState = playerState;
            _rng = new Random();

            font = content.Load<SpriteFont>("Fonts/TitleFontSmll");

            // Initialize dialogue sounds
            dialogueSounds = new List<SoundEffectInstance>
        {
            content.Load<SoundEffect>("Sounds/Dialogue_2").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_3").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_4").CreateInstance(),
            //content.Load<SoundEffect>("Sounds/Dialogue_5").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_6").CreateInstance(),
            //content.Load<SoundEffect>("Sounds/Dialogue_7").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_8").CreateInstance(),
            //content.Load<SoundEffect>("Sounds/Dialogue_9").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_10").CreateInstance(),
            //content.Load<SoundEffect>("Sounds/Dialogue_11").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_12").CreateInstance(),
            content.Load<SoundEffect>("Sounds/Dialogue_13").CreateInstance(),
            //content.Load<SoundEffect>("Sounds/Dialogue_14").CreateInstance()

        };

            YesOptionSelected += OnYesOptionSelected;
            NoOptionSelected += OnNoOptionSelected;


        }

        // Initialize all platforms and other environment from the scene
        public void Initialise(Scene scene)
        {
            currentScene = scene;
            isYesOption = false;
            isNoOption = false;
            isDialogueActive = false;


            //showOptions = false;

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

            if (movebles != null)
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
                if (thorns.Count > 0)
                {
                    for (int i = 0; i < thorns.Count; i++)
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

            if (fallingPlatforms != null)
            {
                if (fallingPlatforms.Count > 0)
                {
                    for (int i = 0; i < fallingPlatforms.Count; i++)
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

            if (obstacles1 != null)
            {
                if (obstacles1.Count > 0)
                {
                    for (int i = 0; i < obstacles1.Count; i++)
                    {
                        obstacles1[i].DeleteMe();
                    }
                }
            }
            obstacles1 = new List<Obstacle>();

            if (obstacles2 != null)
            {
                if (obstacles2.Count > 0)
                {
                    for (int i = 0; i < obstacles2.Count; i++)
                    {
                        obstacles2[i].DeleteMe();
                    }
                }
            }
            obstacles2=new List<Obstacle>();

            if (obstacles3 != null)
            {
                if (obstacles3.Count > 0)
                {
                    for (int i = 0; i < obstacles3.Count; i++)
                    {
                        obstacles3[i].DeleteMe();
                    }
                }
            }
            obstacles3 = new List<Obstacle>();


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


            // Initialize the level end trigger
            if (!scene.isCutScene)
            {
                Vector2 startPosition = scene.levelTrigger.startPosition;
                Vector2 endPosition = scene.levelTrigger.endPosition;
                trigger = new LevelTrigger(startPosition, endPosition, content,
                    (SpriteEffects)scene.levelTrigger.SpriteEffect, font, semiTransparentTexture);
                trigger.LevelEnd += LevelEnd;
                levelDialogues = scene.LevelDialogueData; // Assign the dialogue to the level dialogue list
            }

            //Initialise checkpoints
            if(checkPoints!=null)
            {
                if (checkPoints.Count > 0)
                {
                    for (int i = 0; i < checkPoints.Count; i++)
                    {
                        checkPoints[i].DeleteMe();
                    }
                }
            }
            checkPoints = new List<CheckPoint>();
            if (scene.checkPoints != null)
            {
                if (scene.checkPoints.Count > 0)
                {
                    for (int i = 0; i<scene.checkPoints.Count ; i++)
                    {
                        this.checkPoints.Add(new CheckPoint(scene.checkPoints[i].position, 0, scene.checkPoints[i].size));
                    }
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
            //foreach (var platform in platforms) { platform.UpdateMe(); }
            for (int i = 0; i < platforms.Count; i++) { platforms[i].UpdateMe(); }
            foreach (var moveble in movebles) { moveble.UpdateMe(); }
            foreach (var thorn in thorns) { thorn.UpdateMe(); }
            foreach (var platform in fallingPlatforms) { platform.UpdateMe(); }

            // Update obstacles
            if (isYesOption)
            {
                foreach (var obstacle in obstacles1) { obstacle.UpdateMe(); }
                foreach (var obstacle in obstacles2) { obstacle.UpdateMe(); }
                foreach (var obstacle in obstacles3) { obstacle.UpdateMe(); }
            }

            // Update decors
            foreach (var decor in grassDecor) { decor.UpdateMe(); }

            //Update level triger
            if (!isCutScene) { trigger.UpdateMe(); }


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


            //Check points update
            if(checkPoints!=null)
            {
                for (int i =0;i<checkPoints.Count;i++)
                {
                    checkPoints[i].UpdateMe();
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
            if (isYesOption)
            {
                foreach (var obstacle in obstacles1) { obstacle.Draw(spriteBatch); }
                foreach (var obstacle in obstacles2) { obstacle.Draw(spriteBatch); }
                foreach (var obstacle in obstacles3) { obstacle.Draw(spriteBatch); }
            }

            // Draw decors
            foreach (var decor in grassDecor) { decor.DrawMe(spriteBatch); }

            // Draw patch decors
            foreach (var patchdecor in patchDecors) { patchdecor.DrawMe(spriteBatch); }


            //Draw trigger
            if (!isCutScene)
            {
                bool showPressE = !isDialogueActive && !ShowOptions && trigger.currentState == LevelTrigger.TriggerState.phase1;
                trigger.DrawMe(spriteBatch, showPressE);
                //DebugManager.DebugString($"Trigger Position: {trigger.GetPosition()}", new Vector2(0, 60));
                //DebugManager.DebugString($"Trigger State: {trigger.currentState}", new Vector2(0, 80)); // Add this line to track the state

            }

            if (isCutScene)
            {
                foreach (var cutSceneObject in cutSceneObjects)
                {
                    cutSceneObject.DrawMe(spriteBatch, false);
                }
            }

            //checkpoints draw (for debug purposes)
            if (checkPoints != null)
            {
                for (int i = 0; i < checkPoints.Count; i++)
                {
                    checkPoints[i].DrawMe(spriteBatch);
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

                // Play the corresponding dialogue sound
                if (dialogueSounds.Count > 0)
                {
                    int soundIndex = currentDialogueIndex % dialogueSounds.Count;
                    dialogueSounds[soundIndex].Play();
                }

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

        public void DrawLevelPressE(SpriteBatch spriteBatch, Matrix cameraTransform, Vector2 characterPosition)
        {
            if (!isCutScene && !isDialogueActive)
            {
                bool showPressE = !isDialogueActive && !ShowOptions && trigger.currentState == LevelTrigger.TriggerState.phase1 && Vector2.Distance(characterPosition, trigger.GetPosition()) < proximityRange;


                if (showPressE)
                {
                    string pressEText = "Press E";
                    Vector2 pressETextSize = font.MeasureString(pressEText);
                    var triggerScreenPosition = Vector2.Transform(trigger.GetPosition(), cameraTransform);
                    Vector2 pressETextPosition = new Vector2(triggerScreenPosition.X + pressETextSize.X / 2, triggerScreenPosition.Y - 30); // Adjust as needed

                    Rectangle backgroundRectangle = new Rectangle((int)pressETextPosition.X - 5, (int)pressETextPosition.Y - 5, (int)pressETextSize.X + 10, (int)pressETextSize.Y + 10);
                    spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);
                    spriteBatch.DrawString(font, pressEText, pressETextPosition, Color.White);
                }
            }
            


        }

        public void DrawLevelDialogues(SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            if (levelDialogues != null && levelDialogues.Count > 0 && isDialogueActive && !isCutScene)
            {
                var dialogueData = levelDialogues[currentDialogueIndex];
                var textPosition = Vector2.Transform(dialogueData.Position, cameraTransform);

                string dialogueText = dialogueData.Text;
                Vector2 dialogueTextSize = font.MeasureString(dialogueText);
                Vector2 dialogueTextPosition = new Vector2(textPosition.X - dialogueTextSize.X / 2, textPosition.Y - 50); // Adjust as needed

                Rectangle backgroundRectangle = new Rectangle((int)dialogueTextPosition.X - 5, (int)dialogueTextPosition.Y - 5, (int)dialogueTextSize.X + 10, (int)dialogueTextSize.Y + 10);
                spriteBatch.Draw(semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);
                spriteBatch.DrawString(font, dialogueText, dialogueTextPosition, Color.White);
            }
        }

        public void UpdateLevelDialogues(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Character2 character)
        {
            if (levelDialogues != null && levelDialogues.Count > 0 && !isCutScene)
            {
                if (currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    if (!isDialogueActive && !ShowOptions)
                    {
                        isDialogueActive = true;
                        isMovingToTrigger = true;
                        targetPosition = GetTargetPositionInFrontOfTrigger(trigger.GetPosition());
                        character.SetTargetPosition(targetPosition);
                        character._currentState = Character2.CharState.walkToTarget;
                    }
                    else if (isDialogueActive && !isMovingToTrigger)
                    {
                        currentDialogueIndex++;
                        character._currentState = Character2.CharState.dialogue;
                        if (currentDialogueIndex >= levelDialogues.Count)
                        {
                            isDialogueActive = false;
                            ShowOptions = true;
                            currentDialogueIndex = 0;
                            DialogueEnd?.Invoke(this, EventArgs.Empty);
                            trigger.ShowOptions();
                        }
                    }
                }

                if (isMovingToTrigger)
                {
                    if (character.GetCharacterState() != Character2.CharState.walkToTarget)
                    {
                        isMovingToTrigger = false;
                    }
                }
            }
        }



        private Vector2 GetTargetPositionInFrontOfTrigger(Vector2 triggerPosition)
        {
            return new Vector2(triggerPosition.X - 150, triggerPosition.Y + 130f); // Adjust the offset as needed

        }

        public void DrawOptions(SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            if (ShowOptions)
            {
                string yesText = "(Y)YES\n" +
                    "(N)NO";
                string noText = "";
                Vector2 yesTextSize = font.MeasureString(yesText);
                Vector2 noTextSize = font.MeasureString(noText);
                var yesScreenPosition = Vector2.Transform(trigger.GetPosition(), cameraTransform);
                var noScreenPosition = Vector2.Transform(trigger.GetPosition(), cameraTransform);

                Vector2 yesTextPosition = new Vector2(yesScreenPosition.X - 150, yesScreenPosition.Y - 50);
                Vector2 noTextPosition = new Vector2(noScreenPosition.X - 50, noScreenPosition.Y - 50);

                Rectangle yesBackgroundRectangle = new Rectangle((int)yesTextPosition.X - 5, (int)yesTextPosition.Y - 5, (int)yesTextSize.X + 10, (int)yesTextSize.Y + 10);
                Rectangle noBackgroundRectangle = new Rectangle((int)noTextPosition.X - 5, (int)noTextPosition.Y - 5, (int)noTextSize.X + 10, (int)noTextSize.Y + 10);

                spriteBatch.Draw(semiTransparentTexture, yesBackgroundRectangle, Color.Black * 0.5f);
                spriteBatch.DrawString(font, yesText, yesTextPosition, Color.White);

                spriteBatch.Draw(semiTransparentTexture, noBackgroundRectangle, Color.Black * 0.5f);
                spriteBatch.DrawString(font, noText, noTextPosition, Color.White);
            }
        }


        public void HandleOptionSelection(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            if (ShowOptions)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Y) && !previousKeyboardState.IsKeyDown(Keys.Y))
                {
                    //YesOptionSelected?.Invoke(this, EventArgs.Empty); // Raise the event
                    OnYesOptionSelected(this, EventArgs.Empty);
                    // Handle YES option
                    ShowOptions = false;
                    trigger.MoveToEnd();
                }
                else if (currentKeyboardState.IsKeyDown(Keys.N) && !previousKeyboardState.IsKeyDown(Keys.N))
                {
                    // Handle NO option
                   // OnNoOptionSelected(this, EventArgs.Empty);
                    ShowOptions = false;
                    trigger.MoveToEnd();
                }
            }

        }

        //Level end triggers
        public void LevelEnd(object sender, EventArgs e)
        {
            LevelEndTrigger?.Invoke(sender, e);
        }


        public void OnYesOptionSelected(object sender, EventArgs e)
        {
            isYesOption = true;
            if (obstacles1 != null)
            {
                if (obstacles1.Count > 0)
                {
                    for (int i = 0; i < obstacles1.Count; i++)
                    {
                        obstacles1[i].DeleteMe();
                    }
                }
            }
            obstacles1 = new List<Obstacle>();
            foreach (var obstacle in currentScene.obstaclesData1)
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
            foreach (var obstacle in currentScene.obstaclesData2)
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
            foreach (var obstacle in currentScene.obstaclesData3)
            {
                obstacles3.Add(new Obstacle(obstacle.Position, obstacle.Rotation, content, obstacle.slimeType, obstacle.finPosition, obstacle.Speed, obstacle.Size));
            }


        }

        public void OnNoOptionSelected(object sender, EventArgs e)
        {
            if (obstacles1 != null)
            {
                if (obstacles1.Count > 0)
                {
                    for (int i = 0; i < obstacles1.Count; i++)
                    {
                        obstacles1[i].DeleteMe();
                    }
                }
            }
            obstacles1 = new List<Obstacle>();

            if (obstacles3 != null)
            {
                if (obstacles3.Count > 0)
                {
                    for (int i = 0; i < obstacles3.Count; i++)
                    {
                        obstacles3[i].DeleteMe();
                    }
                }
            }
            obstacles3 = new List<Obstacle>();
    

            if (obstacles2 != null)
            {
                if (obstacles2.Count > 0)
                {
                    for (int i = 0; i < obstacles2.Count; i++)
                    {
                        obstacles2[i].DeleteMe();
                    }
                }
            }
            obstacles2 = new List<Obstacle>();


        }
    }
}
