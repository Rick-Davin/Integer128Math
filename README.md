# Integer128Math
Finding largest integer less than or equal to the square root of a UInt128 without using Math.Sqrt.

The solution is written in C# 8.0 in .NET.  There are 3 projects.  The primary project is the library with a Numerics class containing the NearestIntegerRoot method.  There is DemoApp project to show examples of when Math.Sqrt is incorrect and why the slower NearestIntegerRoot is correct.  There is also a BenchmarkingApp project to show the performance difference between Math.Sqrt and NearestIntegerRoot.

Though NearestIntegerRoot is written as a generic method accepting a BinaryInteger<T>, it is primarily written to work with UInt128.  Math.Sqrt is correct of UInt64, and since it is much faster, then you really should use Math.Sqrt for 64 bits or less.
