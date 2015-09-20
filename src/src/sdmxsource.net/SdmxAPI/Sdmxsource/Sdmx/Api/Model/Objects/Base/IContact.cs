// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContact.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a contact (individual or organisation)
    /// </summary>
    public interface IContact : ISdmxObject
    {
        /// <summary>
        /// Gets the id of the contact the id, or null if there is no id
        /// </summary>
        string Id{ get; }

        /// <summary>
        /// Gets the names of the contact list of names, or an empty list if none exist
        /// </summary>
        IList<ITextTypeWrapper> Name{ get; }

       
        /// <summary>
        /// Gets the roles of the contact
        /// </summary>
        IList<ITextTypeWrapper> Role{ get; }

        /// <summary>
        /// Gets the departments of the contact
        /// </summary>
        IList<ITextTypeWrapper> Departments{ get; }

     
        /// <summary>
        /// Gets the email of the contact
        /// </summary>
        IList<string> Email{ get; }

    
        /// <summary>
        /// Gets the fax of the contact
        /// </summary>
        IList<string> Fax{ get; }

     
        /// <summary>
        /// Gets the telephone of the contact
        /// </summary>
        IList<string> Telephone{ get; }
      
        /// <summary>
        /// Gets the uris of the contact
        /// </summary>
        IList<string> Uri{ get; }

    
        /// <summary>
        /// Gets the x400 of the contact
        /// </summary>
        IList<string> X400{ get; }
    }
}
