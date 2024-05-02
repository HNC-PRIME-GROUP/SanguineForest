using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Extention;
using System;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    public class RyanTest : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Character player;

        EnvironmentManager environmentManager;

        KeyboardState curr;
        KeyboardState prev;

        public RyanTest()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
          

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
           // environmentManager = new EnvironmentManager();
            environmentManager.platforms = new List<Platform>();
            //environmentManager.platforms.Add(new Platform(new Vector2(300, 300), 0, new Vector2(100, 100), Content));
            

        }

        protected override void Update(GameTime gameTime)
        {
            //Global time
            Extentions.globalTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Physic update
            PhysicManager.UpdateMe();

            curr = Keyboard.GetState();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //player.UpdateMe(curr, prev);
            environmentManager.UpdateMe();

            prev = curr;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred);

            //Debug test

            player.DrawMe(_spriteBatch);
            environmentManager.DrawMe(_spriteBatch);

            DebugManager.DebugString("pos: " + player.GetPosition(), new Vector2(0, 0));
            //DebugManager.DebugString("pos: " + player.Position, new Vector2(0, 10));

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}