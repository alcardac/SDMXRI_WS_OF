// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConsumerSchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data consumer scheme mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The data consumer scheme mutable core.
    /// </summary>
    [Serializable]
    public class DataConsumerSchemeMutableCore :
        ItemSchemeMutableCore<IDataConsumerMutableObject, IDataConsumer, IDataConsumerScheme>, 
        IDataConsumerSchemeMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerSchemeMutableCore"/> class.
        /// </summary>
        /// <param name="dataConsumerScheme">
        /// The dataConsumerScheme. 
        /// </param>
        public DataConsumerSchemeMutableCore(IDataConsumerScheme dataConsumerScheme)
            : base(dataConsumerScheme)
        {
            foreach (IDataConsumer idataConsumer in dataConsumerScheme.Items)
            {
                this.AddItem(new DataConsumerMutableCore(idataConsumer));
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataConsumerSchemeMutableCore" /> class.
        /// </summary>
        public DataConsumerSchemeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IDataConsumerScheme ImmutableInstance
        {
            get
            {
                return new DataConsumerSchemeCore(this);
            }
        }

        #endregion

        #region Overrides of ItemSchemeMutableCore<IDataConsumerMutableObject,IDataConsumer,IDataConsumerScheme>

        public override IDataConsumerMutableObject CreateItem(string id, string name)
        {
            IDataConsumerMutableObject consumerMutableCore = new DataConsumerMutableCore();
            consumerMutableCore.Id = id;
            consumerMutableCore.AddName("en", name);
            AddItem(consumerMutableCore);
            return consumerMutableCore;
        }

        #endregion

     
    }
}