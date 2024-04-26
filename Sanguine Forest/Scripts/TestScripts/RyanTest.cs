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
    public class RyanTest : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        private List<ScrollingBackground> _scrollingBackground;

        //Camers
        private Camera _camera;

        //Character
        private Character _character;

        //PlayerState
        private PlayerState _playerState;

        //Environment
        private EnvironmentManager _environmentManager;

        ////Parallaxing
        //private ParallaxManager _parallaxManager;


        //Scene
        private Scene _currentScene;

        //Control
        private KeyboardState currState;
        private KeyboardState prevState;

        private InputManager _inputManager;



        public RyanTest()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

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
            _character = new Character(_currentScene.characterPosition, 0, Content.Load<Texture2D>("Sprites/Sprites_Character_v1"));
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-20000, -10000), new Vector2(20000, 10000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_character);
            _camera.SetZoom(1.2f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content);
            _environmentManager.platforms = new List<Platform>();
            //_environmentManager.platforms.Add(new Platform(new Vector2(300, 300), 0, new Vector2(100, 100), Content));
            _environmentManager.Initialise(_currentScene);

            //Load Background
            _scrollingBackground = new List<ScrollingBackground>()
            {
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_01"), _character, 60f)
                {
                    LayerBackground = 0.4f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_02"), _character, 60f)
                {
                    LayerBackground = 0.4f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_03"), _character, 60f)
                {
                    LayerBackground = 0.4f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_04"), _character, 60f)
                {
                    LayerBackground = 0.4f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_05"), _character, 40f)
                {
                    LayerBackground = 0.3f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_06"), _character, 20f)
                {
                    LayerBackground = 0.2f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Sprites/Background_day_07"), _character, 0f)
                {
                    LayerBackground = 0.1f,
                },
            };

            //Set decor and parallaxing
            //_parallaxManager = new ParallaxManager(Content);


        }

        public void HandleInput(GameTime gameTime)
        {
            _inputManager.UpdateMe();
        }


        protected override void Update(GameTime gameTime)
        {

            //save curr state of keyboard
            currState = Keyboard.GetState();

            HandleInput(gameTime);

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
            _character.UpdateMe(_inputManager);

            //Background
            //_parallaxManager.UpdateMe(_character.GetVelocity());


            foreach (var sb in _scrollingBackground)
                sb.Update(gameTime);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //save prev state
            prevState = currState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var sb in _scrollingBackground)
                sb.Draw(gameTime, _spriteBatch);


            _spriteBatch.End();


            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, _camera.GetCam());

            _environmentManager.DrawMe(_spriteBatch);

            //_parallaxManager.Draw(_spriteBatch);


            _character.DrawMe(_spriteBatch);
            DebugManager.DebugString("pos: " + _character.GetPosition(), new Vector2(-500, -500));
            //DebugManager.DebugString("pos: " + _character.Position, new Vector2(-500, -500));

            //Debug test
            DebugManager.DebugRectangle(new Rectangle(50, 50, 50, 50));

            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}