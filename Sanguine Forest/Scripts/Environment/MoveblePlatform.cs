using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Policy;

namespace Sanguine_Forest
{
    /// <summary>
    /// Platform that can move
    /// </summary>
    internal class MoveblePlatform : Platform
    {
        private Vector2 startPoint;
        private Vector2 targetPoint;
        private float speed; 

        public MoveblePlatform(Vector2 position, 
                                float rotation,
                                Vector2 platformSize,
                                ContentManager content,
                                string texturePath,
                                Vector2 targetPoint,
                                float speed): 
                                base(position, rotation, platformSize, content, texturePath) 
        {
            startPoint = position;
            this.targetPoint = targetPoint;
            this.speed = speed;
        }

        public new void UpdateMe()
        {

        }
        
    }
}
