// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Retrieves Identifiable structures from ICrossReference
    /// </summary>
    public interface IIdentifiableRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the agency with the given Id, if the Agency is a child of another Agency (other then SDMX), then it should be a dot separated id, for 
        /// example DEMO.SUBDEMO
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>agency, or null if none could be found with the supplied id</returns>
        IAgency GetAgency(string id);

        /// <summary>
        /// Gets the identifiable objects.
        /// </summary>
        /// <typeparam name="T">The type of the identifiable objects to return.</typeparam>
        /// <param name="structureReference">The structure reference.</param>
        /// <returns>Returns a set of identifiable objects that match the structure reference, which may be a full or partial reference to a maintainable or identifiable</returns>
        ISet<T> GetIdentifiableObjects<T>(IStructureReference structureReference) where T : IIdentifiableObject;
        
        /// <summary>
        /// Resolves an identifiable reference
        /// </summary>
        /// <param name="crossReference"> Cross-reference object
        /// </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/> .
        /// </returns>
        /// <exception cref="CrossReferenceException">
        /// if the ICrossReference could not resolve to an IIdentifiableObject
        /// </exception>
        IIdentifiableObject GetIdentifiableObject(ICrossReference crossReference);

        /// <summary>
        /// Resolves an reference to a Object of type T, this will return the Object of the given type, throwing an exception if either the
        ///     Object can not be resolved or if it is not of type T
        /// </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="crossReference">
        ///     Cross-reference object
        /// </param>
        /// <returns>
        /// The <see cref="T"/> .
        /// </returns>
        /// <exception cref="CrossReferenceException">
        /// if the ICrossReference could not resolve to an IIdentifiableObject
        /// </exception>
        T GetIdentifiableObject<T>(ICrossReference crossReference);

        /// <summary>
        /// Resolves an reference to a Object of type T, this will return the Object of the given type, throwing an exception if e
        ///     Object is not of type T
        /// </summary>
        /// <typeparam name="T">Generic type parameter.
        /// </typeparam>
        /// <param name="crossReference">Structure-reference object
        /// </param>
        /// <returns>
        /// The <see cref="T"/> .
        /// </returns>
        /// <exception cref="SdmxReferenceException">
        /// if the ICrossReference could not resolve to an IIdentifiableObject
        /// </exception>
        T GetIdentifiableObject<T>(IStructureReference crossReference);

        #endregion
    }
}