// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeRefMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code ref mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The code ref mutable core.
    /// </summary>
    [Serializable]
    public class CodeRefMutableCore : IdentifiableMutableCore, ICodeRefMutableObject
    {
        #region Fields

        /// <summary>
        ///   The code refs.
        /// </summary>
        private readonly IList<ICodeRefMutableObject> _codeRefs;

        /// <summary>
        ///   The code id.
        /// </summary>
        private string _codeId;

        /// <summary>
        ///   The code reference.
        /// </summary>
        private IStructureReference _codeReference;

        /// <summary>
        ///   The codelist alias ref.
        /// </summary>
        private string _codelistAliasRef;

        /// <summary>
        ///   The level reference.
        /// </summary>
        private string _levelReference;

        /// <summary>
        ///   The valid from.
        /// </summary>
        private DateTime? _validFrom;

        /// <summary>
        ///   The valid to.
        /// </summary>
        private DateTime? _validTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CodeRefMutableCore" /> class.
        /// </summary>
        public CodeRefMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCode))
        {
            this._codeRefs = new List<ICodeRefMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeRefMutableCore"/> class.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        public CodeRefMutableCore(IHierarchicalCode codeRef)
            : base(codeRef)
        {
            this._codeRefs = new List<ICodeRefMutableObject>();
            this._codeReference = codeRef.CodeReference.CreateMutableInstance();
            this._codelistAliasRef = codeRef.CodelistAliasRef;
            this._codeId = codeRef.CodeId;

            // change list to Mutable CodeRefBeans
            if (codeRef.CodeRefs != null)
            {
                foreach (IHierarchicalCode currentCodeRef in codeRef.CodeRefs)
                {
                    this._codeRefs.Add(new CodeRefMutableCore(currentCodeRef));
                }
            }

            if (codeRef.GetLevel(false) != null)
            {
                this._levelReference = codeRef.GetLevel(false).GetFullIdPath(false);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the code id.
        /// </summary>
        public virtual string CodeId
        {
            get
            {
                return this._codeId;
            }

            set
            {
                this._codeId = value;
            }
        }

        /// <summary>
        ///   Gets or sets the code reference.
        /// </summary>
        public virtual IStructureReference CodeReference
        {
            get
            {
                return this._codeReference;
            }

            set
            {
                this._codeReference = value;
            }
        }

        /// <summary>
        ///   Gets the code refs.
        /// </summary>
        public virtual IList<ICodeRefMutableObject> CodeRefs
        {
            get
            {
                return this._codeRefs;
            }
        }

        /// <summary>
        ///   Gets or sets the codelist alias ref.
        /// </summary>
        public virtual string CodelistAliasRef
        {
            get
            {
                return this._codelistAliasRef;
            }

            set
            {
                this._codelistAliasRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the level reference.
        /// </summary>
        public virtual string LevelReference
        {
            get
            {
                return this._levelReference;
            }

            set
            {
                this._levelReference = value;
            }
        }

        /// <summary>
        ///   Gets or sets the valid from.
        /// </summary>
        public virtual DateTime? ValidFrom
        {
            get
            {
                return this._validFrom;
            }

            set
            {
                this._validFrom = value;
            }
        }

        /// <summary>
        ///   Gets or sets the valid to.
        /// </summary>
        public virtual DateTime? ValidTo
        {
            get
            {
                return this._validTo;
            }

            set
            {
                this._validTo = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add code ref.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        public virtual void AddCodeRef(ICodeRefMutableObject codeRef)
        {
            this._codeRefs.Add(codeRef);
        }

        #endregion
    }
}