// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchicalCodelistObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX IHierarchical Code List
    /// </summary>
    public interface IHierarchicalCodelistObject : IMaintainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets any CodelistRefObjects as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if no ICodelistRef exist
        /// </summary>
        /// <value> </value>
        IList<ICodelistRef> CodelistRef { get; }

        /// <summary>
        ///     Gets any child hierarchies as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if no child hierarchies exist
        /// </summary>
        /// <value> </value>
        IList<IHierarchy> Hierarchies { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IHierarchicalCodelistMutableObject MutableInstance { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub @object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistObject"/> .
        /// </returns>
        /// <param name="actualLocation">
        /// the Uri indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        new IHierarchicalCodelistObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}