using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Sanguine_Forest
{

    /// <summary>
    /// Platform that can fall if character step on it
    /// </summary>
    internal class FallingPlatform : Platform
    {
        private float timeToFall;
        private AnimationModule animationModule;

        public FallingPlatform(Vector2 position, 
                                float rotation, 
                                Vector2 platformSize, 
                                ContentManager content,                                 
                                Dictionary<string,Rectangle> tileDictionary,
                                string[,] tileMap,
                                float timeToFall
                                ): 
            base (position, rotation, platformSize, content, tileDictionary, tileMap ) 
        {
            
        
        }
    }
}
