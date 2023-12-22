using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ComponentManagement
{
    public class BehaviorFunctionExecuter : DynamicObject
    {
        static dynamic instance;
        public static dynamic Execute 
        {
            get
            {
                if(instance == null)
                    instance = new BehaviorFunctionExecuter();

                return instance;
            } 
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            foreach (var func in BehaviorFunctions.functionTypes) 
            {
                if(binder.Name == func.FunctionName )
                {
                    foreach (var o in ComponentCacheManager.GetCache(func.FunctionName)) 
                    {
                        o.DynamicInvoke(args);
                    }

                    result = null;
                    return true;
                }
                else if(binder.Name == func.FunctionName + "InEditor")
                {
                    foreach (var o in ComponentCacheManager.GetCache(func.FunctionName + "InEditor")) 
                    {
                        o.DynamicInvoke(args);
                    }

                    result = null;
                    return true;
                }
            }

            return false;
        }
    }
}
