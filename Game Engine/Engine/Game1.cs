
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Microsoft.Xna.Framework.src.Graphics;
using GameEngine.Editor;
using GameEngine.Game;
using System;
using System.IO;
using GameEngine.ComponentManagement;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Linq;
using GameEngine.Engine;
using GameEngine.Engine.Rendering;
using System.Security.Cryptography;

namespace GameEngine
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Renderer renderer;
        AssetManager assetManager;

        public Game1() //This is the constructor, this function is called whenever the game class is created.
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            renderer = new Renderer(this, graphics, GraphicsDevice);
            assetManager = new AssetManager(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SceneManager.LoadScenes();
            
            GameObject gameObect = new GameObject();
            gameObect.Transform.Position = Vector2.One * 200;
            gameObect.AddComponent<TextureRenderer>().texture = AssetManager.LoadContent<Texture2D>("PlaceHolderTwo");
            gameObect.AddComponent<Move>().Speed = 9;
            gameObect.AddComponent<TestComponent>();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            GameExecuter.Tick();
        }

       
        protected override void Draw(GameTime gameTime)
        {
            renderer.Draw();
        }
    }
}
