// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomAnnotationType.cs" company="Eurostat">
//   Date Created : 2013-07-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The custom annotation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Constant
{
    /// <summary>
    ///     The custom annotation type.
    /// </summary>
    public enum CustomAnnotationType
    {
        /// <summary>
        ///     The none.
        /// </summary>
        None = 0, 

        /// <summary>
        ///     The category scheme node order.
        /// </summary>
        CategorySchemeNodeOrder,

        /// <summary>
        /// The SDMX v2.0 only.
        /// </summary>
        SDMXv20Only,

        /// <summary>
        /// The SDMX v2.1 only.
        /// </summary>
        SDMXv21Only,

        /// <summary>
        /// The Dataflow is not in PRODUCTION 
        /// </summary>
        NonProductionDataflow
    }
}