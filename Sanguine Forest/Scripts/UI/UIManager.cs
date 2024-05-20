using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Sanguine_Forest.Scripts.GameState;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


namespace Sanguine_Forest
{
    internal class UIManager
    {

        public event Action RequestExit;

        public enum GameState
        {
            StartScreen,
            Playing,
            Paused,
            InstructionsFromStart,
            InstructionsFromPause,
        }

        private KeyboardState keyboard;

        public GameState CurrentGameState { get; private set; }
        private GameState previousGameState;

        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private SpriteFont gameFontSmll;
        private SpriteFont gameFontLrg;
        private SpriteFont titleFontSmll;
        private SpriteFont titleFontLrg;

        private float inputDelay = 0.5f; // 200 milliseconds delay
        private float timeSinceLastInput = 0f;

        List<UIButton> startButtons, pauseButtons, instructionButtons;

        int activeButtonIndex = 0;

        private List<string> instructionTexts;
        private List<Vector2> instructionTextPositions;


        private ContentManager content;

        //events for start game and load game
        public event EventHandler NewGameEvent;
        public event EventHandler LoadGameEvent;


        public UIManager(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            LoadContent();
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

            // Measure text size to position it at the center-bottom of the screen
            Vector2 newgameSize = gameFontSmll.MeasureString(newgameText);
            Vector2 loadgameSize = gameFontSmll.MeasureString(loadgameText);
            Vector2 instructionsSize = gameFontSmll.MeasureString(instructionsText);
            Vector2 backSize = gameFontSmll.MeasureString(backText);

            // Calculate positions
            Vector2 newgamePosition = new Vector2((graphicsDevice.Viewport.Width - newgameSize.X) / 2, (graphicsDevice.Viewport.Height - newgameSize.Y) / 2 - 0);
            Vector2 loadgamePosition = new Vector2((graphicsDevice.Viewport.Width - loadgameSize.X) / 2, (graphicsDevice.Viewport.Height - loadgameSize.Y) / 2 + 100);
            Vector2 instructionsPosition = new Vector2((graphicsDevice.Viewport.Width - instructionsSize.X) / 2, (graphicsDevice.Viewport.Height - instructionsSize.Y) / 2 + 200);
            Vector2 backPosition = new Vector2((graphicsDevice.Viewport.Width - backSize.X) / 2, (graphicsDevice.Viewport.Height - backSize.Y) / 2 + 300);


            startButtons = new List<UIButton>
            {
                 new UIButton("NEW GAME", gameFontSmll, newgamePosition),
                 new UIButton("CONTINUE", gameFontSmll, loadgamePosition),
                 new UIButton("INSTRUCTIONS", gameFontSmll, instructionsPosition),
                 new UIButton("EXIT", gameFontSmll, backPosition)

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
            Vector2 saveQuitPosition = new Vector2((graphicsDevice.Viewport.Width - saveQuitSize.X) / 2, (graphicsDevice.Viewport.Height - saveQuitSize.Y) / 2 + 100);
            Vector2 pauseInstructionsPosition = new Vector2((graphicsDevice.Viewport.Width - pauseInstructionsSize.X) / 2, (graphicsDevice.Viewport.Height - pauseInstructionsSize.Y) / 2 + 200);

            pauseButtons = new List<UIButton>
            {
                new UIButton("CONTINUE", gameFontSmll, continuePosition),
                new UIButton("SAVE & QUIT", gameFontSmll, saveQuitPosition),
                new UIButton("INSTRUCTIONS", gameFontSmll, pauseInstructionsPosition),
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

            pauseButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            instructionButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            startButtons[activeButtonIndex].IsActive = true;  // Set the first button as active


            SetupInstructionScreen();


        }



        public void UpdateMe(GameTime gameTime,KeyboardState prev, KeyboardState curr)
        {

            if (previousGameState != CurrentGameState)
            {
                OnStateChanged(previousGameState, CurrentGameState);
                previousGameState = CurrentGameState;
            }

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

            }
        }




        public void DrawMe()
        {
            switch (CurrentGameState)
            {
                case GameState.StartScreen:
                    DrawStartScreen(spriteBatch);
                    break;
                case GameState.Paused:
                    DrawPauseScreen(spriteBatch);
                    break;
                case GameState.InstructionsFromStart:
                    DrawInstructionScreenFromStart(spriteBatch);
                    break;
                case GameState.InstructionsFromPause:
                    DrawInstructionScreenFromPause(spriteBatch);
                    break;

            }
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

                    NewGameEvent?.Invoke(this, EventArgs.Empty);
                    //CurrentGameState = GameState.Playing;
                    

                }
                else if (startButtons[activeButtonIndex].Txt == "CONTINUE")
                {
                    // Restart game logic
                    //  Restart();
                    ResetButtons(startButtons);

                    LoadGameEvent?.Invoke(this, EventArgs.Empty);
                    //CurrentGameState = GameState.LoadGame;

                }
                else if (startButtons[activeButtonIndex].Txt == "INSTRUCTIONS")
                {
                    // Restart game logic
                    //  Restart();
                    ResetButtons(startButtons);
                    CurrentGameState = GameState.InstructionsFromStart;
                }

                else if (startButtons[activeButtonIndex].Txt == "EXIT")
                {
                    // Quit game logic
                    RequestExit?.Invoke();
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
                    // Save and quit logic
                    ResetButtons(pauseButtons);
                    CurrentGameState = GameState.StartScreen;
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
                        ResetButtons(instructionButtons);
                        CurrentGameState = GameState.StartScreen;
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


        private void HandlePlayingInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.P) && prev.IsKeyUp(Keys.P))
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

            //DrawInteractionTextsStartScreen();

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
        " - Accepting challenges during levels (involving alcohol or drugs) will affect gameplay.",
        " - Higher risk levels introduce more obstacles.",
        "",

        "Obstacles:",
        " - Static platforms: solid surfaces to stand on.",
        " - Disappearing platforms: these appear and disappear periodically.",
        " - Thorns: kill you on hit.",
        "",
   
        "Enemies:",
        " - Green slimes: basic enemies that kill you on hit.",
        " - Brown slimes: appear if alcohol consumption is high, they are faster and more challenging."
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
                    // Add cases for other states as needed
            }

            // Start music for the new state
            switch (newState)
            {
                case GameState.StartScreen:
                    AudioManager.PlaySong(0);
                    AudioManager.MusicTrigger(true);
                    break;
                case GameState.Playing:
                    AudioManager.PlaySong(1);
                    AudioManager.MusicTrigger(true);
                    break;
                case GameState.Paused:
                    // Paused state might keep the current song playing or stop it, depending on your design
                    AudioManager.MusicTrigger(false);
                    break;
                    // Add cases for other states as needed
            }
        }

    }


}
