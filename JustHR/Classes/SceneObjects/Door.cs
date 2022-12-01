using JustHR.Classes.Basic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class Door : ISceneObject
    {
        public float Z { get; set; }
        public DoorState State { get; set; } = DoorState.Opend;

        public Door(OfficeScene scene,Controller controller)
        {

        }
    }

    enum DoorState
    {
        Closed,
        Opend
    }
}
