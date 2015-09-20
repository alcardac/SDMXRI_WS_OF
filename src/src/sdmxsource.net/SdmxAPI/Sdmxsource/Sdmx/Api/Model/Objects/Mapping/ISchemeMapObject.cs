// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemeMapObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Scheme Map
    /// </summary>
    public interface ISchemeMapObject : INameableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the source ref.
        /// </summary>
        ICrossReference SourceRef { get; }

        /// <summary>
        ///     Gets the target ref.
        /// </summary>
        ICrossReference TargetRef { get; }

        #endregion
    }
}