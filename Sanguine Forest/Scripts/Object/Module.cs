using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Sanguine_Forest
{
    /// <summary>
    /// always follow for parent gameobject
    /// </summary>
    internal class Module : GameObject
    {


        // Module follow after this object
        protected GameObject _parent;
        protected Vector2 shiftPosition;
        protected float shiftRotation;
        private float distance; 


        /// <summary>
        /// Module is always follow for their parent object
        /// </summary>
        /// <param name="position">Start position</param>
        /// <param name="rotation">Start rotation</param>
        /// <param name="shift">Shift to parent</param>
        public Module(GameObject parent, Vector2 shift) : base (parent.GetPosition(), parent.GetRotation())
        {
            _parent = parent;
            shiftPosition = shift;
            shiftRotation = (float)Math.Atan2(shift.Y, shift.X);
            distance = (float)Math.Sqrt(Math.Pow(shift.X,2)+Math.Pow(shift.Y,2)); // calculate the distance between module and pivot point
        }
        
        /// <summary>
        /// Module follow for the parent
        /// </summary>
        public override void UpdateMe()
        {
            base.UpdateMe();
            SetRotation(_parent.GetRotation() + shiftRotation);
            shiftPosition = new Vector2((float)Math.Cos(GetRotation()), (float)Math.Sin(GetRotation())) * Math.Clamp(distance, 1, float.MaxValue);
            SetPosition(_parent.GetPosition() + shiftPosition);
        }

        public new GameObject GetParent()
        {
            return _parent;
        }





    }
}
