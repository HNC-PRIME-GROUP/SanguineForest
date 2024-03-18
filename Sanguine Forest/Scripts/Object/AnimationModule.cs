using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Extention;
using System.Diagnostics.Contracts;

namespace Sanguine_Forest
{

    /// <summary>
    /// Animation controller. Need a SpriteModule to know what picture it should control. 
    /// </summary>
    internal class AnimationModule : Module
    {
        private SpriteModule _spriteModule;
        private SpriteSheetData _spriteSheetData;
        private AnimationSequence _animationSequence;

        private float animationSpeed;
        private float animationTimer;
        private bool isTimeToNextFrame=false;

        private Rectangle _currentFrame;
        private int _currentFrameIndex=0;
        private string _currentFramStringInd;



        
        /// <summary>
        /// Animation module control frame rectangle for sprite module
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="shift"></param>
        /// <param name="spriteSheetData"></param>
        /// <param name="spriteModule"></param>
        public AnimationModule(GameObject parent, Vector2 shift, SpriteSheetData spriteSheetData, SpriteModule spriteModule) : base (parent, shift) 
        {
            _spriteModule = spriteModule;
            animationSpeed = 1f;
            _currentFrame = spriteSheetData.frameRec;

        }

        /// <summary>
        /// Timer for animations
        /// </summary>
        public override void UpdateMe()
        {
            animationTimer += Extentions.globalTime;
            if(animationTimer>animationSpeed)
            {
                animationTimer = 0f;
                isTimeToNextFrame = true;
            }
            else
            {
                isTimeToNextFrame = false;
            }
        }
        
        /// <summary>
        /// Method to call in an Update. Repeat animation after finish.
        /// </summary>
        /// <param name="sequence"></param>
        public void Play(string sequence)
        {
            if(sequence!=_currentFramStringInd)
            {
                _currentFramStringInd = sequence;
                _animationSequence = _spriteSheetData.animationSequences[sequence];
                _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
                animationTimer = 0f;
                _currentFrameIndex = 0;
            }
            if(isTimeToNextFrame)
            {
                if (_currentFrameIndex < _animationSequence.framCount)
                    _currentFrame.Location = new Vector2(_currentFrame.Width,0).ToPoint();
                else
                    _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
            }

        }

        /// <summary>
        /// Nethod to call in an Update. Play an animation once. 
        /// </summary>
        /// <param name="sequence"></param>
        public void PlayOnce(string sequence)
        {
            if (sequence != _currentFramStringInd)
            {
                _currentFramStringInd = sequence;
                _animationSequence = _spriteSheetData.animationSequences[sequence];
                _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
                animationTimer = 0f;
                _currentFrameIndex = 0;
            }
            if (isTimeToNextFrame)
            {
                if (_currentFrameIndex < _animationSequence.framCount)
                    _currentFrame.Location = new Vector2(_currentFrame.Width, 0).ToPoint();
            }
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

    /// <summary>
    /// Data for animator module
    /// </summary>
    public class SpriteSheetData
    {
        public Rectangle frameRec;        
        public Dictionary<string, AnimationSequence> animationSequences;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameRec">Fram rectangle for this sprite sheet</param>
        /// <param name="animationSequence">All animation sequences in this sprite sheet</param>
        public SpriteSheetData(Rectangle frameRec, Dictionary<string, AnimationSequence> animationSequence)
        {
            this.frameRec = frameRec;            
            animationSequences = animationSequence;

        }

    }

    /// <summary>
    /// One animation sequence
    /// </summary>
    public class AnimationSequence
    {
        //start location of frame rectangle on a sprite sheet
        public Vector2 startFramPos;
        public int framCount;        
    }
  

}
