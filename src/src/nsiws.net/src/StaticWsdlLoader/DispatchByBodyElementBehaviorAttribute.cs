// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DispatchByBodyElementBehaviorAttribute.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dispatch by body element behavior attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Wsdl
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Xml;

    /// <summary>
    ///     The dispatch by body element behavior attribute.
    /// </summary>
    /// <remarks>Based on <see href="http://msdn.microsoft.com/en-us/library/ms750531(v=vs.100).aspx" /> </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DispatchByBodyElementBehaviorAttribute : Attribute, IContractBehavior
    {
        #region Public Methods and Operators

        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract description to modify.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint to modify.
        /// </param>
        /// <param name="bindingParameters">
        /// The objects that binding elements require to support the behavior.
        /// </param>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract description for which the extension is intended.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="clientRuntime">
        /// The client runtime.
        /// </param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract description to be modified.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint that exposes the contract.
        /// </param>
        /// <param name="dispatchRuntime">
        /// The dispatch runtime that controls service execution.
        /// </param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            var dispatchDictionary = new Dictionary<XmlQualifiedName, string>();
            foreach (OperationDescription operationDescription in 
                contractDescription.Operations)
            {
                var dispatchBodyElement = operationDescription.Behaviors.Find<DispatchBodyElementAttribute>();
                if (dispatchBodyElement != null)
                {
                    dispatchDictionary.Add(dispatchBodyElement.QName, operationDescription.Name);
                }
            }

            dispatchRuntime.OperationSelector = new DispatchByBodyElementOperationSelector(dispatchRuntime.UnhandledDispatchOperation.Name, dispatchDictionary);
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract to validate.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint to validate.
        /// </param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}