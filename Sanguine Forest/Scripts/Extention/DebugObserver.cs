using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest
{
    internal class DebugObserver : GameObject
    {

        public DebugObserver(Vector2 position, float rotation) : base(position, rotation) 
        {            
        
        }

        public void UpdateMe(KeyboardState curr)
        {
            if(curr.IsKeyDown(Keys.A))
            {
                position.X -= 10;
            
            }

            if(curr.IsKeyDown(Keys.D))
            {
                position.X += 10;
            }

        }

    }
}
