using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


namespace pz1_c4
{

    public class One_Thread
    {
        public int[] mass = new int[12000000];
        protected int Sum = 0;
        protected double seconds;
        protected Stopwatch stopwatch = new Stopwatch();
        protected Random rand = new Random(5);

        protected void Initialization()
        {
            for (int i = 0; i < mass.Length; i++)
            {
                mass[i] = rand.Next(1, 75);
            }
        }

        protected bool IsArrayFilled()
        {
            for (int i = 0; i < mass.Length; i++)
            {
                if (mass[i] != 0)
                {
                    return true;
                }

            }

            return false;
        }


        public void PrintArray()
        {
            if (IsArrayFilled() == false) Initialization();
            else
                for (int i = 0; i < mass.Length; i++)
                {
                    Console.WriteLine(mass[i]);
                }
        }

        public virtual int GetSum()
        {
            if (IsArrayFilled() == false)
            {
                Initialization();
                stopwatch.Start();

                for (int i = 0; i < mass.Length; i++)
                {
                    Sum += mass[i];
                }

                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;
                seconds = elapsedTime.TotalMilliseconds / 1000.0;

                return Sum;
            }
            else
            {
                stopwatch.Start();

                for (int i = 0; i < mass.Length; i++)
                {
                    Sum += mass[i];
                }

                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;
                seconds = elapsedTime.TotalMilliseconds / 1000.0;

                return Sum;
            }
        }

        public double GetTime()
        {
            return seconds;
        }

    }

    public class Four_Thread : One_Thread
        {

        public override int GetSum()
        {
            if (IsArrayFilled() == false)
            {
                Initialization();
            }

            

            int chunkSize = mass.Length / 4;
            int[] sums = new int[4];
            Thread[] threads = new Thread[4];
            stopwatch.Start();
            
            for (int i = 0; i < 4; i++)
            {
                int start = i * chunkSize;
                int end = (i == 3) ? mass.Length : (i + 1) * chunkSize;
                int index = i; 
                threads[i] = new Thread(() =>
                {
                    for (int j = start; j < end; j++)
                    {
                        sums[index] += mass[j];
                    }
                });
                threads[i].Start();
            }

            for (int i = 0; i < 4; i++)
            {
                threads[i].Join();
                Sum += sums[i];
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            seconds = elapsedTime.TotalMilliseconds / 1000.0;

            return Sum;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            int test_one;          
            One_Thread one = new One_Thread();
            
            test_one = one.GetSum();

            Console.WriteLine($"Сума: {test_one}");
            Console.WriteLine($"Час виконання: {one.GetTime()} с");

            int test_four;
            Four_Thread four = new Four_Thread();

            test_four = four.GetSum();

            Console.WriteLine($"Сума: {test_four}");
            Console.WriteLine($"Час виконання: {four.GetTime()} с");

            double ratio_of_numbers = four.GetTime() / one.GetTime();
            
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"Відношення часу виконання однопотокової версії " +
                              $"алгоритму до часу виконання багатопотокової його версії: {ratio_of_numbers}");


        }
    }
}
