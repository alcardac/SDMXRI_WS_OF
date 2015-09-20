// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalMeasureMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross sectional measure mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The cross sectional measure mutable core.
    /// </summary>
    [Serializable]
    public class CrossSectionalMeasureMutableCore : ComponentMutableCore, ICrossSectionalMeasureMutableObject
    {
        #region Fields

        /// <summary>
        ///   The code.
        /// </summary>
        private string code;

        /// <summary>
        ///   The measure dimension.
        /// </summary>
        private string measureDimension;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalMeasureMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CrossSectionalMeasureMutableCore(ICrossSectionalMeasure objTarget)
            : base(objTarget)
        {
            this.measureDimension = objTarget.MeasureDimension;
            this.code = objTarget.Code;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CrossSectionalMeasureMutableCore" /> class.
        /// </summary>
        public CrossSectionalMeasureMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CrossSectionalMeasure))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the code.
        /// </summary>
        public virtual string Code
        {
            get
            {
                return this.code;
            }

            set
            {
                this.code = value;
            }
        }

        /// <summary>
        ///   Gets or sets the measure dimension.
        /// </summary>
        public virtual string MeasureDimension
        {
            get
            {
                return this.measureDimension;
            }

            set
            {
                this.measureDimension = value;
            }
        }

        #endregion
    }
}