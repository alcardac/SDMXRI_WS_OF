// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DispatchByBodyElementOperationSelector.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dispatch by body element operation selector.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Wsdl
{
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.ServiceModel.Web;
    using System.Xml;

    /// <summary>
    ///     The dispatch by body element operation selector.
    /// </summary>
    /// <remarks>Based on <see href="http://msdn.microsoft.com/en-us/library/ms750531(v=vs.100).aspx" /> </remarks>
    public class DispatchByBodyElementOperationSelector : IDispatchOperationSelector
    {
        #region Fields

        /// <summary>
        ///     The _default operation name.
        /// </summary>
        private readonly string _defaultOperationName;

        /// <summary>
        ///     The _dispatch dictionary.
        /// </summary>
        private readonly IDictionary<XmlQualifiedName, string> _dispatchDictionary;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchByBodyElementOperationSelector"/> class.
        /// </summary>
        /// <param name="defaultOperationName">
        /// The default operation name.
        /// </param>
        /// <param name="dispatchDictionary">
        /// The dispatch dictionary.
        /// </param>
        public DispatchByBodyElementOperationSelector(string defaultOperationName, IDictionary<XmlQualifiedName, string> dispatchDictionary)
        {
            this._defaultOperationName = defaultOperationName;
            this._dispatchDictionary = dispatchDictionary;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Associates a local operation with the incoming method.
        /// </summary>
        /// <returns>
        /// The name of the operation to be associated with the <paramref name="message"/>.
        /// </returns>
        /// <param name="message">
        /// The incoming <see cref="T:System.ServiceModel.Channels.Message"/> to be associated with an
        ///     operation.
        /// </param>
        public string SelectOperation(ref Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            var lookupQName = new XmlQualifiedName(bodyReader.LocalName, bodyReader.NamespaceURI);
            message = CreateMessageCopy(message, bodyReader);
            if (this._dispatchDictionary.ContainsKey(lookupQName))
            {
                return this._dispatchDictionary[lookupQName];
            }

            if (this._defaultOperationName == null)
            {
                throw new WebFaultException<string>(lookupQName.ToString(), HttpStatusCode.BadRequest);
            }

            return this._defaultOperationName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create message copy.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        private static Message CreateMessageCopy(Message message, XmlDictionaryReader body)
        {
            Message copy = Message.CreateMessage(message.Version, message.Headers.Action, body);
            copy.Headers.CopyHeaderFrom(message, 0);
            copy.Properties.CopyProperties(message.Properties);
            return copy;
        }

        #endregion
    }
}