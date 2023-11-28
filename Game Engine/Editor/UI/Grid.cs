using GameEngine.Engine.Events;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GameEngine.Editor.UI
{
    public class Grid
    {
        public void Draw(Rectangle bounds, int cellSize) 
        {
            if (bounds == Rectangle.Empty)
                return;

            DrawHorizontal(bounds, cellSize);
            DrawVertical(bounds, cellSize);
        }

        void DrawHorizontal(Rectangle bounds, int cellSize)
        {
            for (int i = 0; i < bounds.Width; i++)
            {
                if (i % cellSize != 0)
                    continue;
                    
                int offset = (bounds.X / cellSize) * cellSize;
                Color color = GetColor(i + offset, cellSize);
                Renderer.DrawLine(new Vector2(i + offset, bounds.Y), new Vector2(i + offset, bounds.Bottom), color);
            }
        }

        void DrawVertical(Rectangle bounds, int cellSize)
        {
            for (int i = 0; i < bounds.Height; i++)
            {
                if (i % cellSize != 0)
                    continue;

                int offset = (bounds.Y / cellSize) * cellSize;
                Color color = GetColor(i + offset, cellSize);
                Renderer.DrawLine(new Vector2(bounds.X, i + offset), new Vector2(bounds.Right, i + offset), color); 
            }
        }

        Color GetColor(int position, int cellSize)
        {
            if (position % (cellSize * 10) == 0)
                return new Color(255, 255, 255);

            return new Color(255,255,255) * .1f;
        }
    }
}
