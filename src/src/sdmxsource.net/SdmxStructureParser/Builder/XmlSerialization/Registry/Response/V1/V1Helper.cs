// --------------------------------------------------------------------------------------------------------------------
// <copyright file="V1Helper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The v 1 helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V1
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    ///     The v 1 helper.
    /// </summary>
    public class V1Helper
    {
        #region Static Fields

        /// <summary>
        ///     The reference.
        /// </summary>
        private static int referenceNo = 1;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The set header.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="sdmxObjects">
        /// The sdmxObjects.
        /// </param>
        public static void SetHeader(HeaderType header, ISdmxObjects sdmxObjects)
        {
            header.ID = "IDREF" + referenceNo;
            referenceNo++;
            header.Test = false;
            header.Prepared = DateTime.Now;

            string senderId;
            if (sdmxObjects != null && sdmxObjects.Header != null && sdmxObjects.Header.Sender != null)
            {
                // Get header information from the supplied sdmxObjects
                senderId = sdmxObjects.Header.Sender.Id;
            }
            else
            {
                // Get header info from HeaderHelper
                senderId = HeaderHelper.Instance.SenderId;
            }

            header.Sender.id = senderId;
        }

        #endregion
    }
}