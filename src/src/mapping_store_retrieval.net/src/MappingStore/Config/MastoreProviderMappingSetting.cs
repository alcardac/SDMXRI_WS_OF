// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MastoreProviderMappingSetting.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This element allow to configure database related settings. Such mapping store vendor name and database provider
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Configuration;

    /// <summary>
    /// This element allow to configure database related settings. Such mapping store vendor name and database provider
    /// </summary>
    public class MastoreProviderMappingSetting : ConfigurationElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoreProviderMappingSetting"/> class with the specified <paramref name="name"/>
        /// </summary>
        /// <param name="name">
        /// Sets the <see cref="Name"/>.
        /// </param>
        public MastoreProviderMappingSetting(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoreProviderMappingSetting"/> class.
        /// </summary>
        public MastoreProviderMappingSetting()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Mapping Store DDB vendor name
        /// </summary>
        [ConfigurationProperty(SettingConstants.NameAttributeName, IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[SettingConstants.NameAttributeName];
            }

            set
            {
                this[SettingConstants.NameAttributeName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Mapping Store DDB provider (driver name)
        /// </summary>
        [ConfigurationProperty(SettingConstants.ProviderAttributeName, IsRequired = true)]
        public string Provider
        {
            get
            {
                return (string)this[SettingConstants.ProviderAttributeName];
            }

            set
            {
                this[SettingConstants.ProviderAttributeName] = value;
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}