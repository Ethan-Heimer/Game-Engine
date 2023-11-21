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

        public void Draw(Drawer drawer, Rectangle bounds, int cellSize) 
        {
            DrawHorizontal(bounds, drawer, cellSize);
            DrawVertical(bounds, drawer, cellSize);
        }

        void DrawHorizontal(Rectangle bounds, Drawer drawer, int cellSize)
        {
            for (int i = 0; i < bounds.Width + 200; i++)
            {
                if (i % cellSize != 0)
                    continue;
                    
                int offset = (bounds.X / 100) * 100;
                Color color = GetColor(i + offset);
                drawer.DrawLine((new Vector3(i + offset, bounds.Y, 0)), (new Vector3(i + offset, bounds.Bottom + 100, 0)), color);
            }
        }

        void DrawVertical(Rectangle bounds, Drawer drawer, int cellSize)
        {
            for (int i = 0; i < bounds.Height + 200; i++)
            {
                if (i % cellSize != 0)
                    continue;

                int offset = (bounds.Y / 100) * 100;
                Color color = GetColor(i + offset);
                drawer.DrawLine((new Vector3(bounds.X, i + offset, 0)), (new Vector3(bounds.Right + 100, i + offset, 0)), color);
            }
        }

        Color GetColor(int position)
        {
            if (position % 1000 == 0)
                return new Color(200, 200, 200);

            return new Color(70,70, 70);
        }
    }
}
