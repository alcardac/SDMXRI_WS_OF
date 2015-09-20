// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentConstraintAttachmentBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attachment constraint attachment core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The attachment constraint attachment core.
    /// </summary>
    [Serializable]
    public class AttachmentConstraintAttachmentCore : SdmxStructureCore, IAttachmentConstraintAttachment
    {
        #region Fields

        /// <summary>
        ///   The data or metadata set reference.
        /// </summary>
        private readonly IList<IDataAndMetadataSetReference> dataOrMetadataSetReference;

        /// <summary>
        ///   The data sources.
        /// </summary>
        private readonly IList<IDataSource> dataSources;

        /// <summary>
        ///   The structure references.
        /// </summary>
        private readonly IList<ICrossReference> structureReferences;

        /// <summary>
        ///   The target structure.
        /// </summary>
        private SdmxStructureType targetStructure;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentConstraintAttachmentCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AttachmentConstraintAttachmentCore(ConstraintAttachmentType type, IAttachmentConstraintObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraintAttachment), parent)
        {
            this.dataOrMetadataSetReference = new List<IDataAndMetadataSetReference>();
            this.structureReferences = new List<ICrossReference>();
            this.dataSources = new List<IDataSource>();
            if (ObjectUtil.ValidCollection(type.DataSet))
            {
                this.targetStructure = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataset);
                foreach (SetReferenceType setReference in type.DataSet)
                {
                    ICrossReference dataProviderRef = RefUtil.CreateReference(this, setReference.DataProvider);
                    this.dataOrMetadataSetReference.Add(
                        new DataAndMetadataSetReferenceCore(dataProviderRef, setReference.ID, true));
                }
            }

            if (ObjectUtil.ValidCollection(type.MetadataSet))
            {
                this.targetStructure = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataSet);

                foreach (SetReferenceType setReference0 in type.MetadataSet)
                {
                    ICrossReference dataProviderRef1 = RefUtil.CreateReference(this, setReference0.DataProvider);
                    this.dataOrMetadataSetReference.Add(
                        new DataAndMetadataSetReferenceCore(dataProviderRef1, setReference0.ID, false));
                }
            }

            if (ObjectUtil.ValidCollection(type.SimpleDataSource))
            {
                this.targetStructure = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Datasource);

                foreach (Uri dataSource in type.SimpleDataSource)
                {
                    this.dataSources.Add(new DataSourceCore(dataSource.ToString(), this));
                }
            }

            foreach (MaintainableReferenceBaseType xref in type.DataStructure)
            {
                this.AddRef(xref);
            }

            foreach (MaintainableReferenceBaseType ref2 in type.MetadataStructure)
            {
                this.AddRef(ref2);
            }

            foreach (MaintainableReferenceBaseType ref3 in type.Dataflow)
            {
                this.AddRef(ref3);
            }

            foreach (MaintainableReferenceBaseType ref4 in type.Metadataflow)
            {
                this.AddRef(ref4);
            }

            foreach (MaintainableReferenceBaseType ref5 in type.ProvisionAgreement)
            {
                this.AddRef(ref5);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the data or metadata set reference.
        /// </summary>
        public virtual IList<IDataAndMetadataSetReference> DataOrMetadataSetReference
        {
            get
            {
                return new List<IDataAndMetadataSetReference>(this.dataOrMetadataSetReference);
            }
        }

        /// <summary>
        ///   Gets the datasources.
        /// </summary>
        public virtual IList<IDataSource> Datasources
        {
            get
            {
                return new List<IDataSource>(this.dataSources);
            }
        }

        /// <summary>
        ///   Gets the structure references.
        /// </summary>
        public virtual IList<ICrossReference> StructureReferences
        {
            get
            {
                return new List<ICrossReference>(this.structureReferences);
            }
        }

        /// <summary>
        ///   Gets the target structure type.
        /// </summary>
        public virtual SdmxStructureType TargetStructureType
        {
            get
            {
                return this.targetStructure;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IAttachmentConstraintAttachment)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this.dataOrMetadataSetReference, that.DataOrMetadataSetReference))
                {
                    return false;
                }

                if (!this.Equivalent(this.dataSources, that.Datasources, includeFinalProperties))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.structureReferences, that.StructureReferences))
                {
                    return false;
                }

                if (this.targetStructure != that.TargetStructureType)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add ref.
        /// </summary>
        /// <param name="refType">
        /// The ref type. 
        /// </param>
        private void AddRef(MaintainableReferenceBaseType refType)
        {
            ICrossReference crossRef = RefUtil.CreateReference(this, refType);
            this.targetStructure = crossRef.TargetReference;
            this.structureReferences.Add(crossRef);
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES		                     //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       /// The get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
    	   ISet<ISdmxObject> composites = base.GetCompositesInternal();
           base.AddToCompositeSet(this.dataSources, composites);
           return composites;
       }

       #endregion
    }
}