// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureResponseInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query structure response info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;

    /// <summary>
    ///     The query structure response info.
    /// </summary>
    internal class QueryStructureResponseInfo : IQueryStructureResponseInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the status message.
        /// </summary>
        public IStatusMessageInfo StatusMessage { get; set; }

        /// <summary>
        ///     Gets or sets the artefacts.
        /// </summary>
        public IMutableObjects Structure { get; set; }

        #endregion
    }
}