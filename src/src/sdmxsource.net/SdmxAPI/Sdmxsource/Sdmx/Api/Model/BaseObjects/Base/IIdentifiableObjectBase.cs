// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    using System;

    /// <summary>
    ///     An Identifiable Object is one which can be identified uniquely with a URN.
    ///     <p />
    ///     An identifiable also has other attributes such as name and description
    /// </summary>
    public interface IIdentifiableObjectBase : IAnnotableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Id of the Identifiable Object
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets the URN of the Identifiable Object
        /// </summary>
        /// <value> </value>
        Uri Urn { get; }

        #endregion
    }
}