using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    public class Vector2DataBinder : IFieldBinder<Vector2>
    {
        public FieldInfo Field { get; set; }
        public object Owner { get; set; }

        Vector2 Value;
        public Vector2DataBinder(FieldInfo field, object owner)
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

        public Vector2 GetValue()
        {
            return (Vector2)Field.GetValue(Owner);
        }

        public void SetValue(Vector2 value)
        {
            Value = value;
            Field.SetValue(Owner, value);
        }

        public bool HasValueChange()
        {
            if (Value != GetValue())
            {
                Value = GetValue();
                return true;
            }

            return false;
        }
    }
}
