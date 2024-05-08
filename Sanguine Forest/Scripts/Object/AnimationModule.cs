using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Extention;
using System.Diagnostics.Contracts;
using System.Linq;
using System;

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

        //Messing up with events
        public event EventHandler AnimationEnd;


        
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
            animationSpeed = 0f;
            _currentFrame = spriteSheetData.frameRec;
            _spriteSheetData = spriteSheetData;
            List<string> temp = spriteSheetData.animationSequences.Keys.ToList<string>();
            _animationSequence = spriteSheetData.animationSequences[temp[0]] ;
            

        }

        /// <summary>
        /// Timer for animations
        /// </summary>
        public override void UpdateMe()
        {
            animationTimer += Extentions.globalTime;
            if(animationTimer>animationSpeed && _animationSequence.animationState!=AnimationSequence.AnimationState.stopped)
            {
                animationTimer = 0f;
                isTimeToNextFrame = true;

                if (_currentFrameIndex < _animationSequence.framCount)
                {
                    _currentFrame.Location += new Vector2(_currentFrame.Width, 0).ToPoint();
                    _currentFrameIndex++;
                }
                else
                {
                    switch (_animationSequence.animationState)
                    {
                        case AnimationSequence.AnimationState.playing:
                            _currentFrameIndex = 0;
                            _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
                            break;
                        case AnimationSequence.AnimationState.playingOnce:
                            AnimationEnd?.Invoke(this, EventArgs.Empty);
                            _animationSequence.animationState = AnimationSequence.AnimationState.stopped;
                            break;
                        case AnimationSequence.AnimationState.stopped:
                            break;
                    }
                }

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
                _animationSequence.animationState = AnimationSequence.AnimationState.stopped;
                _currentFramStringInd = sequence;                
                _animationSequence = _spriteSheetData.animationSequences[sequence];
                _animationSequence.animationState=AnimationSequence.AnimationState.playing;

                _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
                animationTimer = 0f;
                _currentFrameIndex = 0;
            }

            //if (isTimeToNextFrame)
            //{
            //    if (_currentFrameIndex < _animationSequence.framCount)
            //        _currentFrame.Location = new Vector2(_currentFrame.Width, 0).ToPoint();
            //    else
            //        _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
            //}


        }

        /// <summary>
        /// Nethod to call in an Update. Play an animation once. 
        /// </summary>
        /// <param name="sequence"></param>
        public void PlayOnce(string sequence)
        {
            if (sequence != _currentFramStringInd)
            {
                _animationSequence.animationState = AnimationSequence.AnimationState.stopped;
                _currentFramStringInd = sequence;
                _animationSequence = _spriteSheetData.animationSequences[sequence];
                _animationSequence.animationState = AnimationSequence.AnimationState.playingOnce;
                _currentFrame.Location = _animationSequence.startFramPos.ToPoint();
                animationTimer = 0f;
                _currentFrameIndex = 0;
            }
            //if (isTimeToNextFrame)
            //{
            //    if (_currentFrameIndex < _animationSequence.framCount)
            //        _currentFrame.Location = new Vector2(_currentFrame.Width, 0).ToPoint();
            //}
        }

        /// <summary>
        /// Return frame rectangle for sprite module
        /// </summary>
        /// <returns></returns>
        public Rectangle GetFrameRectangle()
        {
            return _currentFrame;
        }

        /// <summary>
        /// Get animation speed
        /// </summary>
        /// <returns></returns>
        public float GetAnimationSpeed()
        {
            return animationSpeed;
        }

        /// <summary>
        /// Change animation speed
        /// </summary>
        /// <param name="speed"></param>
        public void SetAnimationSpeed(float speed)
        {
            animationSpeed = speed;
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
        public enum AnimationState
        {
            playing,
            playingOnce,
            stopped
        }

        public AnimationState animationState;


        public AnimationSequence(Vector2 startPos, int framCount)
        {
            startFramPos = startPos;
            this.framCount = framCount;
            animationState = AnimationState.stopped;
        }

        //start location of frame rectangle on a sprite sheet
        public Vector2 startFramPos;
        public int framCount;        
    }
  

}
