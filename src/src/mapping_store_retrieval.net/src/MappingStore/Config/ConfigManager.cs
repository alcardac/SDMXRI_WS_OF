//-----------------------------------------------------------------------
// <copyright file="ConfigManager.cs" company="Eurostat">
//  Date Created : 2011-09-08 12:35
//  Copyright (c) 2011 by the European Commission, represented by Eurostat. All rights reserved.
//
//  Licensed under the European Union Public License (EUPL) version 1.1. 
//  If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
//-----------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Configuration;

    /// <summary>
    /// Mapping Store related configuration manager
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// The .config section that holds the configuration
        /// </summary>
        private const string EstatNsiMappingStore = "estat.sri/mapping.store";

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly ConfigManager _instance = new ConfigManager();

        /// <summary>
        /// The Mapping Store configuration section
        /// </summary>
        private readonly MappingStoreConfigSection _config;

        /// <summary>
        /// Prevents a default instance of the <see cref="ConfigManager"/> class from being created. 
        /// </summary>
        private ConfigManager()
        {
            this._config = (MappingStoreConfigSection)ConfigurationManager.GetSection(EstatNsiMappingStore)
                           ?? new MappingStoreConfigSection();
        }

        /// <summary>
        /// Gets the Mapping Store configuration section
        /// </summary>
        public static MappingStoreConfigSection Config
        {
            get
            {
                return _instance._config;
            }
        }

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static ConfigManager Instance 
        {
            get 
            {
                return _instance;
            }
        }
    }
}