// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureHeaderXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure header xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///     The structure header xml bean builder.
    /// </summary>
    public class StructureHeaderXmlBuilder : IBuilder<HeaderType, IHeader>
    {
        #region Constants

        /// <summary>
        ///     The unknown id.
        /// </summary>
        private const string UnknownId = "unknown";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="HeaderType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="HeaderType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual HeaderType Build(IHeader buildFrom)
        {
            var headerType = new HeaderType();
            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                headerType.ID = buildFrom.Id;
            }

            headerType.Test = buildFrom.Test;

            if (buildFrom.Action != null)
            {
                switch (buildFrom.Action.EnumType)
                {
                    case DatasetActionEnumType.Append:
                        headerType.DataSetAction = ActionTypeConstants.Append;
                        break;
                    case DatasetActionEnumType.Replace:
                        headerType.DataSetAction = ActionTypeConstants.Replace;
                        break;
                    case DatasetActionEnumType.Delete:
                        headerType.DataSetAction = ActionTypeConstants.Delete;
                        break;
                    case DatasetActionEnumType.Information:
                        headerType.DataSetAction = ActionTypeConstants.Information;
                        break;
                }
            }

            ProcessTextTypes(buildFrom.Name, headerType.Name);

            ProcessTextTypes(buildFrom.Source, headerType.Source);

            // ALL DATE RELATED INFO
            if (buildFrom.Extracted != null)
            {
                headerType.Extracted = buildFrom.Extracted.Value;
            }

            if (buildFrom.Prepared != null)
            {
                headerType.Prepared = buildFrom.Prepared.Value;
            }

            if (buildFrom.ReportingBegin != null)
            {
                headerType.ReportingBegin = buildFrom.ReportingBegin.Value;
            }

            if (buildFrom.ReportingEnd != null)
            {
                headerType.ReportingEnd = buildFrom.ReportingEnd.Value;
            }

            // SENDER
            BuildParties(buildFrom.Sender, headerType.Sender);

            // RECEIVER
            if (buildFrom.Receiver != null)
            {
                /* foreach */
                foreach (IParty receiver in buildFrom.Receiver)
                {
                    BuildParties(receiver, headerType.Receiver);
                }
            }

            if (!ObjectUtil.ValidCollection(headerType.Receiver))
            {
                var receiver = new PartyType();
                headerType.Receiver.Add(receiver);
                receiver.id = UnknownId;
            }

            return headerType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build contacts.
        /// </summary>
        /// <param name="contactType">
        /// The contact type.
        /// </param>
        /// <param name="contact">
        /// The contact.
        /// </param>
        private static void BuildContact(ContactType contactType, IContact contact)
        {
            // Please check the XSD not the Java version for the correct order.
            // The order in .NET must be the same as in the XSD. 
            ProcessTextTypes(contact.Name, contactType.Name);
            ProcessTextTypes(contact.Departments, contactType.Department);
            ProcessTextTypes(contact.Role, contactType.Role);

            if (contact.Email != null)
            {
                contactType.Email.AddAll(contact.Email);
            }

            if (contact.Fax != null)
            {
                contactType.Fax.AddAll(contact.Fax);
            }

            if (contact.Telephone != null)
            {
                contactType.Telephone.AddAll(contact.Telephone);
            }

            if (contact.Uri != null)
            {
                /* foreach */
                foreach (string uriString in contact.Uri)
                {
                    // Please check GIT log before reverting changes that were fixes.
                    var uriBuilder = new UriBuilder(uriString);
                    contactType.URI.Add(uriBuilder.Uri);
                }
            }

            if (contact.X400 != null)
            {
                contactType.X400.AddAll(contact.X400);
            }
        }

        /// <summary>
        /// Build <paramref name="partyTypes"/> from <paramref name="party"/>
        /// </summary>
        /// <param name="party">
        /// The party.
        /// </param>
        /// <param name="partyTypes">
        /// The party types (XSD generated code).
        /// </param>
        private static void BuildParties(IParty party, ICollection<PartyType> partyTypes)
        {
            if (party != null)
            {
                var partyType = new PartyType();
                partyTypes.Add(partyType);
                string value = party.Id;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    partyType.id = party.Id;
                }

                ProcessTextTypes(party.Name, partyType.Name);

                // CONTACT INFO
                foreach (IContact contact in party.Contacts)
                {
                    var contactType = new ContactType();
                    partyType.Contact.Add(contactType);
                    BuildContact(contactType, contact);
                }
            }
        }

        /// <summary>
        /// Process text types from <paramref name="items"/> to <paramref name="textTypes"/>
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="textTypes">
        /// The text types.
        /// </param>
        private static void ProcessTextTypes(
            IEnumerable<ITextTypeWrapper> items, ICollection<TextType> textTypes)
        {
            if (items != null)
            {
                /* foreach */
                foreach (ITextTypeWrapper itm in items)
                {
                    var name = new TextType();
                    textTypes.Add(name);
                    name.lang = itm.Locale;
                    name.TypedValue = itm.Value;
                }
            }
        }

        #endregion
    }
}