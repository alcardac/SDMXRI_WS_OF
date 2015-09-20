// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Listener.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Listener
{
    /// <summary>
    /// A class that can listen for actions and be invoked.
    /// </summary>
    /// <typeparam name="T">
    /// The type to listen
    /// </typeparam>
    public interface IListener<in T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Listens for changes to object of type T and is notified on change and is passed in T.
        /// </summary>
        /// <param name="obj">
        /// The type of object to listen to.
        /// </param>
        void Invoke(T obj);

        #endregion
    }
}