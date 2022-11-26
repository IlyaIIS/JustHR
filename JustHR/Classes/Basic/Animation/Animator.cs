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
        Animation animation;
        public T AnimationName { get; private set; }

        readonly Dictionary<T, Animation> animationsWithNames;

        public delegate void AnimationOverHandler(T animationName);
        /// <summary>Сюда навешивать методы, которые должны выполняться после конца анимации (событие триггерят только delault анимации).</summary>
        public event AnimationOverHandler OnAnimationOver;

        public Animator(List<Animation> animations, T animationName)
        {
            animationsWithNames = new Dictionary<T, Animation>();
            foreach (Animation animation in animations)
            {
                if (animation.Name.GetType().Equals(typeof(T)))
                    animationsWithNames.Add((T)animation.Name, animation);
                else
                    throw new ArithmeticException("Enum анимации должен быть тот же, что и enum аниматора.");
            }

            AnimationName = animationName;
            animation = animationsWithNames[animationName];
        }

        public void SetAnimation(T animationName)
        {
            if (!AnimationName.Equals(animationName))
            {
                if (animationsWithNames.ContainsKey(animationName))
                {
                    AnimationName = animationName;
                    animation.Tick = 0;
                    animation = animationsWithNames[AnimationName];
                } else
                {
                    throw new ArgumentException("Wrong animationName parameter");
                }
            }
        }

        public int GetFrame()
        {
            return animation.GetFrame();
        }

        public void DoTick()
        {
            bool isOver = animation.DoTick();

            if (isOver)
            {
                OnAnimationOver.Invoke((T)animation.Name);
            }
        }
    }
}
