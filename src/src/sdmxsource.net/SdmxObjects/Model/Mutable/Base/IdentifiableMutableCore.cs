// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The identifiable mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The identifiable mutable core.
    /// </summary>
    [Serializable]
    public abstract class IdentifiableMutableCore : AnnotableMutableCore, IIdentifiableMutableObject
    {
        #region Fields

        /// <summary>
        ///   The id.
        /// </summary>
        private string _id;

        /// <summary>
        ///   The uri.
        /// </summary>
        private Uri _uri;

        /// <summary>
        ///   The urn.
        /// </summary>
        private Uri _urn;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected IdentifiableMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        protected IdentifiableMutableCore(IIdentifiableObject objTarget)
            : base(objTarget)
        {
            this._id = objTarget.Id;
            if (objTarget.Uri != null)
            {
                this._uri = objTarget.Uri;
            }

            this._urn = objTarget.Urn;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the id.
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
        ///   Gets or sets the uri.
        /// </summary>
        public virtual Uri Uri
        {
            get
            {
                return this._uri;
            }

            set
            {
                this._uri = value;
            }
        }

        /// <summary>
        ///   Gets or sets the urn.
        /// </summary>
        public virtual Uri Urn
        {
            get
            {
                return this._urn;
            }

            set
            {
                this._urn = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build identifiable attributes.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        protected internal void BuildIdentifiableAttributes(ISdmxReader reader)
        {
            this._id = reader.GetAttributeValue("id", true);
            string attributeValue = reader.GetAttributeValue("uri", false);
            if (attributeValue != null)
            {
                this.Uri = new Uri(attributeValue);
            }
        }

        #endregion
    }
}