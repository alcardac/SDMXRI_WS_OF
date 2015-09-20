// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryStructureResponseInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The QueryStructureResponseInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;

    /// <summary>
    ///     The QueryStructureResponseInfo interface.
    /// </summary>
    public interface IQueryStructureResponseInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the status message.
        /// </summary>
        IStatusMessageInfo StatusMessage { get; set; }

        /// <summary>
        ///     Gets or sets the artefacts.
        /// </summary>
        IMutableObjects Structure { get; set; }

        #endregion
    }
}