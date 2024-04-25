using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.Extention
{
    public abstract class Component
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spritebach);

        public abstract void Update(GameTime gameTime);
    }
}
