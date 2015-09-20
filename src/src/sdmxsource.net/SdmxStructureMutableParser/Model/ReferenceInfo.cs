// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A class that hold the SDMX v2.0 <c>QueryStructureRequest</c> reference information
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     A class that hold the SDMX v2.0 <c>QueryStructureRequest</c> reference information
    /// </summary>
    public class ReferenceInfo : IReferenceInfo
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceInfo"/> class.
        /// </summary>
        /// <param name="sdmxStructure">
        /// The sdmx structure.
        /// </param>
        public ReferenceInfo(SdmxStructureEnumType sdmxStructure)
        {
            this.SdmxStructure = sdmxStructure;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the agency ID
        /// </summary>
        public string AgencyId { get; set; }

        /// <summary>
        ///     Gets or sets the ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     Gets or sets the reference from.
        /// </summary>
        public INameableMutableObject ReferenceFrom { get; set; }

        /// <summary>
        ///     Gets the sdmx structure.
        /// </summary>
        public SdmxStructureEnumType SdmxStructure { get; private set; }

        /// <summary>
        ///     Gets or sets the urn.
        /// </summary>
        public Uri URN { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create and returns a <see cref="IStructureReference"/> from the <see cref="ID"/>, <see cref="AgencyId"/>,
        ///     <see cref="Version"/>
        ///     and <see cref="SdmxStructure"/>
        /// </summary>
        /// <param name="items">
        /// Optional. Items
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        public IStructureReference CreateReference(params string[] items)
        {
            return new StructureReferenceImpl(this.AgencyId, this.ID, this.Version, this.SdmxStructure, items);
        }

        #endregion
    }
}