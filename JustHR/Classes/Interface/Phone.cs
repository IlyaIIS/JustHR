using Microsoft.Xna.Framework;
using ShaderPack.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Interface
{
    class Phone
    {
        List<Button> Buttons { get; }

        public Phone(List<Button> buttons)
        {
            Buttons = buttons;
        }
    }

    class Button
    {
        public Action Action { get; }
        public ButtonEnum Name { get; }
        public Vector2 Pos { get; } 
        public Vector2 Size { get; }

        public Button(ButtonEnum name, Vector2 pos, Vector2 size, Controller controller, Action action)
        {
            Action = action;
            Pos = pos;
            Size = size;

            controller.OnMouseButtonReleased += (button, x, y) => { 
                if (button == MouseButton.LeftButton)
                {
                    if (x > Pos.X && y > Pos.Y && x < Pos.X + Size.X && y < Pos.Y + Size.Y)
                    {
                        Action.Invoke();
                    }
                }
            };
        }
    }

    enum ButtonEnum
    {
        HeirForHR,
        TakeJob,
        Reject,
        CallBack,
    }
}
