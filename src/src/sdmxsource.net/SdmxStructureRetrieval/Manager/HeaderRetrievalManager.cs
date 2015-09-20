// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderRetrievalManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The header retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Manager
{
    using System;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;

    public class HeaderRetrievalManager : IHeaderRetrievalManager
    {
        /// <summary>
        ///     The sender id.
        /// </summary>
        private string _senderId = "unknown";

        /// <summary>
        /// Gets and sets the sender id.
        /// </summary>
        public string SenderId
        {
            get { return _senderId; }
            set { _senderId = value; }
        }

        /// <summary>
        ///     Gets the header object
        /// </summary>
        public IHeader Header
        {
            get { return new HeaderImpl("IDREF" + Guid.NewGuid().ToString(), _senderId); }
        }

    }
}
