using System.Numerics;

namespace DavinLib
{
    public static class Numerics
    {
        /// <summary>
        /// Returns the number of bits used to represent the specified binary integer type.
        /// </summary>
        /// <remarks>Supported types include <see langword="byte"/>, <see langword="sbyte"/>, <see
        /// langword="short"/>, <see langword="ushort"/>, <see langword="int"/>, <see langword="uint"/>, <see
        /// langword="long"/>, <see langword="ulong"/>, <see cref="Int128"/>, and <see cref="UInt128"/>. For other
        /// types, a <see cref="NotSupportedException"/> is thrown.</remarks>
        /// <typeparam name="T">The binary integer type for which to determine the bit size. Must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
        /// <returns>The number of bits used to represent values of type <typeparamref name="T"/>.</returns>
        public static int BitSize<T>() where T : IBinaryInteger<T>, IMinMaxValue<T>
        {
            // sizeof(T) is not allowed for generic managed types.
            // Instead, use type checks for known types, or use slower code.
            // Cannot use TypeCode as it does not support Int128 or UInt128.
            var type = typeof(T);
            if (type == typeof(byte)) return 8;
            if (type == typeof(sbyte)) return 8;
            if (type == typeof(short)) return 16;
            if (type == typeof(ushort)) return 16;
            if (type == typeof(int)) return 32;
            if (type == typeof(uint)) return 32;
            if (type == typeof(long)) return 64;
            if (type == typeof(ulong)) return 64;
            if (type == typeof(Int128)) return 128;
            if (type == typeof(UInt128)) return 128;
            // Must use slower method.
            int bitLength = 1 + int.CreateChecked(T.Log2(T.MaxValue));
            if (T.MinValue < T.Zero)
            {
                bitLength++;
            }
            return bitLength;
        }

        /// <summary>
        /// Returns the upper and lower 64-bit parts of the given 128-bit unsigned integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static (ulong Upper, ulong Lower) DecomposeUpperLower(this UInt128 value)
        {
            ulong lower = (ulong)(value & 0xFFFFFFFFFFFFFFFF);
            ulong upper = (ulong)(value >> 64);
            return (upper, lower);
        }

        /// <summary>
        /// Returns the upper and lower 64-bit parts of the given 128-bit signed integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static (ulong Upper, ulong Lower) DecomposeUpperLower(this Int128 value)
        {
            ulong lower = (ulong)(value & 0xFFFFFFFFFFFFFFFF);
            ulong upper = (ulong)(value >> 64);
            return (upper, lower);
        }

        /// <summary>
        /// Returns the nearest integer root, i.e. the largest integer value less than or equal to the theoretical square root of the input value.
        /// It does not use <see cref="Math.Sqrt(double)"/> to avoid precision issues with 128-bit integers."/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">A non-negative binary integer.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T NearestIntegerRoot<T>(T value) where T : IBinaryInteger<T> 
        {
            if (value < T.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Cannot compute square root of a negative number.");
            }
            if (value <= T.One)
            {
                // i.e. 0 returns 0, and 1 returns 1.
                return value;
            }

            (T left, T right) = GetInitialRootRange<T>(value);
            T result = T.One;

            while (left <= right)
            {
                // Mid calc is awakward looking but will SAFELY be properly less than MaxValue
                // and not wrap around to MinValue.
                T mid = left + ((right - left) >> 1);
                T midSquared = mid * mid;

                if (midSquared == value)
                {
                    return mid;
                }

                // EDGE CASE WARNING: wrap around occured when exceeding MaxValue. 
                bool adjustRight = (midSquared > value) || (midSquared <= mid);

                if (adjustRight)
                {
                    right = mid - T.One;
                }
                else
                {
                    left = mid + T.One;
                    result = mid;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets an initial range for the possible root.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static (T left, T right) GetInitialRootRange<T>(T value) where T : IBinaryInteger<T> 
        {
            int bits = 1 + int.CreateChecked(T.Log2(value));
            int oddOffset = int.IsEvenInteger(bits) ? 0 : 1;
            int halfBits = bits >> 1; // Aka divide by 2;
            T left = T.One << (halfBits - 1 + oddOffset);
            return (left, (T.One << (halfBits + oddOffset)) - T.One);
        }
    }
}