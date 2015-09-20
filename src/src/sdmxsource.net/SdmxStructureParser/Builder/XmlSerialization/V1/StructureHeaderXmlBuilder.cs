// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureHeaderXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   structures the header xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType;

    /// <summary>
    ///     structures the header xml bean builder.
    /// </summary>
    public class StructureHeaderXmlBuilder : IBuilder<HeaderType, IHeader>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// builds the from.
        /// </param>
        /// <returns>
        /// The <see cref="HeaderType"/>.
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
                        headerType.DataSetAction = ActionTypeConstants.Update;
                        break;
                    case DatasetActionEnumType.Replace:
                        headerType.DataSetAction = ActionTypeConstants.Update;
                        break;
                    case DatasetActionEnumType.Delete:
                        headerType.DataSetAction = ActionTypeConstants.Delete;
                        break;
                    case DatasetActionEnumType.Information:
                        headerType.DataSetAction = ActionTypeConstants.Update;
                        break;
                }
            }

            CopyText(buildFrom.Name, headerType.Name);
            CopyText(buildFrom.Source, headerType.Source);

            // ALL DATE RELATED INFO
            if (buildFrom.Extracted != null)
            {
                headerType.Extracted = buildFrom.Extracted;
            }

            if (buildFrom.Prepared != null)
            {
                headerType.Prepared = buildFrom.Prepared;
            }

            if (buildFrom.ReportingBegin != null)
            {
                headerType.ReportingBegin = buildFrom.ReportingBegin;
            }

            if (buildFrom.ReportingEnd != null)
            {
                headerType.ReportingEnd = buildFrom.ReportingEnd;
            }

            // SENDER
            var senderTpe = new PartyType();
            headerType.Sender = senderTpe;
            CopyParty(buildFrom.Sender, senderTpe);

            // RECEIVER
            if (buildFrom.Receiver != null)
            {
                /* foreach */
                foreach (IParty receiver in buildFrom.Receiver)
                {
                    var receiverType = new PartyType();
                    headerType.Receiver.Add(receiverType);
                    CopyParty(receiver, receiverType);
                }
            }

            if (!ObjectUtil.ValidCollection(headerType.Receiver))
            {
                var receiverType7 = new PartyType();
                headerType.Receiver.Add(receiverType7);
                receiverType7.id = "unknown";
            }

            return headerType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// builds the contact.
        /// </summary>
        /// <param name="contactType">
        /// contacts the type.
        /// </param>
        /// <param name="contact">
        /// The contact.
        /// </param>
        private static void BuildContact(ContactType contactType, IContact contact)
        {
            CopyText(contact.Departments, contactType.Department);
            CopyText(contact.Name, contactType.Name);
            CopyText(contact.Role, contactType.Role);

            if (contact.Email != null)
            {
                /* foreach */
                foreach (string val in contact.Email)
                {
                    contactType.Email.Add(val);
                }
            }

            if (contact.Fax != null)
            {
                /* foreach */
                foreach (string val4 in contact.Fax)
                {
                    contactType.Fax.Add(val4);
                }
            }

            if (contact.Telephone != null)
            {
                /* foreach */
                foreach (string val5 in contact.Telephone)
                {
                    contactType.Telephone.Add(val5);
                }
            }

            if (contact.Uri != null)
            {
                /* foreach */
                foreach (string val6 in contact.Uri)
                {
                    contactType.URI.Add(new Uri(val6));
                }
            }

            if (contact.X400 != null)
            {
                /* foreach */
                foreach (string val7 in contact.X400)
                {
                    contactType.X400.Add(val7);
                }
            }
        }

        /// <summary>
        /// copies the party.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="partyType">
        /// The destination.
        /// </param>
        private static void CopyParty(IParty source, PartyType partyType)
        {
            if (source != null)
            {
                string value = source.Id;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    partyType.id = source.Id;
                }

                CopyText(source.Name, partyType.Name);

                // CONTACT INFO
                foreach (IContact contact in source.Contacts)
                {
                    var contactType = new ContactType();
                    partyType.Contact.Add(contactType);
                    BuildContact(contactType, contact);
                }
            }
        }

        /// <summary>
        /// Copies the text.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="destination">
        /// The destination.
        /// </param>
        private static void CopyText(
            IEnumerable<ITextTypeWrapper> source, ICollection<TextType> destination)
        {
            if (source != null)
            {
                foreach (ITextTypeWrapper locale in source)
                {
                    destination.Add(new TextType { lang = locale.Locale, TypedValue = locale.Value });
                }
            }
        }

        #endregion
    }
}