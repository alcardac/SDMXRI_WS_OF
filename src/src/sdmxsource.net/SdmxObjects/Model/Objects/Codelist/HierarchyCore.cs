// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchy core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using HierarchyType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.HierarchyType;
    using LevelType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.LevelType;
    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The hierarchy core.
    /// </summary>
    [Serializable]
    public class HierarchyCore : NameableCore, IHierarchy
    {
        #region Fields

        /// <summary>
        ///   The _code refs.
        /// </summary>
        private readonly IList<IHierarchicalCode> _codeRefs;

        /// <summary>
        ///   The _has formal levels.
        /// </summary>
        private readonly bool _hasFormalLevels;

        /// <summary>
        ///   The _level.
        /// </summary>
        private readonly ILevelObject _level;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="hierarchy">
        /// The hierarchy. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchyCore(IHierarchicalCodelistObject parent, IHierarchyMutableObject hierarchy)
            : base(hierarchy, parent)
        {
            // LEVELS MUST BE SET BEFORE ANYTHING ELSE
            this._hasFormalLevels = hierarchy.FormalLevels;
            if (hierarchy.ChildLevel != null)
            {
                this._level = new LevelCore(this, hierarchy.ChildLevel);
            }

            this._codeRefs = new List<IHierarchicalCode>();

            if (hierarchy.HierarchicalCodeObjects != null)
            {
                foreach (ICodeRefMutableObject currentCoderef in hierarchy.HierarchicalCodeObjects)
                {
                    this._codeRefs.Add(new HierarchicalCodeCore(currentCoderef, this));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="hierarchy">
        /// The hierarchy. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchyCore(IHierarchicalCodelistObject parent, HierarchyType hierarchy)
            : base(hierarchy, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy), parent)
        {
            this._codeRefs = new List<IHierarchicalCode>();

            // LEVELS MUST BE SET BEFORE ANYTHING ELSE
            this._hasFormalLevels = hierarchy.leveled;

            if (hierarchy.Level != null)
            {
                this._level = new LevelCore(this, hierarchy.Level);
            }

            if (hierarchy.HierarchicalCode != null)
            {
                foreach (HierarchicalCodeType currentCoderef in hierarchy.HierarchicalCode)
                {
                    this._codeRefs.Add(new HierarchicalCodeCore(currentCoderef, this));
                }
            }

            try
            {
                if (hierarchy.leveled)
                {
                    if (this._level == null)
                    {
                        throw new SdmxSemmanticException(
                            "Hierarchy declares itself as levelled, but does not define any levels");
                    }
                }

                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="hierarchy">
        /// The hierarchy. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public HierarchyCore(
            IHierarchicalCodelistObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.HierarchyType hierarchy)
            : base(
                hierarchy, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy), 
                hierarchy.id, 
                null, 
                hierarchy.Name, 
                hierarchy.Description, 
                hierarchy.Annotations, 
                parent)
        {
            // LEVELS MUST BE SET BEFORE ANYTHING ELSE
            this._codeRefs = new List<IHierarchicalCode>();

            if (ObjectUtil.ValidCollection(hierarchy.Level))
            {
                var levelMap = new SortedList<int, LevelType>();
                foreach (LevelType levl in hierarchy.Level)
                {
                    levelMap.Add(Convert.ToInt32(levl.Order), levl);
                }

                IList<LevelType> levelList = new List<LevelType>();

                for (int i = 1; i <= levelMap.Count; i++)
                {
                    if (levelMap.ContainsKey(i))
                    {
                        levelList.Add(levelMap[i]);
                    }
                    else
                    {
                        // %%% .Factory.NewInstance();
                        var defaultLevel = new LevelType();

                        // %%% defaultLevel.AddNewName().SetStringValue("Default");
                        defaultLevel.id = "DEFAULT";
                        var t = new TextType();
                        t.TypedValue = "Default";
                        defaultLevel.Name.Add(t);
                        levelList.Add(defaultLevel);
                    }
                }

                this._level = new LevelCore(this, levelList, 0);
                this._hasFormalLevels = true;
            }

            if (hierarchy.CodeRef != null)
            {
                foreach (CodeRefType currentCoderef in hierarchy.CodeRef)
                {
                    this._codeRefs.Add(new HierarchicalCodeCore(currentCoderef, this));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the hierarchical code objects.
        /// </summary>
        public virtual IList<IHierarchicalCode> HierarchicalCodeObjects
        {
            get
            {
                return new List<IHierarchicalCode>(this._codeRefs);
            }
        }

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        /// <summary>
        ///   Gets the level.
        /// </summary>
        public virtual ILevelObject Level
        {
            get
            {
                return this._level;
            }
        }

        /// <summary>
        ///   Gets the maintainable parent.
        /// </summary>
        public new IHierarchicalCodelistObject MaintainableParent
        {
            get
            {
                return (IHierarchicalCodelistObject)base.MaintainableParent;
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
            if (sdmxObject == null)
            {
                return false;
            }

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IHierarchy)sdmxObject;
                if (!this.Equivalent(this._codeRefs, that.HierarchicalCodeObjects, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._level, that.Level, includeFinalProperties))
                {
                    return false;
                }

                if (this._hasFormalLevels != that.HasFormalLevels())
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get level at position.
        /// </summary>
        /// <param name="levelPos">
        /// The level pos. 
        /// </param>
        /// <returns>
        /// The <see cref="ILevelObject"/> . 
        /// </returns>
        public virtual ILevelObject GetLevelAtPosition(int levelPos)
        {
            ILevelObject currentLevel = this._level;
            for (int i = 1; i <= levelPos; i++)
            {
                if (currentLevel == null)
                {
                    return null;
                }

                if (i == levelPos)
                {
                    return currentLevel;
                }

                currentLevel = currentLevel.ChildLevel;
            }

            return null;
        }

        /// <summary>
        ///   The has formal levels.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasFormalLevels()
        {
            return this._hasFormalLevels;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (!ObjectUtil.ValidCollection(this._codeRefs))
            {
                throw new SdmxSemmanticException("Hierarchy must contain at least one hierarchical code");
            }

            if (this._hasFormalLevels)
            {
                this.ValidateHasLevel(this._codeRefs);
            }
        }

        /// <summary>
        /// The validate has level.
        /// </summary>
        /// <param name="codeRefs0">
        /// The code refs 0. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private void ValidateHasLevel(IList<IHierarchicalCode> codeRefs0)
        {
            if (codeRefs0 != null)
            {
                foreach (IHierarchicalCode currentHCode in codeRefs0)
                {
                    this.ValidateHasLevel(currentHCode.CodeRefs);
                    if (currentHCode.GetLevel(true) == null)
                    {
                        throw new SdmxSemmanticException(
                            "Hierarchy indicates formal levels, but Hierarchical Code '" + currentHCode.Urn
                            +
                            "' is missing a level reference and there is no default level for it's depth in the hierarchy");
                    }
                }
            }
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
        base.AddToCompositeSet(this._codeRefs, composites);
        base.AddToCompositeSet(this._level, composites);
        return composites;
     }

        #endregion
    }
}