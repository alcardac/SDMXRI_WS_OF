// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Builder
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    /// Any Classes implementing this interface are capable of building an object of one type <typeparamref name="TK"/>
    ///     from an object of another type <typeparamref name="TV"/>
    /// </summary>
    /// <typeparam name="TK">
    /// - The type of object that gets built by the builder
    /// </typeparam>
    /// <typeparam name="TV">
    /// - The type of object that the builder requires to build the object from
    /// </typeparam>
    public interface IBuilder<out TK, in TV>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type <typeparamref name="TK"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <typeparamref name="TK"/>
        /// </returns>
        /// <exception cref="SdmxException">
        /// - If anything goes wrong during the build process
        /// </exception>
        TK Build(TV buildFrom);

        #endregion
    }
}