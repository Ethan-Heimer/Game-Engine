using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Engine;
using GameEngine.Engine.Events;
using Microsoft.Xna.Framework;

namespace GameEngine.Rendering
{
    public static class CameraManager
    {
        static ICamera MainCamera = null;

        
        public static void Init()
        {
            EngineEventManager.AddEventListener<OnEngineTickEvent>((e) => Update());

            //change where this executes
            EngineEventManager.AddEventListener<OnEnterPlayMode>((e) => SetMainCamera(null));
        }

        public static void Update() { MainCamera?.OnUpdate(); }

        public static Matrix GetTransformantionMaxtrix()
        {
            Matrix? matrix = MainCamera?.GetTransformationMatrix();

            if(matrix == null)
                return Matrix.Identity;
            
            return (Matrix)matrix;
        }

        public static void LookAt(Vector2 vector2) => MainCamera.LookAt(vector2);
        public static Rectangle GetVisableArea()
        {
            Rectangle? rect = MainCamera?.GetVisibleArea();

            if(rect == null)
                return Rectangle.Empty;

            return (Rectangle)rect;
        }

        public static void SetMainCamera(ICamera camera) => MainCamera = camera;
    }
}
