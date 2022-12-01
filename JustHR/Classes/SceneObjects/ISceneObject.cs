using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    interface ISceneObject
    {
        public float Z { get; set; }
    }

    interface IClickable
    {
        public Rectangle Collision { get; }
        public void Click(OfficeScene scene);
    }
}
