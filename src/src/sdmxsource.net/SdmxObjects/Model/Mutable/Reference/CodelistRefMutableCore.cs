// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistRefMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist ref mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The codelist ref mutable core.
    /// </summary>
    [Serializable]
    public class CodelistRefMutableCore : MutableCore, ICodelistRefMutableObject
    {
        #region Fields

        /// <summary>
        ///   The alias.
        /// </summary>
        private string alias;

        /// <summary>
        ///   The structure reference.
        /// </summary>
        private IStructureReference structureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CodelistRefMutableCore" /> class.
        /// </summary>
        public CodelistRefMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListRef))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistRefMutableCore"/> class.
        /// </summary>
        /// <param name="codelistRef">
        /// The codelistRef. 
        /// </param>
        public CodelistRefMutableCore(ICodelistRef codelistRef)
            : base(codelistRef)
        {
            this.alias = codelistRef.Alias;
            this.structureReference = codelistRef.CodelistReference.CreateMutableInstance();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the alias.
        /// </summary>
        public virtual string Alias
        {
            get
            {
                return this.alias;
            }

            set
            {
                this.alias = value;
            }
        }

        /// <summary>
        ///   Gets or sets the codelist reference.
        /// </summary>
        public virtual IStructureReference CodelistReference
        {
            get
            {
                return this.structureReference;
            }

            set
            {
                this.structureReference = value;
            }
        }

        #endregion
    }
}