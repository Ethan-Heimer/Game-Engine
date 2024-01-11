using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class Move : Behavior
    {
        public int Speed;
       
        public void Update() 
        {
            if(InputManager.IsKeyDown(Keys.A)) 
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X - Speed, gameObject.Transform.Position.Y);
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X + Speed, gameObject.Transform.Position.Y);
            }

            if(InputManager.IsKeyDown(Keys.W))
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X, gameObject.Transform.Position.Y-Speed);
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X, gameObject.Transform.Position.Y+Speed);
            }
        }
    }
}
