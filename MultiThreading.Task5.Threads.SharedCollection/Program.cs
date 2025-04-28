/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> resource = new List<int>();
        static EventWaitHandle printerWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        static EventWaitHandle producerWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        static EventWaitHandle exitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);



        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var produserThread = new Thread(Producer);
            var printThread = new Thread(Print);
            produserThread.Start();
            printThread.Start();
            produserThread.Join();
            printThread.Join();

            Console.ReadLine();
        }

        private static void Producer()
        {
            for (int i = 0; i < 10; i++)
            {
                lock (resource) {
                    resource.Add(i);                    
                }
                printerWaitHandle.Set();
                producerWaitHandle.WaitOne();
            }
            exitHandle.Set();

        }

        private static void Print()
        {
            int indexOfWait = WaitHandle.WaitAny(new WaitHandle[] { printerWaitHandle, exitHandle });
            if (indexOfWait == 1)
            {
                return;
            }
            lock (resource)
            {
                Console.WriteLine($"Current value: { String.Join(", ", resource) }");
            }
            producerWaitHandle.Set();
            Print();
        }
    }
}
