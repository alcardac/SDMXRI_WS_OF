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
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Factory;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Structureparser.Factory;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;

    /// <summary>
    ///     The structure builder.
    /// </summary>
    public class StructureBuilder : IWriterBuilder<IStructureWriterManager, XmlWriter>
    {
        #region Fields

        /// <summary>
        ///     The _endpoint.
        /// </summary>
        private readonly WebServiceEndpoint _endpoint;

        /// <summary>
        ///     The _schema.
        /// </summary>
        private readonly SdmxSchema _schema;

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
        public StructureBuilder(WebServiceEndpoint endpoint, SdmxSchema schema)
        {
            this._endpoint = endpoint;
            this._schema = schema;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="IStructureWriterManager" />.
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException">Unsupported format.</exception>
        public IStructureWriterManager Build(XmlWriter writer, Queue<Action> actions)
        {
            actions.RunAll();
            IStructureWriterManager structureWritingManager;
            switch (this._schema.EnumType)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    structureWritingManager = this._endpoint == WebServiceEndpoint.EstatEndpoint
                                                  ? new StructureWriterManager(new SdmxStructureWriterV2Factory(writer))
                                                  : new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    structureWritingManager = new StructureWriterManager(new SdmxStructureWriterFactory(writer));
                    break;
                default:
                    throw new SdmxSemmanticException(string.Format("Unsupported format {0}", this._schema));
            }

            return structureWritingManager;
        }

        #endregion
    }
}