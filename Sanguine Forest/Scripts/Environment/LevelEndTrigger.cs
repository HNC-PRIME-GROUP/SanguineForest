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
    internal class LevelEndTrigger : GameObject
    {

        private SpriteModule _spriteModule;
        private PhysicModule _physicsModule;

        private PlayerState _playerState;

        public LevelEndTrigger(Vector2 position, float rotation, ContentManager content, PlayerState playerState) : base(position, rotation)         
        {
            _spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>(""), Extention.Extentions.SpriteLayer.environment1);
            _physicsModule = new PhysicModule(this, Vector2.Zero, new Vector2(256, 512));
            _playerState = playerState;
        }

        public override void Collided(Collision collision)
        {
            if(collision.GetCollidedPhysicModule().GetParent() is Character2)
            {
                _playerState.lvlCounter++;
                //Logic with UI to win and load the next level
            }
        }


    }
}
