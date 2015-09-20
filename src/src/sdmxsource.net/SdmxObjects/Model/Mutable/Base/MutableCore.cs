// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///   The mutable core.
    /// </summary>
    [Serializable]
    public abstract class MutableCore : IMutableObject
    {
        #region Fields

        /// <summary>
        ///   The structure type.
        /// </summary>
        private SdmxStructureType structureType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableCore"/> class.
        /// </summary>
        /// <param name="structureType0">
        /// The structure type 0. 
        /// </param>
        protected internal MutableCore(SdmxStructureType structureType0)
        {
            this.structureType = structureType0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        protected internal MutableCore(ISdmxObject createdFrom)
        {
            this.structureType = createdFrom.StructureType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the structure type.
        /// </summary>
        public virtual SdmxStructureType StructureType
        {
            get
            {
                return this.structureType;
            }

            set
            {
                this.structureType = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create tertiary.
        /// </summary>
        /// <param name="isSet">
        /// The is set. 
        /// </param>
        /// <param name="valueren">
        /// The valueren. 
        /// </param>
        /// <returns>
        /// The <see cref="TertiaryBool"/> . 
        /// </returns>
        protected internal static TertiaryBool CreateTertiary(bool isSet, bool valueren)
        {
            return SdmxObjectUtil.CreateTertiary(isSet, valueren);
        }

        #endregion
    }
}