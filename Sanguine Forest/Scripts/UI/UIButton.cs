using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sanguine_Forest
{
    /// <summary>
    /// UI button
    /// </summary>
    internal class UIButton
    {
        public String Txt;
        private Vector2 Pos;
        private Color _color;
        private SpriteFont _font;

        // Indicate if this button is currently active
        public bool IsActive { get; set; }

        public UIButton(String txt, SpriteFont font, Vector2 pos)
        {
            Txt = txt;
            _font = font;
            Pos = pos;
            _color = Color.White;

        }

        public void Update()
        {
            _color = IsActive ? Color.White : Color.DarkGray; // change color if active
        }

        public void Draw(SpriteBatch sb)
        {
            if (_font == null)
            {
                throw new Exception("Font is null!");
            }
            if (string.IsNullOrEmpty(Txt))
            {
                throw new Exception("Text is null or empty!");
            }
            sb.DrawString(_font, Txt, Pos, _color, 0, Vector2.Zero, GetFontScale(), SpriteEffects.None, 0);
        }
        private float GetFontScale()
        {
            //Vector2 currentResolution = new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            //float scale = currentResolution.X / designResolution.X;
            return 1;
        }
    }
}

