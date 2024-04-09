using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using Sanguine_Forest.Scripts.TestScripts;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    public class IuriiGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //Camera
        private Camera camera;

        //Test objects
        private IuriiTestGameObject _gameObject;
        private IuriiTestGameObject _gameObject1;

        //Test scene
        private Scene scene;


        //Control
        private KeyboardState currKeyState;
        private KeyboardState prevKeyState;


        public IuriiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {

            //AudioSetting
            AudioManager.GeneralVolume = 1.0f;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera(Vector2.Zero, new Vector2(-5000, -5000 ), new Vector2(5000, 5000), new Vector2(720, 720));

            //test object
            _gameObject = new IuriiTestGameObject(Vector2.Zero,0f,Content);
            _gameObject1 = new IuriiTestGameObject(new Vector2(100,100), 0f, Content);

            _gameObject._SpriteModule.SetScale(0.3f);
            _gameObject1._SpriteModule.SetScale(0.3f);

            camera.SetCameraTarget(_gameObject);


            //test scene creation
            scene = FileLoader.LoadFromJson<Scene>("D:/Documents/EdinburghCollegeVScode/SanguineForest/Sanguine Forest/Content/Scenes/SceneTest");



            //Debug initialising
            DebugManager.SpriteBatch = _spriteBatch;
            DebugManager.DebugTexture = Content.Load<Texture2D>("Extentions/DebugBounds");
            DebugManager.DebugFont = Content.Load<SpriteFont>("Extentions/debugFont");
            DebugManager.isWorking = true;

        }

        protected override void Update(GameTime gameTime)
        {
            currKeyState = Keyboard.GetState();
            //Global time
            Extentions.globalTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.UpdateMe();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _gameObject.UpdateMe(currKeyState,prevKeyState);
            _gameObject1.UpdateMe();
            camera.UpdateMe();

            //test scene creation
            //if(!currKeyState.IsKeyDown(Keys.Space)|| prevKeyState.IsKeyDown(Keys.Space))
            //{
            //    FileLoader.SaveToJson<Scene>(scene, "D:/Documents/EdinburghCollegeVScode/SanguineForest/Sanguine Forest/Content/Scenes/SceneTest");
            //}

            //scene test
            if(!currKeyState.IsKeyDown(Keys.Space)||prevKeyState.IsKeyDown(Keys.Space)) 
            {
               
            }

            prevKeyState = currKeyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront,null,null,null,null,null, camera.GetCam());

            _gameObject.DrawMe(_spriteBatch);
            _gameObject1.DrawMe(_spriteBatch);

            //Debug test
            DebugManager.DebugRectangle(new Rectangle(50, 50, 50, 50));

            

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}