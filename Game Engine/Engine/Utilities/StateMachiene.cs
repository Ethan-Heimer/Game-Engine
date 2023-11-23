using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Utilities
{
    public class BasicStateMachiene : IStateMachiene
    {
        IState currentState;

        public void SetCurrentState(IState state)
        {
            currentState.OnLeave();
            currentState = state;
            currentState.OnEnter();
        }

        public void Tick() => currentState.WhileInState();
    }

    public interface IStateMachiene
    {
        void SetCurrentState(IState currentState);
        void Tick();
    }
}
