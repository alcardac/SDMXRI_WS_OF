// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionListMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension list mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The dimension list mutable core.
    /// </summary>
    [Serializable]
    public class DimensionListMutableCore : IdentifiableMutableCore, IDimensionListMutableObject
    {
        #region Fields

        /// <summary>
        ///   The dimensions.
        /// </summary>
        private IList<IDimensionMutableObject> dimensions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DimensionListMutableCore" /> class.
        /// </summary>
        public DimensionListMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptor))
        {
            this.dimensions = new List<IDimensionMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionListMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public DimensionListMutableCore(IDimensionList objTarget)
            : base(objTarget)
        {
            this.dimensions = new List<IDimensionMutableObject>();
            if (objTarget.Dimensions != null)
            {
                foreach (IDimension currentDimension in objTarget.Dimensions)
                {
                    this.dimensions.Add(new DimensionMutableCore(currentDimension));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the dimensions.
        /// </summary>
        public IList<IDimensionMutableObject> Dimensions
        {
            get
            {
                return this.dimensions;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        public void AddDimension(IDimensionMutableObject dimension)
        {
            if (this.dimensions == null)
            {
                this.dimensions = new List<IDimensionMutableObject>();
            }

            this.dimensions.Add(dimension);
        }

        #endregion
    }
}