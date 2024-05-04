using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Sanguine_Forest
{

    /// <summary>
    /// Platform to jump on it
    /// </summary>
    internal class Platform : GameObject
    {


        //Sprite
        public SpriteModule _spriteModule;

        //Collision
        public PhysicModule platformPhysic;


        public Platform(Vector2 position, float rotation, Vector2 platformSize, ContentManager content, Dictionary<string,Rectangle> tileDictionary, string[,] tileMap): base(position, rotation) 
        {

            //Sptire and graphic
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/TileSet_Mossy"), Extention.Extentions.SpriteLayer.environment1);

            
            //_spriteModule.SetDrawRectangle(platformPhysic.GetPhysicRectangle());


          
            _spriteModule.TillingMe(tileDictionary,tileMap,new Rectangle((int)Math.Round(GetPosition().X), (int)Math.Round(GetPosition().Y), 
                (int)Math.Round(platformSize.X / 64)*64, (int)Math.Round(platformSize.Y / 64)*64), new Rectangle(0,0,64,64));

            //Collision
            platformPhysic = new PhysicModule(this, new Vector2(18,25), new Vector2((int)Math.Round(platformSize.X / 64)*64-36, (int)Math.Round(platformSize.Y / 64) * 64-50));
            platformPhysic.isPhysicActive = true;


        }

        public new void UpdateMe()
        {   
            _spriteModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch spriteBatch) 
        {
            _spriteModule.DrawMe(spriteBatch);
            //DebugManager.DebugRectangle(platformPhysic.GetPhysicRectangle());
        }

        /// <summary>
        /// Return exactly rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPlatformRectangle()
        {
            return platformPhysic.GetPhysicRectangle();
        }

    }

    internal struct PlatformData
    {
        public Vector2 position;
        public float rotation;
        public Vector2 platformSize;
    }
}
