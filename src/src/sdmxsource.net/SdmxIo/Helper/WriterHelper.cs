// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriterHelper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class contains static helper methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Helper
{
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     This class contains static helper methods
    /// </summary>
    public static class WriterHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Check if the specified <paramref name="errors"/> contains any errors. i.e. is not empty. If it is not empty an
        ///     <see cref="SdmxNotImplementedException"/>
        ///     is thrown
        /// </summary>
        /// <param name="errors">
        /// The string containing
        /// </param>
        /// <exception cref="SdmxNotImplementedException">
        /// the specified
        ///     <paramref name="errors"/>
        ///     is not empty and contains errors
        /// </exception>
        public static void ValidateErrors(string errors)
        {
            if (!string.IsNullOrEmpty(errors))
            {
                throw new SdmxNotImplementedException(errors);
            }
        }

        #endregion
    }
}