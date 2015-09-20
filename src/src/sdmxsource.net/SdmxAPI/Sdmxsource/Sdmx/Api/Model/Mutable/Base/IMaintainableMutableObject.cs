// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintainableMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The MaintainableMutableObject interface.
    /// </summary>
    public interface IMaintainableMutableObject : INameableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the agency id.
        /// </summary>
        string AgencyId { get; set; }

        /// <summary>
        ///     Gets or sets the end date.
        /// </summary>
        DateTime? EndDate { get; set; }

        /// <summary>
        ///     Gets or sets the external reference.
        /// </summary>
        TertiaryBool ExternalReference { get; set; }

        /// <summary>
        ///     Gets or sets the final structure.
        /// </summary>
        TertiaryBool FinalStructure { get; set; }

        /// <summary>
        ///     Gets a representation of itself in a Object which can not be modified, modifications to the mutable Object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        IMaintainableObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets or sets the service url.
        /// </summary>
         Uri ServiceURL { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        DateTime? StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the structure url.
        /// </summary>
         Uri StructureURL { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether stub.
        /// </summary>
        bool Stub { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        string Version { get; set; }

        #endregion
    }
}