﻿using NullObject.Attributes;
using System;

namespace NullObject.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object self)
        {
            return self is null || Attribute.IsDefined(self.GetType(), typeof(NullAttribute));
        }
    }
}
