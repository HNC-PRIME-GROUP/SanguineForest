using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sanguine_Forest
{
    /// <summary>
    /// 
    /// Rectangle that follow for GameObject. Every physic module subscribed for physic manager. Recieve data about interceptions from PM and signal to their parents
    /// </summary>
    internal class PhysicModule : Module
    {

        private Rectangle physicRec;
        private Vector2 rectangleHalfSize;
        public bool isPhysicActive = false;


        public PhysicModule(GameObject parent, Vector2 shift, Vector2 rectangleSize) : base(parent, shift)
        {
            physicRec = new Rectangle((int)Math.Round(parent.GetPosition().X), (int)Math.Round(parent.GetPosition().Y),
                (int)Math.Round(rectangleSize.X), (int)Math.Round(rectangleSize.Y));
            // to center this 
            rectangleHalfSize  = new Vector2(physicRec.Width/2, physicRec.Height/2);
            PhysicManager.AddObject(this);
        }

        public new void UpdateMe()
        {
            base.UpdateMe();

            physicRec.Location = (GetPosition() - rectangleHalfSize).ToPoint();
        }

        /// <summary>
        /// Physic manager call this method and sent there data about collided module. 
        /// Method send the data abput parent of collided module
        /// </summary>
        /// <param name="obj"></param>
        public new void Collided(Collision collision)
        {
            _parent.Collided(collision);
        }

        /// <summary>
        /// Physic rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPhysicRectangle()
        {
            return physicRec;
        }


    }
}
