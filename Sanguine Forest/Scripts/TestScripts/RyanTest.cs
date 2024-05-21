using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using System.IO;
using System;
using Sanguine_Forest.Scripts.Environment;
using System.Collections.Generic;
using Sanguine_Forest.Scripts.GameState;

namespace Sanguine_Forest
{
    public class RyanTest : Game
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

        //UI manager and UI assets
        public UIManager _uiManager;
        Texture2D semiTransparentTexture;

        //Debug tools
        private DebugObserver _debugObserver;
        private bool isObserverWork = false;




        public RyanTest()
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
            FileLoader.RootFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Content"));


            base.Initialize();
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

            //_character.SetCharacterScale(0.3f);
            _debugObserver = new DebugObserver(Vector2.Zero, 0);
            _camera = new Camera(_debugObserver.GetPosition(), new Vector2(-10000, -10000), new Vector2(10000, 10000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_debugObserver);

            //_camera.SetZoom(1f);

            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/SceneStart.json");
            //Set decor and parallaxing
            _parallaxManager = new ParallaxManager(Content, _camera);

            // Initialize UI manager. Create an Exit method for UIManager
            _uiManager = new UIManager(_spriteBatch, GraphicsDevice, Content);
            _uiManager.RequestExit += Exit;
            _uiManager.NewGameEvent += NewGameLoad;
            _uiManager.LoadGameEvent += LoadGame;

            // Create a 1x1 pixel texture and set it to a semi-transparent color
            semiTransparentTexture = new Texture2D(GraphicsDevice, 1, 1);
            semiTransparentTexture.SetData(new[] { new Color(0, 0, 0, 128) }); // Adjust alpha to increase/decrease darkness


            //AudioManager.Initialize(new ListenerModule(null, Vector2.Zero)); // Assuming you have a listener in your game
            AudioManager.LoadContent(this);
            //Debug camera
            DebugManager.Camera = _camera;


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

            //UI manager update
            _uiManager.UpdateMe(gameTime, currState, prevState); // Update UI manager which will handle state transitions

            // Update the audio manager
            AudioManager.Update(gameTime);

            switch (_uiManager.CurrentGameState)
            {
                case UIManager.GameState.StartScreen:
                    _parallaxManager.UpdateMe(); //parallax update
                    break;
                case UIManager.GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case UIManager.GameState.Paused:
                    _environmentManager.UpdateMe(); //Environment update
                    break;
                case UIManager.GameState.InstructionsFromStart:
                    // freeze the game or setup for restart
                    break;
                case UIManager.GameState.InstructionsFromPause:
                    // freeze the game or setup for restart
                    break;
            }


            ////camera
            _camera.UpdateMe();

            //Parallax            
            _parallaxManager.UpdateMe();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //save prev state
            prevState = currState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (_uiManager.CurrentGameState == UIManager.GameState.Playing)
            {
                Matrix cameraTransform = _camera.GetCam();
                _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, cameraTransform);

                _environmentManager.DrawMe(_spriteBatch, _character.GetPosition());
                _character.DrawMe(_spriteBatch);
                _parallaxManager.DrawMe(_spriteBatch);


                // Debug test
                DebugManager.DebugString("Camera pos:" + _camera.position, new Vector2(0, 0));
                DebugManager.DebugString("Character pos: " + _character.GetPosition(), new Vector2(0, 20));
                if (isObserverWork)
                    DebugManager.DebugString("Observer pos: " + _debugObserver.GetPosition(), new Vector2(0, 40));



                _spriteBatch.End();

                // Begin a new sprite batch without camera transformations for UI and Cutscene
                _spriteBatch.Begin();
                _environmentManager.DrawCutSceneDialogues(_spriteBatch, cameraTransform);
                _environmentManager.DrawPressEPrompts(_spriteBatch, cameraTransform, _character.GetPosition());
                _spriteBatch.End();
            }
            else if (_uiManager.CurrentGameState == UIManager.GameState.StartScreen)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

                _parallaxManager.DrawMe(_spriteBatch);

                _spriteBatch.End();

                // Begin a new sprite batch without any camera transformations
                _spriteBatch.Begin();

                // Draw semi-transparent overlay over the whole screen
                _spriteBatch.Draw(semiTransparentTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

                _uiManager.DrawMe();

                _spriteBatch.End();
            }
            else if (_uiManager.CurrentGameState == UIManager.GameState.Paused)
            {
                _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

                _environmentManager.DrawMe(_spriteBatch, _character.GetPosition());
                _character.DrawMe(_spriteBatch);
                _parallaxManager.DrawMe(_spriteBatch);

                _spriteBatch.End();

                // Begin a new sprite batch without any camera transformations
                _spriteBatch.Begin();

                // Draw semi-transparent overlay over the whole screen
                _spriteBatch.Draw(semiTransparentTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

                _uiManager.DrawMe();

                _spriteBatch.End();
            }

            base.Draw(gameTime);
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
            _parallaxManager.UpdateMe();

        }


        //load, next level and new game methods

        /// <summary>
        /// Load game from default states
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewGameLoad(object sender, EventArgs e)
        {
            //Load player state and scene
            _playerState = FileLoader.LoadFromJson<PlayerState>(FileLoader.RootFolder + "/PlayerState/DefaultState.json");
            //should be level 1
            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/Scene_" + "1" + ".json");

            //Set character and camera
            _character = new Character2(_currentScene.characterPosition, 0, Content);
            //_character.SetCharacterScale(0.3f);
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-10000, -10000), new Vector2(10000, 10000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_character);
            //_camera.SetZoom(1f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content, _playerState, semiTransparentTexture);
            _environmentManager.Initialise(_currentScene);
            _character.DeathEvent += _environmentManager.DeathUpdate; //attach the update fo environment to death of character
            _environmentManager.LevelEndTrigger += NextLevel;

            //gamestate update
            _uiManager.SetGameState(UIManager.GameState.Playing);
            //_uiManager.CurrentGameState = UIManager.GameState.Playing;

        }
        /// <summary>
        /// Load game from saves state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadGame(object sender, EventArgs e)
        {
            //Load player state and scene
            _playerState = FileLoader.LoadFromJson<PlayerState>(FileLoader.RootFolder + "/PlayerState/SavedState.json");
            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/Scene_" + _playerState.lvlCounter + ".json");

            //Set character and camera
            _character = new Character2(_currentScene.characterPosition, 0, Content);
            //_character.SetCharacterScale(0.3f);
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-10000, -10000), new Vector2(10000, 10000), new Vector2(1920, 1080));
            _debugObserver = new DebugObserver(_character.GetPosition(), 0);
            _camera.SetCameraTarget(_character);
            //_camera.SetZoom(1f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content, _playerState, semiTransparentTexture);
            _environmentManager.Initialise(_currentScene);
            _character.DeathEvent += _environmentManager.DeathUpdate; //attach the update fo environment to death of character
            _environmentManager.LevelEndTrigger += NextLevel;

            //Update the game state
            _uiManager.SetGameState(UIManager.GameState.Playing);
            //_uiManager.CurrentGameState= UIManager.GameState.Playing;
        }

        /// <summary>
        /// Next level loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NextLevel(object sender, EventArgs e)
        {
            _playerState.lvlCounter++;
            _environmentManager.LevelEndTrigger -= NextLevel;
            FileLoader.DeleteFile(FileLoader.RootFolder + "/PlayerState/SavedState");
            FileLoader.SaveToJson(_playerState, FileLoader.RootFolder + "/PlayerState/SavedState");
            LoadGame(sender, e);

        }

    }
}