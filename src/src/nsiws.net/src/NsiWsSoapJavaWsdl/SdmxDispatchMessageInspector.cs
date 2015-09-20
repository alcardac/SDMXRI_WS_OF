// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxDispatchMessageInspector.cs" company="Eurostat">
//   Date Created : 2013-10-25
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sdmx dispatch message inspector.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The SDMX dispatch message inspector.
    /// </summary>
    public class SdmxDispatchMessageInspector : IDispatchMessageInspector
    {
        #region Fields

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the
        ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/>
        ///     method.
        /// </returns>
        /// <param name="request">
        /// The request message.
        /// </param>
        /// <param name="channel">
        /// The incoming channel.
        /// </param>
        /// <param name="instanceContext">
        /// The current service instance.
        /// </param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">
        /// The reply message. This value is null if the operation is one way.
        /// </param>
        /// <param name="correlationState">
        /// The correlation object returned from the
        ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/>
        ///     method.
        /// </param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply == null)
            {
                return;
            }

            if (reply.IsEmpty)
            {
            }
        }

        #endregion
    }
}