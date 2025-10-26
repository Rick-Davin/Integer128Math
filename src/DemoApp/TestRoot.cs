using System.Numerics;

namespace DemoApp
{
    public class TestRoot<T> where T : IBinaryInteger<T>, IFormattable
    {
        public static TestRoot<T> Create(T value, bool useMathSqrt) 
        {
            T root;
            if (useMathSqrt)
            {
                double dValue = double.CreateChecked(value);
                root = T.CreateChecked(Math.Sqrt(dValue))   ;
            }
            else
            {
                root = DavinLib.Numerics.NearestIntegerRoot(value);
            }

            return new TestRoot<T>(value, useMathSqrt, root, root * root);
        }

        private TestRoot(T value, bool useSqrt, T squareRoot, T squared) 
        { 
            this.Value = value;
            this.UseMathSqrt = useSqrt;
            this.Root = squareRoot;
            this.Squared = squared;
        }

        public T Value { get; } 
        public bool UseMathSqrt { get; }   
        public T Root { get; }
        public T Squared { get; }
        public string Label => UseMathSqrt ? "Math.Sqrt" : "NearestIntegerRoot";

        public bool IsGood => !IsTooBig && !IsWrappedAround;
        public bool IsTooBig => Value > T.One && Squared > Value;
        public bool IsWrappedAround => Value > T.One && Squared <= Root;

        public override string ToString()
        {
            return Label + ": " + Root.ToString("N0", null).Replace(",", "_").Replace(".", "_");
        }
    }
}
