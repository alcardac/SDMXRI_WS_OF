// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentConstraintAttachmentMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The content constraint attachment mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The content constraint attachment mutable core.
    /// </summary>
    [Serializable]
    public class ContentConstraintAttachmentMutableCore : MutableCore, IConstraintAttachmentMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _data or metadata set reference.
        /// </summary>
        private IDataAndMetadataSetMutableReference _dataOrMetadataSetReference;

        /// <summary>
        ///   The _data sources.
        /// </summary>
        private IList<IDataSourceMutableObject> _dataSources;

        /// <summary>
        ///   The _structure reference.
        /// </summary>
        private ISet<IStructureReference> _structureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ContentConstraintAttachmentMutableCore" /> class.
        /// </summary>
        public ContentConstraintAttachmentMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraintAttachment))
        {
            this._structureReference = new HashSet<IStructureReference>();
            this._dataSources = new List<IDataSourceMutableObject>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentConstraintAttachmentMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public ContentConstraintAttachmentMutableCore(IConstraintAttachment immutable)
            : base(immutable)
        {
            this._structureReference = new HashSet<IStructureReference>();
            this._dataSources = new List<IDataSourceMutableObject>();
            if (immutable.DataOrMetadataSetReference != null)
            {
                this._dataOrMetadataSetReference = immutable.DataOrMetadataSetReference.CreateMutableInstance();
            }

            if (immutable.StructureReference != null)
            {
                foreach (ICrossReference xsRef in immutable.StructureReference)
                {
                    this._structureReference.Add(xsRef.CreateMutableInstance());
                }
            }

            if (ObjectUtil.ValidCollection(immutable.DataSources))
            {
                foreach (IDataSource d in immutable.DataSources)
                {
                    this._dataSources.Add(d.CreateMutableInstance());
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the data or metadata set reference.
        /// </summary>
        public virtual IDataAndMetadataSetMutableReference DataOrMetadataSetReference
        {
            get
            {
                return this._dataOrMetadataSetReference;
            }

            set
            {
                this._dataOrMetadataSetReference = value;
            }
        }

        /// <summary>
        ///   Gets the data sources.
        /// </summary>
        public virtual IList<IDataSourceMutableObject> DataSources
        {
            get
            {
                return this._dataSources;
            }
        }

        /// <summary>
        ///   Gets the structure reference.
        /// </summary>
        public virtual ISet<IStructureReference> StructureReference
        {
            get
            {
                return this._structureReference;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add data sources.
        /// </summary>
        /// <param name="dataSourceMutableObject">
        /// The dataSourceMutableObject. 
        /// </param>
        public virtual void AddDataSources(IDataSourceMutableObject dataSourceMutableObject)
        {
            if (dataSourceMutableObject != null)
            {
                this._dataSources.Add(dataSourceMutableObject);
            }
        }

        /// <summary>
        /// The add structure reference.
        /// </summary>
        /// <param name="structureReference">
        /// The dataSourceMutableObject. 
        /// </param>
        public virtual void AddStructureReference(IStructureReference structureReference)
        {
            if (this._structureReference == null)
            {
                this._structureReference = new HashSet<IStructureReference>();
            }

            this._structureReference.Add(structureReference);
        }

        #endregion
    }
}