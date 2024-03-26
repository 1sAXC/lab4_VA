namespace lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Укажите способ ввода: ");
            string x = Console.ReadLine();
            switch (x)
            {
                case "1":
                    CreateFirst();
                    break;
                case "2":
                    CreateSecond();
                    break;
                default:
                    Console.WriteLine("Неверно указан способ");
                    return;
            }
        }

        static void CreateFirst()
        {
            Console.WriteLine("Введите количество уравнений в системе:");
            int n = int.Parse(Console.ReadLine());

            double[,] coefficients = new double[n, n];
            double[] constants = new double[n];

            Console.WriteLine("Введите коэффициенты и свободные члены:");

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Уравнение {i + 1}:");
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"Коэффициент a[{i + 1},{j + 1}]: ");
                    coefficients[i, j] = double.Parse(Console.ReadLine());
                }
                Console.Write($"Свободный член b[{i + 1}]: ");
                constants[i] = double.Parse(Console.ReadLine());
            }

            if (Check(coefficients, constants))
            {
                Yakobi(coefficients, constants);
            }
            else
            {
                Console.WriteLine("Система не сходится или имеет несоклько решений");
                return;
            }
        }

        static void CreateSecond()
        {
            Console.WriteLine("Введите количество уравнений в системе:");
            int n = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите интервалы для случайных чисел (a и b):");
            Console.Write("a: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("b: ");
            double b = double.Parse(Console.ReadLine());

            Random random = new Random();
            double[,] coefficients = new double[n, n];
            double[] constants = new double[n];

            Console.WriteLine("Случайно сгенерированная система:");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    coefficients[i, j] = random.NextDouble() * (b - a) + a;
                    Console.WriteLine($"a[{i + 1},{j + 1}] = {coefficients[i, j]}");
                }
                constants[i] = random.NextDouble() * (b - a) + a;
                Console.WriteLine($"b[{i + 1}] = {constants[i]}");
            }

            if (Check(coefficients, constants))
            {
                Yakobi(coefficients, constants);
            }
            else
            {
                Console.WriteLine("Система не сходится или имеет несоклько решений");
                return;
            }
            
        }

        static bool Check(double[,] coefficients, double[] constats )
        {
            double Aij = 0;
            double Aii = 0;
            for( int i = 1; i < constats.Length; i++)
            {
                for (int j = 1; j < constats.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    Aij += coefficients[i, j];
                    Aii += coefficients[i, i];
                }
            }
            return Math.Abs(Aij) < Math.Abs(Aii);
        }

        static double[] Yakobi(double[,] coefficients, double[] constants)
        {
            double eps = 0.0000001;
            double[] results0 = new double[constants.Length];
            double[] results1 = new double[constants.Length];
            double[] results2 = new double[constants.Length];
            for (int i = 0; i < constants.Length; i++)
            {
                results1[i] = constants[i];
            }
            int index = 0;
            while (Math.Abs(Max(results1, results0)) > eps)
            {
                if (index != 0)
                {
                    Print(index, results1);
                }
                for (int i = 0; i < results1.Length; i++)
                {
                    results0[i] = results1[i];
                }
                for (int i = 0; i < constants.Length; i++)
                {
                    results1[i] = 1 / coefficients[i, i] * Calculate(coefficients, constants, results0, i);
                }
                index++;

            }
            return results1;            
        }

        static double Max(double[] arr1, double[] arr2)
        {
            double max = Math.Abs(arr1[0] - arr2[0]);
            for (int i = 1; i < arr1.Length; i++)
            {
                if (Math.Abs((arr1[i] - arr2[i])) > max)
                {
                    max = Math.Abs((arr1[i] - arr2[i]));
                }
            }
            return max;
        }

        static double Calculate(double[,] coefficients, double[] constants, double[] results0, int j)
        {
            double x = constants[j];
            for (int i = 0; i < constants.Length; i++)
            {
                if (j == i)
                {
                    continue;
                }
                x -= coefficients[j, i] * results0[i];
            }
            return x;
        }

        static void Print(int n, double[] results)
        {
            Console.WriteLine($"Шаг {n}:");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"Элемент {i}: {results[i]}");
            }
        }        
    }
}

