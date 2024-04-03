using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using Sanguine_Forest.Scripts.TestScripts;

namespace Sanguine_Forest
{
    public class IuriiGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IuriiTestGameObject _gameObject;


        //Control
        private KeyboardState currKeyState;
        private KeyboardState prevKeyState;


        public IuriiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
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


            _gameObject = new IuriiTestGameObject(Vector2.Zero,0f,Content);

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


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _gameObject.UpdateMe(currKeyState,prevKeyState);


            prevKeyState = currKeyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred);



            //Debug test
            DebugManager.DebugRectangle(new Rectangle(50, 50, 50, 50));

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}