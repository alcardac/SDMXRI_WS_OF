// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintAttachmentBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint attachment core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The constraint attachment core.
    /// </summary>
    [Serializable]
    public class ConstraintAttachmentCore : SdmxStructureCore, IConstraintAttachment
    {
        #region Fields

        /// <summary>
        ///   The cross reference.
        /// </summary>
        private readonly ISet<ICrossReference> crossReference;

        /// <summary>
        ///   The data or metadata set reference.
        /// </summary>
        private readonly IDataAndMetadataSetReference dataOrMetadataSetReference;

        /// <summary>
        ///   The data sources.
        /// </summary>
        private readonly IList<IDataSource> dataSources;

        #endregion

        // FUNC Validation of ALL Constraints and This CrossRefence/DataSource Pair should be grouped

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintAttachmentCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="constraint">
        /// The constraint. 
        /// </param>
        public ConstraintAttachmentCore(IConstraintAttachmentMutableObject mutable, IConstraintObject constraint)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraintAttachment), constraint)
        {
            this.crossReference = new HashSet<ICrossReference>();
            this.dataSources = new List<IDataSource>();
            if (mutable.DataOrMetadataSetReference != null)
            {
                this.dataOrMetadataSetReference = new DataAndMetadataSetReferenceCore(
                    mutable.DataOrMetadataSetReference);
            }

            if (mutable.StructureReference != null)
            {
                foreach (IStructureReference structureReference in mutable.StructureReference)
                {
                    this.crossReference.Add(new CrossReferenceImpl(this, structureReference));
                }
            }

            if (ObjectUtil.ValidCollection(mutable.DataSources))
            {
                foreach (IDataSourceMutableObject each in mutable.DataSources)
                {
                    this.dataSources.Add(new DataSourceCore(each, this));
                }
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintAttachmentCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="constraint">
        /// The constraint. 
        /// </param>
        public ConstraintAttachmentCore(ConstraintAttachmentType type, IConstraintObject constraint)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraintAttachment), constraint)
        {
            this.crossReference = new HashSet<ICrossReference>();
            this.dataSources = new List<IDataSource>();
            if (type.SimpleDataSource != null)
            {
                foreach (Uri dataSource in type.SimpleDataSource)
                {
                    this.dataSources.Add(new DataSourceCore(dataSource.ToString(), this));
                }
            }

            if (type.DataProvider != null)
            {
                this.crossReference.Add(RefUtil.CreateReference(this, type.DataProvider));
            }

            if (ObjectUtil.ValidCollection(type.DataSet))
            {
                // THERE IS ONLY ONE DATA SET
                SetReferenceType setRef = type.DataSet[0];
                ICrossReference xref = RefUtil.CreateReference(this, setRef.DataProvider);
                this.dataOrMetadataSetReference = new DataAndMetadataSetReferenceCore(xref, setRef.ID, true);
            }

            if (ObjectUtil.ValidCollection(type.MetadataSet))
            {
                // THERE IS ONLY ONE METADATA SET
                SetReferenceType setRef0 = type.MetadataSet[0];
                ICrossReference ref1 = RefUtil.CreateReference(this, setRef0.DataProvider);
                this.dataOrMetadataSetReference = new DataAndMetadataSetReferenceCore(ref1, setRef0.ID, false);
            }

            this.AddRef(type.DataStructure);
            this.AddRef(type.MetadataStructure);
            this.AddRef(type.Dataflow);
            this.AddRef(type.Metadataflow);
            this.AddRef(type.ProvisionAgreement);

            foreach (QueryableDataSourceType queryableDataSource in type.QueryableDataSource)
            {
                this.dataSources.Add(new DataSourceCore(queryableDataSource, this));
            }

            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the data or metadata set reference.
        /// </summary>
        public virtual IDataAndMetadataSetReference DataOrMetadataSetReference
        {
            get
            {
                return this.dataOrMetadataSetReference;
            }
        }

        /// <summary>
        ///   Gets the data sources.
        /// </summary>
        public virtual IList<IDataSource> DataSources
        {
            get
            {
                return new List<IDataSource>(this.dataSources);
            }
        }

        /// <summary>
        ///   Gets the structure reference.
        /// </summary>
        public virtual ISet<ICrossReference> StructureReference
        {
            get
            {
                return this.crossReference;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The create mutable instance.
        /// </summary>
        /// <returns> The <see cref="IConstraintAttachmentMutableObject" /> . </returns>
        public virtual IConstraintAttachmentMutableObject CreateMutableInstance()
        {
            return new ContentConstraintAttachmentMutableCore(this);
        }

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
                var that = (IConstraintAttachment)sdmxObject;
                if (!ObjectUtil.Equivalent(this.dataOrMetadataSetReference, that.DataOrMetadataSetReference))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.crossReference, that.StructureReference))
                {
                    return false;
                }

                if (!this.Equivalent(this.dataSources, that.DataSources, includeFinalProperties))
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
        /// <param name="refListType">
        /// The ref list type. 
        /// </param>
        /// <typeparam name="T">Generic type param of type MaintainableReferenceBaseType
        /// </typeparam>
        private void AddRef<T>(IList<T> refListType) where T : MaintainableReferenceBaseType
        {
            if (ObjectUtil.ValidCollection(refListType))
            {
                foreach (T xref in refListType)
                {
                    ICrossReference crossRef = RefUtil.CreateReference(this, xref);
                    this.crossReference.Add(crossRef);
                }
            }
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            SdmxStructureType constrainingType = default(SdmxStructureType) /* was: null */;

            // Checking that there is at least something in this Attachment
            if (this.dataOrMetadataSetReference == null && !ObjectUtil.ValidCollection(this.crossReference)
                && !ObjectUtil.ValidCollection(this.dataSources))
            {
                throw new SdmxSemmanticException("The ContentConstraint doesn't have a Constraint Attachment defined");
            }

            foreach (ICrossReference xsRef in this.crossReference)
            {
                if (constrainingType == null)
                {
                    constrainingType = xsRef.TargetReference;
                }
                else
                {
                    switch (xsRef.TargetReference.EnumType)
                    {
                        case SdmxStructureEnumType.Dsd:
                            if (constrainingType.EnumType != SdmxStructureEnumType.Dataflow
                                && constrainingType.EnumType != SdmxStructureEnumType.ProvisionAgreement)
                            {
                                constrainingType = xsRef.TargetReference;
                            }

                            break;
                        case SdmxStructureEnumType.Dataflow:
                            if (constrainingType.EnumType != SdmxStructureEnumType.ProvisionAgreement)
                            {
                                constrainingType = xsRef.TargetReference;
                            }

                            break;
                        case SdmxStructureEnumType.ProvisionAgreement:
                            constrainingType = xsRef.TargetReference;
                            break;
                    }

                    // if(constrainingType != xsRef.getTargetReference()) {
                    // throw new SdmxSemmanticException("ContentConstraint's ConstraintAttachment may only reference structures of the same type, got '"+constrainingType+"' and '"+xsRef.getTargetReference()+"'");
                    // }
                }
            }
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