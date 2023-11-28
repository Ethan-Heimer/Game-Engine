using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Utilities
{
    public interface IState
    {
        void OnEnter();
        void OnLeave();
        void WhileInState();
    }
}
