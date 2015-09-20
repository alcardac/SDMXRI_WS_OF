// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageFunction.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The message function.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Constants
{
    /// <summary>
    /// The message function.
    /// </summary>
    public enum MessageFunction
    {
        /// <summary>
        /// The null.
        /// </summary>
        Null = 0, 

        /// <summary>
        /// The statistical definitions.
        /// </summary>
        StatisticalDefinitions = 73, 

        /// <summary>
        /// The statistical data.
        /// </summary>
        StatisticalData = 74, 

        /// <summary>
        /// The data set list.
        /// </summary>
        DataSetList = -1
    }
}