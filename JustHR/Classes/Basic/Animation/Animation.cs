using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaderPack.Classes
{
    /// <summary>
    /// Хранит кадры, их продолжительность в тиках и текущий тик (что позволяет узнать текущий кадр |_____|____|___|___v____|).
    /// </summary>
    class Animation
    {
        public Enum Name { get; }
        public AnimationType Type { get; }
        public int TickNum { get; }
        public int Tick { get; set; }
        public bool IsOver { get { return Tick >= TickNum; } }
        
        private readonly List<AnimationFrame> frames;

        public Animation(Enum name, AnimationType type, List<AnimationFrame> frames)
        {
            Name = name;
            Type = type;
            this.frames = frames;
            TickNum = frames.Sum((frame) => frame.Length);
        }

        public bool DoTick()
        {
            Tick++;

            if (Tick >= TickNum)
            {
                switch (Type)
                {
                    case AnimationType.Default:
                        Tick = TickNum - 1;
                        return true;
                    case AnimationType.Repeatable:
                        Tick = Tick % TickNum;
                        return false;
                    case AnimationType.EndStopable:
                        Tick = Tick - 1;
                        return false;
                    default:
                        throw new NotImplementedException();
                }
            }
            
            return false;
        }

        public void DoRepetableTick()
        {
            Tick = (Tick + 1) % TickNum;
        }
        public void DoBoundedTick()
        {
            Tick = Math.Max(Tick + 1, TickNum);
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
        Default,     //после завершения останавливается и сигнализирует о завершении
        Repeatable,  //после завершения начинает воспроизводиться назоно
        EndStopable, //после завершения останавливатеся, но не сигнализирует о завершении
    }
}
