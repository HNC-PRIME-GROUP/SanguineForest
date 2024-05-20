using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Object with no real collision body, but that can add some information
    /// </summary>
    internal class Decor : GameObject
    {

        private AnimationModule _animationModule;
        public SpriteModule _spriteModule;
        private string grassType;


        public Decor(Vector2 position, float rotation, ContentManager content, string grassType) : base(position, rotation)
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Grass"), Extention.Extentions.SpriteLayer.environment2);
            Dictionary<string, AnimationSequence> animations = new Dictionary<string, AnimationSequence>()
            {
                { "Flower", new AnimationSequence(Vector2.Zero, 4) },
                { "Grass1", new AnimationSequence(new Vector2(0, 512), 4) },
                { "Grass2", new AnimationSequence(new Vector2(0, 1024), 4) },
                { "Grass3", new AnimationSequence(new Vector2(0, 1536), 4) },
                { "Grass4", new AnimationSequence(new Vector2(0, 2048), 4) }
            };
            SpriteSheetData spriteSheet = new SpriteSheetData(new Rectangle(0, 0, 512, 512), animations);
            _animationModule = new AnimationModule(this, Vector2.Zero, spriteSheet, _spriteModule);
            _spriteModule.AnimtaionInitialise(_animationModule);
            this.grassType = grassType;
            _animationModule.SetAnimationSpeed(0.2f);
            _spriteModule.SetDrawRectangle(new Rectangle(GetPosition().ToPoint(), new Vector2(78, 78).ToPoint()));

        }

        public new void UpdateMe()
        {
            _spriteModule.UpdateMe();
            _animationModule.UpdateMe();
            _animationModule.Play(grassType);
        }


        public void DrawMe(SpriteBatch spriteBatch)
        {
                _spriteModule.DrawMe(spriteBatch);
        }
    }

    public struct DecorGrassData
    {
        public Vector2 Position;
        public float Rotation;
        public string GrassType;
    }
}
