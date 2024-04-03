using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Extention;

namespace Sanguine_Forest
{
    /// <summary>
    /// Camera that follow for particular game object (may be we want to add some empty gameobject to fly around the level before attach the camera to character)
    /// </summary>
    internal class Camera
    {

        public Vector2 position;
        public float zoom;

        //Size of game
        public Vector2 screenSize;

        //Camera can't cross the border.
        public Vector2 leftUpperBorder;
        public Vector2 rightBottomBorder;

        //Shaking effect variables
        public Vector2 savedPos;
        private float shakeTimer;
        private float shakeTime;
        public Random rng;
        private int shakePower;

        /// <summary>
        /// Camera 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="leftUpBorder"></param>
        /// <param name="rightBottBorder"></param>
        /// <param name="screenSize">Size and proportions</param>
        public Camera(Vector2 pos, Vector2 leftUpBorder, Vector2 rightBottBorder, Vector2 screenSize)
        {
            this.position = pos;
            leftUpperBorder = leftUpBorder;
            rightBottomBorder = rightBottBorder;
            this.screenSize = screenSize;
            zoom = 1;
            rng = new Random();
            shakeTimer = 0;
            shakeTime = 1.5f;
        }

        /// <summary>
        /// Mehtod that return the matrix (used for spriteBatch Begin method in a Game1)
        /// </summary>
        /// <returns></returns>
        public Matrix GetCam()
        {
            Matrix temp;
            temp = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
            temp *= Matrix.CreateScale(zoom);
            return temp;
        }

        public void UpdateMe(GameObject cameraTarget)
        {
            //following for the target in allowed borders
            if(cameraTarget.GetPosition().X+ screenSize.X/(2*zoom)<rightBottomBorder.X &&
                cameraTarget.GetPosition().X+screenSize.X/(2*zoom)>leftUpperBorder.X)
            {
                
                position.X = (-cameraTarget.GetPosition().X + screenSize.X / (2 * zoom));
            }

            if(cameraTarget.GetPosition().Y+screenSize.Y/(2*zoom)<rightBottomBorder.Y &&
                cameraTarget.GetPosition().Y+screenSize.Y/(2*zoom)>leftUpperBorder.Y)
            {
                position.Y = (-cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom));
            }


        }

        public void Shaking(Vector2 cameraTarget)
        {
            if(shakeTimer>0)
            {
                var temp = rng.Next(-shakePower, shakePower);
                //shaking in borders of bounds
                if (cameraTarget.X + temp + screenSize.X / (2 * zoom) < rightBottomBorder.X 
                    && cameraTarget.X + temp + screenSize.X / (2 * zoom) > leftUpperBorder.X)
                {
                    position.X += temp;
                }
                else
                {
                    position.X -= temp;
                }
                if (cameraTarget.Y + temp + screenSize.Y / (2 * zoom) < rightBottomBorder.Y 
                    && cameraTarget.Y + temp + screenSize.Y / (2 * zoom) > leftUpperBorder.Y)
                {
                    position.Y += temp;
                }
                else
                {
                    position.Y -= temp;
                }
                shakeTimer -= 0.1f*Extentions.globalTime;
            }
        }

        /// <summary>
        /// Initialise the shaking with power (unfortunatelly timer of shaking is needed)
        /// </summary>
        /// <param name="power"></param>
        public void StartShaking(int power)
        {
            shakeTimer = shakeTime;
            shakePower = power;
        }


        public void SetZoom(float zoom)
        {
            this.zoom = zoom;
        }

        public float GetZoom()
        {
            return zoom;
        }



    }
}
