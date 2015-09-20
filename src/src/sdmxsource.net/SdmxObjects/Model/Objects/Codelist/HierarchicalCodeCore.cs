// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   ICodeRef represents a Codelist item in a IHierarchicalCodelistObject, named from the 2.0 SDMX Schema (in 2.1 it is called a HierarchicalCode).
//   It can have child CodeRefObjects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using CodeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CodeRefType;

    /// <summary>
    ///   ICodeRef represents a Codelist item in a IHierarchicalCodelistObject, named from the 2.0 SDMX Schema (in 2.1 it is called a HierarchicalCode).
    ///   It can have child CodeRefObjects.
    /// </summary>
    [Serializable]
    public class HierarchicalCodeCore : IdentifiableCore, IHierarchicalCode
    {
        #region Fields

        /// <summary>
        ///   The code refs.
        /// </summary>
        private readonly IList<IHierarchicalCode> _codeRefs;

        /// <summary>
        ///   The codelist alias ref.
        /// </summary>
        private readonly string _codelistAliasRef;

        /// <summary>
        ///   The level ref.
        /// </summary>
        private readonly string _levelRef;

        /// <summary>
        ///   The valid from.
        /// </summary>
        private readonly ISdmxDate _validFrom;

        /// <summary>
        ///   The valid to.
        /// </summary>
        private readonly ISdmxDate _validTo;

        /// <summary>
        ///   The code id.
        /// </summary>
        private string _codeId;

        /// <summary>
        ///   The code reference.
        /// </summary>
        private ICrossReference _codeReference;

        /// <summary>
        ///   The level.
        /// </summary>
        private ILevelObject _level;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeCore"/> class.
        /// </summary>
        /// <param name="codeRefMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchicalCodeCore(ICodeRefMutableObject codeRefMutableObject, IHierarchy parent)
            : base(GenerateId(codeRefMutableObject, parent), parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();
            try
            {
                if (codeRefMutableObject.CodeReference != null)
                {
                    this._codeReference = new CrossReferenceImpl(this, codeRefMutableObject.CodeReference);
                }

                this._codelistAliasRef = codeRefMutableObject.CodelistAliasRef;
                this._codeId = codeRefMutableObject.CodeId;
                this._levelRef = codeRefMutableObject.LevelReference;

                this.BuildCodeReferences(codeRefMutableObject, o => o.CodeRefs, (o, code) => new HierarchicalCodeCore(o, code));
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), th);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeCore"/> class.
        /// </summary>
        /// <param name="codeRefMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private HierarchicalCodeCore(ICodeRefMutableObject codeRefMutableObject, ISdmxStructure parent)
            : base(GenerateId(codeRefMutableObject, parent), parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();
            try
            {
                if (codeRefMutableObject.CodeReference != null)
                {
                    this._codeReference = new CrossReferenceImpl(this, codeRefMutableObject.CodeReference);
                }

                this._codelistAliasRef = codeRefMutableObject.CodelistAliasRef;
                this._codeId = codeRefMutableObject.CodeId;
                this._levelRef = codeRefMutableObject.LevelReference;
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), th);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeCore"/> class.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchicalCodeCore(HierarchicalCodeType codeRef, IHierarchy parent)
            : base(codeRef, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCode), parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();

           // Either a Code ref is local or not
            if (codeRef.CodeID != null)
            {
                var refBaseType = codeRef.CodeID.GetTypedRef<LocalCodeRefType>();
                if (refBaseType != null && codeRef.CodelistAliasRef != null)
                {
                    // Local - has CodelistAliasRef and CodeID
                    this._codeId = refBaseType.id;
                    this._codelistAliasRef = codeRef.CodelistAliasRef;
                }
            }

            if (codeRef.Level != null)
            {
                this._levelRef = RefUtil.CreateLocalIdReference(codeRef.Level);
            }

            if (codeRef.Code != null)
            {
                this._codeReference = RefUtil.CreateReference(this, codeRef.Code);
            }

            if (codeRef.validFrom != null)
            {
                this._validFrom = new SdmxDateCore(codeRef.validFrom.Value, TimeFormatEnumType.DateTime);
            }

            if (codeRef.validTo != null)
            {
                this._validTo = new SdmxDateCore(codeRef.validTo.Value, TimeFormatEnumType.DateTime);
            }

            // Children
            if (codeRef.HierarchicalCode != null)
            {
                this.BuildCodeReferences(codeRef, type => type.HierarchicalCode, (type, code) => new HierarchicalCodeCore(type, code));
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeCore"/> class.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private HierarchicalCodeCore(HierarchicalCodeType codeRef, ISdmxStructure parent)
            : base(codeRef, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCode), parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();

            // Either a Code ref is local or not
            if (codeRef.CodeID != null)
            {
                var refBaseType = codeRef.CodeID.GetTypedRef<LocalCodeRefType>();
                if (refBaseType != null && codeRef.CodelistAliasRef != null)
                {
                    // Local - has CodelistAliasRef and CodeID
                    this._codeId = refBaseType.id;
                    this._codelistAliasRef = codeRef.CodelistAliasRef;
                }
            }

            if (codeRef.Level != null)
            {
                this._levelRef = RefUtil.CreateLocalIdReference(codeRef.Level);
            }

            if (codeRef.Code != null)
            {
                this._codeReference = RefUtil.CreateReference(this, codeRef.Code);
            }

            if (codeRef.validFrom != null)
            {
                this._validFrom = new SdmxDateCore(codeRef.validFrom.Value, TimeFormatEnumType.DateTime);
            }

            if (codeRef.validTo != null)
            {
                this._validTo = new SdmxDateCore(codeRef.validTo.Value, TimeFormatEnumType.DateTime);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeCore"/> class.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchicalCodeCore(CodeRefType codeRef, IHierarchy parent)
            : base(
                GenerateId(codeRef, parent),
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCode),
                parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();
            if (codeRef.URN != null)
            {
                this._codeReference = new CrossReferenceImpl(this, codeRef.URN);
            }

            this._codeId = codeRef.CodeID;
            this._codelistAliasRef = codeRef.CodelistAliasRef;

            this.BuildCodeReferences(codeRef, type => type.CodeRef, (type, code) => new HierarchicalCodeCore(type, code));

            if (codeRef.ValidFrom != null)
            {
                this._validFrom = new SdmxDateCore(codeRef.ValidFrom);
            }

            if (codeRef.ValidTo != null)
            {
                this._validTo = new SdmxDateCore(codeRef.ValidTo);
            }

            if (codeRef.LevelRef != null)
            {
                this._levelRef = string.Join(".", this.BuildLevelRef(codeRef));
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeCore"/> class.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private HierarchicalCodeCore(CodeRefType codeRef, ISdmxStructure parent)
            : base(
                GenerateId(codeRef, parent), 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCode), 
                parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();
            if (codeRef.URN != null)
            {
                this._codeReference = new CrossReferenceImpl(this, codeRef.URN);
            }

            this._codeId = codeRef.CodeID;
            this._codelistAliasRef = codeRef.CodelistAliasRef;

            if (codeRef.ValidFrom != null)
            {
                this._validFrom = new SdmxDateCore(codeRef.ValidFrom);
            }

            if (codeRef.ValidTo != null)
            {
                this._validTo = new SdmxDateCore(codeRef.ValidTo);
            }

            if (codeRef.LevelRef != null)
            {
                this._levelRef = string.Join(".", this.BuildLevelRef(codeRef));
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        /// <summary>
        ///   Gets the code id.
        /// </summary>
        public virtual string CodeId
        {
            get
            {
                return this._codeId;
            }
        }

        /// <summary>
        ///   Gets the code reference.
        /// </summary>
        public virtual ICrossReference CodeReference
        {
            get
            {
                return this._codeReference ?? (this._codeReference = this.GenerateCodelistReference());
            }
        }

        /// <summary>
        ///   Gets the code refs.
        /// </summary>
        public virtual IList<IHierarchicalCode> CodeRefs
        {
            get
            {
                return new List<IHierarchicalCode>(this._codeRefs);
            }
        }

        /// <summary>
        ///   Gets the codelist alias ref.
        /// </summary>
        public virtual string CodelistAliasRef
        {
            get
            {
                return this._codelistAliasRef;
            }
        }

        /// <summary>
        ///   Gets the level in hierarchy.
        /// </summary>
        public virtual int LevelInHierarchy
        {
            get
            {
                return this.GetFullIdPath(false).Split('.').Length;
            }
        }

        /// <summary>
        ///   Gets the valid from.
        /// </summary>
        public virtual ISdmxDate ValidFrom
        {
            get
            {
                return this._validFrom;
            }
        }

        /// <summary>
        ///   Gets the valid to.
        /// </summary>
        public virtual ISdmxDate ValidTo
        {
            get
            {
                return this._validTo;
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
        /// <param name="includeFinalProperties"> Set to true to compare also final properties.</param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null)
            {
                return false;
            }

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IHierarchicalCode)sdmxObject;
                if (!this.Equivalent(this._codeRefs, that.CodeRefs, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._codeReference, that.CodeReference))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._codeId, that.CodeId))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._validFrom, that.ValidFrom))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._validTo, that.ValidTo))
                {
                    return false;
                }

                var levelObject = this.GetLevel(true);

                var thatLevelObject = that.GetLevel(true);
                if (!ObjectUtil.Equivalent(levelObject, thatLevelObject))
                {
                    return false;
                }

                if (this.LevelInHierarchy != that.LevelInHierarchy)
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get level.
        /// </summary>
        /// <param name="acceptDefault">
        /// The accept default. 
        /// </param>
        /// <returns>
        /// The <see cref="ILevelObject"/> . 
        /// </returns>
        public virtual ILevelObject GetLevel(bool acceptDefault)
        {
            if (string.IsNullOrWhiteSpace(this._levelRef))
            {
                if (!acceptDefault)
                {
                    return null;
                }

                if (this._level == null)
                {
                    var hb = this.GetParent<IHierarchy>(false);
                    this._level = hb.GetLevelAtPosition(this.LevelInHierarchy);
                }
            }

            return this._level;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate a code id, this is either the code alias (2.0 attribute not in 2.1) if this is not present then will use the 
        ///   code id of the referenced code - if this id is the same as another id in the hierarchical level, then will postfix with an underscore
        ///   followed by an integer, starting at 2 (working it's way up until a unique id is found).
        /// </summary>
        /// <param name="codeRef">Coderef object
        /// </param>
        /// <param name="parent">Sdmx object
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        private static string GenerateId(CodeRefType codeRef, ISdmxObject parent)
        {
            if (!string.IsNullOrWhiteSpace(codeRef.NodeAliasID))
            {
                return codeRef.NodeAliasID;
            }

            IList<IHierarchicalCode> currentChildren = null;
            if (parent.StructureType == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy))
            {
                currentChildren = ((IHierarchy)parent).HierarchicalCodeObjects;
            }
            else
            {
                // ((IHierarchicalCode)parent).CodeRefs; //### EROARE ce draq face cu un apel de prop (getter)?
            }

            string codeId0;
            if (!string.IsNullOrWhiteSpace(codeRef.CodeID))
            {
                codeId0 = codeRef.CodeID;
            }
            else
            {
                IStructureReference localXsRef;
                if (codeRef.URN != null)
                {
                    localXsRef = new StructureReferenceImpl(codeRef.URN);
                }
                else
                {
                    throw new SdmxSemmanticException(
                        "Could not generate Hierarchical Code Id - no NodeAlias Id, Code ID, or Code URN found");
                }

                if (localXsRef.TargetReference.EnumType != SdmxStructureEnumType.Code)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.ReferenceErrorUnexpectedStructure, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code).StructureType, 
                        localXsRef.TargetReference.GetType());
                }

                codeId0 = localXsRef.ChildReference.Id;
            }

            // Check CodeId is unique 
            string inputId = codeId0;
            int i = 2;
            while (!IsCodeIdUnique(currentChildren, codeId0))
            {
                codeId0 = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", inputId, string.Empty, i);
                i++;
            }

            return codeId0;
        }

        /// <summary>
        /// Generate a code id, this is either the code alias (2.0 attribute not in 2.1) if this is not present then will use the 
        ///   code id of the referenced code - if this id is the same as another id in the hierarchical level, then will postfix with an underscore
        ///   followed by an integer, starting at 2 (working it's way up until a unique id is found).
        /// </summary>
        /// <param name="codeRef">Coderef object
        /// </param>
        /// <param name="parent">Sdmx object
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        private static ICodeRefMutableObject GenerateId(ICodeRefMutableObject codeRef, ISdmxObject parent)
        {
            if (!string.IsNullOrWhiteSpace(codeRef.Id))
            {
                return codeRef;
            }

            IList<IHierarchicalCode> currentChildren = null;
            if (parent.StructureType == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy))
            {
                currentChildren = ((IHierarchy)parent).HierarchicalCodeObjects;
            }
            else
            {
                // ((IHierarchicalCode)parent).CodeRefs; //### EROARE ce draq face cu un apel de prop (getter)?
            }

            string codeId0;
            if (!string.IsNullOrWhiteSpace(codeRef.CodeId))
            {
                codeId0 = codeRef.CodeId;
            }
            else
            {
                IStructureReference localXsRef;
                if (codeRef.Urn != null)
                {
                    localXsRef = new StructureReferenceImpl(codeRef.Urn);
                }
                else
                {
                    throw new SdmxSemmanticException(
                        "Could not generate Hierarchical Code Id - no NodeAlias Id, Code ID, or Code URN found");
                }

                if (localXsRef.TargetReference.EnumType != SdmxStructureEnumType.Code)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.ReferenceErrorUnexpectedStructure,
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code).StructureType,
                        localXsRef.TargetReference.GetType());
                }

                codeId0 = localXsRef.ChildReference.Id;
            }

            // Check CodeId is unique 
            string inputId = codeId0;
            int i = 2;
            while (!IsCodeIdUnique(currentChildren, codeId0))
            {
                codeId0 = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", inputId, string.Empty, i);
                i++;
            }

            codeRef.Id = codeId0;

            return codeRef;
        }

        /// <summary>
        /// The is code id unique.
        /// </summary>
        /// <param name="currentChildren">
        /// The current children. 
        /// </param>
        /// <param name="codeId0">
        /// The code id 0. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        private static bool IsCodeIdUnique(IEnumerable<IHierarchicalCode> currentChildren, string codeId0)
        {
            if (currentChildren != null)
            {
                foreach (IHierarchicalCode currentChild in currentChildren)
                {
                    if (currentChild.Id.Equals(codeId0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Build level ref.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{String}"/>.
        /// </returns>
        private IEnumerable<string> BuildLevelRef(CodeRefType codeRef)
        {
            var hierarchy = this.GetParent<IHierarchy>(false);
            ILevelObject level = hierarchy.Level;
            while (!level.Id.Equals(codeRef.LevelRef))
            {
                yield return level.Id;
                level = level.ChildLevel;
            }

            yield return level.Id;
        }

        /// <summary>
        /// Build code references 
        /// </summary>
        /// <param name="codeRef">
        /// The code reference.
        /// </param>
        /// <param name="childRefs">The method to get child references</param>
        /// <param name="ctor">The method to create new <see cref="HierarchicalCodeCore"/></param>
        /// <typeparam name="T">The type of input code reference.</typeparam>
        private void BuildCodeReferences<T>(T codeRef, Func<T, IEnumerable<T>> childRefs, Func<T, HierarchicalCodeCore, HierarchicalCodeCore> ctor)
        {
            var stack = new Queue<KeyValuePair<T, HierarchicalCodeCore>>(childRefs(codeRef).Select(type => new KeyValuePair<T, HierarchicalCodeCore>(type, this)));
            while (stack.Count > 0)
            {
                var current = stack.Dequeue();
                var newCode = ctor(current.Key, current.Value);
                current.Value._codeRefs.Add(newCode);
                foreach (var hierarchicalCode in childRefs(current.Key))
                {
                    stack.Enqueue(new KeyValuePair<T, HierarchicalCodeCore>(hierarchicalCode, newCode));
                }
            }
        }

        /// <summary>
        ///   The generate codelist reference.
        /// </summary>
        /// <returns> The <see cref="ICrossReference" /> . </returns>
        private ICrossReference GenerateCodelistReference()
        {
            var hcl = (IHierarchicalCodelistObject)this.MaintainableParent;
            if (hcl.CodelistRef != null)
            {
                foreach (ICodelistRef currentCodelistRef in hcl.CodelistRef)
                {
                    if (currentCodelistRef.Alias.Equals(this.CodelistAliasRef))
                    {
                        IMaintainableRefObject codelistRef = currentCodelistRef.CodelistReference.MaintainableReference;
                        return new CrossReferenceImpl(
                            this, 
                            codelistRef.AgencyId, 
                            codelistRef.MaintainableId, 
                            codelistRef.Version, 
                            SdmxStructureEnumType.Code, 
                            this._codeId);
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            // Each hierarchy's coderef must have an AliasRef with CodeId, OR a URN alone.
            if (!string.IsNullOrWhiteSpace(this._codelistAliasRef))
            {
                if (string.IsNullOrWhiteSpace(this._codeId))
                {
                    throw new SdmxSemmanticException(ExceptionCode.CodeRefMissingCodeId, this.ToString());
                }

                if (this._codeReference != null)
                {
                    ICrossReference generatedReference = this.GenerateCodelistReference();
                    if (!generatedReference.TargetUrn.Equals(this._codeReference.TargetUrn))
                    {
                        throw new SdmxSemmanticException(
                            "Code reference was supplied both by a codelist alias ('" + this._codelistAliasRef
                            + "') and code Id ('" + this._codeId + "'), and by a code URN ('"
                            + this._codeReference.TargetUrn + "') - " + "the two references contradict each other");
                    }
                }
                else
                {
                    this._codeReference = this.CodeReference;
                    if (this._codeReference == null)
                    {
                        throw new SdmxSemmanticException(
                            "Could not resolve reference to codelist with alias : " + this._codelistAliasRef);
                    }
                }
            }
            else if (this._codeReference != null)
            {
                if (!string.IsNullOrWhiteSpace(this._codeId))
                {
                    if (!this._codeReference.ChildReference.Id.Equals(this._codeId))
                    {
                        throw new SdmxSemmanticException(
                            "Code id was supplied both by a code Id ('" + this._codeId + "'), and by a code URN ('"
                            + this._codeReference.TargetUrn + "') - " + "the two references contradict each other");
                    }
                }

                if (this._codeReference.TargetReference.EnumType != SdmxStructureEnumType.Code)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.ReferenceErrorUnexpectedStructure, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code).StructureType, 
                        this._codeReference.TargetReference.GetType());
                }

                this._codeId = this._codeReference.ChildReference.Id;
            }
            else
            {
                throw new SdmxSemmanticException(ExceptionCode.CodeRefMissingCodeReference, this.ToString());
            }

            if (this._validFrom != null && this._validTo != null)
            {
                if (this._validFrom.IsLater(this._validTo))
                {
                    throw new SdmxSemmanticException("Hierarchical Code IsError - Valid from can not be after valid to");
                }
            }

            if (!string.IsNullOrWhiteSpace(this._levelRef))
            {
                var heirarchy = this.GetParent<IHierarchy>(false);

                string[] levelSplit = this._levelRef.Split('.');
                this._level = heirarchy.GetLevelAtPosition(levelSplit.Length);
                if (this._level == null)
                {
                    throw new SdmxSemmanticException(
                        "Hierarchical Code IsError - Could not find level with id '" + this._levelRef
                        +
                        "', ensure the Level Reference is dot '.' seperated e.g. L1.L2.L3 (where L1 is the Id of the first level, L2 is the Id of the second level and L3 is the id of the third level) ");
                }

                string levelFullId = this._level.GetFullIdPath(false);
                if (!levelFullId.Equals(this._levelRef))
                {
                    throw new SdmxSemmanticException(
                        "Hierarchical Code IsError - Could not find level with id '" + this._levelRef
                        + "', Level id at depth '" + levelSplit.Length + "' is '" + levelFullId
                        +
                        "', ensure the Level Reference is dot '.' seperated e.g. L1.L2.L3 (where L1 is the Id of the first level, L2 is the Id of the second level and L3 is the id of the third level) ");
                }
            }
            else
            {
                var heirarchy0 = this.GetParent<IHierarchy>(false);
                if (heirarchy0.HasFormalLevels())
                {
                    int depthOfHierarchy = this.LevelInHierarchy;
                    this._level = heirarchy0.GetLevelAtPosition(depthOfHierarchy);
                    if (this._level == null)
                    {
                        throw new SdmxSemmanticException(
                            "Hierarchical Code IsError - Hierarchy states that there are formal levels, but there is no level defined for a hierarchy of depth '"
                            + depthOfHierarchy + 1 + "'");
                    }
                }
            }
        }

     ///////////////////////////////////////////////////////////////////////////////////////////////////
     ////////////COMPOSITES				 //////////////////////////////////////////////////
     ///////////////////////////////////////////////////////////////////////////////////////////////////

     /// <summary>
     ///   The get composites internal.
     /// </summary>
     protected override ISet<ISdmxObject> GetCompositesInternal() 
     {
    	ISet<ISdmxObject> composites = base.GetCompositesInternal();
        base.AddToCompositeSet(this._codeRefs, composites);
        return composites;
     }

        #endregion
    }
}