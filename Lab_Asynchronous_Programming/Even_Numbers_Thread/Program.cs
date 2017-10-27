namespace Even_Numbers_Thread
{
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            int start = int.Parse(Console.ReadLine());
            int end = int.Parse(Console.ReadLine());

            Thread thread = new Thread(() =>
            {
                PrintEvenNumbersInRange(start,end);
            });

            thread.Start();
            thread.Join();

            Console.WriteLine("Thread finished work");
        }

        public static void PrintEvenNumbersInRange(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
