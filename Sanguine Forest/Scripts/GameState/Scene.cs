using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment.Obstacle;
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
        public List<MovebalPlatformData> moveablPlatforms;
        public List<FallingPlatformData> fallingPlatforms;

        //Obstacles
        public List<ObstacleData> obstaclesData1;
        public List<ObstacleData> obstaclesData2;
        public List<ObstacleData> obstaclesData3;

        //Obstacles
        public List<ThornsData> thorns;
        
        //Decor
        public List<DecorGrassData> decors; //Grass

        //Cutscene mode
        public bool isCutScene;
        public List<CutSceneObjectData> cutSceneObjects;
        public List<CutSceneDialogue> cutSceneDialogues;







        public Scene()
        {

        }




    }
}
