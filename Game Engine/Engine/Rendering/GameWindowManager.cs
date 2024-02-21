using GameEngine.Debugging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Rendering
{
    [Note(note ="Pos = (320, 180) Size = (1920, 1080) on home machiene")]
    public static class GameWindowManager
    {
        static GameWindow gameWindow;

        public static void Init(GameWindow window)
        {
            gameWindow = window;
        }

        public static Vector2 WindowPosition()
        {
            var pos = new Vector2(gameWindow.ClientBounds.X, gameWindow.ClientBounds.Y);
            return pos;
        }

        public static Vector2 WindowSize() 
        {
            var size = new Vector2(gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
            return size;
        }
    }
}
