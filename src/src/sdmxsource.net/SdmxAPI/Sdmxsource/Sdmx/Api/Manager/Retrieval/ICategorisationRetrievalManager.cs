// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorisationRetrievalManager.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    #endregion

    /// <summary>
    ///     Responsible for retrieving categorisations for particular structures
    /// </summary>
    public interface ICategorisationRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets categorisations for a given identifiable
        /// </summary>
        /// <param name="identifiable">Identifiable Object
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationObject}"/> .
        /// </returns>
        ISet<ICategorisationObject> GetCategorisations(IIdentifiableObject identifiable);

        /// <summary>
        /// Gets the categorisations for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>all categorisations for the given category</returns>
        ISet<ICategorisationObject> GetCategorisationsForCategory(ICategoryObject category);

        /// <summary>
        /// Gets the categorisation base objects for a given identifiable
        /// </summary>
        /// <param name="identifiable">Identifiable Object </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationObjectBase}"/> .
        /// </returns>
        ISet<ICategorisationObjectBase> GetCategorisationBaseObjectForCategory(IIdentifiableObject identifiable);

        /// <summary>
        /// Gets the categorisation base objects for a given category, optionally a filter of allowable categorised
        ///     structure types can be returned
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <param name="includeTypes">
        /// The include Types.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationObjectBase}"/> .
        /// </returns>
        ISet<ICategorisationObjectBase> GetCategorisationBaseObjectForCategory(ICategoryObject category, params SdmxStructureType[] includeTypes);

        /// <summary>
        /// Gets the categorizations for category scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns>all categorizations for the given category scheme</returns>
        ISet<ICategorisationObject> GetCategorisationsForCategoryScheme(ICategorySchemeObject scheme);

        #endregion
    }
}