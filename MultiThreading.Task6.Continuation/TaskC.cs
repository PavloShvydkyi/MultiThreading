using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    static class TaskC
    {
        public static void Execute()
        {
            Console.WriteLine("Task С");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");

            Console.WriteLine();

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Task cancelation = new Task(() => Cancell(tcs));
            Task parent = tcs.Task;
            Task continuation = parent.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
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
