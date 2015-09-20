// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSetTargetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data set target mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The data set target mutable core.
    /// </summary>
    [Serializable]
    public class DataSetTargetMutableCore : IdentifiableMutableCore, IDataSetTargetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The text type.
        /// </summary>
        private TextType textType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetTargetMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public DataSetTargetMutableCore(IDataSetTarget objTarget)
            : base(objTarget)
        {
            this.textType = objTarget.TextType;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataSetTargetMutableCore" /> class.
        /// </summary>
        public DataSetTargetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DatasetTarget))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the text type.
        /// </summary>
        public virtual TextType TextType
        {
            get
            {
                return this.textType;
            }

            set
            {
                this.textType = value;
            }
        }

        #endregion
    }
}