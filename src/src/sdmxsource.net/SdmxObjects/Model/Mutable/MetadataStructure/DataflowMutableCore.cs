// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;

    /// <summary>
    ///   The dataflow mutable core.
    /// </summary>
    [Serializable]
    public class DataflowMutableCore : MaintainableMutableCore<IDataflowObject>, IDataflowMutableObject
    {
        #region Fields

        /// <summary>
        ///   The key family ref.
        /// </summary>
        private IStructureReference dataStructureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataflowMutableCore" /> class.
        /// </summary>
        public DataflowMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
        {
        }

        public DataflowMutableCore(IDataStructureObject dataStructure) : base(dataStructure)
        {
            base.StructureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow);
            this.dataStructureReference = dataStructure.AsReference;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public DataflowMutableCore(IDataflowObject objTarget)
            : base(objTarget)
        {
            if (objTarget.DataStructureRef != null)
            {
                this.dataStructureReference = objTarget.DataStructureRef.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the data structure ref.
        /// </summary>
        public virtual IStructureReference DataStructureRef
        {
            get
            {
                return this.dataStructureReference;
            }

            set
            {
                this.dataStructureReference = value;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IDataflowObject ImmutableInstance
        {
            get
            {
                return new DataflowObjectCore(this);
            }
        }

        #endregion
    }
}