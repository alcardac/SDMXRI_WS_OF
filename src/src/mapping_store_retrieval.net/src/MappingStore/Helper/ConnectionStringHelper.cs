// -----------------------------------------------------------------------
// <copyright file="ConnectionStringHelper.cs" company="Eurostat">
//   Date Created : 2012-06-15
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Configuration;

    /// <summary>
    /// Helper singleton class to get and save configuration settings
    /// </summary>
    public sealed class ConnectionStringHelper
    {
        /// <summary>
        /// The connection string name
        /// </summary>
        public const string ConnectionStringName = "MappingStoreServer";

        /// <summary>
        /// The protection provider
        /// </summary>
        private const string ProtectionProvider = "ConfiguredProtectedConfigurationProvider";
        
        /// <summary>
        /// This should be "configProtectedData"
        /// </summary>
        private const string ProtectionProviderSectionName = "configProtectedData";

        /// <summary>
        /// Singleton instance 
        /// </summary>
        private static readonly ConnectionStringHelper _instance = new ConnectionStringHelper();

        /// <summary>
        /// Prevents a default instance of the <see cref="ConnectionStringHelper"/> class from being created.
        /// </summary>
        private ConnectionStringHelper()
        {
        }

        /// <summary>
        /// Gets the singleton instance 
        /// </summary>
        public static ConnectionStringHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Gets the Mapping Store connection string settings
        /// </summary>
        public ConnectionStringSettings MappingStoreConnectionStringSettings
        {
            get
            {
                // TODO use openmappedexeconfiguration and save to program data
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                ConnectionStringsSection configSection = config.ConnectionStrings;
                if (configSection.SectionInformation.IsProtected)
                {
                    configSection.SectionInformation.UnprotectSection();
                }

                ConnectionStringSettings connectionStringSetting = configSection.ConnectionStrings[ConnectionStringName];
                return connectionStringSetting;
            }
        }

        /// <summary>
        /// Save connection string settings together with Protect Provider.
        /// </summary>
        /// <param name="connectionString">
        /// The connection String.
        /// </param>
        /// <param name="providerName">
        /// The provider Name.
        /// </param>
        public void Save(string connectionString, string providerName)
        {
            var connectionStringSettings = new ConnectionStringSettings(ConnectionStringName, connectionString, providerName);

            // save config
            // TODO use openmappedexeconfiguration and save to program data
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConnectionStringsSection configSection = config.ConnectionStrings;

            if (configSection.SectionInformation.IsProtected)
            {
                configSection.SectionInformation.UnprotectSection();
            }

            // just changing the connection string doesn't seem to work...
            configSection.ConnectionStrings.Remove(ConnectionStringName);
            
            config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);
            CreateProtectProvider(config);
            
            configSection.SectionInformation.ProtectSection(ProtectionProvider);
            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("configuration");
        }

        /// <summary>
        /// Gets or creates protected provider section
        /// </summary>
        /// <param name="config">The configuration</param>
        private static void CreateProtectProvider(Configuration config)
        {
            var protectedConfigurationSection = config.GetSection(ProtectionProviderSectionName) as ProtectedConfigurationSection;

            bool found = false;
            if (protectedConfigurationSection != null)
            {
                foreach (ProviderSettings provider in protectedConfigurationSection.Providers)
                {
                    if (provider.Name.Equals(ProtectionProvider))
                    {
                        found = true;
                        break;
                    }
                }
            }
            else
            {
                protectedConfigurationSection = new ProtectedConfigurationSection();
                config.Sections.Add(ProtectionProviderSectionName, protectedConfigurationSection);
            }

            if (!found)
            {
                protectedConfigurationSection.Providers.Add(
                    new ProviderSettings(ProtectionProvider, typeof(DpapiProtectedConfigurationProvider).AssemblyQualifiedName));
            }
        }

        /// <summary>
        /// Determines whether [has connection string].
        /// </summary>
        /// <returns>True if there are connection string set</returns>
        public static bool HasConnectionString()
        {
            return Instance.MappingStoreConnectionStringSettings != null;
        }
    }
}