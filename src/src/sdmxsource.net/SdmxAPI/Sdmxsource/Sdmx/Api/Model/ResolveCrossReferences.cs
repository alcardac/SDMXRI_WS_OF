// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolveCrossReferences.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    /// <summary>
    ///     Flag options for handling Cross References
    /// </summary>
    public enum ResolveCrossReferences
    {
        /// <summary>
        ///     DO NOT RESOLVE CROSS REFERENCES
        /// </summary>
        DoNotResolve, 

        /// <summary>
        ///     RESOLVE CROSS REFERENCES INCLUDING AGENCIES
        /// </summary>
        ResolveAll, 

        /// <summary>
        ///     RESOLVE CROSS REFERENCES EXCLUDING AGENCIES
        /// </summary>
        ResolveExcludeAgencies
    }
}