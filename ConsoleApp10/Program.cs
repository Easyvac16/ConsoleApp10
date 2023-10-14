namespace ConsoleApp10
{
    internal class Program
    {
        static int[] numbers = new int[10000];
        static int min = int.MaxValue;
        static int max = int.MinValue;
        static double average = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("---------Task1---------");

            Console.WriteLine("Введіть початок діапазону чисел: ");
            int start = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть кінець діапазону чисел: ");
            int end = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть кількість потоків: ");
            int numThreads = int.Parse(Console.ReadLine());

            List<Thread> threads = new();

            int numbersPerThread = (end - start + 1) / numThreads;

            for (int i = 0; i < numThreads; i++)
            {
                int threadStart = start + i * numbersPerThread;
                int threadEnd = Math.Min(threadStart + numbersPerThread - 1, end);

                Thread displayThread = new(() => DisplayNumbers(threadStart, threadEnd));
                threads.Add(displayThread);
                displayThread.Start();
                Console.WriteLine($"Thread{i}");
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }


            Console.WriteLine("---------Task4---------");
            Random random = new Random();
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(1, 1001);
            }

            Thread minThread = new Thread(FindMin);
            Thread maxThread = new Thread(FindMax);
            Thread avgThread = new Thread(CalculateAverage);
            Thread fileWriter = new Thread(WriteToFile);

            minThread.Start();
            maxThread.Start();
            avgThread.Start();
            fileWriter.Start();

            minThread.Join();
            maxThread.Join();
            avgThread.Join();
            fileWriter.Join();

            Console.WriteLine($"Мінімум: {min}");
            Console.WriteLine($"Максимум: {max}");
            Console.WriteLine($"Середнє арифметичне: {average}");

        }

        static void DisplayNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                Console.WriteLine(i);
            }
        }

        static void FindMin()
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < min)
                {
                    min = numbers[i];
                }
            }
        }

        static void FindMax()
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > max)
                {
                    max = numbers[i];
                }
            }
        }

        static void CalculateAverage()
        {
            int sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            average = sum / numbers.Length;
        }
        static void WriteToFile()
        {
            using (StreamWriter writer = new StreamWriter("results.txt"))
            {
                writer.WriteLine("Набір чисел:");
                foreach (int num in numbers)
                {
                    writer.WriteLine(num);
                }

                writer.WriteLine($"Мінімум: {min}");
                writer.WriteLine($"Максимум: {max}");
                writer.WriteLine($"Середнє арифметичне: {average}");
            }
        }
    }
}