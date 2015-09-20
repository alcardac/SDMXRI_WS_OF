// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OraganisationSchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation scheme object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    /// The organisation scheme object core.
    /// </summary>
    /// <typeparam name="TItem">Generic type param of type: IOrganisation
    /// </typeparam>
    /// <typeparam name="TMaint">Generic type param of type: IMaintainableObject
    /// </typeparam>
    /// <typeparam name="TMaintMutable">Generic type param of type: IMaintainableMutableObject
    /// </typeparam>
    /// <typeparam name="TItemMutable">Generic type param of type: IItemMutableObject
    /// </typeparam>
    [Serializable]
    public abstract class OrganisationSchemeCore<TItem, TMaint, TMaintMutable, TItemMutable> :
        ItemSchemeObjectCore<TItem, TMaint, TMaintMutable, TItemMutable>,
        IOrganisationScheme<TItem>
        where TItem : IOrganisation
        where TMaint : IMaintainableObject
        where TMaintMutable : IItemSchemeMutableObject<TItemMutable>
        where TItemMutable : IItemMutableObject
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The itemMutableObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        protected internal OrganisationSchemeCore(
            IItemSchemeObject<TItem> agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this.ValidateOrganisationSchemeAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        /* @SuppressWarnings("rawtypes")*/

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class. 
        ///   The organisation scheme object core.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The itemMutableObject. 
        /// </param>
        /// $$$ ??? IItemSchemeMutableObject whitout T?
        protected internal OrganisationSchemeCore(IItemSchemeMutableObject<TItemMutable> itemMutableObject)
            : base(itemMutableObject)
        {
            this.ValidateOrganisationSchemeAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected internal OrganisationSchemeCore(
            OrganisationSchemeType createdFrom, SdmxStructureType structureType)
            : base(createdFrom, structureType)
        {
            this.ValidateOrganisationSchemeAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="endDate">
        /// The end date. 
        /// </param>
        /// <param name="startDate">
        /// The start date. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="isFinal">
        /// The is final. 
        /// </param>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="description">
        /// The description. 
        /// </param>
        /// <param name="isExternalReference">
        /// The is external reference. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        protected internal OrganisationSchemeCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            object endDate, 
            object startDate, 
            string version, 
            TertiaryBool isFinal, 
            string agencyId, 
            string id, 
            Uri uri, 
            IList<TextType> name, 
            IList<TextType> description, 
            TertiaryBool isExternalReference, 
            AnnotationsType annotationsType)
            : base(
                createdFrom, 
                structureType, 
                endDate, 
                startDate, 
                version, 
                isFinal, 
                agencyId, 
                id, 
                uri, 
                name, 
                description, 
                isExternalReference, 
                annotationsType)
        {
            this.ValidateOrganisationSchemeAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeCore{TItem,TMaint,TMaintMutable,TItemMutable}"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="isExternalReference">
        /// The is external reference. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        protected internal OrganisationSchemeCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string version, 
            string agencyId, 
            string id, 
            Uri uri, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> name, 
            TertiaryBool isExternalReference, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationsType)
            : base(createdFrom, structureType, version, agencyId, id, uri, name, isExternalReference, annotationsType)
        {
            this.ValidateOrganisationSchemeAttributes();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate organisation scheme attributes.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void ValidateOrganisationSchemeAttributes()
        {
            if (this.IsFinal.IsTrue)
            {
                throw new SdmxSemmanticException("Organisation Schemes can not be set to final");
            }
        }

        #endregion

        #region Implementation of IOrganisationScheme<TItem>

        
        //public abstract IOrganisationSchemeMutableObject<IOrganisationMutableObject> MutableInstance { get; }

      
        #endregion

        #region Implementation of IOrganisationScheme

        IOrganisationSchemeMutableObject<IOrganisationScheme<TItem>, IOrganisationMutableObject> IOrganisationScheme<TItem>.MutableInstance
        {
            get
            {
                //TODO: Redesign
                return (IOrganisationSchemeMutableObject<IOrganisationScheme<TItem>, IOrganisationMutableObject>)this.MutableInstance;
             
            }
        }

        public abstract override TMaintMutable MutableInstance { get; }

        #endregion

        
    }
}