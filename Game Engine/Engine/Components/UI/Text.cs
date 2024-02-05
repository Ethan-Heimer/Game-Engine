using GameEngine.Editor;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Components.UI
{
    public class Text : UIElement
    {
        public string text = "";
        public Color color;

        protected override void OnGUI(Layer layer, Canvas canvas)
        {
            var font = AssetManager.LoadContent<SpriteFont>("Fonts\\Arial");

            Renderer.DrawText(font, text, UIPosition, color, layer);
        }
    }
}
