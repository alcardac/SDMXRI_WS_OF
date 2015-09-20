// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameableMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The nameable mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The nameable mutable core.
    /// </summary>
    [Serializable]
    public abstract class NameableMutableCore : IdentifiableMutableCore, INameableMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _description.
        /// </summary>
        private readonly IList<ITextTypeWrapperMutableObject> _descriptions = new List<ITextTypeWrapperMutableObject>();

        /// <summary>
        ///   The _name.
        /// </summary>
        private readonly IList<ITextTypeWrapperMutableObject> _names = new List<ITextTypeWrapperMutableObject>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        protected NameableMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        protected NameableMutableCore(INameableObject objTarget)
            : base(objTarget)
        {
            if (objTarget.Name != null)
            {
                foreach (ITextTypeWrapper currentTextType in objTarget.Names)
                {
                    this._names.Add(new TextTypeWrapperMutableCore(currentTextType));
                }
            }

            if (objTarget.Description != null)
            {
                foreach (ITextTypeWrapper currentTextType0 in objTarget.Descriptions)
                {
                    this._descriptions.Add(new TextTypeWrapperMutableCore(currentTextType0));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public virtual IList<ITextTypeWrapperMutableObject> Descriptions
        {
            get
            {
                return this._descriptions;
            }
        }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        public virtual IList<ITextTypeWrapperMutableObject> Names
        {
            get
            {
                return this._names;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add description.
        /// </summary>
        /// <param name="locale">
        /// The locale. 
        /// </param>
        /// <param name="name">
        /// The name 0. 
        /// </param>
        public virtual void AddDescription(string locale, string name)
        {
            foreach (ITextTypeWrapperMutableObject textType in this._descriptions)
            {
                if (textType.Locale.Equals(locale))
                {
                    textType.Value = name;
                    return;
                }
            }

            ITextTypeWrapperMutableObject tt = new TextTypeWrapperMutableCore();
            tt.Locale = locale;
            tt.Value = name;
            this._descriptions.Add(tt);
        }

        /// <summary>
        /// The add name.
        /// </summary>
        /// <param name="locale">
        /// The locale. 
        /// </param>
        /// <param name="name">
        /// The name 0. 
        /// </param>
        public virtual void AddName(string locale, string name)
        {
            foreach (ITextTypeWrapperMutableObject textType in this._names)
            {
                if (textType.Locale.Equals(locale))
                {
                    textType.Value = name;
                    return;
                }
            }

            ITextTypeWrapperMutableObject tt = new TextTypeWrapperMutableCore();
            tt.Locale = locale;
            tt.Value = name;
            this._names.Add(tt);
        }

        /// <summary>
        /// The get name.
        /// </summary>
        /// <param name="defaultIfNull">
        /// The default if null. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        public virtual string GetName(bool defaultIfNull)
        {
            // HACK This does not work properly
            foreach (ITextTypeWrapperMutableObject mutable in this._names)
            {
                return mutable.Value;
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The process reader.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected internal override bool ProcessReader(ISdmxReader reader)
        {
            if (base.ProcessReader(reader))
            {
                return true;
            }

            if (reader.CurrentElement.Equals("Name"))
            {
                string lang = reader.GetAttributeValue("lang", false);
                this.AddName(lang, reader.CurrentElementValue);
                return true;
            }

            if (reader.CurrentElement.Equals("Description"))
            {
                string lang0 = reader.GetAttributeValue("lang", false);
                this.AddDescription(lang0, reader.CurrentElementValue);
                return true;
            }

            return false;
        }

        #endregion
    }
}