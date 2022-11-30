using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Basic.Animations
{
    /// <summary>
    /// Класс, управляющий анимациями. Умеет переключайть анимации напрямую или через событие конца анимации. 
    /// </summary>
    /// <typeparam name="T">Enum, содержащий названия анимаций. Все анимации аниматора должны иметь имена из этого Enum'а.</typeparam>
    class MoveAnimator<T> where T : Enum
    {
        public MoveAnimation CurAnimation { get; private set; }
        public T AnimationName { get { return (T)CurAnimation.Name; } }
        Vector2 curPos;

        readonly Dictionary<T, MoveAnimation> animationsWithNames;

        public delegate void AnimationOverHandler(MoveAnimator<T> animator);
        /// <summary>Сюда навешивать методы, которые должны выполняться после конца анимации.</summary>
        public event AnimationOverHandler OnAnimationOver;

        public MoveAnimator(List<MoveAnimation> animations, T startAnimationName, Vector2 startPos)
        {
            animationsWithNames = new Dictionary<T, MoveAnimation>();
            foreach (MoveAnimation animation in animations)
            {
                if (animation.Name.GetType().Equals(typeof(T)))
                    animationsWithNames.Add((T)animation.Name, animation);
                else
                    throw new ArithmeticException("Enum анимации должен быть тот же, что и enum аниматора.");

                animation.OnBoundReached += () => OnAnimationOver?.Invoke(this);
            }

            CurAnimation = animationsWithNames[startAnimationName];

            curPos = startPos;
        }

        public void SetAnimation(T animationName)
        {
            curPos += CurAnimation.GetPos();

            if (!CurAnimation.Name.Equals(animationName))
            {
                if (animationsWithNames.ContainsKey(animationName))
                {
                    CurAnimation = animationsWithNames[animationName];
                }
                else
                {
                    throw new ArgumentException("Wrong animationName parameter");
                }
            }

            CurAnimation.Tick = 0;
        }

        public Vector2 GetPos()
        {
            return curPos + CurAnimation.GetPos();
        }

        public void DoTick()
        {
            CurAnimation.DoTick();
        }
    }
}
