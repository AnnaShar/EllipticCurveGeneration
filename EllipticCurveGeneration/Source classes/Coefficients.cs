namespace EllipticCurveGeneration
{
    public class Coefficients
    {
        public int N { get;  set; }
        public int r { get;  set; }
        public bool IsCheckingDeduction { get;  set; }

        public Coefficients() { }
        public Coefficients(int N, int r)
        {
            this.N = N;
            this.r = r;
        }

    }
}
