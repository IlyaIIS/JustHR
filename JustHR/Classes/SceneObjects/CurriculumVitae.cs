﻿using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes.SceneObjects
{
    class CurriculumVitae : ISceneObject
    {
        public bool IsExpanded { get; set; }

        public CurriculumVitae(OfficeScene scene, Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                foreach (ISceneObject objct in scene.Objects)
                {
                    if (objct.GetType() == typeof(Character))
                    {
                        Character character = (Character)objct;
                        if (character.IsSitting())
                        {
                            if (key == MouseButton.LeftButton)
                            {
                                if (!IsExpanded)
                                {
                                    if (x > 400 && y > 440 && x < 640 && y < 530)
                                    {
                                        soundEffects[SoundsEnum.vc_opening].Play();
                                        IsExpanded = true;
                                    }
                                }
                                else
                                {
                                    if (x < 200 || y < 40 || x > 850 || y > 540)
                                    {
                                        soundEffects[SoundsEnum.vc_closing].Play();
                                        IsExpanded = false;
                                    }
                                }
                            }
                        }
                    }
                }
                
            };
        }
    }
}
