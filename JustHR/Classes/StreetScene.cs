using JustHR.Classes.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes
{
    class StreetScene : IScene
    {
        public BackgroundsEnum Background { get; } = BackgroundsEnum.Street;
        public Menu Menu { get; }

        public List<ISceneObject> Objects { get; } = new List<ISceneObject>();

        public StreetScene(Menu menu)
        {
            Menu = menu;
        }
    }

    enum BackgroundsEnum
    {
        Street,
        Office
    }
}
