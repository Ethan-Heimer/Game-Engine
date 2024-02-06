﻿using GameEngine.Editor.Windows;
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
        Button button;
        public void Awake()
        {
            //Console.WriteLine(gameObject.Transform);
        }

        public void Start()
        {
            Console.WriteLine("Start!! " + gameObject.Name);
            button = gameObject.GetComponent<Button>();

            button.OnClick += () => Console.WriteLine("Click");
        }

        public void Update()
        {
            //Console.WriteLine("Update!!!");
            //TextureRenderer render = gameObject.renderer;
            //render.Sprite = new Rendering.Sprite("PlaceHolderTwo");
            //Console.WriteLine(transform.Size);
            
        }

        public void OnDraw()
        {
           
        }

        public void ParamFunc(string inputOne, int inputTwo)
        {
            Console.WriteLine(inputOne + " " + inputTwo);
        }
    }
}
