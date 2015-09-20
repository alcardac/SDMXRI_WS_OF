// -----------------------------------------------------------------------
// <copyright file="IAdvancedMutableStructureSearchManager.cs" company="Eurostat">
//   Date Created : 2013-06-11
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Manager
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    ///  Implementations of this interface are able to process a complex structure query and return the <see cref="IMutableObjects"/> containing <see cref="IMaintainableMutableObject"/> which correspond to the query parameters
    /// </summary>
    public interface IAuthAdvancedMutableStructureSearchManager
    {
        /// <summary>
        /// Process the specified <paramref name="structureQuery"/> returning an <see cref="IMutableObjects"/> container which contains the Maintainable Structure hat correspond to the <paramref name="structureQuery"/> query parameters.
        /// </summary>
        /// <param name="structureQuery">
        /// The structure query.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        IMutableObjects GetMaintainables(IComplexStructureQuery structureQuery, IList<IMaintainableRefObject> allowedDataflows);
    }
}