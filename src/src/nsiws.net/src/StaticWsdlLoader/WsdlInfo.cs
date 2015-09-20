// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WsdlInfo.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The wsdl info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Wsdl
{
    using System;

    /// <summary>
    /// The WSDL info.
    /// </summary>
    public class WsdlInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the original path.
        /// </summary>
        public string OriginalPath { get; set; }

        #endregion
    }
}