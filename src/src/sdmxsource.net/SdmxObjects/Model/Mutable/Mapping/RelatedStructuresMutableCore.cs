// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelatedStructuresMutableCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The related structures mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The related structures mutable core.
    /// </summary>
    [Serializable]
    public class RelatedStructuresMutableCore : MutableCore, IRelatedStructuresMutableObject
    {
        #region Fields

        /// <summary>
        ///   The category scheme ref.
        /// </summary>
        private IList<IStructureReference> _categorySchemeRef;

        /// <summary>
        ///   The concept scheme ref.
        /// </summary>
        private IList<IStructureReference> _conceptSchemeRef;

        /// <summary>
        ///   The hier codelist ref.
        /// </summary>
        private IList<IStructureReference> _hierCodelistRef;

        /// <summary>
        ///   The key family ref.
        /// </summary>
        private IList<IStructureReference> _keyFamilyRef;

        /// <summary>
        ///   The metadata structure ref.
        /// </summary>
        private IList<IStructureReference> _metadataStructureRef;

        /// <summary>
        ///   The org scheme ref.
        /// </summary>
        private IList<IStructureReference> _orgSchemeRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="RelatedStructuresMutableCore" /> class.
        /// </summary>
        public RelatedStructuresMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RelatedStructures))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedStructuresMutableCore"/> class.
        /// </summary>
        /// <param name="relStrucType">
        /// The rel struc type. 
        /// </param>
        public RelatedStructuresMutableCore(IRelatedStructures relStrucType)
            : base(relStrucType)
        {
            this._keyFamilyRef = ConvertList(relStrucType.DataStructureRef);
            this._metadataStructureRef = ConvertList(relStrucType.MetadataStructureRef);
            this._conceptSchemeRef = ConvertList(relStrucType.ConceptSchemeRef);
            this._categorySchemeRef = ConvertList(relStrucType.CategorySchemeRef);
            this._orgSchemeRef = ConvertList(relStrucType.OrgSchemeRef);
            this._hierCodelistRef = ConvertList(relStrucType.HierCodelistRef);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the category scheme ref.
        /// </summary>
        public virtual IList<IStructureReference> CategorySchemeRef
        {
            get
            {
                return this._categorySchemeRef;
            }

            set
            {
                this._categorySchemeRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the concept scheme ref.
        /// </summary>
        public virtual IList<IStructureReference> ConceptSchemeRef
        {
            get
            {
                return this._conceptSchemeRef;
            }

            set
            {
                this._conceptSchemeRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the data structure ref.
        /// </summary>
        public virtual IList<IStructureReference> DataStructureRef
        {
            get
            {
                return this._keyFamilyRef;
            }

            set
            {
                this._keyFamilyRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the hier codelist ref.
        /// </summary>
        public virtual IList<IStructureReference> HierCodelistRef
        {
            get
            {
                return new ReadOnlyCollection<IStructureReference>(this._hierCodelistRef);
            }

            set
            {
                this._hierCodelistRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the metadata structure ref.
        /// </summary>
        public virtual IList<IStructureReference> MetadataStructureRef
        {
            get
            {
                return new ReadOnlyCollection<IStructureReference>(this._metadataStructureRef);
            }

            set
            {
                this._metadataStructureRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the org scheme ref.
        /// </summary>
        public virtual IList<IStructureReference> OrgSchemeRef
        {
            get
            {
                return new ReadOnlyCollection<IStructureReference>(this._orgSchemeRef);
            }

            set
            {
                this._orgSchemeRef = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The convert list.
        /// </summary>
        /// <param name="inputList">
        /// The input list. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{IStructureReference}"/> . 
        /// </returns>
        private static IList<IStructureReference> ConvertList(IEnumerable<ICrossReference> inputList)
        {
            IList<IStructureReference> returnList = new List<IStructureReference>();
            if (inputList != null)
            {
                foreach (ICrossReference currentCrossReference in inputList)
                {
                    returnList.Add(currentCrossReference.CreateMutableInstance());
                }
            }

            return returnList;
        }

        #endregion
    }
}