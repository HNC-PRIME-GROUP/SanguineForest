using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sanguine_Forest.Scripts.Extention;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.Environment
{
    public class Sprite : Component
    {
        public Texture2D _texture;

        protected float _layerBackground { get; set; }

        public float LayerBackground
        {
            get { return _layerBackground; }
            set 
            { 
                _layerBackground = value; 
            }
        }

        public Vector2 PositionBackground;

        public Rectangle RectangleBackground
        {
            get
            {
                return new Rectangle((int)PositionBackground.X, (int)PositionBackground.Y, _texture.Width, _texture.Height);
            }
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }


        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebach)
        {
            spritebach.Draw(_texture, PositionBackground, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, LayerBackground);
        }

    }
}
