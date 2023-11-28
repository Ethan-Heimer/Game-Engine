using GameEngine.Pointer;
using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using GameEngine.Editor.Widgets;

namespace GameEngine.Editor
{
    public class GameObjectScaleManipulator : IPointerManipulator<GameObject>
    {
        
        float startScale;

        bool active
        {
            get
            {
                return InputManager.IsKeyDown(Keys.S);
            }
        }

        LineWidget lineWidget = new LineWidget();

        public void RegisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.RegisterCallback<TargetSelected<GameObject>>(OnSelect);
            pointer.RegisterCallback<TargetDeselected<GameObject>>(OnDeselect);

            pointer.RegisterCallback<TargetDragged<GameObject>>(OnDrag);
            pointer.RegisterCallback<PointerUpdated<GameObject>>(OnUpdate);
        }

        public void UnregisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.UnregisterCallback<TargetSelected<GameObject>>(OnSelect);
            pointer.UnregisterCallback<TargetDeselected<GameObject>>(OnDeselect);

            pointer.UnregisterCallback<TargetDragged<GameObject>>(OnDrag);
            pointer.UnregisterCallback<PointerUpdated<GameObject>>(OnUpdate); 
        }

        void OnSelect(TargetSelected<GameObject> e)
        {
            startScale = e.Target.Transform.Scale;
        }

        void OnDeselect(TargetDeselected<GameObject> e) 
        {
            startScale = 0;
        }

        void OnUpdate(PointerUpdated<GameObject> e)
        {
            if (!active || e.Target == null) return;

            lineWidget.Position = e.Target.Transform.Position;
            lineWidget.Position2 = e.pointerPosition;

            WidgetDrawer.Draw(lineWidget);
        }

        void OnDrag(TargetDragged<GameObject> e)
        {
            if(!active) return;

            float scale = (startScale += (e.dragDelta.X + e.dragDelta.Y)/2)/100;
            e.Target.Transform.Scale = scale;
        }
    }
}
