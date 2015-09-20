// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The level core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The level core.
    /// </summary>
    [Serializable]
    public class LevelCore : NameableCore, ILevelObject
    {
        #region Fields

        /// <summary>
        ///   The itext format.
        /// </summary>
        private readonly ITextFormat textFormat;

        /// <summary>
        ///   The child level.
        /// </summary>
        private readonly ILevelObject childLevel;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="levelMutable">
        /// The level mutable. 
        /// </param>
        public LevelCore(IIdentifiableObject parent, ILevelMutableObject levelMutable)
            : base(levelMutable, parent)
        {
            if (levelMutable.CodingFormat != null)
            {
                this.textFormat = new TextFormatObjectCore(levelMutable.CodingFormat, this);
            }

            if (levelMutable.ChildLevel != null)
            {
                this.childLevel = new LevelCore(this, levelMutable.ChildLevel);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="levels">
        /// The levels. 
        /// </param>
        /// <param name="pos">
        /// The pos. 
        /// </param>
        public LevelCore(IIdentifiableObject parent, IList<LevelType> levels, int pos)
            : base(
                GetLevel(levels, pos), 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Level), 
                GetLevel(levels, pos).id, 
                null, 
                GetLevel(levels, pos).Name, 
                GetLevel(levels, pos).Description, 
                GetLevel(levels, pos).Annotations, 
                parent)
        {
            LevelType level = GetLevel(levels, pos);
            if (level.CodingType != null)
            {
                this.textFormat = new TextFormatObjectCore(level.CodingType, this);
            }

            if (levels.Count > pos + 1)
            {
                this.childLevel = new LevelCore(this, levels, ++pos);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="level">
        /// The level. 
        /// </param>
        public LevelCore(IIdentifiableObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.LevelType level)
            : base(level, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Level), parent)
        {
            if (level.CodingFormat != null)
            {
                this.textFormat = new TextFormatObjectCore(level.CodingFormat, this);
            }

            if (level.Level != null)
            {
                this.childLevel = new LevelCore(this, level.Level);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the child level.
        /// </summary>
        public virtual ILevelObject ChildLevel
        {
            get
            {
                return this.childLevel;
            }
        }

        /// <summary>
        ///   Gets the coding format.
        /// </summary>
        public virtual ITextFormat CodingFormat
        {
            get
            {
                return this.textFormat;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
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
                var that = (ILevelObject)sdmxObject;
                if (!this.Equivalent(this.childLevel, that.ChildLevel, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.textFormat, that.CodingFormat, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        ///   The has child.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasChild()
        {
            return this.childLevel != null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get level.
        /// </summary>
        /// <param name="levels">
        /// The levels. 
        /// </param>
        /// <param name="pos">
        /// The pos. 
        /// </param>
        /// <returns>
        /// The <see cref="LevelType"/> . 
        /// </returns>
        private static LevelType GetLevel(IList<LevelType> levels, int pos)
        {
            return levels[pos];
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       ///   The get composites internal.
       /// </summary>
      protected override ISet<ISdmxObject> GetCompositesInternal()
      {
    	ISet<ISdmxObject> composites = base.GetCompositesInternal();
        base.AddToCompositeSet(this.childLevel, composites);
        base.AddToCompositeSet(this.textFormat, composites);
        return composites;
      }

        #endregion
    }
}