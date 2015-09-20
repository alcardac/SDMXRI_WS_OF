// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingStoreConfigSection.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Mapping store related configuration
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Configuration;

    /// <summary>
    /// Mapping store related configuration
    /// </summary>
    public class MappingStoreConfigSection : ConfigurationSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the dataflow Settings
        /// </summary>
        [ConfigurationProperty(SettingConstants.DataflowConfiguration)]
        public DataflowConfigurationSection DataflowConfiguration
        {
            get
            {
                return (DataflowConfigurationSection)this [SettingConstants.DataflowConfiguration];
            }
        }

        /// <summary>
        /// Gets Dissemination Database (DDB) Settings
        /// </summary>
        [ConfigurationProperty(SettingConstants.DisseminationDatabaseSettings)]
        public MastoreProviderMappingSettingCollection DisseminationDatabaseSettings
        {
            get
            {
                return (MastoreProviderMappingSettingCollection)this[SettingConstants.DisseminationDatabaseSettings];
            }
        }

        /// <summary>
        /// Gets Dissemination Database (DDB) Settings
        /// </summary>
        [ConfigurationProperty(SettingConstants.GeneralDatabaseSettings)]
        public DatabaseSettingCollection GeneralDatabaseSettings
        {
            get
            {
                return (DatabaseSettingCollection)this[SettingConstants.GeneralDatabaseSettings];
            }
        }

        #endregion
    }
}