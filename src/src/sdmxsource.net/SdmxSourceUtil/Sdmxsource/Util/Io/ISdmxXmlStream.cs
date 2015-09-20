// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxXmlStream.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Io
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The interface used for XML streaming and carrying SDMX-ML message information
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="ISdmxXmlStream" />.
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureParsingManagerFast.cs" lang="cs" />
    /// </example> 
    public interface ISdmxXmlStream : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether it has reader.
        /// </summary>
        bool HasReader { get; }

        /// <summary>
        ///     Gets a value indicating whether it has writer.
        /// </summary>
        bool HasWriter { get; }

        /// <summary>
        ///     Gets the message type.
        /// </summary>
        MessageEnumType MessageType { get; }

        /// <summary>
        ///     Gets the query message type.
        /// </summary>
        IList<QueryMessageEnumType> QueryMessageTypes { get; }

        /// <summary>
        ///     Gets the reader.
        /// </summary>
        XmlReader Reader { get; }

        /// <summary>
        ///     Gets the registry type.
        /// </summary>
        RegistryMessageEnumType RegistryType { get; }

        /// <summary>
        ///     Gets the schema enum type.
        /// </summary>
        SdmxSchemaEnumType SdmxVersion { get; }

        /// <summary>
        ///     Gets the writer.
        /// </summary>
        XmlWriter Writer { get; }

        #endregion
    }
}