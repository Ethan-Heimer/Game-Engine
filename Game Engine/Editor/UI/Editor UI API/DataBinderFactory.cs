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
        public Type TryGetBinder(Type dataType, Type BinderType)
        {
            if (!dataType.IsSerializable)
                return null;

            return BinderType.MakeGenericType(dataType);
        }

        public Type TryGetBinder(FieldInfo field, Type BinderType)
        {
            Type fieldType = field.FieldType;

            if (!fieldType.IsSerializable)
                return null;

            return BinderType.MakeGenericType(fieldType);
        }

        public Type TryGetBinder(PropertyInfo field, Type BinderType)
        {
            Type fieldType = field.PropertyType;

            if (!fieldType.IsSerializable)
                return null;

            return BinderType.MakeGenericType(fieldType);
        }
    }
}
