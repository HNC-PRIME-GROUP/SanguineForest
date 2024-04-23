using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Extention
{
    static class Extentions
    {
        static public float globalTime;
        static public float ModulasClamp(float value, float min, float max)
        {
            float ret;
            if (value >= max)
            {
                ret = min + value % (max);
                return ret;
            }
            else if (value < min)
            {
                ret = (max) - Math.Abs(value % (max));
                return ret;
            }
            return value;
        }

        static public decimal ModulasClamp(decimal value, decimal min, decimal max) 
        {
            decimal ret;
            if (value >= max)
            {
                ret = min + value % (max);
                return ret;
            }
            else if (value < min)
            {
                ret = (max) - Math.Abs(value % (max));
                return ret;
            }
            return value;
        }


        public enum SpriteLayer
        {

            foreground=0,
            character1,
            character2,
            environment1,
            environment2,
            background_Back=3,
            background_Mid=2,
            background_Fore=1,
            obstacles,
            Length
        }

    }
}
