using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Extention;

namespace Sanguine_Forest
{

    /// <summary>
    /// Animation controller. Need a SpriteModule to know what picture it should control. 
    /// </summary>
    internal class AnimationModule : Module
    {
        private SpriteModule _spriteModule;
        private SpriteSheetData _spriteSheetData;

        private float animationSpeed;
        private float animationTimer;
        private Rectangle _currentFrame;

        

        public AnimationModule(GameObject parent, Vector2 shift, SpriteSheetData spriteSheetData, SpriteModule spriteModule) : base (parent, shift) 
        {
            _spriteModule = spriteModule;
            animationSpeed = 1f;
            _currentFrame = spriteSheetData.frameRec;



        }


        public override void UpdateMe()
        {

        }

        public void Play(string sequence)
        {
            

        }

        public void PlayOnce(string sequence)
        {

        }

        /// <summary>
        /// Return frame rectangle for sprite module
        /// </summary>
        /// <returns></returns>
        public Rectangle GetFrameRectangle()
        {
            return _currentFrame;
        }

        


    }


    public class SpriteSheetData
    {
        public Rectangle frameRec;
        public int rows, columns;        
        public Dictionary<string, int[]> animationSequence;

        public SpriteSheetData(Rectangle frameRec, Dictionary<string, int[]> animationSequence)
        {
            this.frameRec = frameRec;            
            this.animationSequence = animationSequence;

        }

    }

  

}
