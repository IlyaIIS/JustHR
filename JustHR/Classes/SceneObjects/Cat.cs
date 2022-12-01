using JustHR.Classes.Basic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes.SceneObjects
{
    class Cat : ISceneObject, IClickable
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; }

        public CatPosition Position { get; }
        public int AngryWeight { get; set; }
        public bool IsAngry { get { return AngryWeight > 0; } }

        public Dictionary<Enum, SoundEffectInstance> SoundEffects;
        private int touchNum = 0;


        public Cat(Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            Random rnd = new Random();
            Position = (CatPosition)rnd.Next(3);
            AngryWeight = 0;

            SoundEffects = soundEffects;

            switch (Position)
            {
                case CatPosition.OnTable:
                    Collision = new Rectangle(50, 255, 100, 80);
                    Z = 0.9f;
                    break;
                case CatPosition.OnCooler:
                    Collision = new Rectangle(230, 170, 170, 100);
                    Z = 0.31f;
                    break;
                case CatPosition.OnChair:
                    Collision = new Rectangle(750, 440, 130, 100);
                    Z = 0.51f;
                    break;
                default:
                    break;
            }
        }

        public void Click(OfficeScene scene)
        {
            if (IsAngry)
            {
                SoundEffects[SoundsEnum.cat_hisses].Play();

                Player.Mentality = MathHelper.Clamp(Player.Mentality - 5, 0, 100);
            }
            else
            {
                Random rnd = new Random();
                if (rnd.Next(touchNum) == 0)
                {
                    SoundEffects[SoundsEnum.mur_short].Volume = Settings.GlobalVolume;
                    SoundEffects[SoundsEnum.mur_short].Stop();
                    SoundEffects[SoundsEnum.mur_short].Play();
                    Player.Mentality = MathHelper.Clamp(Player.Mentality + 5, 0, 100);
                    touchNum++;
                }
                else
                {
                    SoundEffects[SoundsEnum.cat_hisses].Stop();
                    SoundEffects[SoundsEnum.cat_hisses].Play();
                    touchNum = 0;
                    AngryWeight = 3;
                    Player.Mentality = MathHelper.Clamp(Player.Mentality - 5, 0, 100);
                }
            }
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