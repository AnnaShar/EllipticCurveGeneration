
namespace EllipticCurveGeneration
{
    public class EllipticCurve
    {
        public int p { get; private set; }
        public int A { get; private set; }
        public Point Q { get; private set; }
        public int r { get; private set; }

        public EllipticCurve(int p, int A, Point point, int r)
        {
            this.p = p;
            this.A = A;
            Q = point;
            this.r = r;
        }

    }
}
