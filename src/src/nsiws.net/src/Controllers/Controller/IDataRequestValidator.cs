// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataRequestValidator.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The DataRequestValidator interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    ///     The DataRequestValidator interface.
    /// </summary>
    public interface IDataRequestValidator
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates the specified data query.
        /// </summary>
        /// <param name="dataQuery">
        /// The data query.
        /// </param>
        void Validate(IBaseDataQuery dataQuery);

        #endregion
    }
}