using JustHR.Classes.Basic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Interface
{
    class Phone
    {
        public List<Button> Buttons { get; }


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
        //public Dictionary<Enum, SoundEffect> SoundEffects;

        public Button(ButtonEnum name, Vector2 pos, Vector2 size, Controller controller, Action action)
        {
            Name = name;
            Action = action;
            Pos = pos;
            Size = size;
            //SoundEffects = soundEffects;

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
        AcceptButton,
        JobResponsibilitiesButton,
        RecallButton,
        RejectButton,
        StartButton,
        StartDayButton,
    }
}
