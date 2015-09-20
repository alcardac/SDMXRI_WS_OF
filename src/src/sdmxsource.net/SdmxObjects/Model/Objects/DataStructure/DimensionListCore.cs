// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionListBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension list core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using DimensionList = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.DimensionList;
    using DimensionType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.DimensionType;

    /// <summary>
    ///   The dimension list core.
    /// </summary>
    [Serializable]
    public class DimensionListCore : IdentifiableCore, IDimensionList
    {
        #region Fields

        /// <summary>
        ///   The dimensions.
        /// </summary>
        private readonly IList<IDimension> dimensions;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionListCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DimensionListCore(IDimensionListMutableObject itemMutableObject, IDataStructureObject parent)
            : base(itemMutableObject, parent)
        {
            this.dimensions = new List<IDimension>();
            if (itemMutableObject.Dimensions != null)
            {
                int pos = 1;

                foreach (IDimensionMutableObject currentDimension in itemMutableObject.Dimensions)
                {
                    this.dimensions.Add(new DimensionCore(currentDimension, pos, this));
                    pos++;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionListCore"/> class.
        /// </summary>
        /// <param name="dimensionList">
        /// The dimension list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DimensionListCore(DimensionListType dimensionList, IMaintainableObject parent)
            : base(dimensionList, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptor), parent)
        {
            this.dimensions = new List<IDimension>();
            int pos = 1;
            if (dimensionList.Dimension != null)
            {
                foreach (Dimension dimension in dimensionList.Dimension)
                {
                    this.dimensions.Add(new DimensionCore(dimension.Content, this, pos));
                    pos++;
                }
            }

            if (ObjectUtil.ValidCollection(dimensionList.MeasureDimension))
            {
                if (dimensionList.MeasureDimension.Count > 1)
                {
                    throw new SdmxSemmanticException("Can not have more then one measure dimension");
                }

                this.dimensions.Add(new DimensionCore(dimensionList.MeasureDimension[0].Content, this, pos));
                pos++;
            }

            if (ObjectUtil.ValidCollection(dimensionList.TimeDimension))
            {
                if (dimensionList.TimeDimension.Count > 1)
                {
                    throw new SdmxSemmanticException("Can not have more then one time dimension");
                }

                this.dimensions.Add(new DimensionCore(dimensionList.TimeDimension[0].Content, this, pos));
            }

            this.ValidateDimensionList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionListCore"/> class.
        /// </summary>
        /// <param name="keyFamily">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        public DimensionListCore(KeyFamilyType keyFamily, IMaintainableObject parent)
            : base(
                DimensionList.FixedId, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptor), 
                parent)
        {
            this.dimensions = new List<IDimension>();
            int pos = 1;
            ComponentsType components = keyFamily.Components;
            try
            {
                if (components != null)
                {
                    foreach (DimensionType currentDimension in components.Dimension)
                    {
                        this.dimensions.Add(new DimensionCore(currentDimension, this, pos));
                        pos++;
                    }

                    if (components.TimeDimension != null)
                    {
                        this.dimensions.Add(new DimensionCore(components.TimeDimension, this, pos));
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            this.ValidateDimensionList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionListCore"/> class.
        /// </summary>
        /// <param name="keyFamily">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DimensionListCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.KeyFamilyType keyFamily, IMaintainableObject parent)
            : base(
                DimensionList.FixedId, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptor), 
                parent)
        {
            this.dimensions = new List<IDimension>();
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.ComponentsType components = keyFamily.Components;
            int pos = 1;
            if (components != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.DimensionType currentDimension in
                    components.Dimension)
                {
                    this.dimensions.Add(new DimensionCore(currentDimension, this, pos));
                    pos++;
                }

                if (components.TimeDimension != null)
                {
                    this.dimensions.Add(new DimensionCore(components.TimeDimension, this, pos));
                }
            }

            this.ValidateDimensionList();
        }

        #endregion

        #region Public Properties

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
        ///   Gets the dimensions.
        /// </summary>
        public virtual IList<IDimension> Dimensions
        {
            get
            {
                return new List<IDimension>(this.dimensions);
            }
        }

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return DimensionList.FixedId;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
                var that = (IDimensionList)sdmxObject;
                if (!this.Equivalent(this.Dimensions, that.Dimensions, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate dimension list.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void ValidateDimensionList()
        {
            IList<string> idList = new List<string>();
            ((List<IDimension>)this.dimensions).Sort();

            foreach (IDimension dimension in this.dimensions)
            {
                if (idList.Contains(dimension.Id))
                {
                    throw new SdmxSemmanticException("Duplicate Dimensions Id : " + dimension.Id);
                }
            }
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       ///   Get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
        	ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(dimensions, composites);
            return composites;
       }

        #endregion
    }
}