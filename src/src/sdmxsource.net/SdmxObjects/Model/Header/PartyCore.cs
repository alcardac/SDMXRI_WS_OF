// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The party core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The party core.
    /// </summary>
    [Serializable]
    public class PartyCore : IParty
    {
        #region Fields

        /// <summary>
        ///   The _name.
        /// </summary>
        private readonly IList<ITextTypeWrapper> _name;

        /// <summary>
        ///   The contacts.
        /// </summary>
        private readonly IList<Org.Sdmxsource.Sdmx.Api.Model.Objects.Base.IContact> contacts;

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The time zone.
        /// </summary>
        private readonly string timeZone;

        private static readonly Regex _timeZoneRegex = new Regex("^(\\+|\\-)(14:00|((0[0-9]|1[0-3]):[0-5][0-9]))$", RegexOptions.Compiled);

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ATTRIBUTES               ////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyCore"/> class.
        /// </summary>
        /// <param name="name">
        /// The name 0. 
        /// </param>
        /// <param name="id">
        /// The id 1. 
        /// </param>
        /// <param name="contacts">
        /// The contacts 2. 
        /// </param>
        /// <param name="timeZone">
        /// The time zone 3. 
        /// </param>
        public PartyCore(IList<ITextTypeWrapper> name, string id, IList<IContact> contacts, string timeZone)
            : this()
        {
            if (name != null)
            {
                this._name = new List<ITextTypeWrapper>(name);
            }

            this.id = id;
            if (contacts != null)
            {
                this.contacts = new List<IContact>(contacts);
            }

            this.timeZone = timeZone;
            this.Validate();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //////////BUILD FROM V2.1 SCHEMA        /////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyCore"/> class.
        /// </summary>
        /// <param name="partyType">
        /// The party type. 
        /// </param>
        public PartyCore(PartyType partyType)
            : this()
        {
            this.id = partyType.id;

            if (ObjectUtil.ValidCollection(partyType.Name))
            {
                foreach (Name tt in partyType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            if (ObjectUtil.ValidCollection(partyType.Contact))
            {
                foreach (ContactType contactType in partyType.Contact)
                {
                    this.contacts.Add(new ContactCore(contactType));
                }
            }

            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyCore"/> class.
        /// </summary>
        /// <param name="senderType">
        /// The sender type. 
        /// </param>
        public PartyCore(SenderType senderType)
            : this()
        {
            this.id = senderType.id;

            if (ObjectUtil.ValidCollection(senderType.Name))
            {
                foreach (Name tt in senderType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            if (ObjectUtil.ValidCollection(senderType.Contact))
            {
                foreach (ContactType contactType in senderType.Contact)
                {
                    this.contacts.Add(new ContactCore(contactType));
                }
            }

            this.timeZone = senderType.Timezone;
            this.Validate();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //////////BUILD FROM V2.0 SCHEMA        /////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyCore"/> class.
        /// </summary>
        /// <param name="partyType">
        /// The party type. 
        /// </param>
        public PartyCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.PartyType partyType)
            : this()
        {
            this.id = partyType.id;
            if (ObjectUtil.ValidCollection(partyType.Name))
            {
                foreach (TextType tt in partyType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            if (ObjectUtil.ValidCollection(partyType.Contact))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.ContactType contactType in partyType.Contact)
                {
                    this.contacts.Add(new ContactCore(contactType));
                }
            }

            this.Validate();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //////////BUILD FROM V1.0 SCHEMA        /////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyCore"/> class.
        /// </summary>
        /// <param name="partyType">
        /// The party type. 
        /// </param>
        public PartyCore(Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.PartyType partyType)
            : this()
        {
            this.id = partyType.id;
            if (ObjectUtil.ValidCollection(partyType.Name))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType tt in partyType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            if (ObjectUtil.ValidCollection(partyType.Contact))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.ContactType contactType in partyType.Contact)
                {
                    this.contacts.Add(new ContactCore(contactType));
                }
            }

            this.Validate();
        }

        /// <summary>
        ///   Prevents a default instance of the <see cref="PartyCore" /> class from being created.
        /// </summary>
        private PartyCore()
        {
            this._name = new List<ITextTypeWrapper>();
            this.contacts = new List<IContact>();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //////////VALIDATE                        /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the contacts.
        /// </summary>
        public virtual IList<IContact> Contacts
        {
            get
            {
                return new List<IContact>(this.contacts);
            }
        }

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Name
        {
            get
            {
                return new List<ITextTypeWrapper>(this._name);
            }
        }

        /// <summary>
        ///   Gets the time zone.
        /// </summary>
        public virtual string TimeZone
        {
            get
            {
                return this.timeZone;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.id))
            {
                throw new SdmxSemmanticException("Party missing mandatory id");
            }

            if (this.timeZone != null)
            {
                // Pattern idPattern = ILOG.J2CsMapping.Text.Pattern.Compile("(\\+|\\-)(14:00|((0[0-9]|1[0-3]):[0-5][0-9]))");
                if (!_timeZoneRegex.IsMatch(this.timeZone))
                {
                    throw new SdmxSemmanticException(
                        "Time zone '" + this.timeZone
                        +
                        "' is in an invalid format. please ensure the format matches the patttern (\\+|\\-)(14:00|((0[0-9]|1[0-3]):[0-5][0-9]) example +12:30");
                }
            }
        }

        #endregion

        #region Methods

        #endregion

    }
}