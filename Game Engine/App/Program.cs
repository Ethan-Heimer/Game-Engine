using Microsoft.Xna.Framework;
using System;
using GameEngine.Editor;
using GameEngine;
using GameEngine.Editor.Windows;

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
                App app = new App();
                app.InitializeComponent();

                Editor.Start();
                GameInitalizer.Run(game);
                
            }
         }
     }

}