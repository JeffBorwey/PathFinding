﻿using System.Globalization;
using System.Windows.Controls;
using ValueRange;
using ValueRange.Extensions;

namespace WPFVersion.ValidationRules
{
    internal sealed class RangedDoubleValidationRule : ValidationRule
    {
        public InclusiveValueRange<double> ValueRange { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            value = value?.ToString().Replace('.', ',');
            if (IsValidValue(value))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, null);
        }

        private bool IsValidValue(object value)
        {
            return double.TryParse(value?.ToString(), out var result)
                && ValueRange.Contains(result);
        }
    }
}
