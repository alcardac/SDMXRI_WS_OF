// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowConfigurationSection.cs" company="Eurostat">
//   Date Created : 2013-05-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dataflow configuration section.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Configuration;

    using Estat.Sri.MappingStoreRetrieval.Engine;

    /// <summary>
    ///     The dataflow configuration section.
    /// </summary>
    public class DataflowConfigurationSection : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether ignore production for data (<see cref="MappingSetRetriever" />).
        /// </summary>
        [ConfigurationProperty(SettingConstants.IgnoreProductionForData, IsRequired = false, DefaultValue = false)]
        public bool IgnoreProductionForData
        {
            get
            {
                return (bool)this[SettingConstants.IgnoreProductionForData];
            }

            set
            {
                this[SettingConstants.IgnoreProductionForData] = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether ignore production for dataflow retrieval. Except for
        ///     <see
        ///         cref="MappingSetRetriever" />
        ///     . For that use <see cref="IgnoreProductionForData" />
        /// </summary>
        [ConfigurationProperty(SettingConstants.IgnoreProduction, IsRequired = false, DefaultValue = false)]
        public bool IgnoreProductionForStructure
        {
            get
            {
                return (bool)this[SettingConstants.IgnoreProduction];
            }

            set
            {
                this[SettingConstants.IgnoreProduction] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        #endregion
    }
}