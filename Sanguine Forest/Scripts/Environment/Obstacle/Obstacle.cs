using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.Environment.Obstacle
{
    internal class Obstacle : GameObject
    {
        private PlayerState playerState;
        private Character playerCharacter;
        private SpriteModule spriteModule;
        private PhysicModule physicModule; // The collision detection module
        private int alcoholLevelThreshold; // Alcoholic level required for this obstacle to be visible


        public Obstacle(Vector2 position, float rotation, Texture2D texture, int alcoholLevelThreshold, Character PlayerCharacter)
           : base(position, rotation)
        {
            this.spriteModule = new SpriteModule(this, Vector2.Zero, texture, Extentions.SpriteLayer.obstacles);
            this.physicModule = new PhysicModule(this, Vector2.Zero, new Vector2(texture.Width, texture.Height));
            this.physicModule.isPhysicActive = true;
            this.alcoholLevelThreshold = alcoholLevelThreshold;
            this.playerCharacter = PlayerCharacter;  // Set the PlayerCharacter parameter
        }
        public void Draw(SpriteBatch spriteBatch, int currentAlcoholLevel)
        {
            if (currentAlcoholLevel >= alcoholLevelThreshold)
            {
                // Delegate drawing to the spriteModule
                spriteModule.DrawMe(spriteBatch);
            }
        }
        public override void UpdateMe()
        {
            base.UpdateMe();
            physicModule.UpdateMe();
        }
        // Override the collided method
        public override void Collided(Collision collision)
        {
            if (collision.GetCollidedPhysicModule().GetParent() == playerCharacter)
            {
                playerState.IsAlive = false; // Mark the player as dead if collided with obstacle
            }
        }
    }
}