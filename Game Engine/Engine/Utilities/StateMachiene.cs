using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Utilities
{
    public class StateMachiene<T> : IStateMachiene<T> where T : IState
    {
        T currentState;

        public void SetCurrentState(T state)
        {
            currentState.OnLeave();
            currentState = state;
            currentState.OnEnter();
        }

        public virtual void Tick() => currentState.WhileInState();
    }

    public interface IStateMachiene<T> where T : IState
    {
        void SetCurrentState(T currentState);
        void Tick();
    }
}
