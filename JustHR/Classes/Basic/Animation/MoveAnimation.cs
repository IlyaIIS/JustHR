using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustHR.Classes.Basic.Animations
{
    /// <summary>
    /// Хранит текущий тик и может по нему рассчитать смещение координаты
    /// </summary>
    class MoveAnimation
    {
        public Enum Name { get; }
        public AnimationType Type { get; }
        public int TickNum { get; }
        public int Tick { get; set; }
        public bool IsOver { get { return Tick >= TickNum - 1; } }
        private bool wasOverReached = false;

        public delegate Vector2 MoveDelegate(MoveAnimation self);
        private readonly MoveDelegate move;
        public event Action OnBoundReached;

        public MoveAnimation(Enum name, AnimationType type, int tickNum, MoveDelegate move)
        {
            Name = name;
            Type = type;
            TickNum = tickNum;
            this.move = move;
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

        public Vector2 GetPos()
        {
            return move(this);
        }
    }
}

