// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataConstraintRetrievalManager.cs" company="Eurostat">
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

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The DataConstraintRetrievalManager manages the retrieval of data constraints for a given constrainable artefact
    /// </summary>
    public interface IDataConstraintRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// The cube region takes into account whether data exists for a concept/code combination based
        ///     on the selected codes in the data query, the constraint codes are determined from what has been attached to the
        ///     constrainable artifact if it is given, if the constrainable artifact is null, then the key family from the data query
        ///     will be used as the constrainable artifact.
        ///     <p/>
        ///     A set of valid keys is returned based on the data query passed in, which contains concept/code selections,
        ///     in this way the result is `what is still a valid selection based on the selections that have already been made`
        /// </summary>
        /// <param name="dataQuery">
        /// The data Query.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IKeyValue}"/> .
        /// </returns>
        ISet<IKeyValue> FilterKeysUsingCubeRegion(IDataQuery dataQuery);

        /// <summary>
        /// Gets all the valid key values for this constrainable Object
        /// </summary>
        /// <param name="constrainable">
        /// The constrainable.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IKeyValue}"/> .
        /// </returns>
        ISet<IKeyValue> GetAllValidKeyValues(IConstrainableObject constrainable);

        /// <summary>
        /// Gets the end date for the constrainable Object
        /// </summary>
        /// <param name="constrainable">
        /// The constrainable.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/> .
        /// </returns>
        DateTime GetDataEndDate(IConstrainableObject constrainable);

        /// <summary>
        /// Gets the start date for the constrainable Object
        /// </summary>
        /// <param name="constrainable">
        /// The constrainable.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/> .
        /// </returns>
        DateTime GetDataStartDate(IConstrainableObject constrainable);

        /// <summary>
        /// Gets a value indicating whether the there are constraints available for this constrainable Object
        /// </summary>
        /// <param name="constrainable">
        /// The constrainable.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool HasConstraint(IConstrainableObject constrainable);

        #endregion
    }
}