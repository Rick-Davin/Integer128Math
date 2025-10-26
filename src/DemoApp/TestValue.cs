using System.Numerics;

namespace DemoApp
{
    public class TestValue<T> where T : IBinaryInteger<T>, IFormattable
    {
        public static TestValue<T> Create(T value) 
        {
            TestRoot<T> mathSqrt = TestRoot<T>.Create(value, useMathSqrt: true);
            TestRoot<T> nearestRoot = TestRoot<T>.Create(value, useMathSqrt: false);
            return new TestValue<T>(value, mathSqrt, nearestRoot);
        }

        private TestValue(T value, TestRoot<T> mathSqrt, TestRoot<T> nearestRoot)
        { 
            this.Value = value; 
            this.MathSqrt = mathSqrt;   
            this.NearestRoot = nearestRoot; 
        }

        public T Value { get; }
        public TestRoot<T> MathSqrt { get; }
        public TestRoot<T> NearestRoot { get; }

        public bool AreRootsEqual => NearestRoot.Root == MathSqrt.Root;    

        public override string ToString()
        {
            return Value.ToString("N0", null).Replace(",", "_").Replace(".", "_");
        }
    }
}
