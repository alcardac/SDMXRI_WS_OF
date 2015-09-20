// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReferenceInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   An interface for holding the SDMX v2.0 <c>QueryStructureRequest</c> reference information
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     An interface for holding the SDMX v2.0 <c>QueryStructureRequest</c> reference information
    /// </summary>
    public interface IReferenceInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the agency ID
        /// </summary>
        string AgencyId { get; set; }

        /// <summary>
        ///     Gets or sets the ID
        /// </summary>
        string ID { get; set; }

        /// <summary>
        ///     Gets or sets the reference from artefact.
        /// </summary>
        INameableMutableObject ReferenceFrom { get; set; }

        /// <summary>
        ///     Gets the SDMX structure.
        /// </summary>
        SdmxStructureEnumType SdmxStructure { get; }

        /// <summary>
        ///     Gets or sets the urn.
        /// </summary>
        Uri URN { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        string Version { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create and returns a <see cref="IStructureReference"/> from the <see cref="ReferenceInfo.ID"/>,
        ///     <see cref="ReferenceInfo.AgencyId"/>
        ///     ,
        ///     <see cref="ReferenceInfo.Version"/>
        ///     and <see cref="ReferenceInfo.SdmxStructure"/>
        /// </summary>
        /// <param name="items">
        /// Optional. Items
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        IStructureReference CreateReference(params string[] items);

        #endregion
    }
}