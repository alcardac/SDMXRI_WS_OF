// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataPersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;

    #endregion

    /// <summary>
    ///     Manages the persistence of data obtained from a DataReaderEngine
    /// </summary>
    public interface IDataPersistenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Persists the data obtainable from the DataReaderEngine
        /// </summary>
        /// <param name="dataReader">
        /// - the reader to read the data in
        /// </param>
        void Persist(IDataReaderEngine dataReader);

        #endregion
    }
}