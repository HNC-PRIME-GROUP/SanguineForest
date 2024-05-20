using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Extention;
using Microsoft.VisualBasic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Camera that follow for particular game object (may be we want to add some empty gameobject to fly around the level before attach the camera to character)
    /// </summary>
    internal class Camera
    {

        public Vector2 position;
        public float zoom;

        //Camera target object
        private GameObject cameraTarget;

        //Size of game
        public Vector2 screenSize;

        //Camera can't cross the border.
        public Vector2 leftUpperBorder;
        public Vector2 rightBottomBorder;

        //Camera smooth moving 
        public Vector2 _velocity;
        private float speed = 10;

        //Shaking effect variables
        public Vector2 savedPos;
        private float shakeTimer;
        private float shakeTime;
        public Random rng;
        private int shakePower;

        // Add properties for the Y-axis boundaries
        private float topBoundary;
        private float bottomBoundary;

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
            this.screenSize  = screenSize;
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

        public void UpdateMe()
        {

            //velocity counting
            //if (position.X - 50 + screenSize.X / (2 * zoom) != cameraTarget.GetPosition().X)
            //{
            //    _velocity.X = cameraTarget.GetPosition().X - position.X - 50 + screenSize.X / (2 * zoom);
            //}
            //else
            //{
            //    _velocity.X = 0;
            //}

            //if (position.Y + 50 + screenSize.Y / (2 * zoom) != cameraTarget.GetPosition().Y)
            //{
            //    _velocity.Y = cameraTarget.GetPosition().Y - position.Y + 50 + screenSize.Y / (2 * zoom);
            //}
            //else
            //{
            //    _velocity.Y = 0;
            //}

            //if (Math.Abs(-cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom) - position.X) > speed &&
            //    Math.Abs(-cameraTarget.GetPosition().Y + 50 + screenSize.Y / (2 * zoom)-position.Y) > speed)
            //{
            //    _velocity = -cameraTarget.GetPosition() - position - new Vector2(-50, 50) + new Vector2(screenSize.X / (2 * zoom), screenSize.Y / (2 * zoom));
            //    _velocity.Normalize();
            //}
            //else
            //{
            //    _velocity = Vector2.Zero;
            //}


            //position += _velocity * speed;

            //_velocity = new Vector2(-cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom) - position.X, - cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom)-position.Y);
            //_velocity.Normalize();
            //speed = 10;//(float)Math.Pow(Vector2.Distance(cameraTarget.GetPosition(), position)/200,2);
            ////speed +=  acceleration;



            //////following for the target in allowed borders
            //if (cameraTarget.GetPosition().X + screenSize.X / (2 * zoom) < rightBottomBorder.X &&
            //    cameraTarget.GetPosition().X + screenSize.X / (2 * zoom) > leftUpperBorder.X)
            //{
            //    //if (Vector2.Distance(position,new Vector2(-cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom), -cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom))) > 10)
            //    //{ position.X += _velocity.X * speed; }
            //    var oldPositionX = position.X;
            //    position.X = MathHelper.Lerp(position.X, -cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom), 0.04f);
            //    //-cameraTarget.GetPosition().X-50 + screenSize.X / (2 * zoom);
            //    _velocity.X = position.X - oldPositionX;


            //}

            // Previous Logic /2 screenHeight

            //if (cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom) < rightBottomBorder.Y &&
            //    cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom) > leftUpperBorder.Y)
            //{
            //    //if(Vector2.Distance(position, new Vector2(-cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom), -cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom))) > 10)
            //    //{ position.Y += _velocity.Y * speed; }
            //    var oldPositionY = position.Y;
            //    position.Y = MathHelper.Lerp(position.Y, -cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom),0.04f);
            //    _velocity.Y=position.Y - oldPositionY;
            //    //- cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom);

            //}

            // Updated Logic /3 ScreenHeignt

            if (cameraTarget != null)
            {
                float targetX = -cameraTarget.GetPosition().X + screenSize.X / (2 * zoom)-100;
                float targetY = -cameraTarget.GetPosition().Y + (screenSize.Y / 2f) / zoom;

                if (targetX + screenSize.X / (2 * zoom) < rightBottomBorder.X &&
                    targetX + screenSize.X / (2 * zoom) > leftUpperBorder.X)
                {
                    position.X = MathHelper.Lerp(position.X, targetX, 0.04f);
                    _velocity.X = position.X - targetX;
                }

                if (targetY + screenSize.Y / (2 * zoom) < rightBottomBorder.Y &&
                    targetY + screenSize.Y / (2 * zoom) > leftUpperBorder.Y)
                {
                    position.Y = MathHelper.Lerp(position.Y, targetY, 0.04f);
                    _velocity.Y = position.Y - targetY;
                }



                // Update X - axis position
                //if (cameraTarget.GetPosition().X + screenSize.X / (2 * zoom) < rightBottomBorder.X &&
                //    cameraTarget.GetPosition().X + screenSize.X / (2 * zoom) > leftUpperBorder.X)
                //{
                //    position.X = -cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom);
                //}

                //// Update Y-axis position
                //float halfScreenHeight = screenSize.Y / (2 * zoom);
                //float targetY = cameraTarget.GetPosition().Y;

                //// Check if the target Y position adjusted by half the screen height is within the vertical game bounds
                //if (targetY - halfScreenHeight < 0) // Checking if it goes above the top border
                //{
                //    position.Y = halfScreenHeight; // Keep the camera centered within the boundary
                //}
                //else if (targetY + halfScreenHeight > 1080) // Checking if it goes below the bottom border
                //{
                //    position.Y = 1080 - halfScreenHeight; // Adjust to keep within the bottom boundary
                //}
                //else
                //{
                //    position.Y = targetY; // Normal follow behavior
                //}


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



        /// <summary>
        /// Change zoom
        /// </summary>
        /// <param name="zoom"></param>
        public void SetZoom(float zoom)
        {
            this.zoom = zoom;
        }

        /// <summary>
        /// Get zoom
        /// </summary>
        /// <returns></returns>
        public float GetZoom()
        {
            return zoom;
        }


        /// <summary>
        /// Get current camera terget
        /// </summary>
        /// <returns></returns>
        public GameObject GetCameraTarget()
        {
            return cameraTarget;
        }

        /// <summary>
        /// Set current camera target
        /// </summary>
        /// <param name="cameraTarget"></param>
        public void SetCameraTarget(GameObject cameraTarget)
        {
            this.cameraTarget = cameraTarget;
            //following for the target in allowed borders
            if (cameraTarget.GetPosition().X + screenSize.X / (2 * zoom) < rightBottomBorder.X &&
                cameraTarget.GetPosition().X + screenSize.X / (2 * zoom) > leftUpperBorder.X)
            {

                position.X = - cameraTarget.GetPosition().X - 50 + screenSize.X / (2 * zoom);
            }

            if (cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom) < rightBottomBorder.Y &&
                cameraTarget.GetPosition().Y + screenSize.Y / (2 * zoom) > leftUpperBorder.Y)
            {
                position.Y = - cameraTarget.GetPosition().Y + 50 + screenSize.Y / (2 * zoom);
            }
        }

        public float GetVelocityX()
        {
            return _velocity.X;
        }

        public float GetVelocityY()
        {
            return _velocity.Y;
        }

    }
}
