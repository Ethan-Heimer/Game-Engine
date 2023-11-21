using GameEngine.Rendering;
using Microsoft.Build.Tasks.Xaml;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Engine.Rendering
{
    public class EditorCamera : ICamera
    {
        Matrix transformMatrix; //A transformation matrix containing info on our position, how much we are rotated and zoomed etc.

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        
        float zoom;
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value; if (zoom < 0.1f) zoom = 0.1f; //Negative zoom will flip image.
            }
        }

        Rectangle screenRect;
        public Rectangle ScreenRect
        {
            get { return screenRect; }
        }

        static public bool updateYAxis = false; //Should the camera move along on the y axis?
        static public bool updateXAxis = true; //Should the camera move along on the x axis?

        public EditorCamera()
        {
            Zoom = 1f;
            Rotation = 0.0f;

            //Start the camera at the center of the screen:
            Position = new Vector2(Resolution.VirtualWidth / 2, Resolution.VirtualHeight / 2);
        }

        public void OnUpdate()
        {
            Move();
            ZoomCamera();
            CalculateMatrixAndRectangle();
        }

        /// <summary>
        /// Immediately sets the camera to look at the position passed in.
        /// </summary>
        public void LookAt(Vector2 lookAt)
        {
            //Immediately looks at the vector passed in:
            var xPos = 0f;
            var yPos = 0f;  

            if (updateXAxis == true)
                xPos = lookAt.X;
            if (updateYAxis == true)
                xPos = lookAt.Y;

            Position = new Vector2 (xPos, yPos);
        }

        private void CalculateMatrixAndRectangle()
        {
            //The math involved with calculated our transformMatrix and screenRect is a little intense, so instead of calling the math whenever we need these variables,
            //we'll calculate them once each frame and store them... when someone needs these variables we will simply return the stored variable instead of re cauclating them every time.

            //Calculate the camera transform matrix:
            transformMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0)) * Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(Resolution.VirtualWidth
                            * 0.5f, Resolution.VirtualHeight * 0.5f, 0));

            //Now combine the camera's matrix with the Resolution Manager's transform matrix to get our final working matrix:
            transformMatrix = transformMatrix * Resolution.getTransformationMatrix();

            //Round the X and Y translation so the camera doesn't jerk as it moves:
            transformMatrix.M41 = (float)Math.Round(transformMatrix.M41, 0);
            transformMatrix.M42 = (float)Math.Round(transformMatrix.M42, 0);

            //Calculate the rectangle that represents where our camera is at in the world:
            screenRect = GetVisibleArea();
        }

        /// <summary>
        /// Calculates the screenRect based on where the camera currently is.
        /// </summary>
        public Rectangle GetVisibleArea()
        {
            Matrix inverseViewMatrix = Matrix.Invert(transformMatrix);
            Vector2 tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            Vector2 tr = Vector2.Transform(new Vector2(Resolution.VirtualWidth, 0), inverseViewMatrix);
            Vector2 bl = Vector2.Transform(new Vector2(0, Resolution.VirtualHeight), inverseViewMatrix);
            Vector2 br = Vector2.Transform(new Vector2(Resolution.VirtualWidth, Resolution.VirtualHeight), inverseViewMatrix);
            Vector2 min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            Vector2 max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            return new Rectangle((int)min.X, (int)min.Y, (int)(Resolution.VirtualWidth / zoom), (int)(Resolution.VirtualHeight / zoom));
        }

        public Matrix GetTransformationMatrix()
        {
            return transformMatrix; //Return the transformMatrix we calculated earlier in this frame.
        }

        Vector2 prevMousePos;
        void Move()
        {
            Vector2 currentMousePos = InputManager.RawMousePosition();
            Vector2 delta = currentMousePos - prevMousePos;

            if (InputManager.MouseMiddleDown())
            {
                Position -= delta/(zoom * 1.5f);
            }

            prevMousePos = InputManager.RawMousePosition();
        }

        void ZoomCamera()
        {
            Zoom += InputManager.ScrollDelta() * .001f;
        }
    }
}
