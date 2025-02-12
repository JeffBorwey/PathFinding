﻿using Random.Interface;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using ValueRange;
using ValueRange.Extensions;

namespace Random.Realizations.Generators
{
    /// <summary>
    /// A random number generator 
    /// based on <see cref="RandomNumberGenerator"/>
    /// </summary>
    /// <remarks>See <see cref="https://docs.microsoft.com/en-us/archive/msdn-magazine/2007/september/net-matters-tales-from-the-cryptorandom"/></remarks>
    public sealed class CryptoRandom : IRandom, IDisposable
    {
        private const int IntSize = sizeof(int);
        private const int MaxBufferSize = IntSize << 4;

        private uint Seed
        {
            get
            {
                uint number = BitConverter.ToUInt32(buffer, currentBufferPosition);
                currentBufferPosition += IntSize;
                VerifyBuffer();
                return number;
            }
        }

        public CryptoRandom()
        {
            generator = RandomNumberGenerator.Create();
            buffer = new byte[MaxBufferSize];
            currentBufferPosition = 0;
            generator.GetBytes(buffer);
        }

        /// <summary>
        /// Creates new random <see cref="int"/> value
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns>Cryptographically strong 
        /// random <see cref="int"/></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Next(int minValue, int maxValue)
        {
            var range = new InclusiveValueRange<int>(maxValue, minValue);
            long module = (long)range.Amplitude() + 1;
            long max = 1 + (long)uint.MaxValue;
            long remainder = max % module;
            uint seed = Seed;
            while (seed >= max - remainder)
            {
                seed = Seed;
            }
            return (int)(seed % module) + range.LowerValueOfRange;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing == false || isDisposing)
            {
                return;
            }

            isDisposing = true;
            generator.Dispose();
        }

        private void VerifyBuffer()
        {
            if (currentBufferPosition >= MaxBufferSize)
            {
                currentBufferPosition = 0;
                generator.GetBytes(buffer);
            }
        }

        ~CryptoRandom()
        {
            Dispose(false);
        }

        private readonly byte[] buffer;
        private readonly RandomNumberGenerator generator;

        private bool isDisposing;
        private int currentBufferPosition;
    }
}