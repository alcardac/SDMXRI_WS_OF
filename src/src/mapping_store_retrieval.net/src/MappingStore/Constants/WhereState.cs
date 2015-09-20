// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhereState.cs" company="Eurostat">
//   Date Created : 2013-03-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The where state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    /// <summary>
    /// The SQL WHERE state.
    /// </summary>
    internal enum WhereState
    {
        /// <summary>
        /// No WHERE clause exists.
        /// </summary>
        Nothing, 

        /// <summary>
        /// The WHERE clause exists but nothing else.
        /// </summary>
        Where, 

        /// <summary>
        /// The WHERE clause exists together with clauses.
        /// </summary>
        And
    }
}