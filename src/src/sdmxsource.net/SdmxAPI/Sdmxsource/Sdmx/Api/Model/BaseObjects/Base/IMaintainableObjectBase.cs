// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintainableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A Maintainable Object is one that is maintainable by a maintenance agency
    /// </summary>
    public interface IMaintainableObjectBase : INameableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the agency id of the agency maintaining this object
        /// </summary>
        /// <value> </value>
        string AgencyId { get; }

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IMaintainableObject BuiltFrom { get; }

        /// <summary>
        ///     Gets a value indicating whether the maintainable is final
        /// </summary>
        /// <value> </value>
        bool Final { get; }

        /// <summary>
        ///     Gets the version of this maintainable object
        /// </summary>
        /// <value> </value>
        string Version { get; }

        #endregion
    }
}