using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Sanguine_Forest.Scripts.GameState;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


namespace Sanguine_Forest
{
    public class UIManager
    {

        public event Action RequestExit;

        public enum GameState
        {
            StartScreen,
            Playing,
            Paused,
            InstructionsFromStart,
            InstructionsFromPause,
            Credits,
        }

        private KeyboardState keyboard;

        public GameState CurrentGameState { get; private set; }
        public GameState previousGameState;

        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private SpriteFont gameFontSmll;
        private SpriteFont gameFontLrg;
        private SpriteFont titleFontSmll;
        private SpriteFont titleFontLrg;

        private float inputDelay = 0.5f; // 200 milliseconds delay
        private float timeSinceLastInput = 0f;

        List<UIButton> startButtons, pauseButtons, instructionButtons, creditsButtons;

        int activeButtonIndex = 0;

        private List<string> instructionTexts;
        private List<Vector2> instructionTextPositions;

        private List<string> creditsTexts;
        private List<Vector2> creditsTextPositions;

        private ContentManager content;

        //events for start game and load game
        public event EventHandler NewGameEvent;
        public event EventHandler LoadGameEvent;

        //Save and quit event
        public event EventHandler SaveGame;


        public event Action FadeOutComplete;
        private Action onFadeOutAction;

        private float fadeAlpha;
        private float fadeSpeed = 1.0f;
        private bool isFadingIn;
        private bool isFadingOut;
        private Texture2D fadeTexture;


        public UIManager(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            LoadContent();
            fadeTexture = new Texture2D(graphicsDevice, 1, 1);
            fadeTexture.SetData(new[] { Color.Black });
        }

        private void LoadContent()
        {
            gameFontSmll = content.Load<SpriteFont>("Fonts/TrajanRegular"); // 24
            gameFontLrg = content.Load<SpriteFont>("Fonts/GameFontLrg"); // 30
            titleFontSmll = content.Load<SpriteFont>("Fonts/TrajanBold"); // 80
            titleFontLrg = content.Load<SpriteFont>("Fonts/TrajanBold"); // 100

            // Start Screen Buttons
            string newgameText = "NEW GAME";
            string loadgameText = "CONTINUE";
            string instructionsText = "INSTRUCTIONS";
            string backText = "EXIT";
            string creditsText = "CREDITS";


            // Measure text size to position it at the center-bottom of the screen
            Vector2 newgameSize = gameFontSmll.MeasureString(newgameText);
            Vector2 loadgameSize = gameFontSmll.MeasureString(loadgameText);
            Vector2 instructionsSize = gameFontSmll.MeasureString(instructionsText);
            Vector2 backSize = gameFontSmll.MeasureString(backText);
            Vector2 creditsSize = gameFontSmll.MeasureString(creditsText);


            // Calculate positions
            Vector2 newgamePosition = new Vector2((graphicsDevice.Viewport.Width - newgameSize.X) / 2, (graphicsDevice.Viewport.Height - newgameSize.Y) / 2 - 0);
            Vector2 loadgamePosition = new Vector2((graphicsDevice.Viewport.Width - loadgameSize.X) / 2, (graphicsDevice.Viewport.Height - loadgameSize.Y) / 2 + 100);
            Vector2 instructionsPosition = new Vector2((graphicsDevice.Viewport.Width - instructionsSize.X) / 2, (graphicsDevice.Viewport.Height - instructionsSize.Y) / 2 + 200);
            Vector2 backPosition = new Vector2((graphicsDevice.Viewport.Width - backSize.X) / 2, (graphicsDevice.Viewport.Height - backSize.Y) / 2 + 400);
            Vector2 creditsPosition = new Vector2((graphicsDevice.Viewport.Width - creditsSize.X) / 2, (graphicsDevice.Viewport.Height - creditsSize.Y) / 2 + 300);



            startButtons = new List<UIButton>
            {
                 new UIButton("NEW GAME", gameFontSmll, newgamePosition),
                 new UIButton("CONTINUE", gameFontSmll, loadgamePosition),
                 new UIButton("INSTRUCTIONS", gameFontSmll, instructionsPosition),
                 new UIButton("CREDITS", gameFontSmll, creditsPosition),
                 new UIButton("EXIT", gameFontSmll, backPosition),


            };

            // Pause Screen Buttons
            string continueText = "CONTINUE";
            string saveQuitText = "SAVE & QUIT";
            string pauseInstructionsText = "INSTRUCTIONS";

            // Measure text size to position it at the center-bottom of the screen
            Vector2 continueSize = gameFontSmll.MeasureString(continueText);
            Vector2 saveQuitSize = gameFontSmll.MeasureString(saveQuitText);
            Vector2 pauseInstructionsSize = gameFontSmll.MeasureString(pauseInstructionsText);

            // Calculate positions
            Vector2 continuePosition = new Vector2((graphicsDevice.Viewport.Width - continueSize.X) / 2, (graphicsDevice.Viewport.Height - continueSize.Y) / 2 - 0);
            Vector2 saveQuitPosition = new Vector2((graphicsDevice.Viewport.Width - saveQuitSize.X) / 2, (graphicsDevice.Viewport.Height - saveQuitSize.Y) / 2 + 200);
            Vector2 pauseInstructionsPosition = new Vector2((graphicsDevice.Viewport.Width - pauseInstructionsSize.X) / 2, (graphicsDevice.Viewport.Height - pauseInstructionsSize.Y) / 2 + 100);

            pauseButtons = new List<UIButton>
            {
                new UIButton("CONTINUE", gameFontSmll, continuePosition),
                new UIButton("INSTRUCTIONS", gameFontSmll, pauseInstructionsPosition),
                new UIButton("SAVE & QUIT", gameFontSmll, saveQuitPosition),
            };


            // Instruction Screen Buttons
            string instructionBackText = "BACK";

            // Measure text size to position it at the center-bottom of the screen
            Vector2 instructionBackSize = gameFontSmll.MeasureString(instructionBackText);

            // Calculate position
            Vector2 instructionBackPosition = new Vector2((graphicsDevice.Viewport.Width - instructionBackSize.X) / 2, (graphicsDevice.Viewport.Height -  100));

            instructionButtons = new List<UIButton>
            {
                new UIButton("BACK", gameFontSmll, instructionBackPosition)
            };


            // Instruction Screen Buttons
            string criditBackText = "BACK";

            // Measure text size to position it at the center-bottom of the screen
            Vector2 creditBackSize = gameFontSmll.MeasureString(criditBackText);

            // Calculate position
            Vector2 creditBackPosition = new Vector2((graphicsDevice.Viewport.Width - creditBackSize.X) / 2, (graphicsDevice.Viewport.Height - 100));

            creditsButtons = new List<UIButton>
            {
                new UIButton("BACK", gameFontSmll, creditBackPosition)
            };

            pauseButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            instructionButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            startButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            creditsButtons[activeButtonIndex].IsActive = true;  // Set the first button as active



            SetupInstructionScreen();
            SetupCreditText();


        }



