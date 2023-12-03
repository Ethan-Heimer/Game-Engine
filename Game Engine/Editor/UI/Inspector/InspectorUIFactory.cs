using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    public class InspectorUIFactory
    {
        public bool TryGetUI(FieldInfo field, out UIComponent component)
        {
            component = null;
            Type fieldType = field.FieldType;

            if (!fieldType.IsSerializable)
                return false;

            MethodInfo mi = this.GetType().GetMethod("GetUI");
            var method = mi.MakeGenericMethod(fieldType);
            component = (UIComponent)method.Invoke(this, new object[] { field });

            Console.WriteLine(component + " try value");

            if (component != null)
                return true;

            return false;
        }

        public UIComponent GetUI<T>(FieldInfo fieldType)
        {
           
            Type template = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(typeof(IFieldTemplate<T>)));
            Console.WriteLine(template + " templeate");

            if (template == null)
                return null;

            IFieldTemplate<T> fieldTemplate = Activator.CreateInstance(template) as IFieldTemplate<T>;

            return fieldTemplate.GetUI(fieldType);
        }
    }
}
