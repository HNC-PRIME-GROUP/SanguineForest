using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Data about all game systems that should be loaded before cut scene start
    /// </summary>
    internal class CutScene
    {

        //graphic
        public SpriteModule _SpriteModule;
        private AnimationModule _animationModule;
        private Dictionary<string, AnimationSequence> _animations;
        private SpriteSheetData _spriteSheetData;

        //Character state
        public enum CharState
        {
            idle,
            stop
        }

        private CharState _currentState;


    }
}
