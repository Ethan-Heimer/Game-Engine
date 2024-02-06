using GameEngine.Engine.Utilities;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Components.UI
{
    public class Button : UIElement
    {
        public Color BaseColor;
        public Color HoverColor;

        public override void Update()
        {
            base.Update();
        }
        protected override void OnGUI(Layer layer, Canvas canvas)
        {
            Color color;

            if (IsMouseHover)
                color = HoverColor;
            else
                color = BaseColor;

            Rectangle rectangle = new Rectangle
            {
                X = (int)UIPosition.X,
                Y = (int)UIPosition.Y,

                Width = (int)Size.X,
                Height = (int)Size.Y
            };

            Renderer.RenderRect(rectangle, color, layer);
        }

        
    }
}
