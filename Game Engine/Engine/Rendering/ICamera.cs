using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Rendering
{
    public interface ICamera
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }    
        float Zoom { get; set; } 


        void OnUpdate();
        Matrix GetTransformationMatrix();
        void LookAt(Vector2 lookAt);
        Rectangle GetVisibleArea();
    }
}
