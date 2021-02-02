﻿using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Represents range of values (inclusively)
    /// </summary>
    public class ValueRange
    {
        public int UpperValueOfRange { get; }
        public int LowerValueOfRange { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ValueRange"/> 
        /// with <paramref name="upperValueOfRange"/> and 
        /// <paramref name="lowerValueOfRange"/> as extreme values of the range
        /// </summary>
        /// <param name="upperValueOfRange"></param>
        /// <param name="lowerValueOfRange"></param>
        /// <remarks>If <paramref name="upperValueOfRange"/> 
        /// is less than <paramref name="lowerValueOfRange"/>
        /// then this parameters will be swapped</remarks>
        public ValueRange(int upperValueOfRange, int lowerValueOfRange)
        {
            SwapIfLess(ref upperValueOfRange,
                ref lowerValueOfRange, Comparer<int>.Default);

            UpperValueOfRange = upperValueOfRange;
            LowerValueOfRange = lowerValueOfRange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A new value, equals to upper value of 
        /// range if <paramref name="value"/> is greater 
        /// than upper value of range, or a new value, equals to lower 
        /// value of range if <paramref name="value"/> 
        /// is lower than lower value of range, or the same value
        /// if <paramref name="value"/> is in range of value</returns>
        public int ReturnInRange(int value)
        {
            if (value > UpperValueOfRange)
            {
                value = LowerValueOfRange;
            }
            else if (value < LowerValueOfRange)
            {
                value = UpperValueOfRange;
            }

            return value;
        }

        /// <summary>
        /// Checks whether values is in range of value inclusively
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if <paramref name="value"/> 
        /// is in range inclusively and false if not</returns>
        public bool IsInRange(int value)
        {
            return value <= UpperValueOfRange && value >= LowerValueOfRange;
        }

        /// <summary>
        /// Swaps values if <paramref name="greaterValue"/> 
        /// is lower than <paramref name="lowerValue"/> 
        /// according to <paramref name="comparer"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="greaterValue"></param>
        /// <param name="lowerValue"></param>
        /// <param name="comparer"></param>
        public static void SwapIfLess<TSource>(ref TSource greaterValue,
            ref TSource lowerValue, IComparer<TSource> comparer)
        {
            if (comparer.Compare(greaterValue, lowerValue) < 0)
            {
                var temp = greaterValue;
                greaterValue = lowerValue;
                lowerValue = temp;
            }
        }
    }
}