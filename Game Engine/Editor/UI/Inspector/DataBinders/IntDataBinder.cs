using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    partial class IntDataBinder : IFieldBinder<int>
    {
        public FieldInfo Field { get; set; }
        public object Owner { get; set; }

        int Value;
        public IntDataBinder(FieldInfo field, object owner)
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

        public int GetValue()
        {
            return (int)Field.GetValue(Owner);
        }

        public void SetValue(int value) 
        {
            Value = value;
            Console.WriteLine("Set");
            Field.SetValue(Owner, value);
        }

        public bool HasValueChange()
        {
            if(Value != GetValue())
            {
                Console.WriteLine("Changed");
                Value = GetValue();
                return true;
            }

            return false;
        }
    }
}
