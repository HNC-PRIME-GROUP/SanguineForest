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


        /// <summary>
        /// Method for camera following
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pos"></param>
        /// <param name="camera"></param>
        static public void DebugString(string message, Vector2 pos, Camera camera)
        {
            if (isWorking)
            {
                SpriteBatch.DrawString(DebugFont, message, new Vector2(-camera.position.X+pos.X, -camera.position.Y+pos.Y), Color.White);
            }
        }

        /// <summary>
        /// Method without camera  (put in SpriteBanch that didn't take camera as an input)
        /// </summary>
        static public void DebugString(string message, Vector2 pos)
        {
            if (isWorking)
            {
                SpriteBatch.DrawString(DebugFont, message, new Vector2(pos.X, pos.Y), Color.White);
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
