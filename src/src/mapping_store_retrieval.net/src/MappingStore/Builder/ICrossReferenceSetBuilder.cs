// -----------------------------------------------------------------------
// <copyright file="ICrossReferenceSetBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-25
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The CrossReference set Builder interface.
    /// </summary>
    public interface ICrossReferenceSetBuilder : IBuilder<ISet<IStructureReference>, IIdentifiableMutableObject>
    {
    }
}