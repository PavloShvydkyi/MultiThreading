using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
     static class TaskA
    {
        public static void Execute() {
            Console.WriteLine("Task A");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine();
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var parent = tcs.Task;
            var continuation = parent.ContinueWith((t) => Continuation(t));
            Task cancelation = new Task(() => Cancell(tcs));
            cancelation.Start();
            try
            {
                continuation.Wait();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception is catched");
                Console.WriteLine(ex.Message);
            }

        }

        private static void Continuation(Task t)
        {
            Console.WriteLine("Status of parent {0}", t.Status);
            Console.WriteLine($"Continuation task started on thread {Environment.CurrentManagedThreadId}");
        }


        private static void Cancell(TaskCompletionSource<bool> tcs)
        {
            Thread.Sleep(250);
            var rnd = new Random();
            var randomValue = rnd.Next(1, 10);
            if (randomValue > 5)
            {
                Console.WriteLine("Randomize fail " + randomValue);
                tcs.SetCanceled();
            }
            else
            {
                Console.WriteLine("Randomize success " + randomValue);
                tcs.SetResult(true);
            }
        }
    }
}
