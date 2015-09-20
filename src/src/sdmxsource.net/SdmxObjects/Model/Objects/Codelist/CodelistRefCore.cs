// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistRefBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist ref core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The codelist ref core.
    /// </summary>
    [Serializable]
    public class CodelistRefCore : SdmxStructureCore, ICodelistRef
    {
        #region Fields

        /// <summary>
        ///   The alias.
        /// </summary>
        private readonly string alias;

        /// <summary>
        ///   The codelist reference.
        /// </summary>
        private readonly ICrossReference codelistReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistRefCore"/> class.
        /// </summary>
        /// <param name="codelistRefMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public CodelistRefCore(ICodelistRefMutableObject codelistRefMutableObject, ISdmxStructure parent)
            : base(codelistRefMutableObject, parent)
        {
            this.alias = codelistRefMutableObject.Alias;
            if (codelistRefMutableObject.CodelistReference != null)
            {
                this.codelistReference = new CrossReferenceImpl(this, codelistRefMutableObject.CodelistReference);
            }

            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistRefCore"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="alias0">
        /// The alias 0. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public CodelistRefCore(
            string agencyId, string maintainableId, string version, string alias0, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListRef), parent)
        {
            this.alias = alias0;
            this.codelistReference = new CrossReferenceImpl(
                this, agencyId, maintainableId, version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistRefCore"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn. 
        /// </param>
        /// <param name="alias0">
        /// The alias 0. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public CodelistRefCore(string urn, string alias0, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListRef), parent)
        {
            this.alias = alias0;
            this.codelistReference = new CrossReferenceImpl(this, urn);
            this.Validate();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistRefCore"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn. 
        /// </param>
        /// <param name="alias0">
        /// The alias 0. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public CodelistRefCore(Uri urn, string alias0, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListRef), parent)
        {
            this.alias = alias0;
            this.codelistReference = new CrossReferenceImpl(this, urn);
            this.Validate();
        }


        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the alias.
        /// </summary>
        public virtual string Alias
        {
            get
            {
                return this.alias;
            }
        }

        /// <summary>
        ///   Gets the codelist reference.
        /// </summary>
        public virtual ICrossReference CodelistReference
        {
            get
            {
                return this.codelistReference;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
                var that = (ICodelistRef)sdmxObject;
                if (!ObjectUtil.Equivalent(this.alias, that.Alias))
                {
                    return false;
                }

                if (!this.Equivalent(this.codelistReference, that.CodelistReference))
                {
                    return false;
                }

                return true;
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
        private void Validate()
        {
            if (this.codelistReference == null)
            {
                throw new SdmxSemmanticException("HierarchicalCodelist, Codelist Reference Missing a Reference");
            }

            if (this.codelistReference.TargetReference.EnumType != SdmxStructureEnumType.CodeList)
            {
                throw new SdmxSemmanticException(
                    "Referenced structure should be "
                    + SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList).StructureType + " but is declared as "
                    + this.codelistReference.TargetReference.GetType());
            }
        }

        #endregion
    }
}