using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{
    /// <summary>
    /// Platform that can move
    /// </summary>
    internal class MoveblePlatform : Platform
    {
        public MoveblePlatform(Vector2 position, float rotation): base(position, rotation) { }
    }
}
