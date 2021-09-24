﻿using GraphLib.Interfaces;

namespace GraphLib.Base.EndPointsCondition.Interface
{
    /// <summary>
    /// Represents a condition construction
    /// </summary>
    public interface IEndPointsCondition
    {
        /// <summary>
        /// Determines, whether the stored condition is true
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        bool IsTrue(IVertex vertex);

        /// <summary>
        /// Executes the body of the stored condition
        /// </summary>
        /// <param name="vertex"></param>
        void Execute(IVertex vertex);
    }
}