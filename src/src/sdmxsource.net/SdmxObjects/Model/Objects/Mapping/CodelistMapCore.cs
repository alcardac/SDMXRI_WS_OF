// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using CodeMap = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.CodeMap;
    using CodeMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CodeMapType;

    /// <summary>
    ///   The codelist map core.
    /// </summary>
    [Serializable]
    public class CodelistMapCore : ItemSchemeMapCore, ICodelistMapObject
    {
        #region Fields

        /// <summary>
        ///   The src alias.
        /// </summary>
        private readonly string srcAlias;

        /// <summary>
        ///   The target alias.
        /// </summary>
        private readonly string targetAlias;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMapCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistMapCore(ICodelistMapMutableObject itemMutableObject, IStructureSetObject parent)
            : base(itemMutableObject, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListMap), parent)
        {
            this.srcAlias = itemMutableObject.SrcAlias;
            this.targetAlias = itemMutableObject.TargetAlias;

            if (itemMutableObject.TargetRef != null)
            {
                this.TargetRef = new CrossReferenceImpl(this, itemMutableObject.TargetRef);
            }

            if (itemMutableObject.SourceRef != null)
            {
                this.SourceRef = new CrossReferenceImpl(this, itemMutableObject.SourceRef);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMapCore"/> class.
        /// </summary>
        /// <param name="codelistMapType">
        /// The codelist map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistMapCore(CodelistMapType codelistMapType, IStructureSetObject parent)
            : base(codelistMapType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListMap), parent)
        {
            try
            {
                var source = codelistMapType.GetTypedSource<CodelistReferenceType>();
                this.SourceRef = RefUtil.CreateReference(this, source);
                var target = codelistMapType.GetTypedTarget<CodelistReferenceType>();
                this.TargetRef = RefUtil.CreateReference(this, target);

                foreach (CodeMap codeMap in codelistMapType.ItemAssociation)
                {
                    IItemMap item = new ItemMapCore(codeMap.GetTypedSource<LocalCodeReferenceType>().GetTypedRef<LocalCodeRefType>().id, codeMap.GetTypedTarget<LocalCodeReferenceType>().GetTypedRef<LocalCodeRefType>().id, this);
                    this.AddInternalItem(item);
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMapCore"/> class.
        /// </summary>
        /// <param name="codeMapType">
        /// The code map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CodelistMapType codeMapType, IStructureSetObject parent)
            : base(
                codeMapType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListMap), 
                codeMapType.id, 
                null, 
                codeMapType.Name, 
                codeMapType.Description, 
                codeMapType.Annotations, 
                parent)
        {
            try
            {
                if (codeMapType.CodelistRef != null)
                {
                    if (codeMapType.CodelistRef.URN != null)
                    {
                        this.SourceRef = new CrossReferenceImpl(this, codeMapType.CodelistRef.URN);
                    }
                    else
                    {
                        string agencyId = codeMapType.CodelistRef.AgencyID;
                        if (string.IsNullOrWhiteSpace(agencyId))
                        {
                            agencyId = this.MaintainableParent.AgencyId;
                        }

                        this.SourceRef = new CrossReferenceImpl(
                            this, 
                            agencyId, 
                            codeMapType.CodelistRef.CodelistID, 
                            codeMapType.CodelistRef.Version, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
                    }
                }
                else if (codeMapType.HierarchicalCodelistRef != null)
                {
                    if (codeMapType.HierarchicalCodelistRef.URN != null)
                    {
                        this.SourceRef = new CrossReferenceImpl(
                            this, codeMapType.HierarchicalCodelistRef.URN);
                    }
                    else
                    {
                        string agencyId0 = codeMapType.HierarchicalCodelistRef.AgencyID;
                        if (string.IsNullOrWhiteSpace(agencyId0))
                        {
                            agencyId0 = this.MaintainableParent.AgencyId;
                        }

                        this.SourceRef = new CrossReferenceImpl(
                            this, 
                            agencyId0, 
                            codeMapType.HierarchicalCodelistRef.HierarchicalCodelistID, 
                            codeMapType.HierarchicalCodelistRef.Version, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist));
                    }
                }

                if (codeMapType.TargetCodelistRef != null)
                {
                    if (codeMapType.TargetCodelistRef.URN != null)
                    {
                        this.TargetRef = new CrossReferenceImpl(this, codeMapType.TargetCodelistRef.URN);
                    }
                    else
                    {
                        string agencyId1 = codeMapType.TargetCodelistRef.AgencyID;
                        if (string.IsNullOrWhiteSpace(agencyId1))
                        {
                            agencyId1 = this.MaintainableParent.AgencyId;
                        }

                        this.TargetRef = new CrossReferenceImpl(
                            this, 
                            agencyId1, 
                            codeMapType.TargetCodelistRef.CodelistID, 
                            codeMapType.TargetCodelistRef.Version, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
                    }
                }
                else if (codeMapType.TargetHierarchicalCodelistRef != null)
                {
                    if (codeMapType.TargetHierarchicalCodelistRef.URN != null)
                    {
                        this.TargetRef = new CrossReferenceImpl(
                            this, codeMapType.TargetHierarchicalCodelistRef.URN);
                    }
                    else
                    {
                        string agencyId2 = codeMapType.TargetHierarchicalCodelistRef.AgencyID;
                        if (string.IsNullOrWhiteSpace(agencyId2))
                        {
                            agencyId2 = this.MaintainableParent.AgencyId;
                        }

                        this.TargetRef = new CrossReferenceImpl(
                            this, 
                            agencyId2, 
                            codeMapType.TargetHierarchicalCodelistRef.HierarchicalCodelistID, 
                            codeMapType.TargetHierarchicalCodelistRef.Version, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist));
                    }
                }

                // get list of code maps
                if (codeMapType.CodeMap != null)
                {
                    foreach (CodeMapType codeMap in codeMapType.CodeMap)
                    {
                        IItemMap item = new ItemMapCore(
                            codeMap.CodeAlias, codeMap.MapCodeRef, codeMap.MapTargetCodeRef, this);
                        this.AddInternalItem(item);
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the src alias.
        /// </summary>
        public virtual string SrcAlias
        {
            get
            {
                return this.srcAlias;
            }
        }
        
        /// <summary>
        ///   Gets the target alias.
        /// </summary>
        public virtual string TargetAlias
        {
            get
            {
                return this.targetAlias;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
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
                var that = (ICodelistMapObject)sdmxObject;
                if (!ObjectUtil.Equivalent(this.srcAlias, that.SrcAlias))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.srcAlias, that.TargetAlias))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void Validate()
        {
            if (this.SourceRef == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "Source");
            }

            if (this.TargetRef == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "Target");
            }

            if (this.Items == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "CodeMap");
            }
        }

        #endregion
    }
}