// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubscriptionMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The SubscriptionMutableObject interface.
    /// </summary>
    public interface ISubscriptionMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the http post to.
        /// </summary>
        IList<string> HttpPostTo { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ISubscriptionObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets the mail to.
        /// </summary>
        IList<string> MailTo { get; }

        /// <summary>
        ///     Gets or sets the owner.
        /// </summary>
        IStructureReference Owner { get; set; }

        /// <summary>
        ///     Gets the references.
        /// </summary>
        IList<IStructureReference> References { get; }

        /// <summary>
        ///     Gets or sets the subscription type.
        /// </summary>
        SubscriptionEnumType SubscriptionType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add reference.
        /// </summary>
        /// <param name="reference">
        /// The reference.
        /// </param>
        void AddReference(IStructureReference reference);

        #endregion
    }
}