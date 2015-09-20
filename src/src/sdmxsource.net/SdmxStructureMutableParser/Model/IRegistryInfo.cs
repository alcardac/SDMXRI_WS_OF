// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistryInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The RegistryInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    ///     The RegistryInfo interface.
    /// </summary>
    public interface IRegistryInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the <see cref="QueryStructureRequest" /> is set.
        /// </summary>
        bool HasQueryStructureRequest { get; }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="QueryStructureResponse" /> is set.
        /// </summary>
        bool HasQueryStructureResponse { get; }

        /// <summary>
        ///     Gets or sets the header.
        /// </summary>
        IHeader Header { get; set; }

        /// <summary>
        ///     Gets or sets the query structure request info.
        /// </summary>
        IQueryStructureRequestInfo QueryStructureRequest { get; set; }

        /// <summary>
        ///     Gets or sets the query structure response info.
        /// </summary>
        IQueryStructureResponseInfo QueryStructureResponse { get; set; }

        #endregion
    }
}