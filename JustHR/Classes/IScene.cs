using JustHR.Classes.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes
{
    interface IScene
    {
        public BackgroundsEnum Background { get; }
        public Menu Menu { get; }
        public List<ISceneObject> Objects { get; }
    }

    interface ISceneObject
    {
        
    }

    enum SceneObjectsEnum
    {
        BackChair,
        Calendar,
        ChristmasTree,
        Clock,
        Cooler,
        CurriculumVitae,
        Door,
        SideChair,
        Table,
        Whiteboard
    }
}
