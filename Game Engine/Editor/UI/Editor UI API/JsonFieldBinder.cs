using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    internal class JsonFieldBinder<T> : IFieldBinder<T> 
    {
        public FieldInfo Field { get; set; }
        public object Owner { get; set; }
        public string Name
        {
            get
            {
                return Field.Name;
            }
        }
        public void SetValue(T value) 
        {

        }

    }
}
