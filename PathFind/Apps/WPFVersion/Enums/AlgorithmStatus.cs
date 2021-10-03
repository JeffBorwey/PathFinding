﻿using System.ComponentModel;

namespace WPFVersion.Enums
{
    internal enum AlgorithmStatus
    {
        Started = 0,

        Interrupted = 1,

        Finished = 2,

        [Description("Couldn't find path")]
        PathNotFound = 3
    }
}
