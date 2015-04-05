using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSM
{
    public class FSM
    {
        /** Fields **/
        private readonly List<string> States = new List<string>();
        private readonly List<char> Alphabet = new List<char>();
        private readonly List<Transition> Transitions = new List<Transition>();
        private string Starting_State;
        private readonly List<string> Final_States = new List<string>();
        public UserUtility UI;
        private List<string> Group_A = new List<string>();
        private List<string> Group_B = new List<string>();

        /**
         * 
         * The constructor initializes the 4 main variables(colllections) as well as the User Utility;
         * Basically just copied and paste the states and alphabet
         * Setting up FSM methods
         * @param IEnumerable<string> q, IEnumerable<char> sigma, IEnumerable<Transition> delta, string q0, IEnumerable<string> f
         * **/
        public FSM(IEnumerable<string> states, IEnumerable<char> sigma, IEnumerable<Transition> delta, string q0, IEnumerable<string> f)
        {
            States = states.ToList();
            Alphabet = sigma.ToList();
            AddTransitions(delta);
            AddInitialState(q0);
            AddFinalStates(f);
            UI = new UserUtility();
            UI.Write("Ok, FSM Object started.");
        }

        /**
         * Defining the initial states
         * **/
        private void AddInitialState(string q0)
        {
            if (q0 != null && States.Contains(q0))
            {
                Starting_State = q0;
            }
        }
        /**validating the final state
         * **/
        private void AddFinalStates(IEnumerable<string> finalStates)
        {
            foreach (string finalState in finalStates.Where(finalState => States.Contains(finalState)))
            {
                Final_States.Add(finalState);
            }
        }

        /**
         * Adding Transitions
         * @param IEnumerable<Transition>
         * **/
        private void AddTransitions(IEnumerable<Transition> transitions)
        {
            foreach (Transition transition in transitions)
            {
                Transitions.Add(transition);
            }
        }

        /**
         * This is the validation method
         * As for the validation of the states 
         * Check if the states are defined in the list of states
         * @param Transition
         * 
         * **/
        private bool ValidTransition(Transition transition)
        {
            return States.Contains(transition.StartState) &&
                States.Contains(transition.EndState) &&
                Alphabet.Contains(transition.Symbol);
                //!TransitionAlreadyDefined(transition);
        }

        /** If the Transition is already defined, map it out**/
        private bool TransitionAlreadyDefined(Transition transition)
        {
            return Transitions.Any(t => t.StartState == transition.StartState &&
                                t.Symbol == transition.Symbol);
        }
        
        
        /** Methods that validate the string if it's in the alphabet  **/
        /**
         * The check method is
         * used called when checking if the alphabet is in
         * @param string input
         * **/
        public void Check(string input)
        {
            UI.SuccessMessage("Ok. Checking: " + input);
            if (InvalidInputOrFSM(input)) { return; }
            Boolean acceptance; Boolean espeon = true; StringBuilder steps;
            string currentState = Starting_State;
            Transition transition;
            //Look for any espilons connected to the current state. If found, go to the next state
            do
            {
                transition = Transitions.Find(e => e.StartState == currentState && e.Symbol == 'E');
                if (transition == null) { espeon = false; }
            } while (espeon == null);
            
            if (transition != null)
            {
                currentState = transition.EndState;
            }
            steps = new StringBuilder();
            foreach (var symbol in input.ToCharArray())
            {
                transition = Transitions.Find(t => t.StartState == currentState &&
                                                    t.Symbol == symbol);
                
                if (transition == null)
                {
                    UI.FailMessage("No transitions for current state and symbol");
                    //UI.FailMessage(steps.ToString());
                    return;
                }
                currentState = transition.EndState;
                steps.Append(transition + "\n");
            }
            if (Final_States.Contains(currentState))
            {
                UI.SuccessMessage("Accepted the input with steps:\n" + steps);
                UI.SuccessMessage("Accepted!");
                return;
            }
            UI.FailMessage("Stopped in state " + currentState +
                                " which is not a final state.");
            //UI.FailMessage(steps.ToString());
            UI.FailMessage("Rejected!");
        }

        /**
         * The rejection method to be used 
         * if the state has not been found
         * therefore the alphabet is not contained
         * **/
        private bool InvalidInputOrFSM(string input)
        {
            if (InputContainsNotDefinedSymbols(input))
            {
                return true;
            }
            if (InitialStateNotSet())
            {
                UI.FailMessage("No initial state has been set");
                return true;
            }
            if (NoFinalStates())
            {
                UI.FailMessage("No final states have been set");
                return true;
            }
            return false;
        }
        /**
         * Not defined method
         * @param inputs
         * **/
        private bool InputContainsNotDefinedSymbols(string input)
        {
            foreach (var symbol in input.ToCharArray().Where(symbol => !Alphabet.Contains(symbol)))
            {
                UI.FailMessage("REJECTED, because " + symbol + " is not part of the alphabet");
                return true;
            }
            return false;
        }
        /**
         * When there is no initial state
         * return empty string
         * **/
        private bool InitialStateNotSet()
        {
            return string.IsNullOrEmpty(Starting_State);
        }
        /**
         * When there is no final state
         * **/
        private bool NoFinalStates()
        {
            return Final_States.Count == 0;
        }
    }
}
