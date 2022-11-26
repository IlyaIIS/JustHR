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
        private CharacterStateEnum state;
        public CharacterStateEnum State { get { return state; } set { state = value; tick = 0; } }
        private int tick;
        private OfficeScene scene;

        public bool IsAnimationEnded()
        {
            if (State == CharacterStateEnum.Coming)
                return tick > FIRST_LENGTH + SECOND_LENGTH + THIRD_LENGTH + 1;
            else if (State == CharacterStateEnum.Rejected)
                return tick > REJECTED_LENGTH;
            else if (State == CharacterStateEnum.Accepted)
                return tick > ACCEPTED_LENGTH;
            else
                throw new NotImplementedException("Незаданный тип");
        }

        const int FIRST_LENGTH = 30;
        const int SECOND_LENGTH = 60;
        const int THIRD_LENGTH = 15;

        const int REJECTED_LENGTH = 70;
        const int ACCEPTED_LENGTH = 70;

        public Character(Vector2 pos, TextPlace textPlace, HairEnum hairs, AccessoryEnum accessory, ClothesEnum clothes, OfficeScene scene, List<string> speech = null)
        {
            Pos = pos;
            originPos = pos;
            TextPlace = textPlace;
            Hairs = hairs;
            Accessory = accessory;
            Clothes = clothes;
            this.scene = scene; 
            foreach(string str in speech)
                if (str.Length > 300)
                    throw new ArgumentException("Слишком длинная реплика");
            Speech = speech;
        }
        //волосы, ?уши, аксесуар, одежда
        public void DoTick()
        {
            tick++;

            if (State == CharacterStateEnum.Coming)
            {
                if (tick < FIRST_LENGTH)
                {
                    scale = 0.8f + (0.2f / FIRST_LENGTH) * tick;
                    Pos = new Vector2(originPos.X, originPos.Y - tick + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)tick / FIRST_LENGTH) * 2)));
                }
                else if (tick == FIRST_LENGTH)
                {
                    originPos = Pos;
                }
                else if (tick < FIRST_LENGTH + SECOND_LENGTH)
                {
                    int curTick = tick - FIRST_LENGTH;
                    Pos = new Vector2(originPos.X + ((float)curTick / SECOND_LENGTH) * 385, originPos.Y + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)curTick / SECOND_LENGTH) * 4)));
                }
                else if (tick == FIRST_LENGTH + SECOND_LENGTH)
                {
                    originPos = Pos;
                }
                else if (tick <= FIRST_LENGTH + SECOND_LENGTH + THIRD_LENGTH)
                {
                    int curTick = tick - FIRST_LENGTH - SECOND_LENGTH;
                    Pos = new Vector2(originPos.X, originPos.Y + ((float)curTick / THIRD_LENGTH) * 30);
                }
                else if (tick == FIRST_LENGTH + SECOND_LENGTH + THIRD_LENGTH + 1)
                {
                    TextPlace.BeginSpech(Speech);
                }
            } else if (State == CharacterStateEnum.Rejected)
            {
                if(tick  <= REJECTED_LENGTH)
                {
                    Pos = new Vector2(originPos.X - ((float)tick / REJECTED_LENGTH) * 650, originPos.Y + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)tick / REJECTED_LENGTH) * 4)));
                }
                else
                {
                    scene.GenerateNewCharacter();
                }
            }else if (State == CharacterStateEnum.Accepted)
            {
                if(tick <= ACCEPTED_LENGTH)
                {
                    Pos = new Vector2(originPos.X + ((float)tick / ACCEPTED_LENGTH) * 650, originPos.Y + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)tick / ACCEPTED_LENGTH) * 6)));
                } else
                {
                    scene.GenerateNewCharacter();
                }
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

    enum CharacterStateEnum
    {
        Coming,
        Rejected,
        Accepted
    }
}
