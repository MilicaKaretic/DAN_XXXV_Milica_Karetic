using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XXXV_Milica_Karetic
{
    class Program
    {
        /// <summary>
        /// Valid positive int input
        /// </summary>
        /// <returns>Valid positive number</returns>
        public static int ValidPositiveNumber()
        {
            string s = Console.ReadLine();
            int Num;
            bool b = Int32.TryParse(s, out Num);
            while (!b || Num < 1)
            {
                Console.WriteLine("Invalid input. Try again: ");
                s = Console.ReadLine();
                b = Int32.TryParse(s, out Num);
            }
            return Num;
        }

        /// <summary>
        /// Valid guess number input
        /// </summary>
        /// <returns>Number from 1 to 100</returns>
        public static int ValidGuessNumber()
        {
            string s = Console.ReadLine();
            int Num;
            bool b = Int32.TryParse(s, out Num);
            while (!b || Num < 1 || Num > 100)
            {
                Console.WriteLine("Invalid input, please enter number 1-100. Try again: ");
                s = Console.ReadLine();
                b = Int32.TryParse(s, out Num);
            }
            return Num;
        }

        /// <summary>
        /// Method that executes first thread
        /// </summary>
        public static void GetNumOfUsersAndNumber()
        {
            Console.WriteLine("Please enter number of participants:");
            users = ValidPositiveNumber();
            Console.WriteLine("Please enter number to guess: [1-100]");
            guessNumber = ValidGuessNumber();

            //task that creates threads
            Task<List<Thread>> task1 = Task.Run(() =>
            {
                List<Thread> thread = new List<Thread>();

                try
                {
                    for (int i = 0; i < users; i++)
                    {
                        Thread t = new Thread(new ThreadStart(GuessNumber))
                        {
                            Name = string.Format("Participant_{0}", i + 1)
                        };
                        thread.Add(t);
                    }
                    
                }
                catch (OutOfMemoryException ex)
                {
                    Console.WriteLine("Please enter less number of perticipants.");
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                return thread;
            });
            //list of threads
            threads = task1.Result;

            Console.WriteLine("User entered number of participants\nUser created " + users + " participants\nUser entered number to guess.\n");    
        }

        /// <summary>
        /// Method that executes each thread created by task generator
        /// </summary>
        public static void GuessNumber()
        {
            string currentName = Thread.CurrentThread.Name;
            int num = 0;

            while(num != guessNumber)
            {
                num = rnd.Next(1, 101);
                lock (l)
                {
                    Thread.Sleep(100);
                    bool evenGuess = (guessNumber % 2 == 0 ? true : false);

                    Console.WriteLine(currentName + " tried to guess number. His number is " + num);

                    if ((num % 2 == 0 && evenGuess) || (num % 2 != 0 && !evenGuess))
                    {
                        Console.WriteLine(currentName + " guessed the parity of the number!\n");
                    }
                    if (num == guessNumber)
                    {
                        Console.WriteLine(currentName + " wins, requested number is " + guessNumber + "\n");
                        Console.WriteLine("Press eny key to exit...");

                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
            }           
        }

        //number of participants and number to guess
        public static int users, guessNumber;
        public static Random rnd = new Random();
        //object to lock
        public static readonly object l = new object();
        //list of threads
        public static List<Thread> threads;

        static void Main(string[] args)
        {
            Thread firstThread = new Thread(new ThreadStart(GetNumOfUsersAndNumber));
            firstThread.Start();
            firstThread.Join();

            //start created participants threads
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();               
            }
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

            Console.ReadKey();
        }
    }
}
