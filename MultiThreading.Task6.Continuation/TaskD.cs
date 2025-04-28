using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    static class TaskD
    {
        public static void Execute()
        {
            Console.WriteLine("Task D");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var parent = tcs.Task;
            var continuation = parent.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnCanceled);
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
            var thread = new Thread(() =>
            {
                Console.WriteLine($"Continuation task started on thread {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("Parent task was canceled.");
            });

            thread.Start(); 
            thread.Join();

        }


        private static void Cancell(TaskCompletionSource<bool> tcs)
        {
            Thread.Sleep(250);
            var rnd = new Random();
            if (rnd.Next(1, 10) > 2)
            {
                Console.WriteLine("Randomize fail");
                tcs.SetCanceled();
            }
            else
            {
                Console.WriteLine("Randomize success");
                tcs.SetResult(true);
            }
        }
    }
}
