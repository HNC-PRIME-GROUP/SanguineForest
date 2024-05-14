using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class CutSceneObject : GameObject
    {


        //graphic
        public SpriteModule _spriteModule;
        private AnimationModule _animationModule;
        private string NPCType;

        // Text display
        private string _cutsceneText;
        private SpriteFont _font;


        public CutSceneObject(Vector2 position, float rotation, ContentManager content, string NPCType, string cutsceneText, SpriteFont font) : base (position, rotation)         
        {
             _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Sprites_NPC_v1"), Extention.Extentions.SpriteLayer.environment2);
            Dictionary<string, AnimationSequence> animations = new Dictionary<string, AnimationSequence>()
            {   { "Pink", new AnimationSequence(Vector2.Zero, 1) },
                { "Blue", new AnimationSequence(new Vector2(0, 512), 1) },
                { "Green", new AnimationSequence(new Vector2(0, 1024), 1) },
            };
            SpriteSheetData spriteSheet = new SpriteSheetData(new Rectangle(0, 0, 512, 512), animations);
            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheet, _spriteModule);
            _spriteModule.AnimtaionInitialise(_animationModule);
            this.NPCType = NPCType;
            _animationModule.SetAnimationSpeed(0.8f);
            _spriteModule.SetScale(0.25f);

            // Initialize text fields
            _cutsceneText = cutsceneText;
            _font = font;


        }   

        
        public new void UpdateMe()
        {
            _spriteModule.UpdateMe();
            _animationModule.UpdateMe();
            _animationModule.Play(NPCType);

            base.UpdateMe();

        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);

            // Draw the text
            if (!string.IsNullOrEmpty(_cutsceneText))
            {
                Vector2 textPosition = new Vector2(position.X, position.Y - 20); // Adjust text position as needed
                sp.DrawString(_font, _cutsceneText, textPosition, Color.White);
            }
        }

    }

    public struct CutSceneObjectData
    {
        public Vector2 Position;
        public float Rotation;
        public float Scale;
        public string NPCType;
    }
}
