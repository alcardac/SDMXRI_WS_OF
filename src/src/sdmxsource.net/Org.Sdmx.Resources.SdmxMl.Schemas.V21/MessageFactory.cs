// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageFactory.cs" company="EUROSTAT">
//   TODO
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21
{
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    /// Load SDMX Messages into memory objects
    /// </summary>
    public static class MessageFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Load SDMX-ML v2.1 from <paramref name="reader"/> and return it in <typeparamref name="TW"/>
        /// </summary>
        /// <typeparam name="TW">
        /// The element CLR type
        /// </typeparam>
        /// <typeparam name="T">
        /// The element XSD type CLR type
        /// </typeparam>
        /// <param name="reader">
        /// The input <see cref="XmlReader"/>
        /// </param>
        /// <returns>
        /// The SDMX-ML v2.1 from <paramref name="reader"/> as <typeparamref name="TW"/>
        /// </returns>
        public static TW Load<TW, T>(XmlReader reader)
            where T : MessageType
            where TW : XTypedElement
        {
            return XTypedServices.Load<TW, T>(reader, LinqToXsdTypeManager.Instance);
        }

        #endregion
    }
}