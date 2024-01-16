using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Rendering
{
    public static class GameWindowManager
    {
        static GameWindow gameWindow;

        public static void Init(GameWindow window)
        {
            gameWindow = window;
            window.ClientSizeChanged += (e, s) => Console.WriteLine("test");
        }

        public static Vector2 WindowPosition()
        {
            return new Vector2(gameWindow.ClientBounds.X, gameWindow.ClientBounds.Y);
        }

        public static Vector2 WindowSize() 
        {
            return new Vector2(gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
        }
    }
}
