using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Utilities
{
    public class MemberValue
    {
        FieldInfo field;
        PropertyInfo property;

        public bool IsProperty
        { 
            get; 
            private set; 
        }

        public bool IsField
        {
            get;
            private set;
        }
        
        public string Name
        {
            get
            {
                if(IsField)
                    return field.Name;

                if(IsProperty) 
                    return property.Name;

                return "";
            }
        }

        public MemberValue(FieldInfo field)
        { 
            this.field = field;
            IsField = true;
        }

        public MemberValue(PropertyInfo property) 
        {
            this.property = property;
            IsProperty = true;
        }

        public object GetValue(object owner) 
        {
            if(IsField)
                return field.GetValue(owner);

            if(IsProperty)
                return property.GetValue(owner);

            return null;
        }

        public void SetValue(object owner, object value) 
        {
            if(IsField)
                field.SetValue(owner, value);   

            if(IsProperty)
                property.SetValue(owner, value);
        }
    }

    public static class MemberValueExtension
    {
        public static MemberValue ToMemberValue(this FieldInfo info) => new MemberValue(info);
        public static MemberValue ToMemberValue(this PropertyInfo info) => new MemberValue(info);
    }
}
