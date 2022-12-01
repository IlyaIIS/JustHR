using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class Exit : ISceneObject, IClickable
    {
        public float Z { get; set; }
        public Rectangle Collision { get; private set; } = new Rectangle(52, 144, 144, 58);

        public void Click(OfficeScene scene)
        {
            if (scene.Objects.Door.State == DoorState.Closed)
                scene.TriggerBossByExit();
        }
    }
}
