// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INameableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Nameable Objects carry a mandatory name and an optional description, Nameable Objects are also Identifiable
    /// </summary>
    public interface INameableObject : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the description in the default locale
        /// </summary>
        /// <value> first locale value it finds or null if there are none </value>
        string Description { get; }

        /// <summary>
        ///     Gets a list of descriptions for this component
        ///     <p />
        ///     <b>NOTE:</b>The list is a copy so modifying the returned list will not
        ///     be reflected in the IIdentifiableObject instance
        /// </summary>
        /// <value> </value>
        IList<ITextTypeWrapper> Descriptions { get; }

        /// <summary>
        ///     Gets the name in the default locale
        /// </summary>
        /// <value> </value>
        string Name { get; }

        /// <summary>
        ///     Gets a list of names for this component - will return an empty list if no Names exist.
        ///     <p />
        ///     <b>NOTE:</b>The list is a copy so modifying the returned list will not
        ///     be reflected in the IIdentifiableObject instance
        /// </summary>
        /// <value> first locale value it finds or null if there are none </value>
        IList<ITextTypeWrapper> Names { get; }

        #endregion
    }
}