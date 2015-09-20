// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubscriptionObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Subscription
    /// </summary>
    public interface ISubscriptionObject : IMaintainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of HTTP addresses to POST any notifications to
        /// </summary>
        /// <value> </value>
        IList<string> HTTPPostTo { get; }

        /// <summary>
        ///     Gets a list of email addresses to mail any notifications to
        /// </summary>
        /// <value> </value>
        IList<string> MailTo { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ISubscriptionMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets a reference to the owner of this subscription
        /// </summary>
        /// <value> </value>
        ICrossReference Owner { get; }

        /// <summary>
        ///     Gets a list of structures that this subscription is subscribing to,
        ///     or in the case that this is subscribing to a provision or registration, returns the structure
        ///     reference that it is subscribing to the provision or registration by.
        /// </summary>
        /// <value> </value>
        IList<IStructureReference> References { get; }

        /// <summary>
        ///     Gets if this is a subscription for a structure event a provision event or a registration event
        /// </summary>
        /// <value> </value>
        SubscriptionEnumType SubscriptionType { get; }

        #endregion
    }
}