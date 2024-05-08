using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class Obstacle : GameObject
    {
        
        //Visual effect
        private SpriteModule spriteModule;
        private AnimationModule animationModule;
        private string slimeType;

        // The collision detection module
        private PhysicModule physicModule;

        //Speed and movement
        private float speed;
        private Vector2 startPos;
        private Vector2 endPos;
        private Vector2 velocity;

        //Move target selection
        private enum EnemyMove
        {
            there,
            back
        }
        private EnemyMove currMove;

        public Obstacle(Vector2 position, float rotation, ContentManager content, string Type, Vector2 finPosition, float speed, Vector2 obstacleSize)
           : base(position, rotation)
        {

            //Visual effects setting
            this.spriteModule = new SpriteModule(this, Vector2.Zero, content.Load<Texture2D>("Sprites/Slimes"), Extentions.SpriteLayer.obstacles);
            Dictionary<string, AnimationSequence> animationDictionary = new Dictionary<string, AnimationSequence>()
            {
                {"Green", new AnimationSequence(Vector2.Zero,4) },
                {"Brown", new AnimationSequence(new Vector2(0,512),4) }
            };

            SpriteSheetData spriteSheetData = new SpriteSheetData(new Rectangle(0, 0, 512, 512), animationDictionary);

            animationModule = new AnimationModule(this, Vector2.Zero, spriteSheetData, spriteModule);
            spriteModule.AnimtaionInitialise(animationModule);
            spriteModule.SetDrawRectangle(new Rectangle(GetPosition().ToPoint(), obstacleSize.ToPoint()));
            slimeType = Type;
            animationModule.SetAnimationSpeed(0.8f);

            //Physic setting
            this.physicModule = new PhysicModule(this, new Vector2(100,100), obstacleSize-new Vector2(100,100));

            //Movement setting
            this.speed=speed;
            this.startPos = position;
            this.endPos = finPosition;
            currMove = EnemyMove.there;




        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteModule.DrawMe(spriteBatch);
            DebugManager.DebugRectangle(physicModule.GetPhysicRectangle());
        }
        public override void UpdateMe()
        {
            base.UpdateMe();
            
            physicModule.UpdateMe();
            spriteModule.UpdateMe();
            animationModule.UpdateMe();
            animationModule.Play(slimeType);

            switch(currMove)
            {
                case EnemyMove.there:
                    velocity = endPos - position;
                    velocity.Normalize();
                    if(Math.Abs(position.X-endPos.X)>speed*1.5f&&Math.Abs(position.Y-endPos.Y)>speed*1.5f)
                    {
                        position += velocity * speed;
                    }
                    else
                    {
                        currMove=EnemyMove.back;
                    }
                    break;
                case EnemyMove.back:
                    velocity = startPos - position;
                    velocity.Normalize();
                    if (Math.Abs(position.X - startPos.X) > speed * 1.5f && Math.Abs(position.Y - startPos.Y) > speed * 1.5f)
                    {
                        position += velocity * speed;
                    }
                    else
                    {
                        currMove = EnemyMove.there;
                    }
                    break;
            }


        }
        // Override the collided method
        public override void Collided(Collision collision)
        {
            if (collision.GetCollidedPhysicModule().GetParent() is Character2)
            {
                Character2 character = (Character2)collision.GetCollidedPhysicModule().GetParent();
                character.Death();
            }
        }
    }

    public struct ObstacleData
    {
        public Vector2 Position;
        public float Rotation;
        public string slimeType;
        public Vector2 finPosition;
        public float Speed;
        public Vector2 Size;
    }
}