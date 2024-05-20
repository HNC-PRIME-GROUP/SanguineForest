
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

//namespace Sanguine_Forest
//{
//    internal class ListenerModule : Module
//    {
//        private const float MaxDistance = 500f;

//        public ListenerModule(GameObject parent, Vector2 shift) : base(parent, shift) { }

//        public void UpdateSounds(List<SoundEffectInstance> sounds, float generalVolume)
//        {
//            Vector2 listenerPosition = GetPosition();

//            foreach (var sound in sounds)
//            {
//                if (sound.State == SoundState.Playing)
//                {
//                    // Assuming each sound instance has an associated position, passed along with the instance
//                    // Just a placeholder call for actual position update logic
//                    UpdateSoundPosition(sound, listenerPosition, generalVolume);
//                }
//            }
//        }

//        public void UpdateSoundPosition(SoundEffectInstance sound, Vector2 soundPosition, float generalVolume)
//        {
//            Vector2 listenerPosition = GetPosition();
//            float distance = Vector2.Distance(listenerPosition, soundPosition);
//            float volume = Math.Clamp(generalVolume * (1 - (distance / MaxDistance)), 0, generalVolume);
//            float pan = Math.Clamp((soundPosition.X - listenerPosition.X) / MaxDistance, -1, 1);

//            sound.Volume = volume;
//            sound.Pan = pan;
//        }
//    }
//}
