// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRestSchemaQuery.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Query
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// Defines a schema query that contains information defined by the RESTful API
    /// </summary>
    public interface IRestSchemaQuery : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Returns the reference to the structure the query is for, this can be:
        /// <ul>
        ///  <li>Data Structure</li>
        ///  <li>Dataflow</li>
        ///  <li>Provision Agreement</li>
        /// </summary>
        IStructureReference Reference
        {
            get;
        }

        /// <summary>
        /// Returns the dimension at observation
        /// </summary>
        string DimAtObs
        {
            get;
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// For cross-sectional data validation, indicates whether observations are strongly typed
        /// </summary>
        /// <returns>
        /// The boolean
        /// </returns>
        bool IsExplicitMeasure();

        #endregion
    }
}
