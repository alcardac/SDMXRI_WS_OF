// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IItemObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    /// <summary>
    /// An Item is an object which lives inside of an Item Scheme
    /// </summary>
    /// <typeparam name="T">
    /// The parent item scheme
    /// </typeparam>
    public interface IItemObjectBase<out T> : INameableObjectBase
        where T : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Item Scheme that this item lives inside
        /// </summary>
        T ItemScheme { get; }

        #endregion
    }
}