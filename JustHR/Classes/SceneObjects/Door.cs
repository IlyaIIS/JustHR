using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class Door : ISceneObject
    {
        public DoorState State { get; set; } = DoorState.Opend;

        public Door(OfficeScene scene,Controller controller)
        {
            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                if (key == MouseButton.LeftButton)
                    if (x > 15 && y > 105 && x < 230 && y < 330)
                    {
                        scene.TriggerBoosByExit();
                    }
            };
        }
    }

    enum DoorState
    {
        Closed,
        Opend
    }
}
