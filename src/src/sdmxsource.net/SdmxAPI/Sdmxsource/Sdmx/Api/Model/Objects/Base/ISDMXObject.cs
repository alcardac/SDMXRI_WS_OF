// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISDMXObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     An SdmxObject represents any SDMX structural artefact or metadata structure artefact
    ///     <p />
    ///     All classes which inherit from SdmxObject are immutable, meaning they can not have any of their contents modified.
    ///     Any collections returned as a result of a method call will be copies of collections ensuring the immutability of the
    ///     SdmxObject is preserved.  This 'copy paradigm' is also true of composite objects returned, which are mutable, for example any object
    ///     of type <see cref="IList{T}"/> will be a copy of the underlying Date object contained in the SDXMObject.
    /// </summary>
    public interface ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a set of composite Objects to this sdmxObject
        /// </summary>
        ISet<ISdmxObject> Composites { get; }

        /// <summary>
        ///     Gets a set of cross references that are made by this sdmxObject, or by any composite sdmxObject of this sdmxObject
        /// </summary>
        ISet<ICrossReference> CrossReferences { get; }

        /// <summary>
        ///     Gets the parent that this SdmxObject belongs to
        ///     <p />
        ///     If this is a Maintainable Object, then there will be no parent to return, so will return a value of null
        /// </summary>
        ISdmxObject Parent { get; }

        /// <summary>
        ///     Gets the structure type of this component.
        /// </summary>
        /// <value> </value>
        SdmxStructureType StructureType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a value indicating whether the SdmxObject equals the given sdmxObject in every respect (except for the validTo property of a maintainable artefact, this is not taken into consideration)
        ///     <p/>
        ///     This method calls deepEquals on any SdmxObject composites.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject.
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties);

        /// <summary>
        /// Gets any composites of this SdmxObject of the given type
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <typeparam name="T">
        /// Generic type parameter 
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISet{T}"/>.
        /// </returns>
        ISet<T> GetComposites<T>(Type type);

        /// <summary>
        /// Visits all items up the parent hierarchy to return the first occurrence of parent of the given type that this SdmxObject belongs to
        ///     <p/>
        ///     If a parent of the given type does not exist in the hierarchy, null will be returned
        /// </summary>
        /// <typeparam name="T">
        /// Generic type parameter. 
        /// </typeparam>
        /// <param name="includeThisInSearch">
        /// if true then this type will be first checked to see if it is of the given type 
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T GetParent<T>(bool includeThisInSearch) where T : class;

        #endregion
    }
}