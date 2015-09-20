// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchical.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// A IHierarchical object is one which can contain a single hierarchy of objects,
    ///     the type of which is defined by generics.
    /// </summary>
    /// <typeparam name="T">
    /// - the type of object in the hierarchy
    /// </typeparam>
    public interface IHierarchical<T>
    {
        #region Public Properties

        /// <summary>
        ///     Gets any children of this object.  If there are no children
        ///     then a <c>null</c> will be returned.
        /// </summary>
        /// <value> child categories </value>
        IList<T> Children { get; }

        /// <summary>
        ///     Gets the parent Object of this object, a null object reference will be
        ///     returned if rther is no parent
        /// </summary>
        /// <value> </value>
        T Parent { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets <c>true</c> if this Object contains children,
        ///     if this is the case the method call <c>getChildren()</c> is
        ///     guaranteed to return a Set with length greater then 0.
        /// </summary>
        /// <returns> true if this IHierarchical Object contains children </returns>
        bool HasChildren();

        /// <summary>
        ///     Gets <c>true</c> if this Object has a parent, false otherwise.
        /// </summary>
        /// <returns> true if this Object has a parent </returns>
        bool HasParent();

        #endregion
    }
}