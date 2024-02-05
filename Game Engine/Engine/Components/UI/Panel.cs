using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Components.UI
{
    public class Panel : UIElement
    {
        public Vector2 Size;
        public Color Color;

        protected override void OnGUI(Layer layer, Canvas canvas)
        {
            Rectangle rectangle = new Rectangle
            {
                X = (int)UIPosition.X,
                Y = (int)UIPosition.Y,

                Width = (int)Size.X,
                Height = (int)Size.Y
            };

            Renderer.RenderRect(rectangle, Color, layer);
        }
    }
}
