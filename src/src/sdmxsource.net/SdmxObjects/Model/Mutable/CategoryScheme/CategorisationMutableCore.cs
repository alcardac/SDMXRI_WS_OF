// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorisationMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The categorisation mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme;

    /// <summary>
    ///   The categorisation mutable core.
    /// </summary>
    [Serializable]
    public class CategorisationMutableCore : MaintainableMutableCore<ICategorisationObject>, 
                                             ICategorisationMutableObject
    {
        #region Fields

        /// <summary>
        ///   The icategory ref.
        /// </summary>
        private IStructureReference categoryRef;

        /// <summary>
        ///   The structure reference.
        /// </summary>
        private IStructureReference structureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CategorisationMutableCore" /> class.
        /// </summary>
        public CategorisationMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CategorisationMutableCore(ICategorisationObject objTarget)
            : base(objTarget)
        {
            if (objTarget.CategoryReference != null)
            {
                this.categoryRef = objTarget.CategoryReference.CreateMutableInstance();
            }

            if (objTarget.StructureReference != null)
            {
                this.structureReference = objTarget.StructureReference.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the category reference.
        /// </summary>
        public virtual IStructureReference CategoryReference
        {
            get
            {
                return this.categoryRef;
            }

            set
            {
                this.categoryRef = value;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override ICategorisationObject ImmutableInstance
        {
            get
            {
                return new CategorisationObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets or sets the structure reference.
        /// </summary>
        public virtual IStructureReference StructureReference
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