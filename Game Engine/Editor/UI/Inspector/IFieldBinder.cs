using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    public interface IFieldBinder<T>
    {
        FieldInfo Field { get; set; }
        object Owner { get; set; }  

        string Name { get;}   

        T GetValue();
        void SetValue(T value);

        //void CheckChanged();
        //void OnChanged();
    }
}
