using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Diagnostics;
using Sanguine_Forest.Scripts.Environment;
using System.Collections.Generic;
using Sanguine_Forest.Scripts.GameState;


namespace Sanguine_Forest
{
    public class AlbertoTestGame : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Camers
        private Camera _camera;

        //Character
        private Character2 _character;

        //PlayerState
        private PlayerState _playerState;

        //Environment
        private EnvironmentManager _environmentManager;

        //Parallaxing
        private ParallaxManager _parallaxManager;

        //Scene
        private Scene _currentScene;

        //Control
        private KeyboardState currState;
        private KeyboardState prevState;

        private InputManager _inputManager;

        //Debug tools
        private DebugObserver _debugObserver;
        private bool isObserverWork = false;

        private UIManager _uiManager;

        Texture2D semiTransparentTexture;

        public AlbertoTestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredBackBufferHeight = Extentions.ScreenHeight;
            _graphics.PreferredBackBufferWidth = Extentions.ScreenWidth;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {


            _inputManager = new InputManager();

            FileLoader.RootFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Content"));

            base.Initialize();
        }

        private void ExitGame()
        {
            // Exit method of the Game class to exit the game
            Exit();
        }

        protected override void LoadContent()
        {

            //graphic installing
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Debug initialising
            DebugManager.SpriteBatch = _spriteBatch;
            DebugManager.DebugTexture = Content.Load<Texture2D>("Extentions/DebugBounds");
            DebugManager.DebugFont = Content.Load<SpriteFont>("Extentions/debugFont");
            DebugManager.isWorking = true;

            //Audio
            //AudioSetting
            //AudioManager.GeneralVolume = 1.0f;

            //Load player state and scene
            _playerState = FileLoader.LoadFromJson<PlayerState>(FileLoader.RootFolder + "/PlayerState/DefaultState.json");
            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/Scene_" + "Alberto" + ".json");

            //Set character and camera
            _character = new Character2(_currentScene.characterPosition, 0, Content);
            //_character.SetCharacterScale(0.3f);
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-10000, -10000), new Vector2(10000, 10000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_character);
            //_camera.SetZoom(1f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content,_playerState);
            _environmentManager.Initialise(_currentScene);

            //Set decor and parallaxing
            _parallaxManager = new ParallaxManager(Content, _camera);

            //Debug camera
            DebugManager.Camera = _camera;
            _debugObserver = new DebugObserver(_character.GetPosition(), 0);

            // Initialize UI manager. Create an Exit method for UIManager
            _uiManager = new UIManager(_spriteBatch, GraphicsDevice, Content);
            _uiManager.RequestExit += ExitGame;

            // Create a 1x1 pixel texture and set it to a semi-transparent color
            semiTransparentTexture = new Texture2D(GraphicsDevice, 1, 1);
            semiTransparentTexture.SetData(new[] { new Color(0, 0, 0, 128) }); // Adjust alpha to increase/decrease darkness

        }

        public void HandleInput(GameTime gameTime)
        {
            _inputManager.UpdateMe();
        }


        protected override void Update(GameTime gameTime)
        {
            //save curr state of keyboard
            currState = Keyboard.GetState();

            //Physic update
            PhysicManager.UpdateMe();

            //Audio manager
            //AudioSetting
            //AudioManager.UpdateMe();

            //Global time
            Extentions.globalTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _uiManager.UpdateMe(currState, prevState); // Update UI manager which will handle state transitions

            switch (_uiManager.CurrentGameState)
            {
                case UIManager.GameState.StartScreen:
                    _parallaxManager.UpdateMe(new Vector2(15, 0));
                    break;
                case UIManager.GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case UIManager.GameState.Paused:
                    _environmentManager.UpdateMe();
                    break;
                case UIManager.GameState.GameOver:
                    // freeze the game or setup for restart
                    break;
                case UIManager.GameState.Win:
                    
                    break;
                case UIManager.GameState.Stats:

                    break;
                case UIManager.GameState.Transitioning:

                    break;
            }

            //Debug observer (flying cam without character)
            if (currState.IsKeyUp(Keys.O) && prevState.IsKeyDown(Keys.O))
            {
                if (!isObserverWork)
                {
                    _camera.SetCameraTarget(_debugObserver);
                    isObserverWork = true;
                }
                else
                {
                    _camera.SetCameraTarget(_character);
                    isObserverWork = false;
                }
            }

            if (isObserverWork)
            {
                _debugObserver.UpdateMe(currState);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //save prev state
            prevState = currState;

            base.Update(gameTime);
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            // Update all game logic here when in Playing state
            _environmentManager.UpdateMe();
            _camera.UpdateMe();
            if (!isObserverWork)
            {
                _character.UpdateMe(prevState, currState);
            }
            _parallaxManager.UpdateMe(new Vector2(_character.GetVelocity(), 0));

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (_uiManager.CurrentGameState == UIManager.GameState.Playing)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

                _environmentManager.DrawMe(_spriteBatch);
                _character.DrawMe(_spriteBatch);
                _parallaxManager.DrawMe(_spriteBatch);

                //Debug test
                // DebugManager.DebugRectangle(new Rectangle(50, 50, 50, 50));
                DebugManager.DebugString("Camera pos:" + _camera.position, new Vector2(0, 0));
                DebugManager.DebugString("Character pos: " + _character.GetPosition(), new Vector2(0, 20));
                if (isObserverWork)
                    DebugManager.DebugString("Observer pos: " + _debugObserver.GetPosition(), new Vector2(0, 40));

                _spriteBatch.End();
            }

            else if (_uiManager.CurrentGameState == UIManager.GameState.StartScreen)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

                _parallaxManager.DrawMe(_spriteBatch); 

                _spriteBatch.End();

                // Begin a new sprite batch without any camera transformations
                _spriteBatch.Begin();

                // Draw  semi-transparent overlay over the whole screen
                _spriteBatch.Draw(semiTransparentTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

                _uiManager.DrawMe();


                _spriteBatch.End();

            }

            else if (_uiManager.CurrentGameState == UIManager.GameState.Paused)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

                _environmentManager.DrawMe(_spriteBatch);
                _character.DrawMe(_spriteBatch);
                _parallaxManager.DrawMe(_spriteBatch);

                _spriteBatch.End();


                // Begin a new sprite batch without any camera transformations
                _spriteBatch.Begin();

                // Draw  semi-transparent overlay over the whole screen
                _spriteBatch.Draw(semiTransparentTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

                _uiManager.DrawMe();


                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}