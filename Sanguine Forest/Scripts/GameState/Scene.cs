using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Sanguine_Forest
{
    /// <summary>
    /// Data about all game systems that should be loaded befor a level started
    /// </summary>
    internal class Scene
    {

        //Character position
        public Vector2 characterPosition;

        //Game state (alcohol etc.)


        //Environments and platforms
        public List<PlatformData> simplePlatforms;
        //public List<MoveblePlatform> moveblPlatforms;
        //public List<FallingPlatform> fallingPlatforms;

        //public List<Decor> decors; // in case if we add some


        //Background parallaxing
        //public List<ParallaxBackground> bachgrounds;




        public Scene()
        {


        }




    }
}
