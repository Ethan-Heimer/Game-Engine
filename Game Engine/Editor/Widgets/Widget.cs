using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Widgets
{
    public abstract class Widget
    {
      
        public  Vector2 Position;
        protected abstract void OnDraw();
        protected void OnDisabled(){ }
    }
}
