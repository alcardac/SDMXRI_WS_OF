// -----------------------------------------------------------------------
// <copyright file="OrganisationMutableObjectCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public abstract class OrganisationMutableObjectCore : ItemMutableCore, IOrganisationMutableObject
    {
        private readonly IList<IContactMutableObject> _contacts = new List<IContactMutableObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationMutableObjectCore"/> class.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        protected OrganisationMutableObjectCore(IOrganisation organisation)
            : base(organisation)
        {
            foreach (IContact contact in organisation.Contacts) 
            {
			    AddContact(new ContactMutableObjectCore(contact));
		    }
        }

        protected OrganisationMutableObjectCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        public IList<IContactMutableObject> Contacts
        {
            get
            {
                return this._contacts;
            }
        }

        public void AddContact(IContactMutableObject contact)
        {
            this._contacts.Add(contact);
        }
    }
}
