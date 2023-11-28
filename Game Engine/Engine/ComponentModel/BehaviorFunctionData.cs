using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace GameEngine.ComponentManagement
{
    public static class BehaviorFunctions
    {
        static BehaviorFunctionList _functions;
        static BehaviorFunctionList Functions
        {
            get
            {
                if (_functions == null)
                {
                    string data = File.ReadAllText("..\\..\\engine\\componentModel\\BehaviorFunctions.json");
                    _functions = JsonSerializer.Deserialize<BehaviorFunctionList>(data);
                }

                return _functions;
            }
        }

        public static BehaviorFunction[] functionTypes
        {
            get { return Functions.behaviorFunctions.ToArray(); }
        }
    }
    public class BehaviorFunctionList
    {
        public IList<BehaviorFunction> behaviorFunctions { get; set; }
    }
    public class BehaviorFunction
    {
        public string FunctionName { get; set; }
        public IList<ParamData> ParamTypes { get; set; }
    }

    public class ParamData
    {
        public string Type { get; set; }
        public string Assembly { get; set; }
    }

}
