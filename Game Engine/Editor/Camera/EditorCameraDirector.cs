using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor
{
    public class EditorCameraDirector
    {
        ICamera camera;
        float panSpeed;
        Vector2 velocity;

        const float decel = 5;

        public EditorCameraDirector(ICamera camera, float panSpeed)
        {
            this.camera = camera;
            this.panSpeed = panSpeed;
        }

        public void Update()
        {
            Move();
            ZoomCamera();
        }


        Vector2 prevMousePos;
        void Move()
        {
            Vector2 currentMousePos = InputManager.RawMousePosition();
            Vector2 delta = currentMousePos - prevMousePos;

            if (InputManager.MouseMiddleDown())
            {
                camera.Position -= delta / (camera.Zoom * 1.5f);
            }

            prevMousePos = InputManager.RawMousePosition();

            Vector2 direction = new Vector2();
            if (InputManager.IsKeyDown(Keys.A))
                direction.X = -1;
            else if (InputManager.IsKeyDown(Keys.D))
                direction.X = 1;

            if (InputManager.IsKeyDown(Keys.W))
                direction.Y = -1;
            else if (InputManager.IsKeyDown(Keys.S))
                direction.Y = 1;

            if(direction.X != 0)
                velocity.X = direction.X * panSpeed;
            if (direction.Y != 0)
                velocity.Y = direction.Y * panSpeed;

            camera.Position += velocity;

            if (velocity.X > 0)
                velocity.X = velocity.X - decel;
            if (velocity.X < 0)
                velocity.X = velocity.X + decel;

            if (velocity.Y > 0)
                velocity.Y = velocity.Y - decel;
            if (velocity.Y < 0)
                velocity.Y = velocity.Y + decel;

        }

        void ZoomCamera()
        {
            camera.Zoom += InputManager.ScrollDelta() * .001f;
        }
    }
}
