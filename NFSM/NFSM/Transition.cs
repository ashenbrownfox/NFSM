using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSM
{
    public class Transition
    {
        public string StartState { get; private set; }
        public char Symbol { get; private set; }
        public string EndState { get; private set; }

        /**
         * Basic Constructor initializes the StartState, endState and symbol
         * **/
        public Transition(string startState, char symbol, string endState)
        {
            StartState = startState;
            Symbol = symbol;
            EndState = endState;
        }

        /**
         * This method will make it easier later on for printing out the states.
         * **/
        public override string ToString()
        {
            return string.Format("({0}, {1}) -> {2}", StartState, Symbol, EndState);
        }
    }
}
