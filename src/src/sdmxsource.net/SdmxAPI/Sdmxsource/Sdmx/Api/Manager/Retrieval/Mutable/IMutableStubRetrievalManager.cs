// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableStubRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     This interfaces is responsible for returning stub Objects (mutable) from reference objects
    /// </summary>
    public interface IMutableStubRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a set of maintainable mutable Objects from a structure reference
        /// </summary>
        /// <param name="structureReference"> The structure reference
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> .
        /// </returns>
        ISet<IMaintainableMutableObject> GetStubObjects(IStructureReference structureReference);

        #endregion
    }
}