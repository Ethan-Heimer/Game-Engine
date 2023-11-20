using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Game
{
    public class TestComponent : Behavior
    {
        public void Awake()
        {
            //Console.WriteLine(gameObject.Transform);
        }

        public void Start()
        {
            Console.WriteLine("Start!!");
        }

        public void Update()
        {
            //Console.WriteLine("Update!!!");
        }

        public void ParamFunc(string inputOne, int inputTwo)
        {
            Console.WriteLine(inputOne + " " + inputTwo);
        }
    }
}
