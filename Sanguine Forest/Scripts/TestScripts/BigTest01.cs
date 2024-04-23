﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using System.IO;
using System;

namespace Sanguine_Forest
{
    public class BigTest01 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //Camers
        private Camera _camera;

        //Character
        private Character _character;

        //PlayerState
        private PlayerState _playerState;

        //Environment
        private EnvironmentManager _environmentManager;
        
        //Parallaxing
        private ParallaxManager _parallaxManager;
        private ParallaxBackground back01;
        private ParallaxBackground back02;
        private ParallaxBackground back03;
        private ParallaxBackground back04;

        //Scene
        private Scene _currentScene;

        //Control
        private KeyboardState currState;
        private KeyboardState prevState;

        

        public BigTest01()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
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



            //Load player state and scene
            _playerState = FileLoader.LoadFromJson<PlayerState>(FileLoader.RootFolder + "/PlayerState/DefaultState.json");
            _currentScene = FileLoader.LoadFromJson<Scene>(FileLoader.RootFolder + "/Scenes/Scene_" + _playerState.lvlCounter+".json");

            //Set character and camera
            _character = new Character(_currentScene.characterPosition, 0, Content.Load<Texture2D>("Sprites/Sprites_Character_v1"));
            _camera = new Camera(_currentScene.characterPosition, new Vector2(-10000, -10000), new Vector2(10000, 10000), new Vector2(1920, 1080));
            _camera.SetCameraTarget(_character);
            _camera.SetZoom(1f);

            //Set the level's objects
            _environmentManager = new EnvironmentManager(Content);
            _environmentManager.Initialise(_currentScene);


            //Set decor and parallaxing
            _parallaxManager = new ParallaxManager();
            Vector2 startParallaxPos = new Vector2(-1920 / 2, -1080 / 2);
            back01 = new ParallaxBackground(startParallaxPos, 0, Content.Load<Texture2D>("Sprites/Background_day_01"), Extentions.SpriteLayer.background_Forest_1, 0.5f);
            back02 = new ParallaxBackground(startParallaxPos, 0, Content.Load<Texture2D>("Sprites/Background_day_02"), Extentions.SpriteLayer.background_Forest_1, 0.5f);
            back03 = new ParallaxBackground(startParallaxPos, 0, Content.Load<Texture2D>("Sprites/Background_day_03"), Extentions.SpriteLayer.background_Forest_1, 0.5f);
            //back04 = new ParallaxBackground(startParallaxPos, 0, Content.Load<Texture2D>("Sprites/Background_day_01"), Extentions.SpriteLayer.background_Forest_4, 0.1f);






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

            //camera
            _camera.UpdateMe();

            //Character
            _character.UpdateMe(currState, prevState);

            //Background
            back01.UpdatePosition(_character.GetVelocity());
            back02.UpdatePosition(_character.GetVelocity());
            back03.UpdatePosition(_character.GetVelocity());



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
            _character.DrawMe(_spriteBatch);

            //Background
            back01.Draw(_spriteBatch);
            back02.Draw(_spriteBatch);
            back03.Draw(_spriteBatch);

            //Debug test
            DebugManager.DebugRectangle(new Rectangle(50, 50, 50, 50));

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}