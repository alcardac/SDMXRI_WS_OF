// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryBuilderV1.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query bean builder v 1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    using QueryMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.QueryMessageType;

    /// <summary>
    ///     The query bean builder v 1.
    /// </summary>
    public class QueryBuilderV1
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="IStructureReference"/> list from the specified SDMX v1.0 <paramref name="queryMessage"/>.
        ///     NOT IMPLEMENTED
        /// </summary>
        /// <param name="queryMessage">
        /// The query message.
        /// </param>
        /// <returns>
        /// a <see cref="IStructureReference"/> list from the specified SDMX v1.0 <paramref name="queryMessage"/>.
        /// </returns>
        /// <exception cref="UnsupportedException">
        /// Not supported.
        /// </exception>
        public IList<IStructureReference> Build(QueryMessageType queryMessage)
        {
            // FUNC build Query from version 1.0 queryMessage
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "QueryMessage");
        }

        #endregion
    }
}