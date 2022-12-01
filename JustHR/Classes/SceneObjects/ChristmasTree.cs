using JustHR.Classes.Basic.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class ChristmasTree : ISceneObject
    {
        public float Z { get; set; }
        public Animator<TreeAnimationEnum> Animator { get; }
        public ChristmasTree()
        {
            Animator = new Animator<TreeAnimationEnum>(new List<Animation>{new Animation(TreeAnimationEnum.Blink, AnimationType.Repeatable, new List<AnimationFrame> {
                new AnimationFrame(40, 0), new AnimationFrame(40, 1)
            })}, TreeAnimationEnum.Blink);
        }
    }

    enum TreeAnimationEnum
    {
        Blink,
    }
}
