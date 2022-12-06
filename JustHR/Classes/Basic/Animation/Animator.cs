using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Basic.Animations
{
    /// <summary>
    /// Класс, управляющий анимациями. Умеет переключайть анимации напрямую или через событие конца анимации. 
    /// </summary>
    /// <typeparam name="T">Enum, содержащий названия анимаций. Все анимации аниматора должны иметь имена из этого Enum'а.</typeparam>
    class Animator<T> where T : Enum
    {
        public Animation CurAnimation { get; private set; }
        public T AnimationName { get { return (T)CurAnimation.Name; } }

        readonly Dictionary<T, Animation> animationsWithNames;

        public delegate void AnimationOverHandler(T animationName);
        /// <summary>Сюда навешивать методы, которые должны выполняться после конца анимации.</summary>
        public event AnimationOverHandler OnAnimationOver;

        public Animator(List<Animation> animations, T startAnimationName)
        {
            animationsWithNames = new Dictionary<T, Animation>();
            foreach (Animation animation in animations)
            {
                if (animation.Name.GetType().Equals(typeof(T)))
                    animationsWithNames.Add((T)animation.Name, animation);
                else
                    throw new ArithmeticException("Enum анимации должен быть тот же, что и enum аниматора.");

                animation.OnBoundReached += () => OnAnimationOver?.Invoke((T)animation.Name);
            }

            CurAnimation = animationsWithNames[startAnimationName];
        }

        public void SetAnimation(T animationName)
        {
            if (!AnimationName.Equals(animationName))
            {
                if (animationsWithNames.ContainsKey(animationName))
                {
                    CurAnimation = animationsWithNames[animationName];
                } else
                {
                    throw new ArgumentException("Wrong animationName parameter");
                }
            }

            CurAnimation.Reset();
        }

        public int GetFrame()
        {
            return CurAnimation.GetFrame();
        }

        public void DoTick()
        {
            CurAnimation.DoTick();
        }
    }
}
