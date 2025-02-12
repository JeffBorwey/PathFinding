﻿using ConsoleVersion.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueRange;
using ValueRange.Extensions;

namespace ConsoleVersion.Model
{
    internal sealed class MenuList : IDisplayable
    {
        public MenuList(IEnumerable<string> menuItemsNames, int columns = 2)
        {
            this.menuItemsNames = menuItemsNames.ToArray();
            menuItemsCount = this.menuItemsNames.Length;
            menuItemNumberPad = menuItemsCount.ToString().Length;
            var columnsRange = new InclusiveValueRange<int>(menuItemsCount, 1);
            this.columns = columnsRange.ReturnInRange(columns);
            longestNameLength = new Lazy<int>(GetLongestNameLength);
            menuList = new Lazy<string>(CreateMenu);
        }

        public void Display()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return menuList.Value;
        }

        private int GetLongestNameLength()
        {
            return menuItemsCount > 0
                ? menuItemsNames.Max(str => str.Length) + 1
                : default;
        }

        private string CreateMenu()
        {
            var stringBuilder = new StringBuilder(NewLine);

            for (int index = 0; index < menuItemsCount; index++)
            {
                string paddedName = menuItemsNames[index].PadRight(longestNameLength.Value);
                string paddedMenuItemIndex = (index + 1).ToString().PadLeft(menuItemNumberPad);
                string format = Format + ((index + 1) % columns == 0 ? NewLine : Space);
                stringBuilder.AppendFormat(format, paddedMenuItemIndex, paddedName);
            }

            return stringBuilder.ToString();
        }

        private readonly Lazy<string> menuList;
        private readonly Lazy<int> longestNameLength;

        private readonly int menuItemsCount;
        private readonly int columns;
        private readonly int menuItemNumberPad;
        private readonly string[] menuItemsNames;

        private const string NewLine = "\n";
        private const string Space = " ";
        private const string Format = "{0}. {1}";
    }
}