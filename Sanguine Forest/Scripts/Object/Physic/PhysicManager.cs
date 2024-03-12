using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Check if two physic modules intersepted and sent them data about each other
    /// </summary>
    static  class PhysicManager
    {

        static private List<PhysicModule> physicModules;

        static PhysicManager()
        {
            physicModules = new List<PhysicModule>();
        }

        /// <summary>
        /// Add new module to the list of physicmodules
        /// </summary>
        /// <param name="obj"></param>
        public static void AddObject(PhysicModule obj)
        {
            physicModules.Add(obj);
        }


        /// <summary>
        /// Remove object from  the list
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveObject(PhysicModule obj)
        {
            physicModules.Remove(obj);
        }

        static public void UpdateMe()
        {
            for(int i = 0; i < physicModules.Count; i++)
            {
                for(int j=0; j < physicModules.Count; j++)
                {
                    if (i != j)
                    {
                        if (physicModules[i].GetPhysicRectangle().Intersects(physicModules[j].GetPhysicRectangle()))
                        {
                            if (physicModules[i].isPhysicActive && physicModules[j].isPhysicActive)
                            {
                                physicModules[i].Collided(physicModules[j]);
                            }
                        }
                    }
                }

            }
        }

    }
}
