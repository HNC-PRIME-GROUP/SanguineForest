using Extention;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest.Scripts.Environment;
using Sanguine_Forest.Scripts.Environment.Obstacle;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static Sanguine_Forest.Decor;
using static Sanguine_Forest.ParallaxManager;

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
        public List<PatchDecorData> patchDecors; //Patch


        //Cutscene mode
        public bool isCutScene;
        public List<CutSceneObjectData> cutSceneObjects;
        public List<CutSceneDialogue> cutSceneDialogues;


        //Level end
        public LevelTriggerData levelTrigger;

        //parallax data
        public Dictionary<Extentions.SpriteLayer, LayerPositionData> ParallaxLayerPositions { get; set; }

        //Camera data (bounds)
        public Vector2 LeftUpperBound;
        public Vector2 RightBottomBound;

        //Checkpoints
        public List<CheckPointData> checkPoints;

        public List<LevelDialogueData> LevelDialogueData { get; set; } // Add LevelDialogueData field


        public Scene()
        {
            ParallaxLayerPositions = new Dictionary<Extentions.SpriteLayer, LayerPositionData>();
        }




    }
}
