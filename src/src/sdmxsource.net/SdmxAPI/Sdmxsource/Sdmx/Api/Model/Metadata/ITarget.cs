// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITarget.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Target contains a set of target reference values which when taken together,
    ///     identify the object or objects to which the reported metadata apply.
    /// </summary>
    public interface ITarget : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the reference values.
        /// </summary>
        IList<IReferenceValue> ReferenceValues { get; }

        #endregion
    }
}