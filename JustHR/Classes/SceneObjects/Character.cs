using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using JustHR.Classes.Basic;
using JustHR.Classes.Basic.Animations;

namespace JustHR.Classes
{
    class Character : ISceneObject
    {
        public float Z { get; set; }
        public bool IsSelectable { get; } = false;
        public Vector2 Pos { get { return moveAnimator.GetPos(); } }
        private MoveAnimator<CharacterMoveState> moveAnimator;
        public float Scale { get; private set; }

        public int ClothNum { get; }

        public TextPlace TextPlace { get; }
        private readonly OfficeScene scene;

        public CharacterTraits Traits { get; }

        public Dictionary<Enum, SoundEffectInstance> SoundEffects { get; }

        private bool isAccepted;

        public bool IsSitting()
        {
            return moveAnimator.AnimationName == CharacterMoveState.Sitting && moveAnimator.CurAnimation.IsOver;
        }

        public void Accept()
        {
            moveAnimator.SetAnimation(CharacterMoveState.StandUpping);
            isAccepted = true;
        }

        public void Reject()
        {
            moveAnimator.SetAnimation(CharacterMoveState.StandUpping);
            isAccepted = false;
        }

        public Character(Vector2 pos, TextPlace textPlace, OfficeScene scene, int clothNum, CharacterTraits traits,
            Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            TextPlace = textPlace;
            ClothNum = clothNum;

            Traits = traits;

            SoundEffects = soundEffects;

            this.scene = scene; 
            foreach(string str in Traits.Speech)
                if (str.Length > 300)
                    throw new ArgumentException("Слишком длинная реплика"); //todo: вынести проверку в момент загрузки реплик

            moveAnimator = GetMoveAnimator(pos);
        }

        private MoveAnimator<CharacterMoveState> GetMoveAnimator(Vector2 pos)
        {
            var animations = new List<MoveAnimation>();
            animations.Add(new MoveAnimation(CharacterMoveState.MovingToTable, AnimationType.Default, 30, (self) =>
            {
                Scale = 0.8f + (0.2f / self.TickNum) * self.Tick;
                return new Vector2(0, - self.Tick + 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)self.Tick / self.TickNum) * 2)));
            }));
            animations[^1].OnBoundReached += () =>
            {
                Door door = scene.Objects.Door;
                door.State = DoorState.Closed;

                SoundEffects[SoundsEnum.door_closing].Play();
            };

            animations.Add(new MoveAnimation(CharacterMoveState.MovingRightAlongTable, AnimationType.Default, 60, (self) =>
            {
                return new Vector2(((float)self.Tick / self.TickNum) * 385, 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)self.Tick / self.TickNum) * 4)));
            }));


            animations.Add(new MoveAnimation(CharacterMoveState.Sitting, AnimationType.Default, 15, (self) =>
            {
                return new Vector2(0, ((float)self.Tick / self.TickNum) * 30);
            }));
            animations[^1].OnBoundReached += () =>
            {
                TextPlace.BeginSpeech(Traits.Speech);
            };

            animations.Add(new MoveAnimation(CharacterMoveState.StandUpping, AnimationType.Default, 15, (self) =>
            {
                return new Vector2(0, -((float)self.Tick / self.TickNum) * 30);
            }));


            animations.Add(new MoveAnimation(CharacterMoveState.Rejected, AnimationType.Default, 70, (self) =>
            {
                return new Vector2(-((float)self.Tick / self.TickNum) * 650, 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)self.Tick / self.TickNum) * 4)));
            }));
            animations[^1].OnBoundReached += () =>
            {
                scene.CharacterExited();
            };

            animations.Add(new MoveAnimation(CharacterMoveState.Accepted, AnimationType.Default, 70, (self) =>
            {
                return new Vector2(((float)self.Tick / self.TickNum) * 750, 10 * MathF.Abs(MathF.Sin(MathF.PI * ((float)self.Tick / self.TickNum) * 6)));
            }));
            animations[^1].OnBoundReached += () =>
            {
                if (Traits.IsBoss)
                    if (scene.Hour >= 18)
                    {
                        scene.Exit();
                    }

                scene.CharacterExited();
            };

            MoveAnimator<CharacterMoveState> moveAnimator = new MoveAnimator<CharacterMoveState>(animations, CharacterMoveState.MovingToTable, pos);

            moveAnimator.OnAnimationOver += (animator) =>
            {
                switch (animator.CurAnimation.Name)
                {
                    case CharacterMoveState.MovingToTable:
                        animator.SetAnimation(CharacterMoveState.MovingRightAlongTable);
                        break;
                    case CharacterMoveState.MovingRightAlongTable:
                        animator.SetAnimation(CharacterMoveState.Sitting);
                        break;
                    case CharacterMoveState.Sitting:
                        break;
                    case CharacterMoveState.StandUpping:
                        if (isAccepted)
                            animator.SetAnimation(CharacterMoveState.Accepted);
                        else
                            animator.SetAnimation(CharacterMoveState.Rejected);
                        break;
                    case CharacterMoveState.Rejected:
                        break;
                    case CharacterMoveState.Accepted:
                        break;
                    default:
                        break;
                }
            };

            return moveAnimator;
        }

        public void DoTick()
        {
            moveAnimator.DoTick();
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

    enum CharacterMoveState
    {
        MovingToTable,
        MovingRightAlongTable,
        Sitting,
        StandUpping,
        Rejected,
        Accepted
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
