// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameableCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The nameable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Util.Extensions;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The nameable core.
    /// </summary>
    [Serializable]
    public abstract class NameableCore : IdentifiableCore, INameableObject
    {
        #region Fields

        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(NameableCore));

        /// <summary>
        ///   The description.
        /// </summary>
        private IList<ITextTypeWrapper> description;

        /// <summary>
        ///   The name.
        /// </summary>
        protected IList<ITextTypeWrapper> name;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The itemMutableObject. 
        /// </param>
        protected internal NameableCore(INameableObject agencyScheme)
            : base(agencyScheme)
        {
            this.name = new List<ITextTypeWrapper>();
            this.description = new List<ITextTypeWrapper>();
            this.name = agencyScheme.Names;
            this.ValidateNameableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The itemMutableObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal NameableCore(INameableMutableObject itemMutableObject, IIdentifiableObject parent)
            : base(itemMutableObject, parent)
        {
            this.name = new List<ITextTypeWrapper>();
            this.description = new List<ITextTypeWrapper>();

            if (itemMutableObject.Names != null)
            {
                foreach (ITextTypeWrapperMutableObject mutable in itemMutableObject.Names)
                {
                    if (!string.IsNullOrWhiteSpace(mutable.Value))
                    {
                        this.name.Add(new TextTypeWrapperImpl(mutable, this));
                    }
                }
            }

            if (itemMutableObject.Descriptions != null)
            {
                foreach (ITextTypeWrapperMutableObject mutable0 in itemMutableObject.Descriptions)
                {
                    if (!string.IsNullOrWhiteSpace(mutable0.Value))
                    {
                        this.description.Add(new TextTypeWrapperImpl(mutable0, this));
                    }
                }
            }

            this.ValidateNameableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public NameableCore(SdmxStructureType structure, SdmxReader reader, IIdentifiableObject parent) {
        // super(structure, reader, parent);
        // string maintainableNode = reader.getCurrentElement();
        // while(reader.peek().equals("Name")) {
        // reader.moveNextElement();
        // string lang = reader.getAttributeValue("lang", false);
        // name.add(new TextTypeWrapperImpl(lang, reader.getCurrentElementValue(), this));
        // }
        // while(reader.peek().equals("Description")) {
        // reader.moveNextElement();
        // string lang = reader.getAttributeValue("lang", false);
        // description.add(new TextTypeWrapperImpl(lang, reader.getCurrentElementValue(), this));
        // }
        // if(!reader.getCurrentElement().equals(maintainableNode)) {
        // reader.moveBackToElement(maintainableNode);
        // }
        // validateNameableAttributes();
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected NameableCore(NameableType createdFrom, SdmxStructureType structureType, IIdentifiableObject parent)
            : base(createdFrom, structureType, parent)
        {
            this.name = new List<ITextTypeWrapper>();
            this.description = new List<ITextTypeWrapper>();
            this.name = TextTypeUtil.WrapTextTypeV21(createdFrom.Name, this);
            this.description = TextTypeUtil.WrapTextTypeV21(createdFrom.Description, this);
            this.ValidateNameableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name0">
        /// The name 0. 
        /// </param>
        /// <param name="description1">
        /// The description 1. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected NameableCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string id, 
            Uri uri, 
            IList<TextType> name0, 
            IList<TextType> description1, 
            AnnotationsType annotationsType, 
            IIdentifiableObject parent)
            : base(createdFrom, structureType, id, uri, annotationsType, parent)
        {
            this.name = new List<ITextTypeWrapper>();
            this.description = new List<ITextTypeWrapper>();
            this.name = TextTypeUtil.WrapTextTypeV2(name0, this);
            this.description = TextTypeUtil.WrapTextTypeV2(description1, this);
            this.ValidateNameableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name0">
        /// The name 0. 
        /// </param>
        /// <param name="description1">
        /// The description 1. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected NameableCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string id, 
            Uri uri, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> name0, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> description1, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationsType, 
            IIdentifiableObject parent)
            : base(createdFrom, structureType, id, uri, annotationsType, parent)
        {
            this.name = new List<ITextTypeWrapper>();
            this.description = new List<ITextTypeWrapper>();
            this.name = TextTypeUtil.WrapTextTypeV1(name0, this);
            this.description = TextTypeUtil.WrapTextTypeV1(description1, this);
            this.ValidateNameableAttributes();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the all text types.
        /// </summary>
        public override IList<ITextTypeWrapper> AllTextTypes
        {
            get
            {
                IList<ITextTypeWrapper> returnList = base.AllTextTypes;
                this.name.AddAll(returnList);
                this.description.AddAll(returnList);
                return returnList;
            }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public virtual string Description
        {
            get
            {
                // HACK This does not work properly
                ITextTypeWrapper ttw = TextTypeUtil.GetDefaultLocale(this.description);
                return (ttw == null) ? null : ttw.Value;
            }
        }

        /// <summary>
        ///   Gets the descriptions.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Descriptions
        {
            get
            {
                return new List<ITextTypeWrapper>(this.description);
            }
        }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        public virtual string Name
        {
            get
            {
                // HACK This does not work properly
                ITextTypeWrapper ttw = TextTypeUtil.GetDefaultLocale(this.name);
                return (ttw == null) ? null : ttw.Value;
            }
        }

        /// <summary>
        ///   Gets or sets the names.
        /// </summary>
        public IList<ITextTypeWrapper> Names
        {
            get
            {
                return new ReadOnlyCollection<ITextTypeWrapper>(this.name);
            }
            
            set
            {
                this.name = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform a deep equal comparison against <paramref name="nameableObject"/>
        /// </summary>
        /// <param name="nameableObject">
        /// The maintainable object to compare against
        /// </param>
        /// <param name="includeFinalProperties">
        /// Set to true to compare final properties. 
        /// These are <see cref="name"/>, <see cref="description"/>. Otherwise those are ignored.
        /// </param>
        /// <returns>
        /// True if the <paramref name="nameableObject"/> deep equals this instance; otherwise false
        /// </returns>
        protected internal bool DeepEqualsNameable(INameableObject nameableObject, bool includeFinalProperties)
        {
            if (nameableObject == null)
            {
                return false;
            }

            if (includeFinalProperties)
            {
                if (!this.Equivalent(this.name, nameableObject.Names, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.description, nameableObject.Descriptions, includeFinalProperties))
                {
                    return false;
                }
            }

            return this.DeepEqualsInternal(nameableObject, includeFinalProperties);
        }

        /// <summary>
        ///   The validate nameable attributes.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected virtual void ValidateNameableAttributes()
        {
            if (this.StructureType.EnumType != SdmxStructureEnumType.Subscription
                && this.StructureType.EnumType != SdmxStructureEnumType.Registration)
            {
                if (this.name == null || this.name.Count == 0)
                {
                    _log.WarnFormat("No names found for structure '{0}' with id '{1}'", this.StructureType, this.Id);
                    throw new SdmxSemmanticException(ExceptionCode.StructureIdentifiableMissingName, this.StructureType + "  "+ this.Id);
                }

                ValidationUtil.ValidateTextType(this.name, null);
                ValidationUtil.ValidateTextType(this.description, null);
            }
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
	   ////////////COMPOSITES								 //////////////////////////////////////////////////
	   ///////////////////////////////////////////////////////////////////////////////////////////////////	

       /// <summary>
       ///   The get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
		ISet<ISdmxObject> composites = base.GetCompositesInternal();
		base.AddToCompositeSet(name, composites);
		base.AddToCompositeSet(description, composites);
		return composites;
	   }

        #endregion
    }
}