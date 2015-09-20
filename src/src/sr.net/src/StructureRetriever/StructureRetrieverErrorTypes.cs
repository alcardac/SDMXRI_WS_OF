// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureRetrieverErrorTypes.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This enumeration contains the StructureRetrieverException error types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever
{
    /// <summary>
    /// This enumeration contains the StructureRetrieverException error types.
    /// </summary>
    public enum StructureRetrieverErrorTypes
    {
        /// <summary>
        ///   Error parsing the input RegistryInterfaceBean
        /// </summary>
        ParsingError,

        /// <summary>
        ///   The mapping store DB takes too long to respond
        /// </summary>
        MappingStoreTimeout,

        /// <summary>
        ///   Could not establish a connection to the mapping store DB
        /// </summary>
        MappingStoreConnectionError,

        /// <summary>
        ///   Could not establish a connection to the mapping store DB, invalid credentials
        /// </summary>
        MappingStoreInvalidCredentials,

        /// <summary>
        ///   Requested structure not found
        /// </summary>
        MissingStructure,

        /// <summary>
        ///   A reference of the requested structure was not found and IsResolveReferences = true
        /// </summary>
        MissingStructureRef,

        /// <summary>
        ///   Could not establish a connection to the dissemination DB
        /// </summary>
        DdbConnectionError
    }
}