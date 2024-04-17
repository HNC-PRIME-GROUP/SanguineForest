using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{
    /// <summary>
    /// Object with no real collision body, but that can add some information
    /// </summary>
    internal class Decor : GameObject
    {

        public Decor(Vector2 position, float rotation) : base(position, rotation) { }


        public void DrawMe(SpriteBatch spriteBatch)
        {

        }
    }
}
