using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Extention;

namespace Sanguine_Forest
{

    /// <summary>
    /// Base for all objects that exist in the game world, except UI
    /// Store data about position
    /// </summary>
    abstract class GameObject
    {
        #region Object transform variables
        /// <summary>
        /// Game object position
        /// </summary>
        protected Vector2 position;

        public void Move(Vector2 movement)
        {
            this.position += movement;
        
        }

        /// <summary>
        /// Rotation with clamp between -PI and PI
        /// </summary>
        private float rotation;
        private float Rotation
        {
            get => rotation;
            set
            {
                rotation = Extentions.ModulasClamp(value, (float)-Math.PI, (float)Math.PI);
            }
        }
        #endregion


        #region Object transform Get/Set methods
        public Vector2 GetPosition()
        {
            Console.WriteLine($"Getting Position");
            return position;
        }

        public void SetPosition(Vector2 position) 
        {
            Console.WriteLine($"Setting Position");
            this.position = position;
            
        }

        public float GetRotation()
        {
            return Rotation;
        }

        public void SetRotation(float rot)
        {
            Rotation = rot;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Start position</param>
        /// <param name="rotation">Start rotation</param>
        public GameObject(Vector2 position, float rotation)
        {
            SetPosition(position);
            SetRotation(rotation);
        }


        /// <summary>
        /// Use the word 'override' to override this method in children.
        /// </summary>
        public virtual void UpdateMe() { }



        #region Methods for modules

        /// <summary>
        /// Use the code word 'override' to override this method in children. 
        /// </summary>
        /// <param name="collision">Game object that collided with the element</param>
        public virtual void Collided(Collision collision) { }


        /// <summary>
        /// Get parent can be called from modules
        /// </summary>
        /// <returns></returns>
        public virtual GameObject GetParent()
        {
            return this;
        }

        #endregion
    }
}
