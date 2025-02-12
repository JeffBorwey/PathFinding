﻿using Random.Interface;
using System;
using System.Runtime.CompilerServices;
using ValueRange;
using ValueRange.Extensions;

namespace Random.Realizations.Generators
{
    /// <summary>
    /// Linear congruential generator
    /// </summary>
    /// <remarks>See Numeric resipes in C, second edition</remarks>
    public sealed class PseudoRandom : IRandom
    {
        private const int Term = 12345;
        private const int Factor = 1103515245;

        private ulong seed;
        private ulong Seed => seed = seed * Factor + Term;

        public PseudoRandom(int seed)
        {
            this.seed = (ulong)seed;
        }

        /// <summary>
        /// Initializes new instance of <see cref="PseudoRandom"/>
        /// using <see cref="Environment.TickCount"/> as seed
        /// </summary>
        public PseudoRandom() : this(Environment.TickCount)
        {

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Next(int minValue, int maxValue)
        {
            var range = new InclusiveValueRange<int>(maxValue, minValue);
            ulong module = (ulong)range.Amplitude() + 1;
            return (int)(Seed % module) + range.LowerValueOfRange;
        }
    }
}