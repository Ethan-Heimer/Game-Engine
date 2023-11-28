using GameEngine.Pointer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Pointers
{
    public interface IPointer<T> where T : IPointerManiplatable
    {
        T Target
        {
            get; set;
        }

        void Update();

        Pointer<T> AddManipulator(IPointerManipulator<T> manipulator);
        Pointer<T> RemoveManipulator(IPointerManipulator<T> manipulator);

        void RegisterCallback<T2>(PointerEvent<T2> callback) where T2 : IPointerEvent;
        void UnregisterCallback<T2>(PointerEvent<T2> callback) where T2 : IPointerEvent;
    }

    public interface IPointerManiplatable
    {

    }
}
