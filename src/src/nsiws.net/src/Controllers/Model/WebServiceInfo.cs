// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebServiceInfo.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds Endpoint/service information for use with the index page
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Model
{
    /// <summary>
    ///     This class holds Endpoint/service information for use with the index page
    /// </summary>
    public class WebServiceInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the Namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        ///     Gets or sets the SDMXMessage.xsd path
        /// </summary>
        public string SchemaPath { get; set; }

        #endregion
    }
}