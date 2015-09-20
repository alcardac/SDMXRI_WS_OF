// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureRequestInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class that hold the SDMX v2.0 <c>QueryStructureRequest</c> information
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using System.Collections.Generic;

    /// <summary>
    ///     A class that hold the SDMX v2.0 <c>QueryStructureRequest</c> information
    /// </summary>
    public class QueryStructureRequestInfo : IQueryStructureRequestInfo
    {
        #region Fields

        /// <summary>
        ///     Gets the reference collection
        /// </summary>
        private readonly IList<IReferenceInfo> _references = new List<IReferenceInfo>();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestInfo"/> class.
        /// </summary>
        public QueryStructureRequestInfo()
        {
            this.ReturnDetails = true;
        }

        #region Public Properties

        /// <summary>
        ///     Gets the references.
        /// </summary>
        public IList<IReferenceInfo> References
        {
            get
            {
                return this._references;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether references should be resolved.
        /// </summary>
        public bool ResolveReferences { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the response should return details of the requested artefacts.
        /// </summary>
        public bool ReturnDetails { get; set; }

        #endregion
    }
}