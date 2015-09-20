// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureBuilder.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The structure builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Controller.Builder

{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;

    using RDFProvider.Structure.Controllers.Builder;
    using RDFProvider.Structure.Manager.Output;
    using RDFProvider.Structure.Factory;
    using RDFProvider.Structure.Manager;

    /// <summary>
    ///     The structure builder.
    /// </summary>
    public class StructureBuilder : IRDFWriterBuilder<IStructureRDFWriterManager, XmlWriter>
    {
        #region Fields

        /// <summary>
        ///     The _endpoint.
        /// </summary>
        private readonly WebServiceEndpoint _endpoint;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureBuilder"/> class.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public StructureBuilder(WebServiceEndpoint endpoint)
        {
            this._endpoint = endpoint;
        }

        #endregion

        #region Public Methods and Operators

        public IStructureRDFWriterManager BuildRDF(XmlWriter writer, Queue<Action> actions)
        {
            actions.RunAll();
            IStructureRDFWriterManager structureWritingManager;         
            structureWritingManager = new StructureRDFWritingManager(new RDFStructureWriterFactory(writer));


            return structureWritingManager;
        }
        #endregion
    }
}