using GameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Engine.ComponentModel;

namespace GameEngine.Engine.Components
{
    public class GameCamera : Behavior
    {
        public float Zoom;

        Camera camera = new Camera();

        public void Start()
        {
            CameraManager.SetMainCamera(camera);
            
        }

        public void Update()
        {
            camera.Position = transform.WorldPosition;
            camera.Zoom = Zoom;

            Console.WriteLine(camera.GetVisibleArea());  
        }

        public void WhileInEditor()
        {
            Renderer.DrawCircle(transform.Position, 10, Color.Red);
        }
    }
}
