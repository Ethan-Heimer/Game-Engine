using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    public class DataBinderFactory
    {
        public Type TryGetBinder(FieldInfo field)
        {
            Type fieldType = field.FieldType;

            if (!fieldType.IsSerializable)
                return null;

            MethodInfo mi = this.GetType().GetMethod("GetBinder", BindingFlags.Instance | BindingFlags.NonPublic);
            var method = mi.MakeGenericMethod(fieldType);
            Type binder = (Type)method.Invoke(this, new object[0]);

            if (binder != null)
                return binder;

            return null;
        }

        Type GetBinder<T>()
        {
            Type template = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(typeof(IFieldBinder<T>)));
            Console.WriteLine(template + " templeate");

            if (template == null)
                return null;

            return template;
        }
    }
}
