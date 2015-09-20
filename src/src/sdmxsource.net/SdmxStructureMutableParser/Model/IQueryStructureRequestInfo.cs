// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryStructureRequestInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   An interface for holding the SDMX v2.0 <c>QueryStructureRequest</c> information
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using System.Collections.Generic;

    /// <summary>
    ///     An interface for holding the SDMX v2.0 <c>QueryStructureRequest</c> information
    /// </summary>
    public interface IQueryStructureRequestInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets the references.
        /// </summary>
        IList<IReferenceInfo> References { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether references should be resolved.
        /// </summary>
        bool ResolveReferences { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the response should return details of the requested artefacts.
        /// </summary>
        bool ReturnDetails { get; set; }

        #endregion
    }
}