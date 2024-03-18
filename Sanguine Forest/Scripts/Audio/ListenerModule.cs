using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{
    /// <summary>
    /// Module that store data for audio manager about position of listener
    /// </summary>
    internal class ListenerModule : Module
    {
        public ListenerModule(GameObject parent, Vector2 shift): base (parent, shift) { }
    }
}
