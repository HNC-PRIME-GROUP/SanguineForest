using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanguine_Forest.Scripts.GameState
{
    public class InputManager
    {

        private KeyboardState prevKb;
        private KeyboardState currKb;

        private GamePadState prevPad;
        private GamePadState currPad;

        public InputManager()
        {
            prevKb = Keyboard.GetState();
            prevPad = GamePad.GetState(PlayerIndex.One);
        }

        // Call this method at the beginning of the update loop to refresh the state
        public void UpdateMe()
        {
            prevKb = currKb;
            currKb = Keyboard.GetState();

            prevPad = currPad;
            currPad = GamePad.GetState(PlayerIndex.One);
        }

        // Check if a specific key was just pressed
        public bool IsKeyPressed(Keys key)
        {
            return currKb.IsKeyDown(key) && prevKb.IsKeyUp(key);
        }

        // Check if a specific key is currently down
        public bool IsKeyDown(Keys key)
        {
            return currKb.IsKeyDown(key);
        }

        // Check if a specific button was just pressed on the gamepad
        public bool IsButtonPressed(Buttons button)
        {
            return currPad.IsButtonDown(button) && prevPad.IsButtonUp(button);
        }

        // Check if a specific button is currently down on the gamepad
        public bool IsButtonDown(Buttons button)
        {
            return currPad.IsButtonDown(button);
        }
    }
}