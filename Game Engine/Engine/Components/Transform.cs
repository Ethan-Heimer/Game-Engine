using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Transform : Behavior
    {
        public Vector2 Position;
        public float Rotation;
        public float Scale = 1;

        public OriginType originType = OriginType.Center;

        
        Vector2 OriginMultiplyer
        {
            get
            {
              
                switch(originType) 
                {
                    case OriginType.Center:
                        return Vector2.One * .5f;
                        
                    case OriginType.Zero:
                        return Vector2.Zero;
                       
                }

                return Vector2.Zero;
            }
        }

        public Vector2 Origin
        {
            get
            {
                var width = gameObject.renderer.Bounds.Width;
                var height = gameObject.renderer.Bounds.Height;

                return new Vector2(width * OriginMultiplyer.X, height * OriginMultiplyer.Y);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                if (gameObject.renderer == null)
                   return Rectangle.Empty;

                Vector2 rotVector = new Vector2(Math.Abs((float)Math.Cos(Rotation * (Math.PI / 180))), Math.Abs((float)Math.Sin(Rotation * (Math.PI / 180))));

                var width = (gameObject.renderer.Bounds.Width * Math.Abs(Scale)) * rotVector.X + (gameObject.renderer.Bounds.Height *  Math.Abs(Scale)) * rotVector.Y;
                var height = (gameObject.renderer.Bounds.Height * Math.Abs(Scale)) * rotVector.X + (gameObject.renderer.Bounds.Width * Math.Abs(Scale)) * rotVector.Y;

                Rectangle bounds = new Rectangle
                {
                    Width = (int)(width),
                    Height = (int)height,

                    X = (int)(Position.X - width * OriginMultiplyer.X),
                    Y = (int)(Position.Y - height * OriginMultiplyer.Y),
                };

                return bounds;
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(-Bounds.Width/2 + Bounds.X, -Bounds.Height/2 + Bounds.Height);
            }
        }

        public double GetRotationInRad()
        {
            Renderer.RenderRect(Bounds, new Color(0, 255, 150) * .1f);
            return Rotation * (Math.PI/180);
        }
    }

    public enum OriginType
    {
        Center,
        Zero
    }
}
