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
    public class BigTest01 : Game
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
        private UIManager _uiManager;
        Texture2D semiTransparentTexture;

        //Debug tools
        private DebugObserver _debugObserver;
        private bool isObserverWork=false;


        

        public BigTest01()
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



            //Load player state and scene
            _playerState = FileLoader.LoadFromJson<PlayerState>(FileLoader.RootFolder + "/PlayerState/DefaultState.json");
            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/Scene_" + "Level_2" + ".json");
            
            //Set character and camera
            _character = new Character2(_currentScene.characterPosition, 0, Content);
            //_character.SetCharacterScale(0.3f);
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-10000, -20000), new Vector2(10000, 20000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_character);
            //_camera.SetZoom(1f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content,_playerState, semiTransparentTexture);
            _environmentManager.Initialise(_currentScene);
            _character.DeathEvent += _environmentManager.DeathUpdate; //attach the update fo environment to death of character


            //Set decor and parallaxing
            //Load Background
            //Set decor and parallaxing
            _parallaxManager = new ParallaxManager(Content, _camera, _currentScene);

            // Initialize UI manager. Create an Exit method for UIManager
            _uiManager = new UIManager(_spriteBatch, GraphicsDevice, Content);
            _uiManager.RequestExit += Exit;

            // Create a 1x1 pixel texture and set it to a semi-transparent color
            semiTransparentTexture = new Texture2D(GraphicsDevice, 1, 1);
            semiTransparentTexture.SetData(new[] { new Color(0, 0, 0, 128) }); // Adjust alpha to increase/decrease darkness

            //Debug camera
            DebugManager.Camera = _camera;
            _debugObserver = new DebugObserver(_character.GetPosition(), 0);


            //Audio
            //AudioSetting
            //AudioManager.GeneralVolume = 1.0f;

            //Load Audio
            //AudioManager.Initialize(new ListenerModule(null, Vector2.Zero)); // Assuming you have a listener in your game
            AudioManager.LoadContent(this);
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
                    _parallaxManager.UpdateMe(new Vector2(15, 0)); //parallax update
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
            _parallaxManager.UpdateMe(new Vector2(_character.GetVelocityX(), _character.GetVelocityY()));

        }

    }
}