using GameEngine.Pointer;
using GameEngine.Pointers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Editor
{
    public class GameObjectTranslateManipulator : IPointerManipulator<GameObject>
    {

        Vector2 truePosition;
        bool active
        {
            get
            {
                return InputManager.IsKeyDown(Keys.T);
            }
        } 

        public void RegisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.RegisterCallback<TargetSelected<GameObject>>(OnSelect);
            pointer.RegisterCallback<TargetDragged<GameObject>>(OnDrag);
        }

        public void UnregisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.UnregisterCallback<TargetSelected<GameObject>>(OnSelect);
            pointer.UnregisterCallback<TargetDragged<GameObject>>(OnDrag);
        }

        void OnSelect(TargetSelected<GameObject> e) 
        {
            truePosition = e.Target.Transform.Position;
        }

        void OnDrag(TargetDragged<GameObject> e)
        {
            if (!active)
                return;

            truePosition += e.dragDelta;

            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                e.Target.Transform.Position = truePosition;
            }
            else
            {
                e.Target.Transform.Position.X = ((int)(truePosition.X / 100)) * 100;
                e.Target.Transform.Position.Y = ((int)(truePosition.Y / 100)) * 100;
            }
        }
    }
}
