// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderSettings.cs" company="Eurostat">
//   Date Created : 2013-10-13
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The header settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Constants
{
    using System.ComponentModel;
    using System.Configuration;

    // This class allows you to handle specific events on the settings class:
    // The SettingChanging event is raised before a setting's value is changed.
    // The PropertyChanged event is raised after a setting's value is changed.
    // The SettingsLoaded event is raised after the setting values are loaded.
    // The SettingsSaving event is raised before the setting values are saved.
    /// <summary>
    /// The header settings.
    /// </summary>
    internal sealed partial class HeaderSettings
    {
        #region Methods

        /// <summary>
        /// The setting changing event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        /// <summary>
        /// The settings saving event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }

        #endregion
    }
}