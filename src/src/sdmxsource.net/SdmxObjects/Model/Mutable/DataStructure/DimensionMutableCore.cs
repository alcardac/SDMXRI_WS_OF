// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The dimension mutable core.
    /// </summary>
    [Serializable]
    public class DimensionMutableCore : ComponentMutableCore, IDimensionMutableObject
    {
        #region Fields

        /// <summary>
        ///   The concept role.
        /// </summary>
        private readonly IList<IStructureReference> _conceptRole;

        /// <summary>
        ///   The frequency dimension.
        /// </summary>
        private bool _frequencyDimension;

        /// <summary>
        ///   The measure dimension.
        /// </summary>
        private bool _measureDimension;

        /// <summary>
        ///   The time dimension.
        /// </summary>
        private bool _timeDimension;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DimensionMutableCore" /> class.
        /// </summary>
        public DimensionMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension))
        {
            this._conceptRole = new List<IStructureReference>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public DimensionMutableCore(IDimension objTarget)
            : base(objTarget)
        {
            this._conceptRole = new List<IStructureReference>();
            this._measureDimension = objTarget.MeasureDimension;
            this._frequencyDimension = objTarget.FrequencyDimension;
            this._timeDimension = objTarget.TimeDimension;
            if (objTarget.ConceptRole != null)
            {
                foreach (ICrossReference currentConceptRole in objTarget.ConceptRole)
                {
                    this._conceptRole.Add(currentConceptRole.CreateMutableInstance());
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the concept role.
        /// </summary>
        public IList<IStructureReference> ConceptRole
        {
            get
            {
                return this._conceptRole;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether frequency dimension.
        /// </summary>
        public bool FrequencyDimension
        {
            get
            {
                return this._frequencyDimension;
            }

            set
            {
                this._frequencyDimension = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether measure dimension.
        /// </summary>
        public bool MeasureDimension
        {
            get
            {
                return this._measureDimension;
            }

            set
            {
                if (value)
                {
                    this.StructureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDimension);
                }

                this._measureDimension = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether time dimension.
        /// </summary>
        public bool TimeDimension
        {
            get
            {
                return this._timeDimension;
            }

            set
            {
                if (value)
                {
                    this.StructureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeDimension);
                }

                this._timeDimension = value;
            }
        }

        #endregion
    }
}