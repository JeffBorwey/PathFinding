﻿using System;
using System.Linq;
using GraphLibrary.PathFindingAlgorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphLibrary.UnitTests.Tests.PathFindingAlgorithmsTests
{
    [TestClass]
    public class NullAlgorithmTests
    {
        [TestMethod]
        public void FindPath_NullALgorithm_ReturnsEmptyPath()
        {
            var algorithm = NullAlgorithm.Instance;

            var path = algorithm.FindPath();

            Assert.IsFalse(path.Any());
        }

        [TestMethod]
        public void Instance_NullAlgorithm_ReturnsSameInstance()
        {
            var instance = NullAlgorithm.Instance;

            Assert.AreSame(instance, NullAlgorithm.Instance);
        }
    }
}
