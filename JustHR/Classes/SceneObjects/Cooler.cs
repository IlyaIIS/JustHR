using JustHR.Classes.Basic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes.SceneObjects
{
    class Cooler : ISceneObject
    {
        public int CofeeLvl { get; set; } = 100;
        private float coffeEffectiveness = 1;
        public float CoffeEffectiveness { get { return coffeEffectiveness; } set { coffeEffectiveness = MathHelper.Clamp(value, 0, 1); } }

        public Cooler(Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                if (CofeeLvl > 0)
                    if (key == MouseButton.LeftButton)
                        if (x > 250 && y > 216 && x < 395 && y < 380)
                        {
                            soundEffects[SoundsEnum.drink_coffee].Stop();
                            soundEffects[SoundsEnum.drink_coffee].Play();

                            CofeeLvl -= 10;
                            Player.Mentality += (int)Math.Round(5 * CoffeEffectiveness);
                            CoffeEffectiveness -= 0.3f;
                        }
            };
        }
    }
}