using JustHR.Classes.Basic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes.SceneObjects
{
    class Cooler : ISceneObject, IClickable
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; } = new Rectangle(250, 215, 145, 175);
        public int CofeeLvl { get; set; } = 100;
        private float coffeEffectiveness = 1;
        public float CoffeEffectiveness { get { return coffeEffectiveness; } set { coffeEffectiveness = MathHelper.Clamp(value, 0, 1); } }
        private Dictionary<Enum, SoundEffectInstance> soundEffects;

        public Cooler(Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            this.soundEffects = soundEffects;
        }

        public void Click(OfficeScene scene)
        {
            if (CofeeLvl > 0)
            {
                soundEffects[SoundsEnum.drink_coffee].Stop();
                soundEffects[SoundsEnum.drink_coffee].Play();

                CofeeLvl -= 10;
                Player.Mentality += (int)Math.Round(5 * CoffeEffectiveness);
                CoffeEffectiveness -= 0.3f;
            }
        }
    }
}