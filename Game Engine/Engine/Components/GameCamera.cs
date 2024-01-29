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
    [ExecuteAlways]
    public class GameCamera : Behavior
    {
        Camera camera = new Camera();

        public void Start()
        {
            ICamera editorCamera = CameraManager.MainCamera;
            CameraManager.SetMainCamera(camera);
        }

        public void Update()
        {
            camera.Position = new Vector2(0,0);
            
           
        }
    }
}
