using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    partial class FloatDataBinder : IFieldBinder<float>
    {

        public FieldInfo Field { get; set; }
        public object Owner { get; set; }

        public FloatDataBinder(FieldInfo field, object owner)
        {
            Field = field;
            Owner = owner;
        }

        public string Name
        {
            get
            {
                return Field.Name;
            }
        }

        public float GetValue()
        {
            return (float)Field.GetValue(Owner); 
        }

        public void SetValue(float value) 
        {
            Field.SetValue(Owner, value);
        }
    }
}
