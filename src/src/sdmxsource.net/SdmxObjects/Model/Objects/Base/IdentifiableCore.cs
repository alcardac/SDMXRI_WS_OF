// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The identifiable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The identifiable core.
    /// </summary>
    [Serializable]
    public abstract class IdentifiableCore : AnnotableCore, IIdentifiableObject
    {
        #region Fields

        /// <summary>
        ///     The _id.
        /// </summary>
        private string _id;

        /// <summary>
        ///     The _uri.
        /// </summary>
        private Uri _uri;

        /// <summary>
        ///     The _urn.
        /// </summary>
        private Uri _urn;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM A NON-IDENTIFIABLE         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencySchemeMutable.
        /// </param>
        protected internal IdentifiableCore(IIdentifiableObject agencyScheme)
            : base(agencyScheme)
        {
            this._id = agencyScheme.Id;
            this._uri = agencyScheme.Uri;
            this.StructureType = agencyScheme.StructureType;
            this.ValidateIdentifiableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencySchemeMutable.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        protected internal IdentifiableCore(IIdentifiableMutableObject itemMutableObject, ISdmxStructure parent)
            : base(itemMutableObject, parent)
        {
            this._id = itemMutableObject.Id;
            this.SetUri(itemMutableObject.Uri);
            this.ValidateIdentifiableAttributes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableCore"/> class.
        /// </summary>
        /// <param name="id0">
        /// The id 0.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        protected IdentifiableCore(string id0, SdmxStructureType structureType, ISdmxStructure parent)
            : base(null, structureType, parent)
        {
            this._id = id0;
            this.ValidateIdentifiableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public IdentifiableCore(SdmxStructureType structure, SdmxReader reader, ISdmxStructure parent) {
        // super(structure, reader, parent);
        // id = reader.getAttributeValue("id", true);
        // if(reader.getAttributeValue("uri", false) != null) {
        // setUri(reader.getAttributeValue("uri", false));
        // }
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableCore"/> class.
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
        protected IdentifiableCore(IdentifiableType createdFrom, SdmxStructureType structureType, ISdmxStructure parent)
            : base(createdFrom, structureType, parent)
        {
            this._id = createdFrom.id;
            this.Uri = createdFrom.uri;
            this.ValidateIdentifiableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="id0">
        /// The id 0.
        /// </param>
        /// <param name="uri1">
        /// The uri 1.
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        protected IdentifiableCore(IXmlSerializable createdFrom, SdmxStructureType structureType, string id0, Uri uri1, AnnotationsType annotationsType, ISdmxStructure parent)
            : base(createdFrom, annotationsType, structureType, parent)
        {
            this._id = id0;
            this.Uri = uri1;
            this.ValidateIdentifiableAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="id0">
        /// The id 0.
        /// </param>
        /// <param name="uri1">
        /// The uri 1.
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        protected IdentifiableCore(
            IXmlSerializable createdFrom, SdmxStructureType structureType, string id0, Uri uri1, Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationsType, ISdmxStructure parent)
            : base(createdFrom, annotationsType, structureType, parent)
        {
            this._id = id0;
            this.Uri = uri1;
            this.ValidateIdentifiableAttributes();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the all text types.
        /// </summary>
        public virtual IList<ITextTypeWrapper> AllTextTypes
        {
            get
            {
                IList<ITextTypeWrapper> returnList = new List<ITextTypeWrapper>();
                foreach (IAnnotation annotation in this.Annotations)
                {
                    annotation.Text.AddAll(returnList);
                }

                return returnList;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        ///     Gets or sets the original id.
        /// </summary>
        protected  string OriginalId
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        ///     Gets the uri.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return this._uri;
            }

            private set
            {
                this._uri = value;
            }
        }

        /// <summary>
        ///     Gets the urn.
        /// </summary>
        public virtual Uri Urn
        {
            get
            {
                return this._urn ?? (this._urn = this.GenerateUrn());
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The as reference.
        /// </summary>
        /// <value> The &lt; see cref= &quot; IStructureReference &quot; / &gt; . </value>
        public virtual IStructureReference AsReference
        {
            get
            {
                return new StructureReferenceImpl(this);
            }
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The agencySchemeMutable.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        public override bool Equals(object obj)
        {
            var that = obj as IIdentifiableObject;
            if (that != null)
            {
                return that.Urn.Equals(this.Urn);
            }

            return false;
        }

        /// <summary>
        /// The get full id path.
        /// </summary>
        /// <param name="includeDifferentTypes">
        /// The include different types.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public virtual string GetFullIdPath(bool includeDifferentTypes)
        {
            if (!this.StructureType.IsMaintainable)
            {
                IList<string> parentIds = this.GetParentIds(includeDifferentTypes);
                string returnId = string.Empty;
                string concat = string.Empty;
                foreach (string currentId in parentIds)
                {
                    returnId += concat + currentId;
                    concat = ".";
                }

                return returnId;
            }

            return null;
        }

        /// <summary>
        ///     The get hash code.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" /> .
        /// </returns>
        public override int GetHashCode()
        {
            return this.Urn.GetHashCode();
        }

        /// <summary>
        /// Sets the URI from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        ///     Cannot create a
        ///     <see cref="Uri"/>
        ///     from
        ///     <paramref name="value"/>
        /// </exception>
        public void SetUri(Uri value)
        {
            this._uri = value;
        }

        /// <summary>
        /// Sets the URI from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        ///     Cannot create a
        ///     <see cref="Uri"/>
        ///     from
        ///     <paramref name="value"/>
        /// </exception>
        public void SetUri(string value)
        {
            if (value == null)
            {
                this._uri = null;
                return;
            }

            try
            {
                this._uri = new Uri(value);
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(string.Format(CultureInfo.InvariantCulture, "Could not create attribute 'uri' with value '{0}'", value), th);
            }
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" /> .
        /// </returns>
        public override string ToString()
        {
            return this.GenerateString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The deep equals internal.
        /// </summary>
        /// <param name="identifiableObject">
        /// The agencySchemeMutable.
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        protected internal bool DeepEqualsInternal(IIdentifiableObject identifiableObject, bool includeFinalProperties)
        {
            if (identifiableObject == null)
            {
                return false;
            }

            if (!ObjectUtil.Equivalent(this._id, identifiableObject.Id))
            {
                return false;
            }
            if (includeFinalProperties)
            {
                //If we don't want to test the final properties, then the URI can be ignored, as this can change
                if (!ObjectUtil.Equivalent(this._uri, identifiableObject.Uri))
                {
                    return false;
                }
            }
            if (!ObjectUtil.Equivalent(this.Urn, identifiableObject.Urn))
            {
                return false;
            }

            return this.DeepEqualsInternalAnnotable(identifiableObject, includeFinalProperties);
        }

        /// <summary>
        /// Returns a list of ids for each identifiable (non-maintainable parent), starting from the top level (index 0 of list) to this level (index 0+X)
        /// </summary>
        /// <param name="includeDifferentTypes">
        /// The include Different Types.
        /// </param>
        /// <returns>
        /// The parent id's.
        /// </returns>
        protected internal virtual IList<string> GetParentIds(bool includeDifferentTypes)
        {
            IList<string> parentIds = new List<string>();
            if (!this.StructureType.IsMaintainable)
            {
                parentIds.Add(this.Id);
                IIdentifiableObject currentParent = this.IdentifiableParent;
                while (currentParent != null)
                {
                    if (!includeDifferentTypes && this.StructureType != currentParent.StructureType)
                    {
                        break;
                    }

                    if (!currentParent.StructureType.IsMaintainable)
                    {
                        parentIds.Add(currentParent.Id);
                        currentParent = currentParent.IdentifiableParent;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return parentIds.Reverse().ToList();
        }

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal virtual void ValidateId(bool startWithIntAllowed)
        {
            if (string.IsNullOrWhiteSpace(this.Id))
            {
                throw new SdmxSemmanticException(ExceptionCode.StructureIdentifiableMissingId, this.StructureType);
            }

            // $$$ No catch above
            this._id = ValidationUtil.CleanAndValidateId(this.Id, startWithIntAllowed);
        }

        /// <summary>
        ///     The validate identifiable attributes.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected virtual internal void ValidateIdentifiableAttributes()
        {
            if (this.Parent == null && !this.StructureType.IsMaintainable)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectNull, "parent");
            }

            this.ValidateId(true);
        }

        /// <summary>
        ///     Generate string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected string GenerateString()
        {
            Uri urn = this._urn ?? (this._urn = this.GenerateUrn());
            if (urn != null)
            {
                return urn.OriginalString;
            }

            return string.Format(CultureInfo.InvariantCulture, "\nId:{0}", this.Id);
        }

        /// <summary>
        ///     Generate urn.
        /// </summary>
        /// <returns>
        ///     The <see cref="Uri" />.
        /// </returns>
        private Uri GenerateUrn()
        {
            IList<string> parentIds = this.GetParentIds(true);
            IMaintainableObject maintainableParent = this.MaintainableParent;
            string[] ids = parentIds.ToArray();

            return this.StructureType.GenerateUrn(maintainableParent.AgencyId, maintainableParent.Id, maintainableParent.Version, ids);
        }

        #endregion
    }
}