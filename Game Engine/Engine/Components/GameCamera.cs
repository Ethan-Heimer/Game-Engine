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
        }

        public void WhileInEditor()
        {
            camera.Position = transform.WorldPosition;
            camera.Zoom = Zoom;

            Renderer.DrawCircle(transform.WorldPosition, 10, Color.Red);

            float width = camera.GetVisibleArea().Width;
            float height = camera.GetVisibleArea().Height;

            Vector2[] verticies = new Vector2[]
            {
                new Vector2(transform.WorldPosition.X - width/2, transform.WorldPosition.Y - height/2),
                new Vector2(transform.WorldPosition.X + width/2, transform.WorldPosition.Y - height/2),
                new Vector2(transform.WorldPosition.X + width/2, transform.WorldPosition.Y + height/2),
                new Vector2(transform.WorldPosition.X - width/2, transform.WorldPosition.Y + height/2),
            };

            Renderer.DrawWireframe(verticies);
           
        }
    }
}
