// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryMessage.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V20.message
{
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.compact;

    using Xml.Schema.Linq;

    /// <summary>
    /// The query message.
    /// </summary>
    public partial class QueryMessage
    {
        #region Public Methods and Operators

        /// <summary>
        /// Load error message to a LINQ2XSD <see cref="QueryMessage"/> object
        /// </summary>
        /// <param name="xmlFile">
        /// The input file
        /// </param>
        /// <returns>
        /// a LINQ2XSD <see cref="Structure"/> object
        /// </returns>
        public static QueryMessage Load(XmlReader xmlFile)
        {
            return XTypedServices.Load<QueryMessage, QueryMessageType>(xmlFile, LinqToXsdTypeManager.Instance);
        }

        #endregion
    }
}