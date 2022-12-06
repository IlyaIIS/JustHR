using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustHR.Classes.Basic.Animations
{
    /// <summary>
    /// Хранит кадры, их продолжительность в тиках и текущий тик (что позволяет узнать текущий кадр |_____|____|___|___v____|).
    /// </summary>
    class Animation
    {
        public Enum Name { get; }
        public AnimationType Type { get; }
        public int TickNum { get; }
        public int Tick { get; private set; }
        public bool IsOver { get { return Tick >= TickNum - 1; } }
        public event Action OnBoundReached;
        private bool wasOverReached;

        private readonly List<AnimationFrame> frames;

        public Animation(Enum name, AnimationType type, List<AnimationFrame> frames)
        {
            Name = name;
            Type = type;
            this.frames = frames;
            TickNum = frames.Sum((frame) => frame.Length);
        }

        public void DoTick()
        {
            if (Type == AnimationType.Default)
                Tick += wasOverReached ? 0 : 1;
            else if (Type == AnimationType.Cycleable)
                Tick += wasOverReached ? -1 : 1;  //(int)Math.Round(Math.Abs((Tick+TickNum/2f)%TickNum-TickNum/2f)*2); функция зигзага /\/\/\
            else
                Tick++;

            if (Tick >= TickNum || Tick < 0)
            {
                switch (Type)
                {
                    case AnimationType.Default:
                        wasOverReached = true;
                        Tick = TickNum - 1;
                        break;
                    case AnimationType.Repeatable:
                        Tick = 0;
                        break;
                    case AnimationType.Cycleable:
                        wasOverReached = !wasOverReached;
                        Tick = Tick + (wasOverReached ? -1 : 1);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                OnBoundReached.Invoke();
            }
        }

        public int GetFrame()
        {
            int ticks = 0;
            for (int i = 0; i < frames.Count; i++)
            {
                ticks += frames[i].Length;
                if (Tick < ticks)
                {
                    return frames[i].Frame;
                }
            }

            throw new Exception("Animation is over");
        }

        public void Reset()
        {
            Tick = 0;
        }
    }

    /// <summary>
    /// Представление кадра. Хранит номер кадра и продолжительность в тиках. 
    /// </summary>
    struct AnimationFrame
    {
        public AnimationFrame(int length, int frame)
        {
            Length = length;
            Frame = frame;
        }
        public int Length { get; }
        public int Frame { get; }
    }

    enum AnimationType
    {
        /// <summary>После завершения останавливается и 1 раз сигнализирует о завершении</summary>
        Default,
        /// <summary>После завершения начинает воспроизводиться назоно</summary>
        Repeatable,  
        /// <summary>После завершения начинает воспроизводиться наоборот</summary>
        Cycleable,
    }
}
