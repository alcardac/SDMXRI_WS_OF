// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The item map core.
    /// </summary>
    [Serializable]
    public class ItemMapCore : SdmxStructureCore, IItemMap
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMapCore"/> class.
        /// </summary>
        /// <param name="itemMapMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemMapCore(IItemMapMutableObject itemMapMutableObject, ISdmxStructure parent)
            : base(itemMapMutableObject, parent)
        {
            this.sourceId = itemMapMutableObject.SourceId;
            this.targetId = itemMapMutableObject.TargetId;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMapCore"/> class.
        /// </summary>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="target">
        /// The target. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemMapCore(string id, string target, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ItemMap), parent)
        {
            this.sourceId = id;
            this.targetId = target;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMapCore"/> class.
        /// </summary>
        /// <param name="alias">
        /// The alias. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="target">
        /// The target. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemMapCore(string alias, string id, string target, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ItemMap), parent)
        {
            this.sourceId = id;
            this.targetId = target;
            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the source id.
        /// </summary>
        public virtual string SourceId
        {
            get
            {
                return this.sourceId;
            }
        }

        /// <summary>
        ///   Gets the target id.
        /// </summary>
        public virtual string TargetId
        {
            get
            {
                return this.targetId;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
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
                var that = (IItemMap)sdmxObject;
                if (!ObjectUtil.Equivalent(this.sourceId, that.SourceId))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.targetId, that.TargetId))
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
        protected internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.sourceId))
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, "ItemMap", "Source Id");
            }

            if (string.IsNullOrWhiteSpace(this.targetId))
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, "ItemMap", "Target Id");
            }

            this.sourceId = ValidationUtil.CleanAndValidateId(this.sourceId, true);
            this.targetId = ValidationUtil.CleanAndValidateId(this.targetId, true);
        }

        #endregion
    }
}