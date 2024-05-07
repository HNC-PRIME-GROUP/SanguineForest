using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.GameState
{
    internal class Camera2
    {
        public Rectangle Viewport;  // Defines the portion of the world to show on the screen.
        public Vector2 Position;
        public float Zoom;

        private GameObject CameraTarget;

        public Camera2(Vector2 position, Vector2 screenSize, GameObject target)
        {
            this.Position = position;
            this.CameraTarget = target;
            this.Viewport = new Rectangle((int)position.X, (int)position.Y, 1920, 1080);
            this.Zoom = 1.0f;

            // Set the viewport size to 50% of the full game resolution
            this.Viewport = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)(screenSize.X * 0.7),
                (int)(screenSize.Y * 0.7)
            );
        }

        public void Update(GameTime gameTime)
        {
            // Follow the character on the X-axis and clamp the movement
            float targetX = CameraTarget.GetPosition().X - Viewport.Width * 0.25f;
            Position.X = MathHelper.Clamp(targetX, -100000, 100000 - Viewport.Width);

            // Fixed Y position relative to the background and character jump range
            Position.Y = MathHelper.Clamp(CameraTarget.GetPosition().Y, 0, 350);  // Adjust these values based on your game's design

            // Update the viewport as the camera moves
            this.Viewport = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
        }

        public Matrix GetTransformation()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) * Matrix.CreateScale(Zoom);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(
                -Position.X,
                -Position.Y, 0) * Matrix.CreateScale(Zoom);
        }
    }
}