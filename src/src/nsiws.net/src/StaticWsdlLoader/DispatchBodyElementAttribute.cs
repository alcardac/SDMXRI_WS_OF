// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DispatchBodyElementAttribute.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dispatch body element attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Wsdl
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Constants;

    /// <summary>
    ///     The dispatch body element attribute.
    /// </summary>
    /// <remarks>Based on <see href="http://msdn.microsoft.com/en-us/library/ms750531(v=vs.100).aspx" /> </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class DispatchBodyElementAttribute : Attribute, IOperationBehavior
    {
        #region Fields

        /// <summary>
        ///     The Body wrapper name.
        /// </summary>
        private readonly XmlQualifiedName _qname;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchBodyElementAttribute"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public DispatchBodyElementAttribute(string name)
        {
            this._qname = new XmlQualifiedName(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchBodyElementAttribute"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="ns">
        /// The namespace.
        /// </param>
        public DispatchBodyElementAttribute(string name, string ns)
        {
            this._qname = new XmlQualifiedName(name, ns);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchBodyElementAttribute"/> class.
        /// </summary>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <param name="ns">
        /// The namespace.
        /// </param>
        public DispatchBodyElementAttribute(SoapOperation operation, string ns)
        {
            this._qname = new XmlQualifiedName(operation.ToString(), ns);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the q name.
        /// </summary>
        internal XmlQualifiedName QName
        {
            get
            {
                return this._qname;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="operationDescription">
        /// The operation being examined. Use for examination only. If the operation description
        ///     is modified, the results are undefined.
        /// </param>
        /// <param name="bindingParameters">
        /// The collection of objects that binding elements require to support the behavior.
        /// </param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">
        /// The operation being examined. Use for examination only. If the operation description
        ///     is modified, the results are undefined.
        /// </param>
        /// <param name="clientOperation">
        /// The run-time object that exposes customization properties for the operation described by
        ///     <paramref name="operationDescription"/>.
        /// </param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">
        /// The operation being examined. Use for examination only. If the operation description
        ///     is modified, the results are undefined.
        /// </param>
        /// <param name="dispatchOperation">
        /// The run-time object that exposes customization properties for the operation described
        ///     by <paramref name="operationDescription"/>.
        /// </param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">
        /// The operation being examined. Use for examination only. If the operation description
        ///     is modified, the results are undefined.
        /// </param>
        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }
}