// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolveExternalSetting.cs" company="Eurostat">
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
    ///     Flag options for handling External References (structures not present but specified with a URI,
    ///     indicating a remote file containing the missing structures).
    /// </summary>
    public enum ResolveExternalSetting
    {
        /// <summary>
        ///     DO NOT RESOLVE EXTERNAL REFERENCES
        /// </summary>
        DoNotResolve, 

        /// <summary>
        ///     RESOLVE EXTERNAL REFERENCES
        /// </summary>
        Resolve, 

        /// <summary>
        ///     RESOLVE EXTERNAL REFERENCES AND SUBSTITUTE THEM IN FOR THE STUB
        /// </summary>
        ResolveSubstitute, 

        /// <summary>
        ///     RESOLVE EXTERNAL REFERENCES - ADD ERROR ANNOTATIONS IF ERRORS OCCUR
        /// </summary>
        ResolveLenient, 

        /// <summary>
        ///     RESOLVE EXTERNAL REFERENCES AND SUBSTITUTE THEM IN FOR THE STUB - ADD ERROR ANNOTATIONS IF ERRORS OCCUR
        /// </summary>
        ResolveSubstituteLenient, 
    }
}