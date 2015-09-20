// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintainableMutableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base
{
    /// <summary>
    ///     A Maintainable Object is one that is maintainable by a maintenance agency
    /// </summary>
    public interface IMaintainableMutableObjectBase : INameableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the agency id of the agency maintaining this object
        /// </summary>
        /// <returns> </returns>
        string AgencyId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this maintainable is final
        /// </summary>
        /// <returns> </returns>
        bool IsFinal { get; set; }

        /// <summary>
        ///     Gets or sets the version of this maintainable object
        /// </summary>
        /// <returns> </returns>
        string Version { get; set; }

        #endregion
    }
}