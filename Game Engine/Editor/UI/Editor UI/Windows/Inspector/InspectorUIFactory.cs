using GameEngine.Debugging;
using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    [Note(note = "GetTemplateType is the slowest thing ever written by man O(N^3) type beat")]
    public class UITemplateFactory<T> where T : UITemplateAttribute
    {
        DataBinderFactory binderFactory;
        Type binderType;

        public UITemplateFactory(Type binderType) 
        {
            this.binderType = binderType;

            binderFactory = new DataBinderFactory();
        }

        public IFieldTemplate TryGetTemplate(FieldInfo field, object owner)
        {
            Type fieldType = field.FieldType;

            if (!fieldType.IsSerializable)
                return null;

            var template = InvokeGetTemplateType(fieldType);
            if(template == null) return null;

            Type binder = binderFactory.TryGetBinder(field, binderType);

            return (IFieldTemplate)Activator.CreateInstance(template, new object[] { binder, field, owner });
        }

        public Type InvokeGetTemplateType(Type fieldType)
        {
            MethodInfo mi = GetType().GetMethod("GetTemplateType");
            var method = mi.MakeGenericMethod(fieldType);
            Type templateType = (Type)method.Invoke(this, new object[0]);

            return templateType;
        }

        public Type GetTemplateType<T2>()
        {
            Type template = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes(typeof(T), true).Length > 0).FirstOrDefault(x => x.BaseType == typeof(FieldTemplate<T2>)); 
            return template;
        }
    }
}
