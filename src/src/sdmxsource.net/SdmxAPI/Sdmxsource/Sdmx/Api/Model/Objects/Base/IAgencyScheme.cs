// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAgencyScheme.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX AgencyScheme
    /// </summary>
    public interface IAgencyScheme : IOrganisationScheme<IAgency>
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the the agency Id of the scheme is that same as IAgencyScheme.DEFAULT_SCHEME
        /// </summary>
        bool DefaultScheme { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        new IAgencySchemeMutableObject MutableInstance { get; }

        #endregion
    }
}