        public void UpdateMe(GameTime gameTime,KeyboardState prev, KeyboardState curr)
        {

            if (previousGameState != CurrentGameState)
            {
                OnStateChanged(previousGameState, CurrentGameState);
                previousGameState = CurrentGameState;
            }

            UpdateFade(gameTime);

            if (IsFading) return;


            switch (CurrentGameState)
            {

                case GameState.StartScreen:
                    HandleStartScreenInput(gameTime, prev, curr);
                    break;
                case GameState.Playing:
                    HandlePlayingInput(prev, curr);
                    break;
                case GameState.Paused:
                    HandlePauseScreenInput(gameTime, prev, curr); 
                    break;
                case GameState.InstructionsFromStart:
                    HandleInstructionScreenInputFromStart(gameTime, prev, curr);
                    break;
                case GameState.InstructionsFromPause:
                    HandleInstructionScreenInputFromPause(gameTime, prev, curr);
                    break;
                case GameState.Credits:
                    HandleCreditsScreenInput(gameTime, prev, curr);
                    break;

            }
        }


        public void DrawMe()
        {
            bool shouldDrawStartScreen = CurrentGameState == GameState.StartScreen && (!isFadingOut || isFadingIn);

            if (shouldDrawStartScreen)
            {
                DrawStartScreen(spriteBatch);
            }
            else
            {
                // Handle other states or do nothing if not in the relevant states
                switch (CurrentGameState)
                {
                    case GameState.Paused:
                        DrawPauseScreen(spriteBatch);
                        break;
                    case GameState.InstructionsFromStart:
                        DrawInstructionScreenFromStart(spriteBatch);
                        break;
                    case GameState.InstructionsFromPause:
                        DrawInstructionScreenFromPause(spriteBatch);
                        break;
                    case GameState.Credits:
                        DrawCreditsScreen(spriteBatch);
                        break;
                }
            }

            // Draw the fade overlay
            DrawFade();
        }

