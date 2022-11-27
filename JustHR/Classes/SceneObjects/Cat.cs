using JustHR.Classes.Basic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes.SceneObjects
{
    class Cat : ISceneObject
    {
        public CatPosition Position { get; }
        public int AngryWeight { get; set; }
        public bool IsAngry { get { return AngryWeight > 0; } }
        public Dictionary<Enum, SoundEffectInstance> SoundEffects;
        private int touchNum = 0;


        public Cat(Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            Random rnd = new Random();
            Position = (CatPosition)rnd.Next(3);
            AngryWeight = 0;

            SoundEffects = soundEffects;

            Point pos;
            Point size;
            switch (Position)
            {
                case CatPosition.OnTable:
                    pos = new Point(50, 255);
                    size = new Point(100, 80);
                    break;
                case CatPosition.OnCooler:
                    pos = new Point(230, 170);
                    size = new Point(170, 100);
                    break;
                case CatPosition.OnChair:
                    pos = new Point(750, 440);
                    size = new Point(130, 100);
                    break;
                default:
                    pos = Point.Zero;
                    size = Point.Zero;
                    break;
            }

            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                if (key == MouseButton.LeftButton)
                {
                    if (x > pos.X && y > pos.Y && x < pos.X + size.X && y < pos.Y + size.Y)
                    {
                        if (IsAngry)
                        {
                            SoundEffects[SoundsEnum.cat_hisses].Play();

                            Settings.Mentality = MathHelper.Clamp(Settings.Mentality - 5, 0, 100);
                        }
                        else
                        {
                            Random rnd = new Random();
                            if (rnd.Next(touchNum) == 0)
                            {
                                SoundEffects[SoundsEnum.mur_short].Volume = Settings.GlobalVolume;
                                SoundEffects[SoundsEnum.mur_short].Stop();
                                SoundEffects[SoundsEnum.mur_short].Play();
                                Settings.Mentality = MathHelper.Clamp(Settings.Mentality + 5, 0, 100);
                                touchNum++;
                            }
                            else
                            {
                                SoundEffects[SoundsEnum.cat_hisses].Stop();
                                SoundEffects[SoundsEnum.cat_hisses].Play();
                                touchNum = 0;
                                AngryWeight = 3;
                                Settings.Mentality = MathHelper.Clamp(Settings.Mentality - 5, 0, 100);
                            }
                        }
                    }
                }
            };
        }
    }

    enum CatState
    {
        Chair,
        Cooler,
        Phone,
        AngryChair,
        AngryCooler,
        AngryPhone
    }

    enum CatPosition
    {
        OnChair,
        OnTable,
        OnCooler
    }
}