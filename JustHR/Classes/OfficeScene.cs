using JustHR.Classes.Interface;
using Microsoft.Xna.Framework;
using ShaderPack.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes
{
    class OfficeScene : IScene
    {
        public BackgroundsEnum Background { get; } = BackgroundsEnum.Office;
        public Menu Menu { get; }
        public List<ISceneObject> Objects { get; } = new List<ISceneObject>();
        public OfficeScene(int day, List<ISceneObject> objects, Controller controller)
        {
            Objects = objects;
            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.TakeJob,new Vector2(50, 380), new Vector2(200, 70), controller, () => {  }));
            buttons.Add(new Button(ButtonEnum.Reject, new Vector2(50, 480), new Vector2(200, 70), controller,() => { }));
            buttons.Add(new Button(ButtonEnum.CallBack, new Vector2(50, 580), new Vector2(200, 70), controller, () => { }));
            Phone phone = new Phone(buttons);

            Menu = new Menu(phone, new TextPlace());
        }
    }
}
