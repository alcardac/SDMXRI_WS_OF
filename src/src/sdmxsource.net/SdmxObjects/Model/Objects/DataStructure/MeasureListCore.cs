// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeasureListBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The measure list core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using System.Collections.Generic;

    using MeasureList = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.MeasureList;
    using PrimaryMeasureType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.PrimaryMeasureType;

    /// <summary>
    ///   The measure list core.
    /// </summary>
    [Serializable]
    public class MeasureListCore : IdentifiableCore, IMeasureList
    {
        #region Fields

        /// <summary>
        ///   The iprimary measure.
        /// </summary>
        private readonly IPrimaryMeasure primaryMeasure;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureListCore"/> class.
        /// </summary>
        /// <param name="measureList">
        /// The measure list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public MeasureListCore(IMeasureListMutableObject measureList, IMaintainableObject parent)
            : base(measureList, parent)
        {
            if (measureList.PrimaryMeasure != null)
            {
                this.primaryMeasure = new PrimaryMeasureCore(measureList.PrimaryMeasure, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureListCore"/> class.
        /// </summary>
        /// <param name="measureList">
        /// The measure list. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public MeasureListCore(MeasureListType measureList, IMaintainableObject parent)
            : base(measureList, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDescriptor), parent)
        {
            if (measureList.Component != null)
            {
                this.primaryMeasure =
                    new PrimaryMeasureCore(
                        (Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.PrimaryMeasureType)
                        measureList.Component[0].Content, 
                        this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureListCore"/> class.
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primary measure. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public MeasureListCore(PrimaryMeasureType primaryMeasure, IMaintainableObject parent)
            : base(
                MeasureList.FixedId, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDescriptor), 
                parent)
        {
            this.primaryMeasure = new PrimaryMeasureCore(primaryMeasure, this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureListCore"/> class.
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primary measure. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public MeasureListCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.PrimaryMeasureType primaryMeasure, 
            IMaintainableObject parent)
            : base(
                MeasureList.FixedId, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDescriptor), 
                parent)
        {
            this.primaryMeasure = new PrimaryMeasureCore(primaryMeasure, this);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return MeasureList.FixedId;
            }
        }

        /// <summary>
        ///   Gets the primary measure.
        /// </summary>
        public virtual IPrimaryMeasure PrimaryMeasure
        {
            get
            {
                return this.primaryMeasure;
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
                var that = (IMeasureList)sdmxObject;
                if (!this.Equivalent(this.primaryMeasure, that.PrimaryMeasure, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
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
    	  base.AddToCompositeSet(this.primaryMeasure, composites);
    	  return composites;
       }

        #endregion
    }
}