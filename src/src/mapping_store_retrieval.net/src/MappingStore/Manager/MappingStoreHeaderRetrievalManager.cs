// -----------------------------------------------------------------------
// <copyright file="MappingStoreHeaderRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-10
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Configuration;

    using Estat.Sri.MappingStoreRetrieval.Engine;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    /// The mapping store header retrieval manager.
    /// </summary>
    public class MappingStoreHeaderRetrievalManager : IHeaderRetrievalManager
    {
        /// <summary>
        /// The _get header.
        /// </summary>
        private readonly Func<HeaderRetrieverEngine, IHeader> _getHeader;

        /// <summary>
        /// The header retriever engine.
        /// </summary>
        private readonly HeaderRetrieverEngine _headerRetrieverEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreHeaderRetrievalManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings for Mapping Store database.
        /// </param>
        /// <param name="getHeader">
        /// The method that gets the Header.
        /// </param>
        public MappingStoreHeaderRetrievalManager(ConnectionStringSettings connectionStringSettings, Func<HeaderRetrieverEngine, IHeader> getHeader)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            if (getHeader == null)
            {
                throw new ArgumentNullException("getHeader");
            }

            this._getHeader = getHeader;
            this._headerRetrieverEngine = new HeaderRetrieverEngine(new Database(connectionStringSettings));
        }

        /// <summary>
        /// Gets a header object
        /// </summary>
        public IHeader Header
        {
            get
            {
                return this._getHeader(this._headerRetrieverEngine);
            }
        }
    }
}