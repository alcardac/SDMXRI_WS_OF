// -----------------------------------------------------------------------
// <copyright file="IStructureSearchManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-09-20
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The StructureSearchManagerFactory interface.
    /// </summary>
    /// <typeparam name="TStructureSearch">
    /// The type of the structure search manager.
    /// </typeparam>
    public interface IStructureSearchManagerFactory<out TStructureSearch>
    {
        /// <summary>
        /// Returns an instance of the structure search manager created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="sdmxSchemaVersion">
        /// The SDMX Schema Version.
        /// </param>
        /// <returns>
        /// The structure search manager.
        /// </returns>
        TStructureSearch GetStructureSearchManager<T>(T settings, SdmxSchema sdmxSchemaVersion);
    }
}