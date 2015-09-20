// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataAttributeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata attributeObject mutable core.
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
    ///     The metadata attributeObject mutable core.
    /// </summary>
    [Serializable]
    public class MetadataAttributeMutableCore : ComponentMutableCore, IMetadataAttributeMutableObject
    {
        #region Fields

        /// <summary>
        ///     The _metadata attributes.
        /// </summary>
        private readonly IList<IMetadataAttributeMutableObject> _metadataAttributes;

        /// <summary>
        ///     The _max occurs.
        /// </summary>
        private int? _maxOccurs;

        /// <summary>
        ///     The _min occurs.
        /// </summary>
        private int? _minOccurs;

        /// <summary>
        ///     The _presentational.
        /// </summary>
        private TertiaryBool _presentational;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataAttributeMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target.
        /// </param>
        public MetadataAttributeMutableCore(IMetadataAttributeObject objTarget)
            : base(objTarget)
        {
            this._metadataAttributes = new List<IMetadataAttributeMutableObject>();
            this._presentational = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._minOccurs = objTarget.MinOccurs;
            this._maxOccurs = objTarget.MaxOccurs;
            this._presentational = objTarget.Presentational;
            if (objTarget.MetadataAttributes != null)
            {
                foreach (IMetadataAttributeObject currentMetadatAttribute in objTarget.MetadataAttributes)
                {
                    this._metadataAttributes.Add(new MetadataAttributeMutableCore(currentMetadatAttribute));
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MetadataAttributeMutableCore" /> class.
        /// </summary>
        public MetadataAttributeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataAttribute))
        {
            this._metadataAttributes = new List<IMetadataAttributeMutableObject>();
            this._presentational = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the max occurs.
        /// </summary>
        public virtual int? MaxOccurs
        {
            get
            {
                return this._maxOccurs;
            }

            set
            {
                this._maxOccurs = value;
            }
        }

        /// <summary>
        ///     Gets the metadata attributes.
        /// </summary>
        public virtual IList<IMetadataAttributeMutableObject> MetadataAttributes
        {
            get
            {
                return this._metadataAttributes;
            }
        }

        /// <summary>
        ///     Gets or sets the min occurs.
        /// </summary>
        public virtual int? MinOccurs
        {
            get
            {
                return this._minOccurs;
            }

            set
            {
                this._minOccurs = value;
            }
        }

        /// <summary>
        ///     Gets or sets the presentational.
        /// </summary>
        public virtual TertiaryBool Presentational
        {
            get
            {
                return this._presentational;
            }

            set
            {
                this._presentational = value;
            }
        }

        #endregion
    }
}