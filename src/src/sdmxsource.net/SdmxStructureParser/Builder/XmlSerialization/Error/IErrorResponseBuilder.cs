// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IErrorResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The error Response Builder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Error
{
    using System;

    using Xml.Schema.Linq;

    /// <summary>
    ///     The error Response Builder interface.
    /// </summary>
    public interface IErrorResponseBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="exceptionCode">
        /// The exception code.
        /// </param>
        /// <returns>
        /// The <see cref="XTypedElement"/>.
        /// </returns>
        XTypedElement BuildErrorResponse(Exception exception, string exceptionCode);

        #endregion
    }
}