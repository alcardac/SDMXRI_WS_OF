// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderXmlsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The header xml beans builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The header xml beans builder.
    /// </summary>
    public class HeaderXmlsBuilder : AbstractBuilder, IBuilder<HeaderType, IHeader>
    {
        #region Constants

        /// <summary>
        ///     The unassigned id.
        /// </summary>
        private const string UnassignedId = "unassigned";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="HeaderType"/>.
        /// </returns>
        public virtual HeaderType Build(IHeader buildFrom)
        {
            var headerType = new HeaderType();

            string value = buildFrom.Id;
            if (buildFrom != null && !string.IsNullOrWhiteSpace(value))
            {
                headerType.ID = buildFrom.Id;
            }
            else
            {
                headerType.ID = UnassignedId;
            }

            if (buildFrom != null && buildFrom.Prepared != null)
            {
                headerType.Prepared = buildFrom.Prepared;
            }
            else
            {
                headerType.Prepared = DateTime.Now;
            }

            if (buildFrom != null && buildFrom.Action != null)
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
                    default:
                        headerType.DataSetAction = ActionTypeConstants.Update;
                        break;
                }
            }
            else
            {
                headerType.DataSetAction = ActionTypeConstants.Update;
            }

            if (buildFrom != null && buildFrom.Sender != null)
            {
                SetSenderInfo(headerType, buildFrom.Sender);
            }
            else
            {
                var sender = new PartyType();
                headerType.Sender = sender;
                sender.id = "unknown";
            }

            return headerType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The set sender info.
        /// </summary>
        /// <param name="headerType">
        /// The header type.
        /// </param>
        /// <param name="party">
        /// The party.
        /// </param>
        private static void SetSenderInfo(HeaderType headerType, IParty party)
        {
            var sender = new PartyType();
            headerType.Sender = sender;

            string value = party.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                sender.id = party.Id;
            }
        }

        #endregion
    }
}