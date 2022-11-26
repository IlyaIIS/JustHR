using JustHR.Classes.Interface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustHR.Classes
{
    class Character : ISceneObject
    {
        private Vector2 originPos;
        public Vector2 Pos { get; private set; }
        public float scale { get; private set; }
        public HairEnum Hairs { get; }
        public AccessoryEnum Accessory { get; }
        public ClothesEnum Clothes { get; }
        public List<string> Speech { get; }
        public TextPlace TextPlace { get; }
        private int tick;

        public Character(Vector2 pos, TextPlace textPlace, HairEnum hairs, AccessoryEnum accessory, ClothesEnum clothes, List<string> speech = null)
        {
            Pos = pos;
            originPos = pos;
            TextPlace = textPlace;
            Hairs = hairs;
            Accessory = accessory;
            Clothes = clothes;
            if (speech.Sum((str) => str.Length) > 300)
                throw new ArgumentException("Слишком длинная реплика");
            Speech = speech;
        }
        //волосы, ?уши, аксесуар, одежда
        public void DoTick()
        {
            tick++;

            const int FIRST_LENGTH = 30;
            const int SECOND_LENGTH = 60;
            const int THIRD_LENGTH = 15;

            if (tick < FIRST_LENGTH)
            {
                scale = 0.8f + (0.2f / FIRST_LENGTH) * tick;
                Pos = new Vector2(originPos.X, originPos.Y - tick + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)tick / FIRST_LENGTH) * 2)));
            }else if (tick == FIRST_LENGTH)
            {
                originPos = Pos;
            }
            else if (tick < FIRST_LENGTH + SECOND_LENGTH)
            {
                int curTick = tick - FIRST_LENGTH;
                Pos = new Vector2(originPos.X + ((float)curTick/SECOND_LENGTH)*385, originPos.Y + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)curTick / SECOND_LENGTH) * 4)));
            } else if (tick == FIRST_LENGTH + SECOND_LENGTH)
            {
                originPos = Pos;
            }
            else if (tick <= FIRST_LENGTH + SECOND_LENGTH + THIRD_LENGTH)
            {
                int curTick = tick - FIRST_LENGTH - SECOND_LENGTH;
                Pos = new Vector2(originPos.X, originPos.Y + ((float)curTick / THIRD_LENGTH)*30);
            } else if (tick == FIRST_LENGTH + SECOND_LENGTH + THIRD_LENGTH + 1)
            {
                TextPlace.BeginSpech(Speech);
            }
        }
    }

    enum HairEnum
    {
        Type1,
    }

    enum AccessoryEnum
    {
        Type1,
    }

    enum ClothesEnum
    {
        Type1,
    }
}
