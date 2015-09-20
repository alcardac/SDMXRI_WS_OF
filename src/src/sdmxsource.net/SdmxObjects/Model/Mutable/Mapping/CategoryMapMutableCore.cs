// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///     The category map mutable core.
    /// </summary>
    [Serializable]
    public class CategoryMapMutableCore : MutableCore, ICategoryMapMutableObject
    {
        #region Fields

        /// <summary>
        ///     The source id.
        /// </summary>
        private readonly IList<string> _sourceId;

        /// <summary>
        ///     The target id.
        /// </summary>
        private readonly IList<string> _targetId;

        /// <summary>
        ///     The alias.
        /// </summary>
        private string _alias;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryMapMutableCore" /> class.
        /// </summary>
        public CategoryMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap))
        {
            this._sourceId = new List<string>();
            this._targetId = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMapMutableCore"/> class.
        /// </summary>
        /// <param name="catType">
        /// The cat type.
        /// </param>
        public CategoryMapMutableCore(ICategoryMap catType)
            : base(catType)
        {
            this._sourceId = new List<string>();
            this._targetId = new List<string>();
            this._alias = catType.Alias;
            if (catType.SourceId != null)
            {
                this._sourceId = catType.SourceId;
            }

            if (catType.TargetId != null)
            {
                this._targetId = catType.TargetId;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the alias.
        /// </summary>
        public virtual string Alias
        {
            get
            {
                return this._alias;
            }

            set
            {
                this._alias = value;
            }
        }

        /// <summary>
        ///     Gets the source id.
        /// </summary>
        public virtual IList<string> SourceId
        {
            get
            {
                return this._sourceId;
            }
        }

        /// <summary>
        ///     Gets the target id.
        /// </summary>
        public virtual IList<string> TargetId
        {
            get
            {
                return this._targetId;
            }
        }

        #endregion
    }
}