using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderPack.Classes
{
    class Camera
    {
        private float zoom;                    
        public Vector2 Pos { get; set; }       
        private float Rotation { get; set; }               
        public int ViewportWidth { get; set; } = 1024;
        public int ViewportHeight { get; set; } = 768;

        public Camera(Controller controller)
        {
            zoom = 1.0f;
            Rotation = 0.0f;
            Pos = Vector2.Zero;

            AddMoveEvent(controller);
            AddZoomChangingEvent(controller);
        }

        private void AddMoveEvent(Controller controller)
        {
            float speed = Settings.CamSpeed;

            controller.OnKeyPressing += (key) =>
            {
                if (key == Keys.Right)
                    Move(new Vector2(speed, 0));
                if (key == Keys.Up)
                    Move(new Vector2(0, -speed));
                if (key == Keys.Left)
                    Move(new Vector2(-speed, 0));
                if (key == Keys.Down)
                    Move(new Vector2(0, speed));
            };
        }

        private void AddZoomChangingEvent(Controller controller)
        {
            controller.OnMouseScroll += (scroll) =>
            {
                if (scroll > 0)
                    Zoom += 0.1f * Zoom;
                else
                    Zoom -= 0.1f * Zoom;
            };
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public void Move(Vector2 amount)
        {
            Pos += amount;
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            Matrix transform =
                Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));
            return transform;
        }
    }
}
