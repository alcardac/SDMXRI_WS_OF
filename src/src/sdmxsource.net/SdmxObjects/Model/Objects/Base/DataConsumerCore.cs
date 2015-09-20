// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConsumerBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data consumer core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    using OrganisationType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationType;

    /// <summary>
    ///   The data consumer core.
    /// </summary>
    [Serializable]
    public class DataConsumerCore : OrganisationCore<IDataConsumer>, IDataConsumer
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataConsumerCore(IDataConsumerMutableObject itemMutableObject, IDataConsumerScheme parent)
            : base(itemMutableObject, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataConsumerCore(DataConsumerType type, IDataConsumerScheme parent)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer), parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerCore"/> class.
        /// </summary>
        /// <param name="organisationType">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataConsumerCore(OrganisationType organisationType, IDataConsumerScheme parent)
            : // base(sdmxObject,  SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer), sdmxObject.id, sdmxObject.uri )
                base(
                organisationType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer), 
                organisationType.CollectorContact,
                organisationType.id, 
                organisationType.uri, 
                organisationType.Name, 
                organisationType.Description, 
                organisationType.Annotations, 
                parent)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsNameable((IDataConsumer)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        public new IDataConsumerScheme MaintainableParent
        {
            get
            {
                return (IDataConsumerScheme)base.MaintainableParent;
            }
        }

        #endregion
    }
}