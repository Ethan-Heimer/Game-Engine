using GameEngine.Editor;
using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine
{
    public static class TempFileHandler
    {
        const string path = "temp";
        public static void Init()
        {
            CreateDirectory();
            EngineEventManager.AddEventListener<OnEngineExitEvent>(e => DeleteDirectory());
        }

        public static void Serialize(object obj, string name)
        {
            BinarySeriailizer serializer = new BinarySeriailizer();
            serializer.Serialize(obj, $"{path}/{name}");
        }

        public static object Deserialize(string name)
        {
            if (!File.Exists($"{path}/{name}"))
                return null;

            BinarySeriailizer serializer = new BinarySeriailizer();
            object obj = serializer.Deserialize($"{path}/{name}");

            File.Delete($"{path}/{name}");

            return obj;
        }

        static void CreateDirectory()
        {
            if(!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
        }

        static void DeleteDirectory()
        {
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

    }
}
