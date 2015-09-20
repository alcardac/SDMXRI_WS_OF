// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A generic builder interface which builds a <typeparamref name="T" /> from a <typeparamref name="TE" /> using the <see cref="Build" /> method
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Builders
{
    /// <summary>
    /// A generic builder interface which builds a <typeparamref name="T"/> from a <typeparamref name="TE"/> using the <see cref="Build"/> method
    /// </summary>
    /// <typeparam name="T">
    /// The output type 
    /// </typeparam>
    /// <typeparam name="TE">
    /// The input type 
    /// </typeparam>
    internal interface IBuilder<T, TE>
    {
        #region Public Methods

        /// <summary>
        /// The method that builds a <typeparamref name="T"/> from a <typeparamref name="TE"/>
        /// </summary>
        /// <param name="info">
        /// The input as <typeparamref name="TE"/> . 
        /// </param>
        /// <returns>
        /// The output as <typeparamref name="T"/> 
        /// </returns>
        T Build(TE info);

        #endregion
    }
}