        private void HandleStartScreenInput(GameTime gameTime, KeyboardState prev, KeyboardState curr)
        {


            timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastInput > inputDelay)
            {
                if (curr.IsKeyDown(Keys.Down) && prev.IsKeyUp(Keys.Down))
                {
                    startButtons[activeButtonIndex].IsActive = false;
                    activeButtonIndex = (activeButtonIndex + 1) % startButtons.Count;
                    startButtons[activeButtonIndex].IsActive = true;
                    timeSinceLastInput = 0f;
                }
                else if (curr.IsKeyDown(Keys.Up) && prev.IsKeyUp(Keys.Up))
                {
                    startButtons[activeButtonIndex].IsActive = false;
                    activeButtonIndex = (activeButtonIndex - 1 + startButtons.Count) % startButtons.Count;
                    startButtons[activeButtonIndex].IsActive = true;
                    timeSinceLastInput = 0f;
                }
            }
            if (curr.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter))
            {
                if (startButtons[activeButtonIndex].Txt == "NEW GAME")
                {
                    ResetButtons(startButtons);
                    StartFadeOut(() =>
                    {
                        NewGameEvent?.Invoke(this, EventArgs.Empty);
                    });
                }
                else if (startButtons[activeButtonIndex].Txt == "CONTINUE")
                {
                    ResetButtons(startButtons);
                    StartFadeOut(() =>
                    {
                        LoadGameEvent?.Invoke(this, EventArgs.Empty);
                    });
                }
                else if (startButtons[activeButtonIndex].Txt == "INSTRUCTIONS")
                {
                    StartFadeOut(() =>
                    {
                        ResetButtons(startButtons);
                        CurrentGameState = GameState.InstructionsFromStart;
                    });
                }
                else if (startButtons[activeButtonIndex].Txt == "EXIT")
                {
                    StartFadeOut(() =>
                    {
                        RequestExit?.Invoke();
                    });
                }
                else if (startButtons[activeButtonIndex].Txt == "CREDITS")
                {
                    StartFadeOut(() =>
                    {
                        ResetButtons(startButtons);
                        CurrentGameState = GameState.Credits;
                    });
                }
            }

