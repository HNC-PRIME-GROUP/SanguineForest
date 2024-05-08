using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Extention;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.Environment.Obstacle
{
    internal class Thorns : GameObject
    {

        //Visual effect
        private SpriteModule _spriteModule;
        private Vector2 tileSize ;

        //Collision
        private PhysicModule _physicModule;

        /// <summary>
        /// Create an instance of thorns
        /// </summary>
        /// <param name="positioon"></param>
        /// <param name="rotation">If it's equal to 0 - horisontal, all other - vertical</param>
        /// <param name="content"></param>
        public Thorns(Vector2 positioon, float rotation, ContentManager content, Vector2 thornsSize) : base(positioon, rotation)
        {

            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/thorns_02"), Extentions.SpriteLayer.environment1);
            tileSize = new Vector2(128, 128);
            Dictionary<string, Rectangle> dictionary = new Dictionary<string, Rectangle>()
            {
                { "tex", new Rectangle(0,0,512,512) }
            };

            string[,] tileMap = new string[(int)Math.Round(thornsSize.Y / tileSize.Y), (int)Math.Round(thornsSize.X / tileSize.X)];
            for (int i = 0; i < tileMap.GetLength(0); i++)
            {
                for (int j = 0; j < tileMap.GetLength(1); j++)
                {
                    tileMap[i, j] = "tex";
                }
            }

            //Depends on rotation we do vertical or horisontal draw rectangle;            

                _spriteModule.TillingMe(dictionary, tileMap, 
                    new Rectangle((int)Math.Round(GetPosition().X), (int)Math.Round(GetPosition().Y), (int)Math.Round(thornsSize.X / tileSize.Y) * (int)tileSize.Y, (int)Math.Round(thornsSize.Y / tileSize.X) * (int)tileSize.X), 
                    new Rectangle(0, 0, (int)tileSize.Y, (int)tileSize.X));


                _physicModule = new PhysicModule(this, new Vector2(0, 0), new Vector2((int)Math.Round(thornsSize.X / tileSize.Y) * tileSize.Y ,
                    (int)Math.Round(thornsSize.Y / tileSize.X) * tileSize.X));
            
            
        }

        public override void Collided(Collision collision)
        {
            base.Collided(collision);
            if(collision.GetCollidedPhysicModule().GetParent() is Character2)
            {
                Character2 character = (Character2)collision.GetCollidedPhysicModule().GetParent();
                if (character.GetCharacterState() != Character2.CharState.Death)
                {
                    character.Death();
                }

            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            _spriteModule.DrawMe(sp);
            //DebugManager.DebugRectangle(_physicModule.GetPhysicRectangle());
        }


    }

    public struct ThornsData
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 ThornsSize;
    }
}
