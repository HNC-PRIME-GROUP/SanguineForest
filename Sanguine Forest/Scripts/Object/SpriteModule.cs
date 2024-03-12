using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{
    /// <summary>
    /// Picture for any game object. 
    /// </summary>
    internal class SpriteModule : Module
    {




        private Texture2D texture;
        private Color color = Color.White;


        public SpriteModule(GameObject parent, Vector2 shift) : base(parent, shift) 
        {
        
        }


        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, GetPosition(), color);
        }


        public void SetColor(Color color)
        {
            this.color = color;
        }

        public Color GetColor()
        {
            return color;
        }
    }
}
