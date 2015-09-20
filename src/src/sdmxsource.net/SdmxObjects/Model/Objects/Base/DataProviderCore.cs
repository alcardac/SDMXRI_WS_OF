// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data provider core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    using OrganisationType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationType;

    /// <summary>
    ///   The data provider core.
    /// </summary>
    [Serializable]
    public class DataProviderCore : OrganisationCore<IDataProvider>, IDataProvider
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataProviderCore(IDataProviderMutableObject itemMutableObject, IDataProviderScheme parent)
            : base(itemMutableObject, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataProviderCore(DataProviderType type, IDataProviderScheme parent)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderCore"/> class.
        /// </summary>
        /// <param name="organisation">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataProviderCore(OrganisationType organisation, IDataProviderScheme parent)
            : base(
                organisation, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), 
                organisation.CollectorContact,
                organisation.id, 
                organisation.uri, 
                organisation.Name, 
                organisation.Description, 
                organisation.Annotations, 
                parent)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the cross referenced constrainables.
        /// </summary>
        public virtual IList<ICrossReference> CrossReferencedConstrainables
        {
            get
            {
                return new List<ICrossReference>();
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
                return this.DeepEqualsNameable((IDataProvider)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        #endregion

        #region Implementation of IDataProvider

        new public IDataProviderScheme MaintainableParent 
        { 
            get
            {
                return (IDataProviderScheme)base.MaintainableParent;
            }
        }

        #endregion

        #region Methods

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////	

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
           return new HashSet<ISdmxObject>();
        }

        #endregion
    }
}