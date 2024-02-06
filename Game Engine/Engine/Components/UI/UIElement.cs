using GameEngine.Engine.Utilities;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameEngine.Engine.Components.UI
{
    public abstract class UIElement : Behavior
    {
        public Vector2 Size;
        public Rectangle ElementRect
        {
            get
            {
                return new Rectangle((int)transform.Position.X, (int)transform.Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
        public bool IsMouseHover
        {
            get
            {
                Vector2 mousePos = InputManager.RawMousePosition();

                return ElementRect.Intersects(mousePos);
            }
        }

        Canvas _canvas;
        Canvas Canvas
        {
            get
            {
                if(_canvas == null)
                    _canvas = gameObject.GetRoot().GetComponent<Canvas>();   
                
                return _canvas;
            }
        }


        bool render
        {
            get
            {
                return Canvas != null;
            }
        }


        protected Vector2 UIPosition;
        

        protected abstract void OnGUI(Layer layer, Canvas canvas);

        public virtual void Update()
        {
            if(render)
                OnGUI(Layer.UI, Canvas);

            UIPosition = transform.WorldPosition;
        }

        public void WhileInEditor()
        {
            if(render)
                OnGUI(Layer.Game, Canvas);

            UIPosition = transform.WorldPosition;
        }
    }
}
