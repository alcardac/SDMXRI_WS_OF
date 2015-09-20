// -----------------------------------------------------------------------
// <copyright file="AuthAdvancedMutableStructureSearchManagerBase.cs" company="Eurostat">
//   Date Created : 2013-09-18
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System.Collections.Generic;

    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// The base class for <see cref="IAuthAdvancedMutableStructureSearchManager"/>
    /// </summary>
    public class AuthAdvancedMutableStructureSearchManagerBase
    {
        /// <summary>
        /// Retrieve the structures referenced by <paramref name="structureQuery" /> and populate the
        /// <paramref name="mutableObjects" />
        /// </summary>
        /// <param name="retrievalManager">The retrieval manager.</param>
        /// <param name="mutableObjects">The mutable objects.</param>
        /// <param name="structureQuery">The structure query.</param>
        /// <param name="allowedDataflows">The allowed Dataflows.</param>
        /// <param name="crossReferenceMutableRetrievalManager">The cross reference mutable retrieval manager.</param>
        protected virtual void PopulateMutables(
            IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalManager,
            IMutableObjects mutableObjects,
            IComplexStructureQuery structureQuery,
            IList<IMaintainableRefObject> allowedDataflows,
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager)
        {
            //// changes here might also apply to AuthMutableStructureSearchManagerBase and/or AuthStructureRetrieverV21 
            var complexStructureQueryDetail = structureQuery.StructureQueryMetadata != null ? structureQuery.StructureQueryMetadata.StructureQueryDetail : ComplexStructureQueryDetail.GetFromEnum(ComplexStructureQueryDetailEnumType.Full);
            mutableObjects.AddIdentifiables(retrievalManager.GetMutableMaintainables(structureQuery.StructureReference, complexStructureQueryDetail, allowedDataflows));
        }
    }
}