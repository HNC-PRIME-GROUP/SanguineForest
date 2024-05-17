using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Extention;


namespace Sanguine_Forest.Scripts.Environment
{
    internal class PatchDecor : GameObject
    {
        public SpriteModule _spriteModule;

        public PatchDecor(Vector2 position, ContentManager content, string patchType) : base(position, 0)
        {
            Texture2D texture = content.Load<Texture2D>(patchType);
            _spriteModule = new SpriteModule(this, Vector2.Zero, texture, Extentions.SpriteLayer.patch_texture_layer);
            _spriteModule.SetDrawRectangle(new Rectangle(GetPosition().ToPoint(), new Vector2(texture.Width, texture.Height).ToPoint()));
        }

        public new void UpdateMe()
        {
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            _spriteModule.DrawMe(spriteBatch);
        }
    }

    public struct PatchDecorData
    {
        public Vector2 Position;
        public string PatchType;
    }
}

