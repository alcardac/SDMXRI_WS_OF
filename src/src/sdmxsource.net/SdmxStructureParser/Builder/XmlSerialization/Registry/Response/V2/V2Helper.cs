// --------------------------------------------------------------------------------------------------------------------
// <copyright file="V2Helper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The v 2 helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    ///     The v 2 helper.
    /// </summary>
    public class V2Helper
    {
        #region Static Fields

        /// <summary>
        ///     The ref.
        /// </summary>
        private static int refNumber = 1;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Sets the header.
        /// </summary>
        public static RegistryInterfaceType Header
        {
            set
            {
                SetHeader(value, null);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Sets the header.
        /// </summary>
        /// <param name="regInterface">
        /// The registry interface.
        /// </param>
        /// <param name="sdmxObjects">
        /// The beans.
        /// </param>
        public static void SetHeader(RegistryInterfaceType regInterface, ISdmxObjects sdmxObjects)
        {
            var header = new HeaderType();
            regInterface.Header = header;
            SetHeader(header, sdmxObjects);
        }

        /// <summary>
        /// Sets the header.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="sdmxObjects">
        /// The beans.
        /// </param>
        public static void SetHeader(HeaderType header, ISdmxObjects sdmxObjects)
        {
            header.ID = "IDREF" + refNumber;
            refNumber++;
            header.Test = false;
            header.Prepared = DateTime.Now;
            var sender = new PartyType();
            header.Sender.Add(sender);

            string senderId;
            if (sdmxObjects != null && sdmxObjects.Header != null && sdmxObjects.Header.Sender != null)
            {
                // Get header information from the supplied beans
                senderId = sdmxObjects.Header.Sender.Id;
            }
            else
            {
                // Get header info from HeaderHelper
                senderId = HeaderHelper.Instance.SenderId;
            }

            sender.id = senderId;

            var receiver = new PartyType();
            header.Receiver.Add(receiver);
            receiver.id = HeaderHelper.Instance.ReceiverId;
        }

        #endregion
    }
}