// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodelistMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical codelist mutable core.
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
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist;

    /// <summary>
    ///   The hierarchical codelist mutable core.
    /// </summary>
    [Serializable]
    public class HierarchicalCodelistMutableCore : MaintainableMutableCore<IHierarchicalCodelistObject>, 
                                                   IHierarchicalCodelistMutableObject
    {
        #region Fields

        /// <summary>
        ///   The codelist ref.
        /// </summary>
        private readonly IList<ICodelistRefMutableObject> _codelistRef;

        /// <summary>
        ///   The hierarchies.
        /// </summary>
        private readonly IList<IHierarchyMutableObject> _hierarchies;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="HierarchicalCodelistMutableCore" /> class.
        /// </summary>
        public HierarchicalCodelistMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist))
        {
            this._hierarchies = new List<IHierarchyMutableObject>();
            this._codelistRef = new List<ICodelistRefMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodelistMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public HierarchicalCodelistMutableCore(IHierarchicalCodelistObject objTarget)
            : base(objTarget)
        {
            this._hierarchies = new List<IHierarchyMutableObject>();
            this._codelistRef = new List<ICodelistRefMutableObject>();

            // Convert the lists into mutable objTarget lists
            if (objTarget.Hierarchies != null)
            {
                foreach (IHierarchy hierarchy in objTarget.Hierarchies)
                {
                    this.AddHierarchies(new HierarchyMutableCore(hierarchy));
                }
            }

            if (objTarget.CodelistRef != null)
            {
                foreach (ICodelistRef c in objTarget.CodelistRef)
                {
                    this.AddCodelistRef(new CodelistRefMutableCore(c));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the codelist ref.
        /// </summary>
        public IList<ICodelistRefMutableObject> CodelistRef
        {
            get
            {
                return this._codelistRef;
            }
        }

        /// <summary>
        ///   Gets the hierarchies.
        /// </summary>
        public IList<IHierarchyMutableObject> Hierarchies
        {
            get
            {
                return this._hierarchies;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IHierarchicalCodelistObject ImmutableInstance
        {
            get
            {
                return new HierarchicalCodelistObjectCore(this);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add codelist ref.
        /// </summary>
        /// <param name="codeListReference">
        /// The codelist ref 0. 
        /// </param>
        public void AddCodelistRef(ICodelistRefMutableObject codeListReference)
        {
            this._codelistRef.Add(codeListReference);
        }

        /// <summary>
        /// The add hierarchies.
        /// </summary>
        /// <param name="hierarchy">
        /// The hierarchy. 
        /// </param>
        public void AddHierarchies(IHierarchyMutableObject hierarchy)
        {
            this._hierarchies.Add(hierarchy);
        }

        #endregion
    }
}