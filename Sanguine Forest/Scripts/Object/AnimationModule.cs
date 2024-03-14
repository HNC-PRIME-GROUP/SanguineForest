using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

        

        public AnimationModule(GameObject parent, Vector2 shift, SpriteSheetData spriteSheetData, SpriteModule spriteModule) : base (parent, shift) 
        {
            _spriteModule = spriteModule;
            animationSpeed = 1f;
            
        }


        public override void UpdateMe()
        {

        }

        public void Play(string sequence)
        {
            
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
