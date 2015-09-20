// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using CategoryMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.CategoryMapType;

    /// <summary>
    ///   The category map core.
    /// </summary>
    [Serializable]
    public class CategoryMapCore : SdmxStructureCore, ICategoryMap
    {
        #region Fields

        /// <summary>
        ///   The alias.
        /// </summary>
        private readonly string alias;

        /// <summary>
        ///   The source id.
        /// </summary>
        private readonly IList<string> sourceId;

        /// <summary>
        ///   The target id.
        /// </summary>
        private readonly IList<string> targetId;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMapCore"/> class.
        /// </summary>
        /// <param name="categoryMapMutable"> Category Map object
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal CategoryMapCore(ICategoryMapMutableObject categoryMapMutable, ISdmxStructure parent)
            : base(categoryMapMutable, parent)
        {
            this.sourceId = new List<string>();
            this.targetId = new List<string>();
            this.alias = categoryMapMutable.Alias;
            if (categoryMapMutable.SourceId != null)
            {
                this.sourceId = new List<string>(categoryMapMutable.SourceId);
            }

            if (categoryMapMutable.TargetId != null)
            {
                this.targetId = new List<string>(categoryMapMutable.TargetId);
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMapCore"/> class.
        /// </summary>
        /// <param name="catType">
        /// The cat type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal CategoryMapCore(CategoryMapType catType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap), parent)
        {
            this.sourceId = new List<string>();
            this.targetId = new List<string>();

            // FUNC 2.1 - this referernce is wrong as it only allows for a single id
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMapCore"/> class.
        /// </summary>
        /// <param name="catType">
        /// The cat type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal CategoryMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryMapType catType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap), parent)
        {
            this.sourceId = new List<string>();
            this.targetId = new List<string>();
            this.alias = catType.categoryAlias;
            PopulateCategoryIdList(this.sourceId, catType.CategoryID);
            PopulateCategoryIdList(this.targetId, catType.TargetCategoryID);
            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the alias.
        /// </summary>
        public virtual string Alias
        {
            get
            {
                return this.alias;
            }
        }

        /// <summary>
        ///   Gets the source id.
        /// </summary>
        public virtual IList<string> SourceId
        {
            get
            {
                return new List<string>(this.sourceId);
            }
        }

        /// <summary>
        ///   Gets the target id.
        /// </summary>
        public virtual IList<string> TargetId
        {
            get
            {
                return new List<string>(this.targetId);
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
                var that = (ICategoryMap)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this.sourceId, that.SourceId))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.targetId, that.TargetId))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.alias, that.Alias))
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
            if (this.sourceId == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, "CategoryMap", "CategoryID");
            }

            if (this.targetId == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, "CategoryMap", "TargetCategoryID");
            }
        }

        /// <summary>
        /// The populate category id list.
        /// </summary>
        /// <param name="catIdList">
        /// The cat id list. 
        /// </param>
        /// <param name="currentIdType">
        /// The current id type. 
        /// </param>
        private static void PopulateCategoryIdList(ICollection<string> catIdList, CategoryIDType currentIdType)
        {
            // TODO possibly use RefUtl.GetCateogryIds
            while (currentIdType != null)
            {
                catIdList.Add(currentIdType.ID);
                currentIdType = currentIdType.CategoryID;
            }
        }

        #endregion
    }
}