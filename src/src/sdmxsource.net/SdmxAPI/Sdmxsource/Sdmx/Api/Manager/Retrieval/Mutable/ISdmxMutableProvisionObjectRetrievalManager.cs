// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxMutableProvisionObjectRetrievalManager.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Manages the retrieval of mutable provision agreements using simple reference of structures that directly reference a provision
    /// </summary>
    public interface ISdmxMutableProvisionObjectRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a list of provisions that match the maintainable reference.
        ///     <p/>
        ///     Gets an empty list if no provisions match the criteria
        /// </summary>
        /// <param name="xref">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// The <see cref="IList{IProvisionAgreementMutableObject}"/> .
        /// </returns>
        IList<IProvisionAgreementMutableObject> GetProvisions(IStructureReference xref);

        #endregion
    }
}