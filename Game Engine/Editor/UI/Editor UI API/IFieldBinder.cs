using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GameEngine.Editor.UI.Inspector
{
    public interface IFieldBinder<T>
    {
        object Owner { get; set; }
        string Name { get;}   

        T GetValue();
        void SetValue(T value);

        bool HasValueChange();
    }
}
