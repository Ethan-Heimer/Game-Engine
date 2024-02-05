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

        public void Update()
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
