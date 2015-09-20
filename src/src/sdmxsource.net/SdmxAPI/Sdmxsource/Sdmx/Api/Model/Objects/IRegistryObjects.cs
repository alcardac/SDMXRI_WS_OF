// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistryObjects.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     IRegistryObjects is a container for all registry content including the following; SdmxObjects which is a container for all structural metadata,
    ///     RegistrationObjects and SubscriptionObjects.
    /// </summary>
    public interface IRegistryObjects
    {
        #region Public Properties

        /// <summary>
        ///     Gets all the registrations in this container, returns an empty list if there are none
        /// </summary>
        /// <value> </value>
        IList<IRegistrationObject> Registrations { get; }

        /// <summary>
        ///     Gets the Sdmx objects contained in this container, returns <c>null</c> if there are none
        /// </summary>
        /// <value> </value>
        ISdmxObjects SdmxObjects { get; }

        /// <summary>
        ///     Gets all the subscriptions in this container, returns an empty list if there are none
        /// </summary>
        IList<ISubscriptionObject> Subscriptions { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the there are IRegistrationObject contained in this container
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasRegistrationObject();

        /// <summary>
        ///     Gets a value indicating whether the there are SdmxObjects contained in this container
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasSdmxObject();

        /// <summary>
        ///     Gets a value indicating whether the there are ISubscriptionObject contained in this container
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasSubscriptionObject();

        #endregion
    }
}