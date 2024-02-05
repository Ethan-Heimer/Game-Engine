using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Utilities
{
    internal static class ReflectionExtention
    {
        public static Type GetRootType(this Type type)
        {
            Type root = type;

            while (true)
            {
                if (root.BaseType == typeof(object) || root.BaseType == null)
                    break;

                root = root.BaseType;
            }

            return root;
        }
        
    }
}
