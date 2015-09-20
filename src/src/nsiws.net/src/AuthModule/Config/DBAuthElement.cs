// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBAuthElement.cs" company="Eurostat">
//   Date Created : 2011-09-07
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <see cref="DbAuthenticationProvider" /> and <see cref="DbAuthorizationProvider" /> configuration section
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule.Config
{
    using System.Configuration;

    /// <summary>
    /// The <see cref="DbAuthenticationProvider"/> and <see cref="DbAuthorizationProvider"/> configuration section
    /// </summary>
    public class DBAuthElement : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="DbAuthenticationProvider"/> configuration element
        /// </summary>
        [ConfigurationProperty("authentication", IsRequired = true)]
        public DBAuthenticationElement Authentication
        {
            get
            {
                return (DBAuthenticationElement)this["authentication"];
            }

            set
            {
                this["authentication"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DbAuthorizationProvider"/> configuration element
        /// </summary>
        [ConfigurationProperty("authorization")]
        public DBAuthorizationElement Authorization
        {
            get
            {
                return (DBAuthorizationElement)this["authorization"];
            }

            set
            {
                this["authorization"] = value;
            }
        }

        #endregion
    }
}