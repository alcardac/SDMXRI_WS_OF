// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchy mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference;

    /// <summary>
    ///   The hierarchy mutable core.
    /// </summary>
    [Serializable]
    public class HierarchyMutableCore : NameableMutableCore, IHierarchyMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _code refs.
        /// </summary>
        private readonly IList<ICodeRefMutableObject> _codeRefs;

        /// <summary>
        ///   The _has formal levels.
        /// </summary>
        private bool _hasFormalLevels;

        /// <summary>
        ///   The _level.
        /// </summary>
        private ILevelMutableObject _level;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="HierarchyMutableCore" /> class.
        /// </summary>
        public HierarchyMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy))
        {
            this._codeRefs = new List<ICodeRefMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public HierarchyMutableCore(IHierarchy objTarget)
            : base(objTarget)
        {
            this._codeRefs = new List<ICodeRefMutableObject>();
            if (objTarget.HierarchicalCodeObjects != null)
            {
                foreach (IHierarchicalCode hierarchicalCode in objTarget.HierarchicalCodeObjects)
                {
                    this.AddHierarchicalCode(new CodeRefMutableCore(hierarchicalCode));
                }
            }

            if (objTarget.Level != null)
            {
                this._level = new LevelMutableCore(objTarget.Level);
            }

            this._hasFormalLevels = objTarget.HasFormalLevels();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the child level.
        /// </summary>
        public ILevelMutableObject ChildLevel
        {
            get
            {
                return this._level;
            }

            set
            {
                this._level = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether formal levels.
        /// </summary>
        public bool FormalLevels
        {
            get
            {
                return this._hasFormalLevels;
            }

            set
            {
                this._hasFormalLevels = value;
            }
        }

        /// <summary>
        ///   Gets the hierarchical code objects.
        /// </summary>
        public IList<ICodeRefMutableObject> HierarchicalCodeObjects
        {
            get
            {
                return this._codeRefs;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add hierarchical code.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref. 
        /// </param>
        public void AddHierarchicalCode(ICodeRefMutableObject codeRef)
        {
            this._codeRefs.Add(codeRef);
        }

        #endregion
    }
}