﻿using ConsoleVersion.Enums;
using GraphLib.Coordinates;
using System;
using System.Linq;

namespace ConsoleVersion.InputClass
{
    internal static class Input
    {
        static Input()
        {
            var menuValues = Enum.GetValues(typeof(MenuOption)).Cast<byte>();
            minMenuValue = menuValues.First();
            maxMenuValue = menuValues.Last();
        }

        public static int InputNumber(string msg, int upper, int lower = 0)
        {
            string choice;
            Console.Write(msg);
            choice = Console.ReadLine();
            while (IsError(choice, upper, lower)) 
            {
                Console.Write(msg);
                choice = Console.ReadLine();
            }
            return int.Parse(choice);
        }

        public static MenuOption InputOption()
        {
            return (MenuOption)InputNumber(
                ConsoleVersionResources.OptionInputMsg,
                maxMenuValue, 
                minMenuValue);
        }

        public static Coordinate2D InputPoint(int width, int height)
        {
            var xCoordinate = InputNumber(ConsoleVersionResources.XCoordinateInputMsg, width);
            var yCoordinate = InputNumber(ConsoleVersionResources.YCoordinateInputMsg, height);

            return new Coordinate2D(xCoordinate, yCoordinate);
        }

        private static bool IsError(string choice, int upper, int lower)
        {
            return !int.TryParse(choice, out var ch) 
                || ch > upper || ch < lower;
        }

        private static readonly byte minMenuValue;
        private static readonly byte maxMenuValue;
    }
}
