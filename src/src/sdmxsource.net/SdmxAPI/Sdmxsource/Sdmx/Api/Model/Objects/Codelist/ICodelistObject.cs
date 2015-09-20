// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodelistObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Code List
    /// </summary>
    public interface ICodelistObject : IItemSchemeObject<ICode>
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICodelistMutableObject MutableInstance { get; }

        /// <summary>
        ///   Gets a value indicating whether the codelist is only reporting a subset of the codes.
        ///     <p />
        ///     Partial codelists are used for dissemination purposes only not for reporting updates.
        /// </summary>
        /// <value> </value>
        new bool Partial { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the code with the given id, returns null if there is no code with the id provided
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ICode"/> .
        /// </returns>
        ICode GetCodeById(string id);

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub @object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <returns>
        /// The <see cref="ICodelistObject"/> .
        /// </returns>
        /// <param name="actualLocation">
        /// the Uri indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        new ICodelistObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}