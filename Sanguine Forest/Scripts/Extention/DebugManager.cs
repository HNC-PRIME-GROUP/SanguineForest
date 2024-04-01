using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    static class DebugManager
    {

        static public SpriteFont DebugFont;
        static public Texture2D DebugTexture;
        static public SpriteBatch SpriteBatch;
        static public bool isWorking;
        static public Camera Camera;

        static public void DebugString(string message, Vector2 pos)
        {
            if (isWorking)
            {
                SpriteBatch.DrawString(DebugFont, message, pos, Color.White);
            }
        }

        static public void DebugRectangle(Rectangle rec)
        {
            if (isWorking)
            {
                SpriteBatch.Draw(DebugTexture, rec, Color.White);
            }
        }

    }
}
