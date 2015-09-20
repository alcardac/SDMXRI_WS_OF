// -----------------------------------------------------------------------
// <copyright file="OrganisationXmlAssembler.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OrganisationXmlAssembler : AbstractAssembler, IAssembler<Organisation, IOrganisation>
    {
        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public virtual void Assemble(Organisation assembleInto, IOrganisation assembleFrom)
        {
            foreach (IContact currentContact in assembleFrom.Contacts)
            {
                var contact = new ContactType();
                assembleInto.Contact.Add(contact);

                if (ObjectUtil.ValidString(currentContact.Id))
                {
                    contact.id = currentContact.Id;
                }
                if (ObjectUtil.ValidCollection(currentContact.Departments))
                {
                    contact.Department = GetTextType(currentContact.Departments);
                }
                if (ObjectUtil.ValidCollection(currentContact.Name))
                {
                    if (contact.Name == null)
                    {
                        contact.Name = new List<Name>();
                    }

                    foreach (var name in currentContact.Name)
                    {
                        contact.Name.Add(new Name { TypedValue = name.Value, lang = name.Locale });
                    }
                }

                if (ObjectUtil.ValidCollection(currentContact.Role))
                {
                    contact.Role = GetTextType(currentContact.Role);
                }

                if (ObjectUtil.ValidCollection(currentContact.Telephone))
                {
                    contact.Telephone.AddAll(currentContact.Telephone);
                }

                if (ObjectUtil.ValidCollection(currentContact.Fax))
                {
                    contact.Fax.AddAll(currentContact.Fax);
                }
                if (ObjectUtil.ValidCollection(currentContact.Email))
                {
                    contact.Email.AddAll(currentContact.Email);
                }

                if (ObjectUtil.ValidCollection(currentContact.Uri))
                {
                    if (contact.URI == null)
                    {
                        contact.URI = new List<Uri>();
                    }

                    foreach (string uri in currentContact.Uri)
                    {
                        contact.URI.Add(new Uri(uri, UriKind.RelativeOrAbsolute)); //TODO: exception
                    }
                }

                if (ObjectUtil.ValidCollection(currentContact.X400))
                {
                    contact.X400.AddAll(currentContact.X400);
                }
            }
        }
    }
}
