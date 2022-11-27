using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using JustHR.Classes.Basic;

namespace JustHR.Classes
{
    class Character : ISceneObject
    {
        private Vector2 originPos;
        public Vector2 Pos { get; private set; }
        public float scale { get; private set; }

        public int ClothNum { get; }

        public TextPlace TextPlace { get; }
        private CharacterStateEnum state;
        public CharacterStateEnum State { get { return state; } set { state = value; tick = 0; } }
        private int tick;
        private OfficeScene scene;

        public bool IsBoss { get; }

        public string FirstName { get; }
        public string SecondName { get; }
        public string Patronumic { get; }

        public string Birthday { get; }

        public int Eyes { get; }
        public int Hairs { get; }
        public int Accessory { get; }

        public ProfessionEnum Profession { get; }
        public GradeEnum Grade { get; }

        public int Professionality { get; }
        public int SocialIntelligence { get; }

        public int HairMoraleImpact { get; }
        public int RejectMaraleImpact { get; }

        public int RequiredSalary { get; }

        public List<string> Speech { get; }

        public Dictionary<Enum, SoundEffectInstance> SoundEffects { get; }


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
        public bool IsSitting()
        {
            return state == CharacterStateEnum.Coming && IsAnimationEnded();
        }

        const int FIRST_LENGTH = 30;
        const int SECOND_LENGTH = 60;
        const int THIRD_LENGTH = 15;

        const int REJECTED_LENGTH = 70;
        const int ACCEPTED_LENGTH = 70;

        public Character(Vector2 pos, TextPlace textPlace, OfficeScene scene, int clothNum, CharacterTraits traits,
            Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            Pos = pos;
            originPos = pos;
            TextPlace = textPlace;
            ClothNum = clothNum;

            IsBoss = traits.IsBoss;
            FirstName = traits.FirstName;
            SecondName = traits.SecondName;
            Patronumic = traits.Patronumic;
            Birthday = traits.Birthday;
            Eyes = traits.Eyes;
            Hairs = traits.Hairs;
            Accessory = traits.Accessory;
            Profession = traits.Profession;
            Grade = traits.Grade;
            Professionality = traits.Professionality;
            SocialIntelligence = traits.SocialIntelligence;
            HairMoraleImpact = traits.HairMoraleImpact;
            RejectMaraleImpact = traits.RejectMaraleImpact;
            RequiredSalary = traits.RequiredSalary;
            Speech = traits.Speech;
            SoundEffects = soundEffects;

            this.scene = scene; 
            foreach(string str in Speech)
                if (str.Length > 300)
                    throw new ArgumentException("Слишком длинная реплика");
            Speech = Speech;
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
                    Door door = scene.GetObject<Door>();
                    door.State = DoorState.Closed;

                    SoundEffects[SoundsEnum.door_closing].Play();
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
                    originPos = Pos;
                    TextPlace.BeginSpeech(Speech);
                }
            } else if (State == CharacterStateEnum.Rejected)
            {
                if(tick < THIRD_LENGTH)
                {
                    Pos = new Vector2(originPos.X, originPos.Y - ((float)tick / THIRD_LENGTH) * 30);
                }
                else if (tick == THIRD_LENGTH)
                {
                    originPos = Pos;
                }
                else if(tick  <= REJECTED_LENGTH + THIRD_LENGTH)
                {
                    int curTick = tick - THIRD_LENGTH;
                    Pos = new Vector2(originPos.X - ((float)curTick / REJECTED_LENGTH) * 650, originPos.Y + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)curTick / REJECTED_LENGTH) * 4)));
                }
                else
                {
                    scene.CharacterExited();
                }
            }else if (State == CharacterStateEnum.Accepted) //Босс всегда уходит сюда
            {
                if (tick < THIRD_LENGTH)
                {
                    Pos = new Vector2(originPos.X, originPos.Y - ((float)tick / THIRD_LENGTH) * 30);
                }
                else if (tick == THIRD_LENGTH)
                {
                    originPos = Pos;
                }
                else if (tick <= ACCEPTED_LENGTH + THIRD_LENGTH)
                {
                    int curTick = (tick - THIRD_LENGTH);
                    Pos = new Vector2(originPos.X + ((float)curTick / (ACCEPTED_LENGTH + THIRD_LENGTH)) * 750, originPos.Y + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)curTick / (ACCEPTED_LENGTH + THIRD_LENGTH)) * 6)));
                } else
                {
                    if (IsBoss)
                        if (scene.Hour >= 18)
                        {
                            scene.Exit();
                        }

                    scene.CharacterExited();
                }
            }
        }
    }

    struct CharacterTraits
    {
        public bool IsBoss { get; }
        public string FirstName { get; }
        public string SecondName { get; }
        public string Patronumic { get; }

        public string Birthday { get; }

        public int Eyes { get; }
        public int Hairs { get; }
        public int Accessory { get; }

        public ProfessionEnum Profession { get; }
        public GradeEnum Grade { get; }

        public int Professionality { get; }
        public int SocialIntelligence { get; }

        public int HairMoraleImpact { get; }
        public int RejectMaraleImpact { get; }

        public int RequiredSalary { get; }

        public List<string> Speech { get; }

        public CharacterTraits(bool isBoss, string firstName, string secondName, string patronumic, string birthday,
            int eyes, int hairs, int accessory,
            ProfessionEnum profession, GradeEnum grade, int professionality,
            int socialIntelligence, int hairMoraleImpact, int rejectMaraleImpact, int requiredSalary,
            List<string> speech)
        {
            IsBoss = isBoss;
            FirstName = firstName;
            SecondName = secondName;
            Patronumic = patronumic;
            Birthday = birthday;
            Eyes = eyes;
            Hairs = hairs;
            Accessory = accessory;
            Profession = profession;
            Grade = grade;
            Professionality = professionality;
            SocialIntelligence = socialIntelligence;
            HairMoraleImpact = hairMoraleImpact;
            RejectMaraleImpact = rejectMaraleImpact;
            RequiredSalary = requiredSalary;
            Speech = speech;
        }
    }

    enum GradeEnum
    {
        Junior,
        Middle,
        Senior
    }
    enum ProfessionEnum
    {
        Developer,
        Designer,
        Analyst,
        Manager,
        Marketer,
        Accountant,
        Sysadmin,
        Cleaner,
    }

    enum ClothesEnum
    {
        Clothes,
        SittingClothes,
        Hairs,
        Eyes,
        Accessories,
    }

    enum CharacterStateEnum
    {
        Coming,
        Rejected,
        Accepted
    }
}
