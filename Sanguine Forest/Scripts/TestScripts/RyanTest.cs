using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;

namespace Sanguine_Forest
{
    public class RyanTest : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Character player;

        public RyanTest()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            DebugManager.SpriteBatch = _spriteBatch;
            DebugManager.DebugTexture = Content.Load<Texture2D>("Extentions/DebugBounds");
            DebugManager.DebugFont = Content.Load<SpriteFont>("Extentions/debugFont");
            DebugManager.isWorking = true;

            player = new Character(new Vector2(0, 0), 0,
                Content.Load<Texture2D>("Sprites/Sprites_Character_v1"));

        }

        protected override void Update(GameTime gameTime)
        {
            //Global time
            Extentions.globalTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            player.UpdateMe();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred);

            //Debug test
            DebugManager.DebugRectangle(  new Rectangle((int)player.pos.X, (int)player.pos.Y, player.txr.Height, player.txr.Width));

            player.DrawMe(_spriteBatch);

            DebugManager.DebugString("pos: " + player.GetPosition(), new Vector2(0, 0));
            DebugManager.DebugString("pos: " + player.pos, new Vector2(0, 10));

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}