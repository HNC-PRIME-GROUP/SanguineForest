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
            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/Scene_" + _playerState.lvlCounter + ".json");

            //Set character and camera
            _character = new Character2(_currentScene.characterPosition, 0, Content);
            //_character.SetCharacterScale(0.3f);
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-10000, -10000), new Vector2(10000, 10000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_character);
            _camera.SetZoom(2f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content,_playerState);
            _environmentManager.Initialise(_currentScene);

            //Set decor and parallaxing
            _parallaxManager = new ParallaxManager(Content, _camera);
            
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

            //Environment update
            _environmentManager.UpdateMe();

            ////camera
            _camera.UpdateMe();

            ////Character
            _character.UpdateMe(prevState, currState);

            //Parallax
            Vector2 velocityVector = new Vector2(_character.GetVelocity(), 0);
            _parallaxManager.UpdateMe(velocityVector);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //save prev state
            prevState = currState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

            _environmentManager.DrawMe(_spriteBatch);

            //Character
            _character.DrawMe(_spriteBatch);

            //Parrallax


            _parallaxManager.DrawMe(_spriteBatch);



            //Debug test
            // DebugManager.DebugRectangle(new Rectangle(50, 50, 50, 50));
            DebugManager.DebugString("Camera pos:"+_camera.position, new Vector2(0,0));
            DebugManager.DebugString("Character pos: "+ _character.GetPosition(), new Vector2(0,20));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}