            foreach (var button in startButtons)
            {
                button.Update();
            }
        }

        private void HandlePauseScreenInput(GameTime gameTime, KeyboardState prev, KeyboardState curr)
        {
            timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastInput > inputDelay)
            {
                if (curr.IsKeyDown(Keys.Down) && prev.IsKeyUp(Keys.Down))
                {
                    pauseButtons[activeButtonIndex].IsActive = false;
                    activeButtonIndex = (activeButtonIndex + 1) % pauseButtons.Count;
                    pauseButtons[activeButtonIndex].IsActive = true;
                    timeSinceLastInput = 0f;
                }
                else if (curr.IsKeyDown(Keys.Up) && prev.IsKeyUp(Keys.Up))
                {
                    pauseButtons[activeButtonIndex].IsActive = false;
                    activeButtonIndex = (activeButtonIndex - 1 + pauseButtons.Count) % pauseButtons.Count;
                    pauseButtons[activeButtonIndex].IsActive = true;
                    timeSinceLastInput = 0f;
                }
            }
            if (curr.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter))
            {
                if (pauseButtons[activeButtonIndex].Txt == "CONTINUE")
                {
                    ResetButtons(pauseButtons);
                    CurrentGameState = GameState.Playing;
                }
                else if (pauseButtons[activeButtonIndex].Txt == "SAVE & QUIT")
                {
                    ResetButtons(pauseButtons);
                    StartFadeOut(() =>
                    {
                        SaveGame?.Invoke(this, EventArgs.Empty);
                        CurrentGameState = GameState.StartScreen;
                    });
                }
                else if (pauseButtons[activeButtonIndex].Txt == "INSTRUCTIONS")
                {
                    ResetButtons(pauseButtons);
                    CurrentGameState = GameState.InstructionsFromPause;
                }

            }
            foreach (var button in pauseButtons)
            {
                button.Update();
            }
        }


        private void HandleInstructionScreenInputFromStart(GameTime gameTime, KeyboardState prev, KeyboardState curr)
        {
            timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastInput > inputDelay)
            {
                if (curr.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter))
                {
                    if (instructionButtons[0].Txt == "BACK")
                    {
                        StartFadeOut(() =>
                        {
                            ResetButtons(instructionButtons);
                            CurrentGameState = GameState.StartScreen;
                        });
                    }
                }
            }
            foreach (var button in instructionButtons)
            {
                button.Update();
            }
        }
        private void HandleInstructionScreenInputFromPause(GameTime gameTime, KeyboardState prev, KeyboardState curr)
        {
            timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastInput > inputDelay)
            {
                if (curr.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter))
                {
                    if (instructionButtons[0].Txt == "BACK")
                    {
                        ResetButtons(instructionButtons);
                        CurrentGameState = GameState.Paused;
                    }
                }
            }
            foreach (var button in instructionButtons)
            {
                button.Update();
            }
        }

        private void HandleCreditsScreenInput(GameTime gameTime, KeyboardState prev, KeyboardState curr)
        {
            timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastInput > inputDelay)
            {
                if (curr.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter))
                {
                    if (instructionButtons[0].Txt == "BACK")
                    {
                        StartFadeOut(() =>
                        {
                            ResetButtons(creditsButtons);
                            CurrentGameState = GameState.StartScreen;
                        });
                    }
                }
            }
            foreach (var button in creditsButtons)
            {
                button.Update();
            }
        }


        private void HandlePlayingInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.Escape) && prev.IsKeyUp(Keys.Escape))
            {

                CurrentGameState = GameState.Paused;

            }
        }


        private void DrawInstructionsScreen(string screenTitle)
        {
            Vector2 size = titleFontLrg.MeasureString(screenTitle);
            Vector2 position = new Vector2((graphicsDevice.Viewport.Width - size.X) / 2, 50);  // Top center of the screen
            spriteBatch.DrawString(titleFontLrg, screenTitle, position, Color.White);
        }

        private void DrawTitleScreen(string screenTitle)
        {
            Vector2 size = titleFontLrg.MeasureString(screenTitle);
            Vector2 position = new Vector2((graphicsDevice.Viewport.Width - size.X) / 2, 200);  // Top center of the screen
            spriteBatch.DrawString(titleFontLrg, screenTitle, position, Color.White);
        }



        private void DrawStartScreen(SpriteBatch sb)
        {
            DrawTitleScreen("Sanguine Forest");

            foreach (var button in startButtons)
            {
                button.Draw(sb);
            }


        }

        private void DrawPauseScreen(SpriteBatch sb)
        {
            DrawTitleScreen("Paused");

            foreach (var button in pauseButtons)
            {
                button.Draw(sb);
            }
        }

        private void DrawInstructionScreenFromStart(SpriteBatch sb)
        {
            DrawInstructionsScreen("Instructions");

            for (int i = 0; i < instructionTexts.Count; i++)
            {
                sb.DrawString(gameFontLrg, instructionTexts[i], instructionTextPositions[i], Color.White);
            }

            foreach (var button in instructionButtons)
            {
                button.Draw(sb);
            }

        }

        private void DrawInstructionScreenFromPause(SpriteBatch sb)
        {

            DrawInstructionsScreen("Instructions");

            for (int i = 0; i < instructionTexts.Count; i++)
            {
                sb.DrawString(gameFontLrg, instructionTexts[i], instructionTextPositions[i], Color.White);
            }

            foreach (var button in instructionButtons)
            {
                button.Draw(sb);
            }

        }

        private void DrawCreditsScreen(SpriteBatch sb)
        {

            DrawInstructionsScreen("THE END");

            for (int i = 0; i < creditsTexts.Count; i++)
            {
                sb.DrawString(gameFontLrg, creditsTexts[i], creditsTextPositions[i], Color.White);
            }

            foreach (var button in instructionButtons)
            {
                button.Draw(sb);
            }

        }

        private void ResetButtons(List<UIButton> btns)
        {
            if (btns == null || btns.Count == 0) return;

            // Deactivate current active button
            btns[activeButtonIndex].IsActive = false;

            // Reset the active button index
            activeButtonIndex = 0;

            // Activate the first button
            btns[activeButtonIndex].IsActive = true;
        }

        private void SetupInstructionScreen()
        {
            instructionTexts = new List<string>
    {
        "Movement Controls:",
        " - Use A and D keys to move left and right.",
        " - Use W key to jump.",
        "",

        "Wall Clinging and Jumping:",
        " - Jump towards a wall to cling onto it.",
        " - Press W to jump off the wall. Wall jumps have more strength.",
        "",

        "Challenges and Consequences:",
        " - At the beginning of each level, you'll face a critical choice: Yes or No.",
        " - Choose wisely, as your decision will have significant consequences.",
        " - Opting for \"Yes\" might introduce unexpected challenges, including the appearance of adversaries. ",
        " - Remember, every choice shapes your journey.",
        "",

        "Obstacles:",
        " - Static platforms: solid surfaces to stand on.",
        " - Disappearing platforms: these appear and disappear periodically.",
        " - Thorns: kill you on hit.",
        "",
        
        "Enemies:",
        " - Slimes: kill you on hit.",
    };

            // Calculate positions for the instruction text
            instructionTextPositions = new List<Vector2>();
            float yOffset = 200; // Start position for the first line of text
            foreach (var text in instructionTexts)
            {
                instructionTextPositions.Add(new Vector2(300, yOffset));
                yOffset += 35; // Line spacing
            }
        }

        private void SetupCreditText()
        {
            creditsTexts = new List<string>
    {
        "                             DEVELOPERS",
        "",
        "           - Lead Programmer: Iurii Kupreev",
        "",
        "           - Lead Designer: Ryan Brisbane",
        "",
        "           - Lead Artist: Alberto Rodriguez Franco",
        "",
        "This game uses free assets except for the characters. ",
        "Special thanks to the artists and platforms providing ",
        "these assets free of charge:",
        "",
        "       - Platforms and slimes enemies: Maaot (maaout.itch.io)",
        "       - Background, grass decoration, and disappearing platforms: ",
        "         Szadi art (szadiart.itch.io)",
        "       - Characters and NPCs: Lead Artist",
        "       - Character Sound Effects: Pixabay (pixabay.com)",
        "       - Background Music: - Surreal Forest by Meydan - (linktr.ee/meydan)",
        "                                     - O X L2 X -> O R1 - (chosic.com)",
        "",
        "Thank you for playing our game. We hope you enjoy it!",
        "",

    };

            // Calculate positions for the instruction text
            creditsTextPositions = new List<Vector2>();
            float yOffset = 150; // Start position for the first line of text
            foreach (var text in creditsTexts)
            {
                creditsTextPositions.Add(new Vector2(550, yOffset));
                yOffset += 38; // Line spacing
            }
        }
        private void OnStateChanged(GameState oldState, GameState newState)
        {
            // Stop music from the previous state
            switch (oldState)
            {
                case GameState.StartScreen:
                    AudioManager.MusicTrigger(false);
                    break;
                case GameState.Playing:
                    AudioManager.MusicTrigger(false);
                    break;
            }

            // Start music for the new state
            switch (newState)
            {
                case GameState.StartScreen:
                    StartFadeIn();
                    AudioManager.PlaySong(0);
                    AudioManager.MusicTrigger(true);
                    break;
                case GameState.Playing:
                    StartFadeIn();
                    AudioManager.PlaySong(1);
                    AudioManager.MusicTrigger(true);
                    break;
                case GameState.Paused:
                    AudioManager.MusicTrigger(false);
                    break;
                case GameState.InstructionsFromStart:
                    StartFadeIn();
                    break;
                case GameState.Credits:
                    StartFadeIn();
                    break;
            }
        }

        public void SetGameState(GameState newState)
        {
            previousGameState = CurrentGameState;
            CurrentGameState = newState;
            
        }

        public void StartFadeIn()
        {
            isFadingIn = true;
            isFadingOut = false;
            fadeAlpha = 1;
        }

        public void StartFadeOut(Action onComplete)
        {
            isFadingOut = true;
            isFadingIn = false;
            fadeAlpha = 0; 
            onFadeOutAction = onComplete;
        }

        private void UpdateFade(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isFadingIn)
            {
                fadeAlpha -= fadeSpeed * delta;
                if (fadeAlpha <= 0)
                {
                    fadeAlpha = 0;
                    isFadingIn = false;
                }
            }
            else if (isFadingOut)
            {
                fadeAlpha += fadeSpeed * delta;
                if (fadeAlpha >= 1)
                {
                    fadeAlpha = 1;
                    isFadingOut = false;
                    onFadeOutAction?.Invoke();
                }
            }
        }

        private void DrawFade()
        {
            if (fadeAlpha > 0)
            {
                spriteBatch.Draw(fadeTexture, new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight), Color.Black * fadeAlpha);
            }
        }

        private bool IsFading => isFadingIn || isFadingOut;

        public bool ShouldDrawScreen()
        {
            return CurrentGameState == GameState.StartScreen && (!isFadingOut || isFadingIn);
        }

    }




}
