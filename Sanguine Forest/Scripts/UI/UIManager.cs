﻿using Microsoft.Xna.Framework.Graphics;
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
            GameOver,
            Win,
            Transitioning,
            Stats
        }

        private KeyboardState keyboard;
        public GameState CurrentGameState;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private SpriteFont gameFontSmll;
        private SpriteFont gameFontLrg;
        private SpriteFont titleFontSmll;
        private SpriteFont titleFontLrg;

        private float inputDelay = 0.5f; // 200 milliseconds delay
        private float timeSinceLastInput = 0f;

        List<UIButton> startButtons, pauseButtons, loadButtons, gameOverButtons, winButtons, instructionButtons;

        int activeButtonIndex = 0;


        private ContentManager content;


        public UIManager(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            LoadContent();
        }

        private void LoadContent()
        {
            gameFontSmll = content.Load<SpriteFont>("Fonts/GameFont"); // 24
            gameFontLrg = content.Load<SpriteFont>("Fonts/GameFontLrg"); // 30
            titleFontSmll = content.Load<SpriteFont>("Fonts/TitleFontSmll"); // 80
            titleFontLrg = content.Load<SpriteFont>("Fonts/TitleFontLrg"); // 100

            //Load Buttons
            string newgameText = "NEW GAME";
            string loadgameText = "LOAD GAME";
            string instructionsText = "INSTRUCTIONS";
            string backText = "EXIT";

            // Measure text size to position it at the center-bottom of the screen
            Vector2 newgameSize = gameFontSmll.MeasureString(newgameText);
            Vector2 loadgameSize = gameFontSmll.MeasureString(loadgameText);
            Vector2 instructionsSize = gameFontSmll.MeasureString(instructionsText);
            Vector2 backSize = gameFontSmll.MeasureString(backText);

            // Calculate positions
            Vector2 newgamePosition = new Vector2((graphicsDevice.Viewport.Width - newgameSize.X) / 2, (graphicsDevice.Viewport.Height - newgameSize.Y) / 2 - 100);
            Vector2 loadgamePosition = new Vector2((graphicsDevice.Viewport.Width - loadgameSize.X) / 2, (graphicsDevice.Viewport.Height - loadgameSize.Y) / 2 + 0);
            Vector2 instructionsPosition = new Vector2((graphicsDevice.Viewport.Width - instructionsSize.X) / 2, (graphicsDevice.Viewport.Height - instructionsSize.Y) / 2 + 100);
            Vector2 backPosition = new Vector2((graphicsDevice.Viewport.Width - backSize.X) / 2, (graphicsDevice.Viewport.Height - backSize.Y) / 2 + 200);


            startButtons = new List<UIButton>
            {
                 new UIButton("NEW GAME", gameFontSmll, newgamePosition),
                 new UIButton("LOAD GAME", gameFontSmll, loadgamePosition),
                 new UIButton("INSTRUCTIONS", gameFontSmll, instructionsPosition),
                 new UIButton("EXIT", gameFontSmll, backPosition)

            };
            //pauseButtons = new List<UIButton>
            //{
            //     new UIButton("RESOLUTION", gameFontSmll, RelativePosition(5, 20), GraphicsDevice),
            //     new UIButton("BRIGHTNESS", gameFontSmll, RelativePosition(5, 30), GraphicsDevice),
            //     new UIButton("BACK", gameFontSmll, RelativePosition(5, 40), GraphicsDevice),

            //};
            //loadButtons = new List<UIButton>
            //{
            //    new UIButton("800x600", gameFontSmll, RelativePosition(5, 20), GraphicsDevice),
            //    new UIButton("1024x768", gameFontSmll, RelativePosition(5, 30), GraphicsDevice),
            //    new UIButton("1280x720", gameFontSmll, RelativePosition(5, 40), GraphicsDevice),
            //    new UIButton("1920x1080", gameFontSmll, RelativePosition(5, 50), GraphicsDevice),
            //    new UIButton("BACK", gameFontSmll, RelativePosition(5, 60), GraphicsDevice),
            //};
            //gameOverButtons = new List<UIButton>
            //{
            //    new UIButton("PlAY", gameFontSmll, RelativePosition(5, 20), GraphicsDevice),
            //    new UIButton("RESTART", gameFontSmll, RelativePosition(5, 30), GraphicsDevice),
            //    new UIButton("OPTIONS", gameFontSmll, RelativePosition(5, 40), GraphicsDevice),
            //    new UIButton("BACK TO TITLE SCREEN", gameFontSmll, RelativePosition(5, 50), GraphicsDevice),
            //    new UIButton("EXIT TO DESKTOP", gameFontSmll, RelativePosition(5, 60), GraphicsDevice),
            //};
            //winButtons = new List<UIButton>
            //{
            //    new UIButton("PlAY", gameFontSmll, RelativePosition(5, 20), GraphicsDevice),
            //    new UIButton("RESTART", gameFontSmll, RelativePosition(5, 30), GraphicsDevice),
            //    new UIButton("OPTIONS", gameFontSmll, RelativePosition(5, 40), GraphicsDevice),
            //    new UIButton("BACK TO TITLE SCREEN", gameFontSmll, RelativePosition(5, 50), GraphicsDevice),
            //    new UIButton("EXIT TO DESKTOP", gameFontSmll, RelativePosition(5, 60), GraphicsDevice),
            //};
            //instructionButtons = new List<UIButton>
            //{
            //    new UIButton("PlAY", gameFontSmll, RelativePosition(5, 20), GraphicsDevice),
            //    new UIButton("RESTART", gameFontSmll, RelativePosition(5, 30), GraphicsDevice),
            //    new UIButton("OPTIONS", gameFontSmll, RelativePosition(5, 40), GraphicsDevice),
            //    new UIButton("BACK TO TITLE SCREEN", gameFontSmll, RelativePosition(5, 50), GraphicsDevice),
            //    new UIButton("EXIT TO DESKTOP", gameFontSmll, RelativePosition(5, 60), GraphicsDevice),
            //};

            //pauseButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            //loadButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            //gameOverButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            //winButtons[activeButtonIndex].IsActive = true;  // Set the first button as active
            //instructionButtons[activeButtonIndex].IsActive = true;  // Set the first button as active

            startButtons[activeButtonIndex].IsActive = true;  // Set the first button as active

        }


        public void UpdateMe(GameTime gameTime,KeyboardState prev, KeyboardState curr)
        {
            switch (CurrentGameState)
            {

                case GameState.StartScreen:
                    HandleStartScreenInput(gameTime, prev, curr);
                    break;
                case GameState.Playing:
                    HandlePlayingInput(prev, curr);
                    break;
                case GameState.Paused:
                    HandlePausedInput(prev, curr);
                    break;
                case GameState.Win:
                    HandleWinInput(prev, curr);
                    break;
                case GameState.GameOver:
                    HandleGameOverInput(prev, curr);
                    break;
                case GameState.Transitioning:
                    HandleTransitioningInput(prev, curr);
                    break;
                case GameState.Stats:
                    HandleStatsInput(prev, curr);
                    break;
            }
        }

        private void HandleStartScreenInput(GameTime gameTime, KeyboardState prev, KeyboardState curr)
        {
            //if (curr.IsKeyDown(Keys.A) && prev.IsKeyUp(Keys.A))
            //    CurrentGameState = GameState.Playing;

            //if (curr.IsKeyDown(Keys.B) && prev.IsKeyUp(Keys.B))
            //    RequestExit?.Invoke(); //  exit the game

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
                    CurrentGameState = GameState.Playing;
                }
                else if (startButtons[activeButtonIndex].Txt == "LOAD GAME")
                {
                    // Restart game logic
                    //  Restart();
                    ResetButtons(startButtons);
                    //CurrentGameState = GameState.LoadGame;
                }
                else if (startButtons[activeButtonIndex].Txt == "INSTRUCTIONS")
                {
                    // Restart game logic
                    //  Restart();
                    ResetButtons(startButtons);
                    //CurrentGameState = GameState.LoadGame;
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

        private void HandlePlayingInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.P))
                CurrentGameState = GameState.Paused;

        }

        private void HandlePausedInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.A) && prev.IsKeyUp(Keys.A))
                CurrentGameState = GameState.Playing;
            if (curr.IsKeyDown(Keys.B) && prev.IsKeyUp(Keys.B))
                CurrentGameState = GameState.StartScreen;
        }

        private void HandleWinInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.A) && prev.IsKeyUp(Keys.A))
                CurrentGameState = GameState.Stats;
        }

        private void HandleGameOverInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.A) && prev.IsKeyUp(Keys.A))
                CurrentGameState = GameState.Stats;
        }

        private void HandleTransitioningInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.A) && prev.IsKeyUp(Keys.A))
                CurrentGameState = GameState.Playing;
        }

        private void HandleStatsInput(KeyboardState prev, KeyboardState curr)
        {
            if (curr.IsKeyDown(Keys.A) && prev.IsKeyUp(Keys.A))
                CurrentGameState = GameState.StartScreen;
            if (curr.IsKeyDown(Keys.B) && prev.IsKeyUp(Keys.B))
                RequestExit();
        }

        public void DrawMe()
        {
            switch (CurrentGameState)
            {
                case GameState.StartScreen:
                    DrawStartScreen(spriteBatch);
                    break;
                case GameState.Playing:
                    DrawPlayingScreen();
                    break;
                case GameState.Paused:
                    DrawPausedScreen();
                    break;
                case GameState.Win:
                    DrawWinScreen();
                    break;
                case GameState.GameOver:
                    DrawGameOverScreen();
                    break;
                case GameState.Transitioning:
                    DrawTransitionScreen();
                    break;
                case GameState.Stats:
                    DrawStatsScreen();
                    break;
            }
        }

        private void DrawScreen(string screenTitle)
        {
            Vector2 size = gameFontLrg.MeasureString(screenTitle);
            Vector2 position = new Vector2((graphicsDevice.Viewport.Width - size.X) / 2, 100);  // Top center of the screen
            spriteBatch.DrawString(gameFontLrg, screenTitle, position, Color.White);
        }

        private void DrawInteractionTexts()
        {
            string okText = "Press A - OK";
            string backText = "Press B - Back";

            // Measure text size to position it at the center-bottom of the screen
            Vector2 okSize = gameFontSmll.MeasureString(okText);
            Vector2 backSize = gameFontSmll.MeasureString(backText);

            // Calculate positions
            Vector2 okPosition = new Vector2((graphicsDevice.Viewport.Width - okSize.X) / 2, graphicsDevice.Viewport.Height - okSize.Y - 50);
            Vector2 backPosition = new Vector2((graphicsDevice.Viewport.Width - backSize.X) / 2, graphicsDevice.Viewport.Height - backSize.Y - 10);

            // Draw text
            spriteBatch.DrawString(gameFontSmll, okText, okPosition, Color.White);
            spriteBatch.DrawString(gameFontSmll, backText, backPosition, Color.White);
        }

        private void DrawInteractionTextsStartScreen()
        {
            string newgameText = "N - NEW GAME";
            string loadgameText = "L - LOAD GAME";
            string instructionsText = "I - INSTRUCTIONS";
            string backText = "ESC - Exit";

            // Measure text size to position it at the center-bottom of the screen
            Vector2 newgameSize = gameFontSmll.MeasureString(newgameText);
            Vector2 loadgameSize = gameFontSmll.MeasureString(loadgameText);
            Vector2 instructionsSize = gameFontSmll.MeasureString(instructionsText);
            Vector2 backSize = gameFontSmll.MeasureString(backText);

            // Calculate positions
            Vector2 newgamePosition = new Vector2((graphicsDevice.Viewport.Width - newgameSize.X) / 2, (graphicsDevice.Viewport.Height - newgameSize.Y) / 2 - 100);
            Vector2 loadgamePosition = new Vector2((graphicsDevice.Viewport.Width - loadgameSize.X) / 2 , (graphicsDevice.Viewport.Height - loadgameSize.Y) / 2 + 0);
            Vector2 instructionsPosition = new Vector2((graphicsDevice.Viewport.Width - instructionsSize.X) / 2, (graphicsDevice.Viewport.Height - instructionsSize.Y) / 2 + 100);
            Vector2 backPosition = new Vector2((graphicsDevice.Viewport.Width - backSize.X) / 2, (graphicsDevice.Viewport.Height - backSize.Y) / 2 + 200);

            // Draw text
            spriteBatch.DrawString(gameFontSmll, newgameText, newgamePosition, Color.White);
            spriteBatch.DrawString(gameFontSmll, loadgameText, loadgamePosition, Color.White);
            // Draw text
            spriteBatch.DrawString(gameFontSmll, instructionsText, instructionsPosition, Color.White);
            spriteBatch.DrawString(gameFontSmll, backText, backPosition, Color.White);
        }

        private void DrawStartScreen(SpriteBatch sb)
        {
            DrawScreen("Sanguine Forest");

            foreach (var button in startButtons)
            {
                button.Draw(sb);
            }

            //DrawInteractionTextsStartScreen();

        }

        private void DrawPlayingScreen()
        {
            DrawScreen("Playing Screen");
        }

        private void DrawPausedScreen()
        {
            DrawScreen("Paused Screen");
            DrawInteractionTexts();
        }

        private void DrawWinScreen()
        {
            DrawScreen("Win Screen");
        }

        private void DrawGameOverScreen()
        {
            DrawScreen("Game Over Screen");
        }

        private void DrawTransitionScreen()
        {
            DrawScreen("Transitioning Screen");
        }

        private void DrawStatsScreen()
        {
            DrawScreen("Stats Screen");
        }
    }

}
