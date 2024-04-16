using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using System;
using System.Diagnostics;

namespace Sanguine_Forest
{
    public class AlbertoGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Texture2D lakeTexture;
        //Texture2D[] forestLayers;

        private ParallaxManager parallaxManager;
        private ObstacleManager obstacleManager;
        private PlayerState playerState;
        private Character playerCharacter;
        public AlbertoGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        public Texture2D LoadTexture(string assetName)
        {
            return Content.Load<Texture2D>(assetName);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            parallaxManager = new ParallaxManager();

            //LOAD LAKE_4
            int segmentWidth = 1920;
            int totalWidth = 0;
            int numberOfSegments = 4; // Adjust this number based on your actual segments
            // Load the first segments
            for (int i = 0; i < numberOfSegments - 1; i++) // Assuming numberOfSegments includes the last segment
            {
                Texture2D segmentTexture = LoadTexture($"Background/Lake_{i + 1}");
                parallaxManager.AddBackground(new ParallaxBackground(
                    new Vector2(totalWidth, 0), 0f, segmentTexture, Extentions.SpriteLayer.background_Lake_4, 1f));
                totalWidth += segmentWidth;

            }

            // Load the last segment with a different width
            segmentWidth = 691; // Width of the last segment
            Texture2D lastSegmentTexture = LoadTexture("Background/Lake__Last");
            parallaxManager.AddBackground(new ParallaxBackground(
                new Vector2(totalWidth, 0), 0f, lastSegmentTexture, Extentions.SpriteLayer.background_Lake_4, 1f));
            totalWidth += segmentWidth; 

            //Load obstacles
            //Texture2D obstacle_Texture = Content.Load<Texture2D>("PathToTexture");
            Texture2D obstacle_Texture = DebugManager.DebugTexture;
            Obstacle myObstacle = new Obstacle(new Vector2(100, 100), 0, obstacle_Texture, 2); // Create new Obstacle, 2 is level of alcohol needed to draw
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector2 deltaMovement = Vector2.Zero;

            // Check for input to determine the direction of movement
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                deltaMovement.X += 100; // Move right
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                deltaMovement.X -= 100; // Move left
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                deltaMovement.Y -= 1; // Move up
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                deltaMovement.Y += 1; // Move down
            }

            Debug.WriteLine($"Delta Movement: {deltaMovement}");

            // Update the parallax manager with the delta movement
            parallaxManager.UpdateMe(deltaMovement);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // Draw the parallax backgrounds
            int currentAlcoholLevel = playerState.AlcoholLevel;
            parallaxManager.Draw(_spriteBatch);
            obstacleManager.Draw(_spriteBatch, currentAlcoholLevel);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}