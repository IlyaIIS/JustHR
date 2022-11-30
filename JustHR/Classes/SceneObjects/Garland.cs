using JustHR.Classes.Basic.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class Garland : ISceneObject
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; }
        public Animator<GarlandAnimationEnum> Animator { get; }
        public Garland()
        {
            var animations = new List<Animation> { new Animation(GarlandAnimationEnum.BlinkAnimation, AnimationType.Repeatable, new List<AnimationFrame>{
                new AnimationFrame(30, 0),
                new AnimationFrame(30, 1)
            })};
            Animator = new Animator<GarlandAnimationEnum>(animations, GarlandAnimationEnum.BlinkAnimation);
        }
    }

    enum GarlandAnimationEnum
    {
        BlinkAnimation
    }
}
