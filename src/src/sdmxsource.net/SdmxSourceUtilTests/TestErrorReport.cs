namespace SdmxSourceUtilTests
{
    using System;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Model.Impl;

    /// <summary> Test unit class for <see cref="ErrorReport"/> </summary>
    [TestFixture]
    public class TestErrorReport
    {
        /// <summary>Test method for <see cref="ErrorReport.Build(System.Exception)"/> </summary>
        [TestCase("TESTING 123")]
        public void Test(string msg)
        {
            ErrorReport error = ErrorReport.Build(new Exception(msg));
            Assert.NotNull(error.ErrorMessage);
            CollectionAssert.Contains(error.ErrorMessage, msg);
            error = ErrorReport.Build(msg);
            CollectionAssert.Contains(error.ErrorMessage, msg);
            error = ErrorReport.Build(msg, new Exception("1", new Exception("2", new Exception("3"))));
            CollectionAssert.Contains(error.ErrorMessage, msg + ": 1");
            CollectionAssert.Contains(error.ErrorMessage, "2");
            CollectionAssert.Contains(error.ErrorMessage, "3");

        }
    }
}