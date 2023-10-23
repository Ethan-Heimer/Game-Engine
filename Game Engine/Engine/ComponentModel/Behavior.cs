using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Behavior : ICloneable
    {
        public GameObject gameObject;
        public Transform transform
        {
            get
            {
                return gameObject.Transform;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
