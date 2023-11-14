using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Events
{
    public interface IEventArgs
    {
        object Sender
        {
            get; set;  
        }
    }
}
