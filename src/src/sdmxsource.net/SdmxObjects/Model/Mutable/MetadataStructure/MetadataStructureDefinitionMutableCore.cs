// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataStructureDefinitionMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata structure definition mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure;

    /// <summary>
    ///   The metadata structure definition mutable core.
    /// </summary>
    [Serializable]
    public class MetadataStructureDefinitionMutableCore : MaintainableMutableCore<IMetadataStructureDefinitionObject>, 
                                                          IMetadataStructureDefinitionMutableObject
    {
        #region Fields

        /// <summary>
        ///   The metadata targets.
        /// </summary>
        private readonly IList<IMetadataTargetMutableObject> _metadataTargets;

        /// <summary>
        ///   The report structures.
        /// </summary>
        private IList<IReportStructureMutableObject> reportStructures;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public MetadataStructureDefinitionMutableCore(IMetadataStructureDefinitionObject objTarget)
            : base(objTarget)
        {
            this._metadataTargets = new List<IMetadataTargetMutableObject>();
            this.reportStructures = new List<IReportStructureMutableObject>();
            if (objTarget.MetadataTargets != null)
            {
                foreach (IMetadataTarget currentMt in objTarget.MetadataTargets)
                {
                    this._metadataTargets.Add(new MetadataTargetMutableCore(currentMt));
                }
            }

            if (objTarget.ReportStructures != null)
            {
                foreach (IReportStructure currentReportStructure in objTarget.ReportStructures)
                {
                    this.reportStructures.Add(new ReportStructureMutableCore(currentReportStructure));
                }
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MetadataStructureDefinitionMutableCore" /> class.
        /// </summary>
        public MetadataStructureDefinitionMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd))
        {
            this._metadataTargets = new List<IMetadataTargetMutableObject>();
            this.reportStructures = new List<IReportStructureMutableObject>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IMetadataStructureDefinitionObject ImmutableInstance
        {
            get
            {
                return new MetadataStructureDefinitionObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets the metadata targets.
        /// </summary>
        public virtual IList<IMetadataTargetMutableObject> MetadataTargets
        {
            get
            {
                return this._metadataTargets;
            }
        }

        /// <summary>
        ///   Gets the report structures.
        /// </summary>
        public virtual IList<IReportStructureMutableObject> ReportStructures
        {
            get
            {
                return this.reportStructures;
            }
        }

        #endregion
    }
}