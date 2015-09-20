// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The item map mutable core.
    /// </summary>
    [Serializable]
    public class ItemMapMutableCore : MutableCore, IItemMapMutableObject
    {
        #region Fields

        /// <summary>
        ///   The source id.
        /// </summary>
        private string sourceId;

        /// <summary>
        ///   The target id.
        /// </summary>
        private string targetId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ItemMapMutableCore" /> class.
        /// </summary>
        public ItemMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ItemMap))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMapMutableCore"/> class.
        /// </summary>
        /// <param name="itemMap">
        /// The itemMap. 
        /// </param>
        public ItemMapMutableCore(IItemMap itemMap)
            : base(itemMap)
        {
            this.SourceId = itemMap.SourceId;
            this.TargetId = itemMap.TargetId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the source id.
        /// </summary>
        public string SourceId
        {
            get
            {
                return this.sourceId;
            }

            set
            {
                this.sourceId = value;
            }
        }

        /// <summary>
        ///   Gets or sets the target id.
        /// </summary>
        public string TargetId
        {
            get
            {
                return this.targetId;
            }

            set
            {
                this.targetId = value;
            }
        }

        #endregion
    }
}