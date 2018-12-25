using System;
using System.Collections.Generic;
using System.IO;

namespace EllipticCurveGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] numbers;
            Dictionary<int, List<int>> primeNumbers = new Dictionary<int, List<int>>();

            using (StreamReader str = new StreamReader("PrimeNumbers.txt"))
            {
                string text = str.ReadToEnd();
                numbers = text.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                primeNumbers = GetPrimeNumbersDictionary(numbers);
            }

            Console.Write("Введите длину l бит простого числа р: ");
            int lengthOfPrime = int.Parse(Console.ReadLine());
            EllipticCurveGenerator generator = new EllipticCurveGenerator(primeNumbers[lengthOfPrime], numbers, lengthOfPrime, 5);
            EllipticCurve curve = generator.GenerateEllipticCurve();
            Console.WriteLine("Построена эллиптическая кривая со следующими параметрами.");
            Console.WriteLine("p = {0}", curve.p);
            Console.WriteLine("A = {0}", curve.A);
            Console.WriteLine("Точка (x0, y0) = ({0}, {1})", curve.Q.X, curve.Q.Y);
            Console.WriteLine("r = {0}", curve.r);
            Console.ReadKey();

        }

               
        private static Dictionary<int, List<int>> GetPrimeNumbersDictionary(string[] numbers)
        {
            Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
            foreach (string number in numbers)
            {
                int binaryNumber = int.Parse(number);
                int numberLength = GetLength(binaryNumber);
                if (!dictionary.ContainsKey(numberLength))
                    dictionary.Add(numberLength, new List<int>());

                dictionary[numberLength].Add(binaryNumber);
            }
            return dictionary;
        }

        private static int GetLength(int number)
        {
            string binaryNumber = Convert.ToString(number, 2);
            return binaryNumber.Length;
        }


        
    }
}
