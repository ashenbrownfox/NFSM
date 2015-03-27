/*
 * Ailun Shen 
 * CS5800
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSM
{
    public class Program
    {
        public static UserUtility UI = new UserUtility();
        public static void Main(string[] args)
        {
            Console.WriteLine("Initial Repository cloned!");
            //string path = ".//..//..//..//"; 
            Console.WriteLine("Hello World!");
            String[] options = { "1) Read DFSM", "2) Read Input Strings", "3) Don't do anything", "4) Exit" };
            Boolean repeat = true;
            string line_buffer;
            //string Start_State = ""; int num_states = 0, num_alphabet = 0, num_accepting_states = 0, num_transitions = 0;
            string[] arraybuffer = new string[1000];
            FSM machine = null;//it's null until you load a DFSM from text file
            while (repeat)
            {
                Console.WriteLine("Please select an option.");
                for (int i = 0; i < options.Length; i++) { Console.WriteLine(options[i]); }
                line_buffer = Console.ReadLine();
                if (line_buffer.StartsWith("1"))
                {
                    Console.WriteLine("You have chosen option 1. ");
                    machine = loadDFSM();
                }
                else if (line_buffer.StartsWith("2"))
                {
                    Console.WriteLine("You have chosen option 2.");
                    Console.WriteLine("Please enter the name of the input file(default is input.txt):");
                    simulate("input.txt", machine);
                }
                else if (line_buffer.StartsWith("3"))
                {
                    Console.WriteLine("You have chosen option 3.");
                    Console.WriteLine("Please enter the name of input file:");
                    //minimize(machine);
                    //Console.WriteLine("Error, unable to minimize. Something went wrong.");
                }
                else
                {
                    repeat = false;
                    Console.WriteLine("Thank you. Now exiting the program.");
                }
            }
            UI.Write("Done. Press any key to continue...");
        }

        public static FSM loadDFSM()
        {
            string path = ".//..//..//..//";
            string line_buffer; string Start_State = "";
            int num_states = 0, num_alphabet = 0, num_accepting_states = 0, num_transitions = 0;
            string[] arraybuffer = new string[1000];
            /*************** reads and processes the DFSM formet file ****************/
            #region
            Console.WriteLine("Please type the name of the states file(default is state.txt):");
            line_buffer = Console.ReadLine();
            FileStream fs = new FileStream(path + line_buffer, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);


            //reads the numbers
            try
            {
                line_buffer = sr.ReadLine();
                num_states = int.Parse(line_buffer);
                line_buffer = sr.ReadLine();
                num_alphabet = int.Parse(line_buffer);
                line_buffer = sr.ReadLine();
                num_accepting_states = int.Parse(line_buffer);
                line_buffer = sr.ReadLine();
                num_transitions = int.Parse(line_buffer);
            }
            catch (Exception ex)
            {
                UI.FailMessage("Error, incorrect state input file.");
            }

            string[] Accepting_States = new string[num_accepting_states];
            string[,] Finite_State_Array = new string[num_states, num_alphabet];
            char[] Alphabet_Array = new char[num_alphabet];
            Start_State = sr.ReadLine();
            line_buffer = sr.ReadLine();

            string[] States_Array = line_buffer.Split(' ');
            line_buffer = sr.ReadLine();
            arraybuffer = line_buffer.Split(' ');
            for (int i = 0; i < arraybuffer.Length; i++)
            {
                Alphabet_Array[i] = arraybuffer[i][0];
            }
            line_buffer = sr.ReadLine();
            Accepting_States = line_buffer.Split(' ');
            //string start, letter, next;
            string[] start_array = new string[num_transitions];
            char[] letter_array = new char[num_transitions];
            string[] next_array = new string[num_transitions];
            for (int i = 0; i < num_transitions; i++)
            {
                line_buffer = sr.ReadLine();
                arraybuffer = line_buffer.Split(' ');
                char[] char_buffer = arraybuffer[1].ToCharArray();
                start_array[i] = arraybuffer[0]; letter_array[i] = char_buffer[0]; next_array[i] = arraybuffer[2];
            }
            //String transition_state = "new Transition(\"q0\", '0', \"q0\")";
            #endregion
            List<String> Q_States; List<char> Alpha; List<Transition> Trans_Delta;
            //States_Array
            Q_States = new List<string> { }; //states
            Alpha = new List<char> { }; //alphabets
            Trans_Delta = new List<Transition> { };
            //After Processing, Stores the Data in 3 Lists
            for (int i = 0; i < num_states; i++)
            {
                Q_States.Add(States_Array[i]);
            }
            for (int i = 0; i < num_alphabet; i++)
            {
                Alpha.Add(Alphabet_Array[i]);
            }
            for (int i = 0; i < num_transitions; i++)
            {
                Trans_Delta.Add(new Transition(start_array[i], letter_array[i], next_array[i]));
            }

            FSM dFSM = new FSM(Q_States, Alpha, Trans_Delta, Start_State, Accepting_States);
            Console.WriteLine("Ok, state machine processed.");
            return dFSM;
        }
        public static void simulate(String filename, FSM passedFSM)
        {
            string path = ".//..//..//..//"; string line_buffer;
            Console.WriteLine("Please type the name of the input file.");
            //string input_file = Console.ReadLine();

            FileStream fs_in = new FileStream(path + "input.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader streamreader = new StreamReader(fs_in);
            string[] input_buff = new string[1000];
            int j = 0;
            while (!streamreader.EndOfStream)
            {
                line_buffer = streamreader.ReadLine();
                input_buff[j] = line_buffer; j++;
            }
            for (int a = 0; a < j; a++)
            {
                passedFSM.Check(input_buff[a]);
            }
        }
    }
}
