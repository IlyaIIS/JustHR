using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace JustHR.Classes.SceneObjects
{
    class Clock : ISceneObject
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; }
        public Clock(OfficeScene scene, Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            controller.OnMouseButtonReleased += (key, x, y) =>
           {
               if (key == MouseButton.LeftButton)
                   if (!scene.Objects.Character.Traits.IsBoss)
                       if (x > 263 && y > 26 && x < 362 && y < 130)
                       {
                           soundEffects[SoundsEnum.tick_1].Stop();
                           soundEffects[SoundsEnum.tick_1].Play();

                           scene.NextHour();
                       }
           };
        }
    }
}
