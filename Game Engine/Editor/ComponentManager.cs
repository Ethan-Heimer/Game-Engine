using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Engine.ComponentModel
{
    public static class ComponentManager
    {
        public static Type[] GetAllComponents()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(x => x.BaseType == typeof(Behavior)).ToArray();
        }
    }
}
