using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace GameEngine.Engine.Utilities
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }

        public static object ToRuntimeObject(this JsonElement element, Type objectType)
        {
            Console.WriteLine(objectType.FullName);
            return typeof(JsonExtensions).GetMethod("ToObject").MakeGenericMethod(objectType).Invoke(null, new object[] { element });
        }
    }
}
