// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContactMutableObject.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IContactMutableObject : IMutableObject
    {
        /// <summary>
        /// Gets or sets the id of the contact
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets the names of the contact.
        /// </summary>
        /// <value>
        /// list of names, or an empty list if none exist
        /// </value>
        IList<ITextTypeWrapperMutableObject> Names { get; }

        /// <summary>
        /// Adds a new name to the list, creates a new list if it is null
        /// </summary>
        /// <param name="name">The name.</param>
        void AddName(ITextTypeWrapperMutableObject name);

        /// <summary>
        /// Gets or sets the roles of the contact
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        IList<ITextTypeWrapperMutableObject> Roles { get; }

        /// <summary>
        /// Adds a new role to the list, creates a new list if it is null
        /// </summary>
        /// <param name="role"></param>
        void AddRole(ITextTypeWrapperMutableObject role);


        /// <summary>
        /// Gets or sets the departments of the contact
        /// </summary>
        IList<ITextTypeWrapperMutableObject> Departments { get; }

        /// <summary>
        /// Adds a new department to the list, creates a new list if it is null
        /// </summary>
        /// <param name="dept"></param>
        void AddDepartment(ITextTypeWrapperMutableObject dept);

        /// <summary>
        /// Gets the email of the contact
        /// </summary>
        IList<string> Email { get; } 

        /// <summary>
        /// Adds a new email to the list, creates a new list if it is null
        /// </summary>
        /// <param name="email"></param>
        void AddEmail(string email);

        /// <summary>
        /// Gets the fax of the contact
        /// </summary>
        IList<string> Fax { get; }

        /// <summary>
        /// Adds a new fax to the list, creates a new list if it is null
        /// </summary>
        /// <param name="fax"></param>
        void AddFax(string fax);

        /// <summary>
        /// Gets the telephone of the contact
        /// </summary>
        IList<string> Telephone { get; }

        /// <summary>
        /// Adds a new telephone to the list, creates a new list if it is null
        /// </summary>
        /// <param name="telephone"></param>
        void AddTelephone(string telephone);
        /**
         * Returns the uris of the contact
         * @return list of uris, or an empty list if none exist
         */
        /// <summary>
        /// 
        /// </summary>
        IList<string> Uri { get; }

        /// <summary>
        /// Adds a new uri to the list, creates a new list if it is null
        /// </summary>
        /// <param name="uri"></param>
        void AddUri(string uri);

        /// <summary>
        /// Returns the x400 of the contact
        /// </summary>
        IList<string> X400 { get; }

        /// <summary>
        /// Adds a new X400 to the list, creates a new list if it is null
        /// </summary>
        /// <param name="x400"></param>
        void AddX400(string x400);
    }
}
