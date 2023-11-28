using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Pointer
{
    public interface IPointerManipulator<T> where T : IPointerManiplatable
    {
        void RegisterCallbacks(IPointer<T> pointer);
        void UnregisterCallbacks(IPointer<T> pointer);
    }
   
}
