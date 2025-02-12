﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerationValues.Attributes
{
    /// <summary>
    /// Marks enum values to ignore
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
    public sealed class EnumValuesIgnoreAttribute : Attribute
    {
        public IReadOnlyCollection<Enum> Ignored { get; }

        public EnumValuesIgnoreAttribute(params object[] values)
        {
            Ignored = new HashSet<Enum>(values.Cast<Enum>());
        }
    }
}