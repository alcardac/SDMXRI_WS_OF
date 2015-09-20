// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComputationMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ComputationMutableObject interface.
    /// </summary>
    public interface IComputationMutableObject : IAnnotableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the description.
        /// </summary>
        IList<ITextTypeWrapperMutableObject> Descriptions { get; }

        /// <summary>
        ///     Gets or sets the local id.
        /// </summary>
        string LocalId { get; set; }

        /// <summary>
        ///     Gets or sets the software language.
        /// </summary>
        string SoftwareLanguage { get; set; }

        /// <summary>
        ///     Gets or sets the software package.
        /// </summary>
        string SoftwarePackage { get; set; }

        /// <summary>
        ///     Gets or sets the software version.
        /// </summary>
        string SoftwareVersion { get; set; }

        #endregion
    }
}