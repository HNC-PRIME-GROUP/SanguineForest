using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sanguine_Forest
{
    /// <summary>
    /// Data about player progress (current level, alcohol using etc.)
    /// </summary>
    public class PlayerState
    {
        public int RiskLevel { get; set; } = 0;
        public bool IsAlive { get; set; } = true;

        //current level
        public int lvlCounter;

        //current save point
        public Vector2 savePoint;

        public void SavePos(object sender, SaveCharacterDataArgs e)
        {
            savePoint = e.savePoint;
        }
    }
}
