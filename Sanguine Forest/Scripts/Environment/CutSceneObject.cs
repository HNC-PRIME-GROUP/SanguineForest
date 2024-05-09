using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class CutSceneObject : GameObject
    {
        private SpriteModule _spriteModule;


        public CutSceneObject (Vector2 position, float rotation, ContentManager content, float scale) : base (position, rotation)         
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>(""), Extention.Extentions.SpriteLayer.environment1);
            _spriteModule.SetScale(scale);
        }   
        
        public new void UpdateMe()
        {
            base.UpdateMe();
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
        }

    }

    public struct CutSceneObjectData
    {
        public Vector2 Position;
        public float Rotation;
        public float Scale;
    }
}
