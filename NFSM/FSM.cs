using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSM
{
    public class FSM
    {
          private readonly List<string> States = new List<string>();
          private readonly List<char> Alphabet = new List<char>();
          private readonly List<Transition> Transitions = new List<Transition>();
          private string Start_State;
          private readonly List<string> F = new List<string>();
          private List<Transition> Espilons = new List<Transition>();
          public UserUtility UI;

          public FSM(IEnumerable<string> q, IEnumerable<char> sigma,
             IEnumerable<Transition> delta, string q0, IEnumerable<string> f){
             States = q.ToList();
             Alphabet = sigma.ToList();
             AddTransitions(delta);
             AddInitialState(q0);
             AddFinalStates(f);
             AddEspilonTrans(delta);
             UI = new UserUtility();
          }

          private void AddTransitions(IEnumerable<Transition> transitions){
             /*
              foreach (var transition in transitions.Where(ValidTransition)){
                Transitions.Add(transition);
             } */
             foreach (Transition transition in transitions)
             {
                 Transitions.Add(transition);
             }
          }

          public void AddEspilonTrans(IEnumerable<Transition> transitions)
          {
              Espilons = Transitions.FindAll(e => e.Symbol == 'E');
              /*
              foreach (Transition transition in transitions)
              {
                  Espilons.Add(transition);
              }*/
          }
          private bool InputContainsNotDefinedSymbols(string input)
          {
              foreach (var symbol in input.ToCharArray().Where(symbol => !Alphabet.Contains(symbol)))
              {
                  UI.FailMessage("REJECTED, because " + symbol + " is not part of the alphabet");
                  return true;
              }
              return false;
          }
          private bool ValidTransition(Transition transition){
             return States.Contains(transition.StartState) &&
                    States.Contains(transition.EndState) &&
                    Alphabet.Contains(transition.Symbol);
          }

          private void AddInitialState(string q0){
             if (q0 != null && States.Contains(q0)){
                Start_State = q0;
             }
          }

          private void AddFinalStates(IEnumerable<string> finalStates){
             foreach (var finalState in finalStates.Where(finalState => States.Contains(finalState))){
                F.Add(finalState);
             }
          }

          public void Accepts(string input){
             UI.Write("Ok Testing " + input);

             if (Accepts(Start_State, input, new StringBuilder())){
                return;
             }
             UI.FailMessage("REJECT!");
          }

          private bool Accepts(string currentState, string input, StringBuilder steps){
              List<string> Current_States = new List<string>();
             if (input.Length > 0){
                var transitions = GetAllTransitions(currentState, input[0]);
                if (Espilons.Count > 0)
                {
                    foreach (var et in Espilons)
                    {
                        Current_States.Add(et.StartState);
                    }
                    foreach (string s in Current_States)
                    {
                        foreach (Transition transition in transitions)
                        {
                            StringBuilder currentSteps = new StringBuilder(steps.ToString() + transition);
                            if (Accepts(transition.EndState, input.Substring(1), currentSteps))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                else
                {
                    foreach (Transition transition in transitions)
                    {
                        StringBuilder currentSteps = new StringBuilder(steps.ToString() + transition);
                        if (Accepts(transition.EndState, input.Substring(1), currentSteps))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                
             }
             if (F.Contains(currentState)){
                 UI.SuccessMessage("ACCEPTED!");
                //UI.SuccessMessage("Successfully accepted the input " + input + " " +
                                     // "in the final state " + currentState +
                                     // " with steps:\n" + steps);
                return true;
             }
             return false;
          }

          private IEnumerable<Transition> GetAllTransitions(string currentState, char symbol){
             return Transitions.FindAll(t => t.StartState == currentState &&
                                       t.Symbol == symbol);
          }
    }
}
