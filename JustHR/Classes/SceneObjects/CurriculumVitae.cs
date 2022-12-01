using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace JustHR.Classes.SceneObjects
{
    class CurriculumVitae : ISceneObject, IClickable
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; } = new Rectangle(400, 440, 240, 90);
        public bool IsSelectable { get; } = true;
        public bool IsExpanded { get; set; }
        private Dictionary<Enum, SoundEffectInstance> soundEffects;

        public CurriculumVitae(OfficeScene scene, Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            this.soundEffects = soundEffects;

            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                if (key == MouseButton.LeftButton)
                    if (scene.Objects.Character.IsSitting())
                        if (IsExpanded)
                        {
                            if (x < 200 || y < 40 || x > 850 || y > 540)
                            {
                                Collision = new Rectangle(400, 440, 240, 90);
                                Z = 0.8f;
                                soundEffects[SoundsEnum.vc_closing].Play();
                                IsExpanded = false;
                            }
                        }
            };
        }

        public void Click(OfficeScene scene)
        {
            if (scene.Objects.Character.IsSitting())
                if (!IsExpanded)
                {
                    Collision = new Rectangle(0, 0, 0, 0);
                    Z = 0.99f;
                    scene.SelectedObject = null;
                    soundEffects[SoundsEnum.vc_opening].Play();
                    IsExpanded = true;
                }
        }
    }
}
