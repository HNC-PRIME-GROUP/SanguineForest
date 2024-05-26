using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Sanguine_Forest
{
    /// <summary>    
    /// Create a visual effect of particles. 
    /// Can be added as a parameter in any module and attached to game object
    /// </summary>
    internal class ParticleSystem : GameObject
    {

        //emission

        //private Particle _particle;
        private List<Particle> _particles;
        public Vector2 Direction;
        private bool isPlaying;

        public float timeBetweenEmission;
        private float timer;

        private Random rng;
        private float emissionCone;

        //Particle setting
        private Texture2D partTex;
        private float opacitySpeed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">Start position for particles</param>
        /// <param name="rot">Rotation of emission (direction of particle flying)</param>
        /// <param name="tex"></param>
        /// <param name="speed">Particles speed</param>
        /// <param name="opacitySpeed">Particles shading speed</param>
        /// <param name="emissionTime">Pause between emission</param>
        /// /// <param name="emissionCone">Random direction</param>
        public ParticleSystem(Vector2 pos, float rot, Texture2D tex, float speed, float opacitySpeed, float emissionTime, float emissionCone) : base(pos, rot)
        {
            partTex = tex;
            //emission
            timeBetweenEmission = emissionTime;
            timer = timeBetweenEmission;
            this.emissionCone = emissionCone;
            rng = new Random();
            //system control
            isPlaying = false;
            _particles = new List<Particle>();
            for (int i = 0; i < 10; i++)
            {
                _particles.Add(new Particle(partTex, position, speed, GetRotation(), opacitySpeed));
            }
        }

        public void UpdateMe(Vector2 pos, float rot)
        {
            //Update positions
            position = pos;
            SetRotation(rot);
            //Update direction of particles' flying
            Direction = new Vector2((float)Math.Sin(GetRotation()), (float)Math.Cos(GetRotation()));
            Direction.Normalize();

            //Update for all flying particles
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].GetState())
                    _particles[i].UpdateMe(position);

            }

            // update id it is turned on
            if (isPlaying)
            {
                if (timer <= 0) //timer of emission
                {
                    for (int i = 0; i < _particles.Count; i++)
                    {
                        //check if we have particles which not in fly
                        if (!_particles[i].GetState())
                        {
                            //Update direction of particles' flying
                            float rotateShift = (float)(rng.NextDouble() - 0.5f) * emissionCone * 2;
                            Direction = new Vector2((float)Math.Sin(GetRotation() + rotateShift), (float)Math.Cos(GetRotation() + rotateShift));
                            Direction.Normalize();
                            //if it is not in a flying - we trigger it to start
                            _particles[i].TriggerMe(position, Direction); //trigger back particle to current system pos
                            timer = timeBetweenEmission;
                            break;
                        }
                    }
                }
                else
                {
                    timer -= 0.1f;
                }
            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            //drawing for all particles
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].DrawMe(sp);
            }
            //debug strings
            //DebugManager.DebugString("isPlaying?: " + isPlaying, new Vector2(0, 0));
            //DebugManager.DebugString("Timer to next particle: "+timer, new Vector2(0, 22));
            //DebugManager.DebugString("direction: "+ Direction, new Vector2(0, 44));
        }

        //trigger to start an emission
        public void Play()
        {
            isPlaying = true;
        }
        //trigger to stop an emission
        public void Stop()
        {
            isPlaying = false;
        }

    }
}
