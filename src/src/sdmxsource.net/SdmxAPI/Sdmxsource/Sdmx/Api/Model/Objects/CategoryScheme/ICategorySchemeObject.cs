// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorySchemeObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Category Scheme
    /// </summary>
    public interface ICategorySchemeObject : IItemSchemeObject<ICategoryObject>
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICategorySchemeMutableObject MutableInstance { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the category with the given id, additional ids are used to represent the categories parents if it is in a hierarchy.
        ///     <p/>
        ///     Gets null if the category can not be found with the given id
        /// </summary>
        /// <param name="id">Category id array
        /// </param>
        /// <returns>
        /// The <see cref="ICategoryObject"/> .
        /// </returns>
        ICategoryObject GetCategory(params string[] id);

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub @object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <param name="actualLocation">
        /// The actual Location.
        /// </param>
        /// <param name="isServiceUrl">
        /// The is Service Uri.
        /// </param>
        /// <returns>
        /// The <see cref="ICategorySchemeObject"/> .
        /// </returns>
        new ICategorySchemeObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}