using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sanguine_Forest;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>
    /// Store data about all obstacle on the level, load them from scene data and 
    /// </summary>
    internal class ObstacleManager
    {
        private List<Obstacle> obstacles;

        public ObstacleManager()
        {
            obstacles = new List<Obstacle>();
        }

        public void AddObstacle(Obstacle obstacle)
        {
            obstacles.Add(obstacle);
        }

        public void Draw(SpriteBatch spriteBatch, int currentAlcoholLevel)
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Draw(spriteBatch, currentAlcoholLevel);
            }
        }
    }
}