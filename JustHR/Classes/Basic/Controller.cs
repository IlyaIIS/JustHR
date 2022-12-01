using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Basic
{
    /// <summary>
    /// Предоставляет интерфейс, активирующий события ввода и позволяющий задать действия на эти события. 
    /// </summary>
    class Controller
    {
        /// <summary>Предыдущее значение скролла мыши. Нужно, чтобы определять, в какую сторону прокручен скролл.</summary>
        private int preMouseScroll = 0;

        /// <summary> Список клавиш, использующихся в игре. Нужен для запоминания, была ли зажата кнопка, чтобы можно было реализовать событие отжатия кнопки. </summary>
        readonly Dictionary<Keys, bool> wasKeyPressed = new Dictionary<Keys, bool>()
        {
            { Keys.R, false},
            { Keys.Left, false},
            { Keys.Up, false},
            { Keys.Right, false},
            { Keys.Down, false},
            { Keys.D1, false},
            { Keys.D2, false},
        };
        readonly Dictionary<MouseButton, bool> wasMouseButtonPressed = new Dictionary<MouseButton, bool>()
        {
            { MouseButton.LeftButton, false},
            { MouseButton.MiddleButton, false},
            { MouseButton.RightButton, false},
        };

        //Делегаты и соответствующие им события
        public delegate void KeyClickHandler(Keys key);
        public event KeyClickHandler OnKeyPressed;
        public event KeyClickHandler OnKeyPressing;
        public event KeyClickHandler OnKeyReleased;
        public delegate void MouseScrollHandler(int scroll);
        public event MouseScrollHandler OnMouseScroll;
        public delegate void MouseButtonHandler(MouseButton button, int x, int y);
        public event MouseButtonHandler OnMouseButtonPressed;
        public event MouseButtonHandler OnMouseButtonPressing;
        public event MouseButtonHandler OnMouseButtonReleased;
        public delegate void MouseMoveHandler(Point lastPos, Point newPos);
        public event MouseMoveHandler OnMouseMoving;

        private Point lastMousePos;

        /// <summary>Активирует заданные контроллеру действия, если произошли соответствующие им события (например нажатие кнопки).</summary>
        public void TriggerControlEvents()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            TriggerEvents(keyState, mouseState);
        }

        /// <summary>Реализует активацию событий управления.</summary>
        private void TriggerEvents(KeyboardState keyState, MouseState mouseState)
        {
            TriggerKeyEvents();
            TriggetMouseMoveEvent();
            TriggerMouseButtonEvents();
            TriggerMouseScrollEvents();

            void TriggerKeyEvents()
            {
                foreach (Keys key in new List<Keys>(wasKeyPressed.Keys))
                {
                    if (keyState.IsKeyDown(key))
                    {
                        if (wasKeyPressed[key])
                        {
                            //OnKeyPressing(key);
                        }
                        else
                        {
                            //OnKeyPressed(key);
                            wasKeyPressed[key] = true;
                        }
                    }
                    else
                    {
                        if (wasKeyPressed[key])
                        {
                            //OnKeyReleased(key);
                            wasKeyPressed[key] = false;
                        }
                    }
                }
            }
            void TriggerMouseButtonEvents()
            {
                TriggerMouseCertainButtonEvents(mouseState.LeftButton, MouseButton.LeftButton);
                TriggerMouseCertainButtonEvents(mouseState.MiddleButton, MouseButton.MiddleButton);
                TriggerMouseCertainButtonEvents(mouseState.RightButton, MouseButton.RightButton);

                void TriggerMouseCertainButtonEvents(ButtonState buttonState, MouseButton certainButton)
                {
                    if (buttonState == ButtonState.Pressed)
                    {
                        if (wasMouseButtonPressed[certainButton])
                        {
                            //OnMouseButtonPressing(certainButton, mouseState.X, mouseState.Y, tile);
                        }
                        else
                        {
                            //OnMouseButtonPressed(certainButton, mouseState.X, mouseState.Y, tile);
                            wasMouseButtonPressed[certainButton] = true;
                        }
                    }
                    else
                    {
                        if (wasMouseButtonPressed[certainButton])
                        {
                            OnMouseButtonReleased(certainButton, mouseState.X, mouseState.Y);
                            wasMouseButtonPressed[certainButton] = false;
                        }
                    }
                }
            }
            void TriggerMouseScrollEvents()
            {
                if (mouseState.ScrollWheelValue > preMouseScroll)
                    //OnMouseScroll(1);
                if (mouseState.ScrollWheelValue < preMouseScroll)
                    //OnMouseScroll(-1);
                preMouseScroll = mouseState.ScrollWheelValue;
            }
            void TriggetMouseMoveEvent()
            {
                if (!mouseState.Position.Equals(lastMousePos))
                {
                    OnMouseMoving?.Invoke(lastMousePos, mouseState.Position);
                    lastMousePos = mouseState.Position;
                }
            }
        }


        void DoActionIfKeyReleased(Keys key, Action action, KeyboardState keyState)
        {
            if (!keyState.IsKeyDown(key))
            {
                if (wasKeyPressed[key])
                {
                    wasKeyPressed[key] = false;
                    action();
                }
            }
            else
            {
                wasKeyPressed[key] = true;
            }
        }
        void AddActionOnKeyPressed(Keys key, Action action)
        {
            OnKeyReleased += (eventKey) =>
            {
                if (key == eventKey)
                    action();
            };
        }
        void AddActionOnKeyPressed(MouseButton button, Action action)
        {
            OnMouseButtonReleased += (eventKey, x, y) =>
            {
                if (button == eventKey)
                    action();
            };
        }

        public void ClearEvents()
        {
            OnMouseButtonReleased = null;
        }

    }
    enum MouseButton
    {
        LeftButton,
        MiddleButton,
        RightButton
    }
}
