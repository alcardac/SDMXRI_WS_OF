// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportStructureMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The report structure mutable core.
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

    /// <summary>
    ///   The report structure mutable core.
    /// </summary>
    [Serializable]
    public class ReportStructureMutableCore : IdentifiableMutableCore, IReportStructureMutableObject
    {
        #region Fields

        /// <summary>
        ///   The metadata attributes.
        /// </summary>
        private readonly IList<IMetadataAttributeMutableObject> _metadataAttributes;

        /// <summary>
        ///   The target metadatas.
        /// </summary>
        private readonly IList<string> _targetMetadatas;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportStructureMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        public ReportStructureMutableCore(IReportStructure objTarget)
            : base(objTarget)
        {
            this._metadataAttributes = new List<IMetadataAttributeMutableObject>();
            if (objTarget.MetadataAttributes != null)
            {
                foreach (IMetadataAttributeObject metadataAttribute in objTarget.MetadataAttributes)
                {
                    this._metadataAttributes.Add(new MetadataAttributeMutableCore(metadataAttribute));
                }
            }

            this._targetMetadatas = objTarget.TargetMetadatas;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReportStructureMutableCore" /> class.
        /// </summary>
        public ReportStructureMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportStructure))
        {
            this._metadataAttributes = new List<IMetadataAttributeMutableObject>();
            this._targetMetadatas = new List<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the metadata attributes.
        /// </summary>
        public IList<IMetadataAttributeMutableObject> MetadataAttributes
        {
            get
            {
                return this._metadataAttributes;
            }
        }

        /// <summary>
        ///   Gets the target metadata.
        /// </summary>
        public IList<string> TargetMetadatas
        {
            get
            {
                return this._targetMetadatas;
            }
        }

        #endregion
    }
}