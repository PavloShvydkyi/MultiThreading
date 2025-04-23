using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task4.Threads.Join
{
    public static class VariantB
    {
        static int recursionCount = 0;
        static int _result = 0;
        static Semaphore semaphore = new Semaphore(0, 1);


        public static void Execute()
        {
            Console.WriteLine("VARIANT B");
            Console.WriteLine();
            recursionCount = 10;

            Random rnd = new Random();
            _result = rnd.Next(1, 255);
            RunThreads(_result);

        }

        static void RunThreads(int data)
        {
            recursionCount--;
            Console.WriteLine($"recursive level {10 - recursionCount} value is {data}");
            ThreadPool.QueueUserWorkItem(decrement, data);
            semaphore.WaitOne();
            if (recursionCount > 0)
            {
                RunThreads(_result);
            }
            return;
        }

        static void decrement(object state)
        {
            int number = (int)state;
            number--;
            _result = number;
            semaphore.Release();
        }
    }
}
