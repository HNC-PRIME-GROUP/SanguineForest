using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class CheckPoint : GameObject
    {

        public PhysicModule PhysicModule;

        public enum CheckPointStates
        {
            wait,
            triggered
        }

        public CheckPointStates currState;

        public CheckPoint(Vector2 position, float rotation, Vector2 size) : base(position, rotation) {
            PhysicModule = new PhysicModule(this, Vector2.Zero, size);
        }


        public new void UpdateMe()
        {
            PhysicModule.UpdateMe();

            switch(currState)
            {
                case CheckPointStates.wait:
                    break;
                case CheckPointStates.triggered:
                    break;
            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            DebugManager.DebugRectangle(PhysicModule.GetPhysicRectangle());
        }


        public void DeleteMe()
        {
            PhysicManager.RemoveObject(PhysicModule);
        }

        public override void Collided(Collision collision)
        {
            base.Collided(collision);
            if(collision.GetCollidedPhysicModule().GetParent() is Character2&&currState==CheckPointStates.wait)
            {
                currState = CheckPointStates.triggered;
            }
        }
    }

    public struct CheckPointData
    {
        public Vector2 position;        
        public Vector2 size;
    }
}
