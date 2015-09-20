// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderSchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data provider scheme mutable core.
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
    ///   The data provider scheme mutable core.
    /// </summary>
    [Serializable]
    public class DataProviderSchemeMutableCore :
        ItemSchemeMutableCore<IDataProviderMutableObject, IDataProvider, IDataProviderScheme>, 
        IDataProviderSchemeMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeMutableCore"/> class.
        /// </summary>
        /// <param name="dataProviderScheme">
        /// The dataProviderScheme. 
        /// </param>
        public DataProviderSchemeMutableCore(IDataProviderScheme dataProviderScheme)
            : base(dataProviderScheme)
        {
            foreach (IDataProvider dataProvider in dataProviderScheme.Items)
            {
                this.AddItem(new DataProviderMutableCore(dataProvider));
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataProviderSchemeMutableCore" /> class.
        /// </summary>
        public DataProviderSchemeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IDataProviderScheme ImmutableInstance
        {
            get
            {
                return new DataProviderSchemeCore(this);
            }
        }

        #endregion

        #region Explicit Interface Properties

       
       

        #endregion

        #region Overrides of ItemSchemeMutableCore<IDataProviderMutableObject,IDataProvider,IDataProviderScheme>

        public override IDataProviderMutableObject CreateItem(string id, string name)
        {
            IDataProviderMutableObject dataProviderMutableCore = new DataProviderMutableCore();
            dataProviderMutableCore.Id = id;
            dataProviderMutableCore.AddName("en", name);
            AddItem(dataProviderMutableCore);
            return dataProviderMutableCore;
        }

        #endregion

        
    }
}