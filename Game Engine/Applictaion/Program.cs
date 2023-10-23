using Microsoft.Xna.Framework;
using System;
using GameEngine.Editor;
using GameEngine;

namespace Program
{
    static class Program
    {
         /// <summary>
         /// The main entry point for the application.
         /// </summary>
         [STAThread]
         static void Main(string[] args)
         {
             using (Game1 game = new Game1())
             {
                 Editor.Open();
                 GameInitalizer.Run(game);
             }
         }
     }

}