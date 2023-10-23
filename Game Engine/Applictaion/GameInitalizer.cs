using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameEngine;


    public static class GameInitalizer
    {
        public static async void Run(Game1 game)
        {
            var signal = new SemaphoreSlim(0, 1);

            game.Exiting += (s, _) => signal.Release();
            game.Run();

            await signal.WaitAsync();
        }
    }

