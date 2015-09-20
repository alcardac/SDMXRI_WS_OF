// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrganisationMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    using System.Collections.Generic;

    /// <summary>
    ///     The OrganisationMutableObject interface.
    /// </summary>
    public interface IOrganisationMutableObject : IItemMutableObject
    {
        /**
 * Returns all the contacts for this organisation, this might reutrn null
 * @return
 */

        /// <summary>
        /// Gets all the contacts for this organisation, this might return empty collection.
        /// </summary>
        IList<IContactMutableObject> Contacts { get; }

        /**
         * Add a new contact to the list
         * @param contact
         */
        /// <summary>
        /// Add a new contact to the list
        /// </summary>
        /// <param name="contact">The contact.</param>
        void AddContact(IContactMutableObject contact);
    }
}