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
        public SpriteModule _SpriteModule;
        private AnimationModule _animationModule;
        private string NPCType;

        // Text display
        public string _cutsceneText;
        private SpriteFont _font;
        private Vector2 _textPosition;

        private Texture2D _semiTransparentTexture;

        public string CutsceneText
        {
            get => _cutsceneText;
            private set => _cutsceneText = value;
        }

        public Vector2 TextPosition
        {
            get => _textPosition;
            private set => _textPosition = value;
        }

        public CutSceneObject(Vector2 position, float rotation, ContentManager content, string NPCType, SpriteFont font, Texture2D semiTransparentTexture) : base(position, rotation)
        {
             _SpriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Sprites_NPC_v1"), Extentions.SpriteLayer.environment2);
            Dictionary<string, AnimationSequence> animations = new Dictionary<string, AnimationSequence>()
            {   
                { "Pink", new AnimationSequence(Vector2.Zero, 1) },
                { "Blue", new AnimationSequence(new Vector2(0, 512), 1) },
                { "Green", new AnimationSequence(new Vector2(0, 1024), 1) },
            };
            SpriteSheetData spriteSheet = new SpriteSheetData(new Rectangle(0, 0, 512, 512), animations);
            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheet, _SpriteModule);
            _SpriteModule.AnimtaionInitialise(_animationModule);
            this.NPCType = NPCType;
            _animationModule.SetAnimationSpeed(0.8f);
            _SpriteModule.SetScale(0.25f);
            _SpriteModule.SetSpriteEffects(SpriteEffects.FlipHorizontally);

            // Initialize text fields 
            _font = font;
            _semiTransparentTexture = semiTransparentTexture;

        }

        public new void UpdateMe()
        {

            _SpriteModule.UpdateMe();
            _animationModule.UpdateMe();
            _animationModule.Play(NPCType);

            base.UpdateMe();

        }

        public void DrawMe(SpriteBatch sp, bool showPressE)
        {
            _SpriteModule.DrawMe(sp);

            if (showPressE)
            {
                string pressEText = "Press E";
                Vector2 pressETextSize = _font.MeasureString(pressEText);
                Vector2 pressETextPosition = new Vector2(GetPosition().X, GetPosition().Y - 30); // Use GetPosition() method

                Rectangle backgroundRectangle = new Rectangle((int)pressETextPosition.X - 5, (int)pressETextPosition.Y - 5, (int)pressETextSize.X + 10, (int)pressETextSize.Y + 10);
                sp.Draw(_semiTransparentTexture, backgroundRectangle, Color.Black * 0.5f);
                sp.DrawString(_font, pressEText, pressETextPosition, Color.White);
            }
        }

        public void SetCutsceneText(string text, Vector2 textPosition)
        {
            _cutsceneText = text;
            _textPosition = textPosition;
        }
    }

    public struct CutSceneObjectData
    {
        public Vector2 Position;
        public float Rotation;
        public float Scale;
        public string NPCType;
    }

    public struct CutSceneDialogue
    {
        //public string Speaker;      // Name of the speaker (optional)
        public string Text;         // The dialogue text
        public Vector2 Position;    // Position where the text should appear
    }
}
