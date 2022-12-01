using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace JustHR.Classes.SceneObjects
{
    class Clock : ISceneObject, IClickable
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; } = new Rectangle(260, 25, 100, 105);
        private readonly Dictionary<Enum, SoundEffectInstance> soundEffects;
        public Clock(Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            this.soundEffects = soundEffects;
        }

        public void Click(OfficeScene scene)
        {
            if (!scene.Objects.Character.Traits.IsBoss)
            {
                soundEffects[SoundsEnum.tick_1].Stop();
                soundEffects[SoundsEnum.tick_1].Play();

                scene.NextHour();
            }
        }
    }
}
