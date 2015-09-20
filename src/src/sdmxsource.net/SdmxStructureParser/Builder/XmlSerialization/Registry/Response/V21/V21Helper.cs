// --------------------------------------------------------------------------------------------------------------------
// <copyright file="V21Helper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   v the 21 helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     v the 21 helper.
    /// </summary>
    public class V21Helper
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
                SetHeader(value, null, null);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// set the header.
        /// </summary>
        /// <param name="regInterface">
        /// the registry interface.
        /// </param>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <param name="receivers">
        /// The receivers.
        /// </param>
        public static void SetHeader(RegistryInterfaceType regInterface, ISdmxObjects beans, params string[] receivers)
        {
            var header = new BasicHeaderType();
            regInterface.Header = header;
            SetHeader(header, beans, receivers);
        }

        /// <summary>
        /// set the header.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <param name="receivers">
        /// The receivers.
        /// </param>
        public static void SetHeader(BaseHeaderType header, ISdmxObjects beans, params string[] receivers)
        {
            header.ID = "IDREF" + refNumber;
            refNumber++;
            header.Test = false;
            header.Prepared = DateTime.Now;
            var sender = new SenderType();
            header.Sender = sender;

            string senderId;
            if (beans != null && beans.Header != null && beans.Header.Sender != null)
            {
                // Get header information from the supplied beans
                senderId = beans.Header.Sender.Id;
            }
            else
            {
                // Get header info from HeaderHelper
                senderId = HeaderHelper.Instance.SenderId;
            }

            sender.id = senderId;

            if (ObjectUtil.ValidArray(receivers))
            {
                /* foreach */
                foreach (string currentReviever in receivers)
                {
                    var receiver = new PartyType();
                    header.Receiver.Add(receiver);
                    receiver.id = currentReviever;
                }
            }
            else
            {
                var receiver0 = new PartyType();
                header.Receiver.Add(receiver0);
                receiver0.id = HeaderHelper.Instance.ReceiverId;
            }
        }

        #endregion
    }
}