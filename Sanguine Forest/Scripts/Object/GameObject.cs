using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{

    /// <summary>
    /// Base for all objects that exist in the game world, except UI
    /// Store data about position
    /// </summary>
    internal class GameObject
    {
        /// <summary>
        /// Game object position
        /// </summary>
        private Vector2 position;


        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2 position) 
        {
            this.position = position;
        }

        /// <summary>
        /// Use the code word 'override' to override this method in children. 
        /// </summary>
        /// <param name="collision">Game object that collided with the element</param>
        public virtual void Collided(GameObject collision) {}

        /// <summary>
        /// Use the word 'override' to override this method in children.
        /// </summary>
        public virtual void UpdateMe() { }


    }
}
