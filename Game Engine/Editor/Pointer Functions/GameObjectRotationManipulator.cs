using GameEngine.Pointer;
using GameEngine.Pointers;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using GameEngine.Editor.Widgets;

namespace GameEngine.Editor
{
    public class GameObjectRotationManipulator : IPointerManipulator<GameObject>
    {
        float startAngle;
        RotationWidget rotationWidget = new RotationWidget();

        bool active
        {
            get
            {
                return InputManager.IsKeyDown(Keys.R);
            }
        }

        public void RegisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.RegisterCallback<TargetSelected<GameObject>>(OnSelect);
            pointer.RegisterCallback<TargetDragged<GameObject>>(OnDrag);
            pointer.RegisterCallback<PointerUpdated<GameObject>>(OnUpdate);
        }

        public void UnregisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.UnregisterCallback<TargetSelected<GameObject>>(OnSelect);
            pointer.UnregisterCallback<TargetDragged<GameObject>>(OnDrag);
            pointer.UnregisterCallback<PointerUpdated<GameObject>>(OnUpdate);
        }
        void OnSelect(TargetSelected<GameObject> e)
        {
            startAngle = e.Target.Transform.Rotation;
        }

        void OnUpdate(PointerUpdated<GameObject> e) 
        {
            if (e.Target == null || !active)
                return;

            rotationWidget.Position = e.Target.Transform.Position;
            rotationWidget.pointerPosition = e.pointerPosition;

            WidgetDrawer.Draw(rotationWidget);
        }

        void OnDrag(TargetDragged<GameObject> e)
        {
            if (!active)
                return;

            Vector2 position = e.Target.Transform.Position;

            float angle = (float)Math.Atan2(e.pointerPosition.Y - position.Y, e.pointerPosition.X - position.X);
            e.Target.Transform.Rotation = startAngle + angle * (float)(180 / Math.PI);
        }
    }
}
