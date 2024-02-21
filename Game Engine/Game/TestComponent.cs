using GameEngine.Editor.Windows;
using GameEngine.Engine.ComponentModel;
using GameEngine.Engine.Components.UI;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Game
{
    [ExecuteAlways]
    public class TestComponent : Behavior
    {
        //Button button;

        public GameObject GameObject;
        public Button button;
       
        public void Start()
        {
            button = gameObject.GetComponent<Button>();
            button.OnClick += () => Console.WriteLine(GameObject.Name);
        }

        
    }
}
