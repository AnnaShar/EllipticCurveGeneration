using System;
using System.Collections.Generic;
using System.Linq;


namespace EllipticCurveGeneration
{
    public class EllipticCurveGenerator
    {
        private string[] _allPrimeNumbers;
        private List<int> _primeNumbersWithConcreteLength;
        private int _primeNumberLength;
        private int _maxPowerExtension;
        private Random _random;

        public EllipticCurveGenerator(List<int> primeNumbersList, string[] allPrimes, int primeNumberLength, int maxPowerExtension)
        {
            _allPrimeNumbers = allPrimes;
            _primeNumbersWithConcreteLength = primeNumbersList;
            _primeNumberLength = primeNumberLength;
            _maxPowerExtension = maxPowerExtension;
            _random = new Random();
        }

        public EllipticCurve GenerateEllipticCurve()
        {
            bool isValidN = false;
            bool isNonDivisibility = false;

            int primeNumber=0;
            Factors factors;
            Coefficients coefficients = new Coefficients();

            while (!isValidN && !isNonDivisibility)
            {
                primeNumber = ChoosePrimeNumber();
                factors = Decompose(primeNumber);
                coefficients = FindValidCoefficients(factors, primeNumber);
                if (coefficients!=null)
                {
                    isValidN = true;
                    try
                    {
                        if (IsPDivisible(coefficients.r, _maxPowerExtension, primeNumber))
                            isNonDivisibility = true;
                    }
                    catch(Exception e)
                    {
                        
                    }
                    finally
                    {
                        isNonDivisibility = CheckDivisibility();
                    }
                }
            }

            bool isAValid = false;
            bool isPointValid = false;
            Point initPoint = new Point();
            int A=0;

            while(!isAValid && !isPointValid)
            {
                initPoint = GeneratePoint();
                A = GetA(initPoint, primeNumber);

                if (coefficients.IsCheckingDeduction)
                    isAValid = IsDeduction(A, coefficients.N);
                else isAValid = IsNonDeduction(A, coefficients.N);

                if (isAValid)
                {
                    for (int i=0; i<coefficients.N; i++)
                    {
                        Point currentPoint = initPoint.DoublingPoint(A);
                        if (currentPoint.Y==0)
                        {
                            isPointValid = true;
                            break;
                        }
                    }
                }
            }

            Point Q = GetQ(initPoint, coefficients.N / coefficients.r);
            EllipticCurve curve = new EllipticCurve(primeNumber, A, Q, coefficients.r);
            return curve;
        }

        private int ChoosePrimeNumber()
        {
            bool primeNumberIsFound = false;
            int index = 0;
            while (!primeNumberIsFound)
            {
                index = _random.Next(_primeNumbersWithConcreteLength.Count);
                if (_primeNumbersWithConcreteLength[index] % 4 == 1)
                    primeNumberIsFound = true;
            }
            return _primeNumbersWithConcreteLength[index];
        }

        private Factors Decompose(int primeNumber)
        {
            bool IsFound = false;

            double a = 0, b = 1;
            double A;
            int index = 0;
            var sqrtN = Math.Sqrt(primeNumber + 0.0);
            while (IsFound == false)
            {
                A = Math.Truncate(((sqrtN + a) / b));
                a = -a + A * b;
                b = Math.Round((primeNumber - a * a) / b);
                index = index + 1;
                if (a * a + b * b == primeNumber)
                    IsFound = true;
            }

            int resultA = Convert.ToInt16(a);
            int resultB = Convert.ToInt16(b);

            var result = new Factors(resultA, resultB);
            return result;
        }

        private Coefficients FindValidCoefficients(Factors primeNumberFactors, int primeNumber)
        {
            Coefficients result = new Coefficients();
            int a = primeNumberFactors.a;
            int b = primeNumberFactors.b;
            int N;
            int[] T = new int[4] { 2 * a, 2 * b, -2 * a, -2 * b };

            for (int i = 0; i < T.Length; i++)
            {
                N = primeNumber + 1 + T[i];
                if (_allPrimeNumbers.Contains((N / 2).ToString()))
                {
                    result.N = N;
                    result.r = N / 2;
                    result.IsCheckingDeduction = false;
                    break;
                }
                if (_allPrimeNumbers.Contains((N / 4).ToString()))
                {
                    result.N = N;
                    result.r = N / 4;
                    result.IsCheckingDeduction = true;
                    break;
                }
            }

            return result;
        }

        private bool IsPDivisible(int r, int m, int p)
        {
            if (r == p)
                return false;
            int i = 1;
            while (i <= m)
            {
                int divident = Convert.ToUInt16(Math.Pow(p, i) - 1);
                if (divident % r == 0)
                    return false;
                i++;
            }

            return true;
        }

        private bool CheckDivisibility()
        {
            return true;
        }
       
        private Point GeneratePoint()
        {
            int x = _random.Next(100);
            int y = _random.Next(100);
            return new Point ( x, y );
        }

        private int GetA(Point point, int modul)
        {
            int y2 = Convert.ToInt32(Math.Pow(point.Y, 2));
            int x3 = Convert.ToInt32(Math.Pow(point.X, 3));
            int xINverse = InverseNumber(point.X, modul);

            int A = (y2 - x3) * xINverse % modul;
            return A;
        }

        private int InverseNumber(int number, int modul)
        {
            int a = number;
            int b = modul;
            int x = 0;
            int d = 1;

            while (a > 0)
            {
                int q = b / a;
                int y = a;
                a = b % a;
                b = y;
                y = d;
                d = x - q * d;
                x = y;
            }

            x = x % modul;
            if (x < 0)
                x = (x + modul) % modul;
            return x;
        }

        private bool IsDeduction(int number, int modul)
        {
            int newNumber = -number;
            while (newNumber < 0)
                newNumber = newNumber + modul;
            bool IsDeduction = false;

            for (int i = 0; i <= modul; i++)
            {
                int x = i * i;
                while (x > modul)
                    x = x - modul;
                if (x == newNumber)
                {
                    IsDeduction = true;
                    break;
                }
            }
            return IsDeduction;
        }

        private bool IsNonDeduction(int number, int modul)
        {
            int newNumber = -number;
            while (newNumber < 0)
                newNumber = newNumber + modul;
            bool IsNonDeduction = true;

            for (int i = 0; i <= modul; i++)
            {
                int x = i * i;
                while (x > modul)
                    x = x - modul;
                if (x == newNumber)
                {
                    IsNonDeduction = false;
                    break;
                }
            }
            return IsNonDeduction;
        }

        private Point GetQ(Point point, int scalar)
        {
            Point Q = point;
            for (int i = 0; i < scalar; i++)
            {
                Q = Q.DoublingPoint(scalar);
            }
            return Q;
        }
    }
}
