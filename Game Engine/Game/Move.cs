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
        public float Speed;
        public void Update() 
        {
            if(Input.IsKeyDown(Keys.A)) 
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X - Speed, gameObject.Transform.Position.Y);
            }
            if (Input.IsKeyDown(Keys.D))
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X + Speed, gameObject.Transform.Position.Y);
            }

            if(Input.IsKeyDown(Keys.W))
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X, gameObject.Transform.Position.Y-Speed);
            }
            if (Input.IsKeyDown(Keys.S))
            {
                gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X, gameObject.Transform.Position.Y+Speed);
            }
        }
    }
}
