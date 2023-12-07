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
        public Type TryGetTemplate(FieldInfo field)
        {
            Type fieldType = field.FieldType;

            if (!fieldType.IsSerializable)
                return null;

            MethodInfo mi = this.GetType().GetMethod("GetUI");
            var method = mi.MakeGenericMethod(fieldType);
            return (Type)method.Invoke(this, new object[0]);
        }

        public Type GetUI<T>()
        {
            Type templateType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.BaseType == typeof(FieldTemplate<T>));
            Console.WriteLine(templateType);
            return templateType;
        }
    }
}
