// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImplementationElement.cs" company="Eurostat">
//   Date Created : 2011-09-07
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Abstract class for *Implementation elements
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule.Config
{
    using System;
    using System.Configuration;
    using System.Globalization;

    /// <summary>
    /// Abstract class for *Implementation elements
    /// </summary>
    public abstract class ImplementationElement : ConfigurationElement
    {
        /// <summary>
        /// The attribute name of the implementation type attribute
        /// </summary>
        private const string ImplementationTypeName = "type";

        #region Public Properties

        /// <summary>
        /// Gets or sets the implementation base type. It uses the syntax accepted by <see cref="System.Type.GetType(string)"/> method
        /// </summary>
        [ConfigurationProperty(ImplementationTypeName, IsRequired = true)]
        public string ImplementationType
        {
            get
            {
                return (string)this[ImplementationTypeName];
            }

            set
            {
                this["type"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if the implementation is valid
        /// </summary>
        /// <param name="value">
        /// The type name
        /// </param>
        /// <exception cref="AuthConfigurationException">
        /// <see cref="Errors.ImplementationMissingAttr"/>, <see cref="Errors.ImplementationCannotLoad"/>
        /// </exception>
        public static void TypeValidator(object value)
        {
            var typeName = value as string;
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(Errors.ImplementationMissingAttr);
            }

            Type type = Type.GetType(typeName, false);
            if (type == null)
            {
                throw new AuthConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Errors.ImplementationCannotLoad, value));
            }
        }

        #endregion
    }
}