/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Random rnd = new();
            var step_1 = MakeStep<int, int[]>(CreateArray, rnd.Next(1,100));
            var step_2 = MakeStep <int[], int[]> (MultiplyArray, step_1.Result);
            var step_3 = MakeStep<int[], int[]>(SortArray, step_2.Result);
            var step_4 = MakeStep<int[], int>(AverageArray, step_3.Result);

            Console.ReadLine();
        }

        private static int AverageArray(int[] data)
        {
            if (data.Length == 0)
            {
                return 0;
            }

            var sum = 0;
            foreach (var item in data)
            {
                sum += item;
            }

            LogStep("Average array " , Serialize(sum / data.Length));
            return sum/data.Length;
        }
        private static int[] SortArray(int[] data)
        {
            Array.Sort(data);
            LogStep("Sort array ", Serialize(data));
            return data;
        }
        private static int[] MultiplyArray(int[] data)
        {
            Random rnd = new();
            var kof = rnd.Next(1, 255);
            var length = data.Length-1;
            for (int i = 0; i < length; i++)
            {
                data[i] = kof*data[i];
            }
            LogStep("Multiply array ", Serialize(data));
            return data;

        }
        private static int[] CreateArray(int lenth)
        {
            Random rnd = new();
            var data = new int[lenth];
            for (int i = 0; i < lenth; i++)
            {
                data[i] = rnd.Next(0, 255);
            }
            LogStep("Create array ", Serialize(data));
            return data;
        }
        private static Task<K> MakeStep<T,K>(Func<T,K> func, T input)   {
            var step = new Task<K>(() => func(input));
            step.Start();
            step.Wait();
            return step;
        }

        private static void LogStep(string name, string value) {
            Console.WriteLine($"Step {name}");
            Console.WriteLine($"Returns value : { value }");
        }

        private static string Serialize(int[] data) {
            return $"[{String.Join(",", data)}]";
        }

        private static string Serialize(int data)
        {
            return data.ToString();
        }

    }
}
