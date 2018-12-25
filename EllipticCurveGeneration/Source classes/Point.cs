using System;

namespace EllipticCurveGeneration
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point DoublingPoint(int A)
        {
            if (Y == 0)
                throw new Exception("Denominator cannot be equal to 0.");
            int lambda = (3 * X * X + A) / 2 * Y;
            int nu = X * (A - X * X) / 2 * Y;
            int x_result = lambda * lambda - 2 * X;
            int y_result = -(lambda * x_result + nu);

            return new Point(x_result, y_result);
        }

    }
}
