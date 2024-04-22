using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Store data about environment on this scene and load them when needed from scene data
    /// </summary>
    internal class EnvironmentManager
    {

        //Content manaher
        private ContentManager content;

        public List<Platform> platforms;
        //private List<MoveblePlatform> movebles;
        //private List<FallingPlatform> falling;
        //private List<Decor> decors;


        //link to content manager
        public EnvironmentManager(ContentManager content) 
        { 
        this.content = content;
        }


        //Creating of all platforms and other environment from the scene
        public void Initialise(Scene scene)
        {
            platforms = new List<Platform>();
            for(int i =0; i<scene.simplePlatforms.Count; i++) 
            {
                platforms.Add(new Platform(scene.simplePlatforms[i].position, scene.simplePlatforms[i].rotation, scene.simplePlatforms[i].platformSize, content));

            }
            

            //movebles = scene.moveblPlatforms;
            //falling = scene.fallingPlatforms;   
            //decors = scene.decors;
        }


        public void UpdateMe()
        {
            //Update for all platforms
            for (int i = 0; i < platforms.Count; i++){platforms[i].UpdateMe();}
            //for (int i = 0; i < movebles.Count; i++) { movebles[i].UpdateMe(); }
            //for (int i = 0; i < falling.Count; i++) falling[i].UpdateMe();

            ////update for decors
            //for(int i =0; i < decors.Count; i++) { decors[i].UpdateMe(); }
        
        }

        public void DrawMe(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < platforms.Count; i++) { platforms[i].DrawMe(spriteBatch); }
            //for (int i = 0;i < movebles.Count; i++) { movebles[i].DrawMe(spriteBatch); }
            //for(int i = 0;i<falling.Count; i++) { falling[i].DrawMe(spriteBatch); }

            //for(int i =0; i < decors.Count; i++) { decors[i].DrawMe(spriteBatch); }

        }

    }
}
