﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class Table : ISceneObject
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; }

    }
}
