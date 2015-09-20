// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSourceMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    using System;

    /// <summary>
    ///     The DataSourceMutableObject interface.
    /// </summary>
    public interface IDataSourceMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the data url.
        /// </summary>
         Uri DataUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether rest datasource.
        /// </summary>
        bool RESTDatasource { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether simple datasource.
        /// </summary>
        bool SimpleDatasource { get; set; }

        /// <summary>
        ///     Gets or sets the wadl url.
        /// </summary>
         Uri WADLUrl { get; set; }

        /// <summary>
        ///     Gets or sets the wsdl url.
        /// </summary>
         Uri WSDLUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether web service datasource.
        /// </summary>
        bool WebServiceDatasource { get; set; }

        #endregion
    }
}