using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ComponentManagement
{
    public class BehaviorMethodCollection : IEnumerable<Delegate>
    {
        Dictionary<Behavior, Delegate> cache = new Dictionary<Behavior, Delegate>();

        string methodName;
        ParamData[] paramTypes;
        public BehaviorMethodCollection(BehaviorFunction function)
        {
            methodName = function.FunctionName;
            paramTypes = function.ParamTypes.ToArray();
        }

        public BehaviorMethodCollection(IEnumerable<Behavior> behaviors, string _methodName)
        {
            methodName = _methodName;

            foreach (var o in behaviors)
                TryCacheBehavior(o); 
        }

        public IEnumerator<Delegate> GetEnumerator()
        {
            return new ComponentMethodEnum(cache.Values.ToArray());
        }

        public void TryCacheBehavior(Behavior behavior) 
        {
            if(TryGetMethod(behavior, out Delegate method) && !cache.ContainsKey(behavior))
                cache.Add(behavior, method);

            Console.WriteLine(behavior.GetType().Name + " cache added");
        }

        public void TryRemoveBehavior(Behavior behavior)
        {
            if (cache.ContainsKey(behavior))
                cache.Remove(behavior);
        }

        public void ClearCache()
        {
            cache.Clear();
        }

        bool TryGetMethod(Behavior behavior, out Delegate method)
        {
            method = null;

            MethodInfo possableMethod = behavior.GetType().GetMethods().FirstOrDefault(m => m.Name == methodName && HasSameParamaters(m));  
            if (possableMethod != null)
            {
                method = Delegate.CreateDelegate(new DelegateTypeFactory().CreateDelegateType(possableMethod), behavior, methodName);
            }


            return possableMethod != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool HasSameParamaters(MethodInfo method)
        {
            Type[] types = paramTypes.Select(x => Type.GetType($"{x.Type}, {x.Assembly}")).ToArray();
            Type[] @params = method.GetParameters().Select(x => x.ParameterType).ToArray();

            return Enumerable.SequenceEqual(types, @params);    
        }
    }

    public class ComponentMethodEnum : IEnumerator<Delegate> 
    {
        public Delegate[] methods;

        int position = -1;

        public ComponentMethodEnum(Delegate[] components)
        {
            this.methods = components;
        }
        public bool MoveNext()
        {
            position++;
            return (position < methods.Length);
        }
        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public Delegate Current
        {
            get
            {
                try
                {
                    return methods[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() { }
    }
}
