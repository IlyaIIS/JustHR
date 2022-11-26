using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class CurriculumVitae : ISceneObject
    {
        public bool IsExpanded { get; set; }

        public CurriculumVitae(Controller controller)
        {
            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                if (key == MouseButton.LeftButton)
                {
                    if (!IsExpanded)
                    {
                        if (x > 400 && y > 440 && x < 640 && y < 530)
                        {
                            IsExpanded = true;
                        }
                    } else
                    {
                        if (x < 200 || y < 40 || x > 850 || y > 540)
                        {
                            IsExpanded = false;
                        }
                    }
                }
            };
        }
    }
}
