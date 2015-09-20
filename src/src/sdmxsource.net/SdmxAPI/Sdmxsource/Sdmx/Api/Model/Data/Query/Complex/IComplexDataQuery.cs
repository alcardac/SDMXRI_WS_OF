// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexDataQuery.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    /// It is a representation of a SOAP DataQuery 2.1
    /// </summary>
    public interface IComplexDataQuery : IBaseDataQuery
    {
        #region Public Properties

        /// <summary>
        /// Returns the suggested maximum response size or null if unspecified
        /// </summary>
        int? DefaultLimit
        {
            get;
        }

        /// <summary>
        /// Returns the type of observations to be returned.<br>
        /// Defaults to ACTIVE
        /// </summary>
        ObservationAction ObservationAction
        {
            get;
        }

        /// <summary>
        /// Returns true or false depending on the existence of explicit measures
        /// Defaults to false
        /// </summary>
        /// <returns>
        /// The boolean
        /// </returns>
        bool HasExplicitMeasures();

        /// <summary>
        /// Returns the id of the dataset from which data will be returned or null if unspecified
        /// </summary>
        string DatasetId
        {
            get;
        }

        /// <summary>
        /// The id of the dataset requested should satisfy the condition implied by the operator this method returns. <br>
        /// For instance if the operator is starts_with then data from a dataset with id that starts with the getDatasetId() result
        /// should be returned.
        /// Defaults to EQUAL
        /// </summary>
        TextSearch DatasetIdOperator
        {
            get;
        }

        /// <summary>
        /// This method accomplishes the following criteria :match data based on when they were last updated <br>
        /// The date points to the start date or the range that the queried date must occur within and/ or to the end period of the range <br>
        /// It returns null if unspecified.
        /// </summary>
        IList<ITimeRange> LastUpdatedDateTimeRange
        {
            get;
        }

        /// <summary>
        /// Returns a list of selection groups. The list is empty if no selection groups have been added.
        /// </summary>
        IList<IComplexDataQuerySelectionGroup> SelectionGroups
        {
            get;
        }

        /// <summary>
        /// Returns the Provision Agreement that the query is returning data for or null if unspecified
        /// </summary>
        IProvisionAgreementObject ProvisionAgreement
        {
            get;
        }

        #endregion
    }
}
