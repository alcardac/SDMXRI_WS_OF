// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataflowMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadataflow mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure;

    /// <summary>
    ///   The metadataflow mutable core.
    /// </summary>
    [Serializable]
    public class MetadataflowMutableCore : MaintainableMutableCore<IMetadataFlow>, IMetadataFlowMutableObject
    {
        #region Fields

        /// <summary>
        ///   The metadata structure ref.
        /// </summary>
        private IStructureReference metadataStructureRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="MetadataflowMutableCore" /> class.
        /// </summary>
        public MetadataflowMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataflowMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public MetadataflowMutableCore(IMetadataFlow objTarget)
            : base(objTarget)
        {
            if (objTarget.MetadataStructureRef != null)
            {
                this.metadataStructureRef = objTarget.MetadataStructureRef.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IMetadataFlow ImmutableInstance
        {
            get
            {
                return new MetadataflowObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets or sets the metadata structure ref.
        /// </summary>
        public virtual IStructureReference MetadataStructureRef
        {
            get
            {
                return this.metadataStructureRef;
            }

            set
            {
                this.metadataStructureRef = value;
            }
        }

        #endregion
    }
}