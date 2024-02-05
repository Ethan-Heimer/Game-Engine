using GameEngine.Engine.Utilities;
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
            return Assembly.GetCallingAssembly().GetTypes().Where(x => x.GetRootType() == typeof(Behavior) && !x.IsAbstract).ToArray();
        }
    }
}
