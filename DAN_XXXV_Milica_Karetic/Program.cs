using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XXXV_Milica_Karetic
{
    class Program
    {

        /// <summary>
        /// Valid positive int input
        /// </summary>
        /// <returns></returns>
        public static int ValidPositiveNumber()
        {
            string s = Console.ReadLine();
            int Num;
            bool b = Int32.TryParse(s, out Num);
            while (!b || Num < 0)
            {
                Console.WriteLine("Invalid input. Try again: ");
                s = Console.ReadLine();
                b = Int32.TryParse(s, out Num);
            }
            return Num;
        }

        /// <summary>
        /// Valid positive int input
        /// </summary>
        /// <returns></returns>
        public static int ValidGuessNumber()
        {
            string s = Console.ReadLine();
            int Num;
            bool b = Int32.TryParse(s, out Num);
            while (Num < 1 && Num > 100)
            {
                Console.WriteLine("Invalid input, please enter number 1-100. Try again: ");
                s = Console.ReadLine();
                b = Int32.TryParse(s, out Num);
            }
            return Num;
        }

        public static void GetNumOfUsersAndNumber()
        {
            Console.WriteLine("Please enter number of participants:");
            users = ValidPositiveNumber();
            Console.WriteLine("Please enter number to guess:");
            guessNumber = ValidGuessNumber();

            Thread.Sleep(1);

            Console.WriteLine("User entered number of participants\nUser created " + users + " participants\nUser entered number to guess.\n");
      
        }

        public static int users, guessNumber;
        public static bool guessed = false;
        public static Random rnd = new Random();
        private static object l = new object();

        public static void GuessNumber()
        {
            Thread.Sleep(100);

            string currentName = Thread.CurrentThread.Name;
            int num = 0;

            while(num != guessNumber)
            {
                num = rnd.Next(1, 101);
                lock (l)
                {
                    Thread.Sleep(100);
                    bool evenGuess = (guessNumber % 2 == 0 ? true : false);

                    Console.WriteLine(currentName + " tried to guess number. " + num);
                    if ((num % 2 == 0 && evenGuess) || (num % 2 != 0 && !evenGuess))
                    {
                        Console.WriteLine(currentName + " guessed the parity of the number!\n");
                    }
                    if (num == guessNumber)
                    {
                        Console.WriteLine(currentName + " wins, requested number is " + guessNumber + "\n");

                        Console.WriteLine("Press eny key to exit");

                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
            }
            
        }

        static void Main(string[] args)
        {
            Thread firstThread = new Thread(new ThreadStart(GetNumOfUsersAndNumber));
            firstThread.Start();
            firstThread.Join();

            Task<List<Thread>> task1 = Task.Run(() =>
            {
                List<Thread> thread = new List<Thread>();

                for (int i = 0; i < users; i++)
                {
                    Thread t = new Thread(new ThreadStart(GuessNumber))
                    {
                        Name = string.Format("Participant_{0}", i + 1)
                    };
                    thread.Add(t);
                }

                return thread;
            });

            List<Thread> threads = task1.Result;

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
