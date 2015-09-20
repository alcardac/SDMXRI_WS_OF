// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorisationObject.cs" company="Eurostat">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Categorisation
    /// </summary>
    public interface ICategorisationObject : IMaintainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a reference to the category that is categorising the structure - this can not be null
        /// </summary>
        ICrossReference CategoryReference { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICategorisationMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets a reference to the structure that this is categorising - this can not be null
        /// </summary>
        /// <value> </value>
        ICrossReference StructureReference { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub @object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <param name="actualLocation">
        /// the URL indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        /// <returns>
        /// The <see cref="ICategorisationObject"/> .
        /// </returns>
        new ICategorisationObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}