using GameEngine.Editor;
using GameEngine.Engine;
using GameEngine.Engine.Events;
using GameEngine.Pointers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Pointer
{
    public class GameObjectPointer : Pointer<GameObject>
    {
        public override void Update()
        {
            TryCaputreTarget();
            TryReleaseTarget();

            CheckDrag();

            base.Update();
        }

        Vector2 prevMousePosition;
        void CheckDrag()
        {
            Vector2 delta = InputManager.MousePositionCamera() - prevMousePosition;
            prevMousePosition = InputManager.MousePositionCamera();

            if (InputManager.MouseLeftDown() && Target != null && delta != Vector2.Zero)
            {
                OnDragged?.Invoke(new TargetDragged<GameObject>()
                {
                    Target = Target,
                    dragDelta = delta,
                    Sender = this,
                    pointerPosition = InputManager.MousePositionCamera()
                });
            }
        }

        void TryCaputreTarget()
        {
            if (InputManager.MouseLeftClicked())
            {
                Vector2 position = InputManager.MousePositionCamera();
                Target = GameObjectManager.GetOverlapping(new Rectangle() 
                {
                    Width = 10,
                    Height = 10,
                    X = (int)position.X,
                    Y = (int)position.Y
                }).FirstOrDefault();

                if (Target != null)
                    OnTargetSelected?.Invoke(new TargetSelected<GameObject>()
                    {
                        Target = Target,
                        Sender = this
                    });
            }
        }

        void TryReleaseTarget()
        {
            if (InputManager.MouseLeftReleased())
            {
                OnTargetDeselected?.Invoke(new TargetDeselected<GameObject>()
                {
                    Target = Target
                });
                Target = null;
            }
        }
    }

    
}
