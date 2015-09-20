// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableStructureSearchManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The MutableRetrievalManagerFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    ///     The MutableRetrievalManagerFactory interface.
    /// </summary>
    public interface IMutableStructureSearchManagerFactory : IStructureSearchManagerFactory<IMutableStructureSearchManager>
    {
    }
}