using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sanguine_Forest
{
    /// <summary>
    /// Animation controller. Need a SpriteModule to know what picture it should control. 
    /// </summary>
    internal class AnimationModule : Module
    {
        private SpriteModule _spriteModule;
        private Rectangle frameRectangle;
        

        public AnimationModule(GameObject parent, Vector2 shift, Rectangle framRec, SpriteModule spriteModule) : base (parent, shift) 
        {
            _spriteModule = spriteModule;
            frameRectangle = framRec;
        }


        public override void UpdateMe()
        {

        }

        public Rectangle GetFrameRectabgle()
        {
            return frameRectangle;
        }




    }
}
