// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComputationObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Process
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///   ComputationType describes a computation in a process.
    /// </summary>
    public interface IComputationObject : IAnnotableObject
    {
        #region Public Properties

        /// <summary>
        ///   Gets the description describe the computation in any form desired by the user
        ///   (these are informational rather than machine-actionable),
        ///   and so may be supplied in multiple, parallel-language versions,
        /// </summary>
        /// <value> </value>
        IList<ITextTypeWrapper> Description { get; }

        /// <summary>
        ///   Gets the localID attribute is an optional identification for the computation within the process.
        /// </summary>
        /// <value> </value>
        string LocalId { get; }

        /// <summary>
        ///   Gets the softwareLanguage attribute holds the coding language that the software package used to perform the computation is written in.
        /// </summary>
        /// <value> </value>
        string SoftwareLanguage { get; }

        /// <summary>
        ///   Gets the softwarePackage attribute holds the name of the software package that is used to perform the computation.
        /// </summary>
        /// <value> </value>
        string SoftwarePackage { get; }

        /// <summary>
        ///   Gets the softwareVersion attribute hold the version of the software package that is used to perform that computation.
        /// </summary>
        /// <value> </value>
        string SoftwareVersion { get; }

        #endregion
    }
}