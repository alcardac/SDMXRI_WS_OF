// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFileValidityStateExtension.cs" company="Eurostat">
//   Date Created : 2014-03-20
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data file validity state extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    /// <summary>
    ///     The data file validity state extension.
    /// </summary>
    public static class DataFileValidityStateExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified data file validity state is set.
        /// </summary>
        /// <param name="dataFileValidityState">
        /// State of the data file validity.
        /// </param>
        /// <returns>
        /// True if it is UnSet
        /// </returns>
        public static bool IsSet(this DataFileValidityState dataFileValidityState)
        {
            return dataFileValidityState != DataFileValidityState.Unset;
        }

        /// <summary>
        /// Determines whether the specified data file validity state is unset.
        /// </summary>
        /// <param name="dataFileValidityState">
        /// State of the data file validity.
        /// </param>
        /// <returns>
        /// True if it is UnSet
        /// </returns>
        public static bool IsUnset(this DataFileValidityState dataFileValidityState)
        {
            return dataFileValidityState == DataFileValidityState.Unset;
        }

        #endregion
    }
}