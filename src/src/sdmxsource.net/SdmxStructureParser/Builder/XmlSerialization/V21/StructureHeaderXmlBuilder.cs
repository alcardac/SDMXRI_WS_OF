// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureHeaderXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure header xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util;

    /// <summary>
    /// The structure header xml bean builder.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the header
    /// </typeparam>
    public class StructureHeaderXmlBuilder<T> : AbstractAssembler, IBuilder<T, IHeader>
        where T : BaseHeaderType, new()
    {
        #region Constants

        /// <summary>
        ///     The unknown id.
        /// </summary>
        private const string UnknownId = "unknown";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="StructureHeaderType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IHeader"/>
        /// </param>
        /// <returns>
        /// The <see cref="StructureHeaderType"/>.
        /// </returns>
        public virtual T Build(IHeader buildFrom)
        {
            var headerType = new T();

            if (buildFrom == null)
            {
                headerType.ID = "IDREF" + Guid.NewGuid().ToString();
                headerType.Test = false;
                headerType.Prepared = DateTime.Now;
                SenderType senderType = new SenderType();
                senderType.id = UnknownId;
                headerType.Sender = senderType;
                PartyType receiverType = new PartyType();
                receiverType.id = UnknownId;
                headerType.Receiver.Add(receiverType);
                return headerType;
            }

            string value = buildFrom.Id;

            // NOTE Please before changing the order of the statements please consult the SDMXMessage.xsd *HeaderType.
            // The order has changed compared to Java version because the Linq2Xsd used by .NET expects everything to be in same order as in the XSD.
            // In other words do not change the order of the statements in this method because the Java has different order. It is ok. 
            if (!string.IsNullOrWhiteSpace(value))
            {
                headerType.ID = buildFrom.Id;
            }

            headerType.Test = buildFrom.Test;

            // ALL DATE RELATED INFO
            if (buildFrom.Prepared != null)
            {
                headerType.Prepared = buildFrom.Prepared;
            }

            // SENDER
            if (buildFrom.Sender != null)
            {
                IParty sender = buildFrom.Sender;
                var senderType = new SenderType();
                headerType.Sender = senderType;
                string value1 = sender.Id;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    senderType.id = sender.Id;
                }

                /* foreach */
                foreach (ITextTypeWrapper name in sender.Name)
                {
                    var textType = new TextType { lang = name.Locale, TypedValue = name.Value };
                    senderType.Name.Add(new Name(textType));
                }

                // CONTACT INFO
                foreach (IContact contact in sender.Contacts)
                {
                    var contactType = new ContactType();
                    senderType.Contact.Add(contactType);
                    BuildContact(contactType, contact);
                }
            }

            // RECEIVER
            if (buildFrom.Receiver != null)
            {
                foreach (IParty receiver in buildFrom.Receiver)
                {
                    var receiverType = new PartyType();
                    headerType.Receiver.Add(receiverType);
                    string value1 = receiver.Id;
                    if (!string.IsNullOrWhiteSpace(value1))
                    {
                        receiverType.id = receiver.Id;
                    }

                    foreach (ITextTypeWrapper name in receiver.Name)
                    {
                        var textType = new TextType { lang = name.Locale, TypedValue = name.Value };
                        receiverType.Name.Add(new Name(textType));
                    }

                    // CONTACT INFO
                    foreach (IContact contact6 in receiver.Contacts)
                    {
                        var contactType = new ContactType();
                        receiverType.Contact.Add(contactType);
                        BuildContact(contactType, contact6);
                    }
                }
            }

            if (!ObjectUtil.ValidCollection(headerType.Receiver))
            {
                var receiverType7 = new PartyType();
                headerType.Receiver.Add(receiverType7);
                receiverType7.id = UnknownId;
            }

            if (buildFrom.Name != null)
            {
                /* foreach */
                foreach (ITextTypeWrapper name in buildFrom.Name)
                {
                    var textType = new TextType { lang = name.Locale, TypedValue = name.Value };
                    headerType.Name.Add(new Name(textType));
                }
            }

            if (buildFrom.Source != null)
            {
                /* foreach */
                foreach (ITextTypeWrapper source in buildFrom.Source)
                {
                    var textType = new TextType { lang = source.Locale, TypedValue = source.Value };
                    headerType.Source.Add(textType);
                }
            }


            return headerType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build contact.
        /// </summary>
        /// <param name="contactType">
        /// The contact type.
        /// </param>
        /// <param name="contact">
        /// The contact.
        /// </param>
        private static void BuildContact(ContactType contactType, IContact contact)
        {
            // Please before changing the order of the statements please consult the SDMXMessage.xsd ContactType.
            // The order has changed compared to Java version because the Linq2Xsd used by .NET expects everything to be in same order as in the XSD.
            // In other words do not change the order of the statements in this method because the Java has different order. It is ok. 
            if (contact.Name != null)
            {
                /* foreach */
                foreach (ITextTypeWrapper name in contact.Name)
                {
                    var textType = new TextType { lang = name.Locale, TypedValue = name.Value };
                    contactType.Name.Add(new Name(textType));
                }
            }

            if (contact.Departments != null)
            {
                /* foreach */
                foreach (ITextTypeWrapper department in contact.Departments)
                {
                    var textType = new TextType { lang = department.Locale, TypedValue = department.Value };
                    contactType.Department.Add(textType);
                }
            }

            if (contact.Role != null)
            {
                /* foreach */
                foreach (ITextTypeWrapper role in contact.Role)
                {
                    var textType = new TextType { lang = role.Locale, TypedValue = role.Value };
                    contactType.Role.Add(textType);
                }
            }

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

        #endregion
    }
}