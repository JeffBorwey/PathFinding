﻿using ConsoleVersion.Interface;
using GraphLib.Interfaces;
using GraphLib.Realizations.Coordinates;
using GraphLib.Realizations.Graphs;
using System;
using System.Runtime.CompilerServices;
using ValueRange;

namespace ConsoleVersion.Extensions
{
    internal static class ValueInputExtensions
    {
        /// <summary>
        /// Return user's console input in range of values
        /// </summary>
        /// <param name="accompanyingMessage"></param>
        /// <param name="rangeOfValidInput"></param>
        /// <returns>A number in the range
        /// <paramref name="rangeOfValidInput"/></returns>
        /// <exception cref="System.IO.IOException"></exception> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue InputValue<TValue>(this IValueInput<TValue> self,
            string accompanyingMessage,
            InclusiveValueRange<TValue> rangeOfValidInput)
            where TValue : struct, IComparable
        {
            return self.InputValue(accompanyingMessage,
                rangeOfValidInput.UpperValueOfRange,
                rangeOfValidInput.LowerValueOfRange);
        }

        /// <summary>
        /// Returns <see cref="Coordinate2D"/> where X belongs to 
        /// [<paramref name="upperPossibleXValue"/>, 0]
        /// and where Y belongs to [<paramref name="upperPossibleYValue"/>, 0]
        /// </summary>
        /// <param name="upperPossibleXValue">An upper value of X
        /// coordinate in range where a lower value is 0</param>
        /// <param name="upperPossibleYValue">An upper value of Y 
        /// coordinate in range where a lower value is 0</param>
        /// <returns>An instance of <see cref="Coordinate2D"/></returns>
        public static ICoordinate InputPoint(this IValueInput<int> self,
            int upperPossibleXValue, int upperPossibleYValue)
        {
            int xCoordinate = self.InputValue(MessagesTexts.XCoordinateInputMsg, upperPossibleXValue);
            int yCoordinate = self.InputValue(MessagesTexts.YCoordinateInputMsg, upperPossibleYValue);

            return new Coordinate2D(xCoordinate, yCoordinate);
        }

        /// <summary>
        /// Inputs <see cref="InclusiveValueRange{TValue}"/> using <paramref name="rangeOfValiInput"/>
        /// to limit the input
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="rangeOfValiInput"></param>
        /// <returns><see cref="InclusiveValueRange{TValue}"/> each extremum value of which lays in the
        /// <paramref name="rangeOfValiInput"/></returns>
        public static InclusiveValueRange<TValue> InputRange<TValue>(this IValueInput<TValue> self,
            InclusiveValueRange<TValue> rangeOfValiInput)
            where TValue : struct, IComparable
        {
            TValue upperValueOfRange = self.InputValue(MessagesTexts.RangeUpperValueInputMsg, rangeOfValiInput);
            TValue lowerValueOfRange = self.InputValue(MessagesTexts.RangeLowerValueInputMsg, rangeOfValiInput);

            return new InclusiveValueRange<TValue>(upperValueOfRange, lowerValueOfRange);
        }

        /// <summary>
        /// Inputs vertex coordinates at tries to get it from <paramref name="graph2D"/>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="graph2D"></param>
        /// <returns></returns>
        public static IVertex InputVertex(this IValueInput<int> self, Graph2D graph2D)
        {
            int upperPossibleXValue = graph2D.Width - 1;
            int upperPossibleYValue = graph2D.Length - 1;

            var point = self.InputPoint(upperPossibleXValue, upperPossibleYValue);

            return graph2D[point];
        }
    }
}
