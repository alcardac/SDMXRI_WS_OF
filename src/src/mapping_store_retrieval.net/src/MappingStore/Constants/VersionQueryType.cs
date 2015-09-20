// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionQueryType.cs" company="Eurostat">
//   Date Created : 2013-02-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The version query type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    /// <summary>
    /// The version query type.
    /// </summary>
    public enum VersionQueryType
    {
        /// <summary>
        /// Return all/any version
        /// </summary>
        All, 

        /// <summary>
        /// Return only the latest version
        /// </summary>
        Latest
    }
}