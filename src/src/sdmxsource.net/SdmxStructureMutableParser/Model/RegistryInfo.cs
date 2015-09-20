// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registry info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    ///     The registry info.
    /// </summary>
    public class RegistryInfo : IRegistryInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the <see cref="IRegistryInfo.QueryStructureRequest" /> is set.
        /// </summary>
        public bool HasQueryStructureRequest
        {
            get
            {
                return this.QueryStructureRequest != null;
            }
        }

        /// <summary>
        ///     Gets  a value indicating whether the <see cref="IRegistryInfo.QueryStructureResponse" /> is set.
        /// </summary>
        public bool HasQueryStructureResponse
        {
            get
            {
                return this.QueryStructureResponse != null;
            }
        }

        /// <summary>
        ///     Gets or sets the header.
        /// </summary>
        public IHeader Header { get; set; }

        /// <summary>
        ///     Gets or sets the query structure request info.
        /// </summary>
        public IQueryStructureRequestInfo QueryStructureRequest { get; set; }

        /// <summary>
        ///     Gets or sets the query structure response info.
        /// </summary>
        public IQueryStructureResponseInfo QueryStructureResponse { get; set; }

        #endregion
    }
}