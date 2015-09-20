// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossDataWriterBuilder.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The cross data writer builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    ///     The cross data writer builder.
    /// </summary>
    public class CrossDataWriterBuilder : IWriterBuilder<ICrossSectionalWriterEngine, XmlWriter>, IWriterBuilder<ICrossSectionalWriterEngine, Stream>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="ICrossSectionalWriterEngine" />.
        /// </returns>
        public ICrossSectionalWriterEngine Build(Stream writer, Queue<Action> actions)
        {
            return this.Build(XmlWriter.Create(writer), actions);
        }

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="ICrossSectionalWriterEngine" />.
        /// </returns>
        public ICrossSectionalWriterEngine Build(XmlWriter writer, Queue<Action> actions)
        {
            return new DelayedCrossWriterEngine(actions, new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)));
        }

        #endregion
    }
}