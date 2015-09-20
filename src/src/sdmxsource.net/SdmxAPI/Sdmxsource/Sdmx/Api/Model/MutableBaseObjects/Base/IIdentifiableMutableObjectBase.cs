// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableMutableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base
{
    using System;

    /// <summary>
    ///     An Identifiable Object is one which can be identified uniquely with a URN.
    ///     <p />
    ///     An identifiable also has other attributes such as name and description
    /// </summary>
    public interface IIdentifiableMutableObjectBase : IAnnotableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the Id of the Identifiable Object
        /// </summary>
        /// <returns> </returns>
        string Id { get; set; }

        /// <summary>
        ///     Gets or sets the URN of the Identifiable Object
        /// </summary>
        /// <returns> </returns>
         Uri Urn { get; set; }

        #endregion
    }
}