// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSource.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     Describes the location and type of DataSsource
    /// </summary>
    public interface IDataSource : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Uri of the datasource, this can never be null
        /// </summary>
        /// <value> </value>
        Uri DataUrl { get; }

        /// <summary>
        ///     Gets a value indicating whether the the getDataUrl() is pointing at a REST web service
        /// </summary>
        /// <value> </value>
        bool RESTDatasource { get; }

        /// <summary>
        ///     Gets a value indicating whether the the getDataUrl() is pointing at a file location
        /// </summary>
        /// <value> </value>
        bool SimpleDatasource { get; }

        /// <summary>
        ///     Gets the Uri of the WSDL - will return null if no WSDL location has been defined
        /// </summary>
        /// <value> </value>
        Uri WsdlUrl { get; }

        /// <summary>
        ///     Gets the WADL Uri - will return null if no WADL location has been defined
        /// </summary>
        /// <value> </value>
        Uri WadlUrl { get; }

        /// <summary>
        ///     Gets a value indicating whether the the getDataUrl() is pointing at a SOAP web service
        /// </summary>
        /// <value> </value>
        bool WebServiceDatasource { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a mutable version of this agencySchemeMutableObject instance
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataSourceMutableObject" /> .
        /// </returns>
        IDataSourceMutableObject CreateMutableInstance();

        #endregion
    }
}