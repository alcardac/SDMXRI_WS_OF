// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractFactory.cs" company="Eurostat">
//   Date Created : 2011-06-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Abstract class for building Factories containing the common code
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Abstract class for building Factories containing the common code
    /// </summary>
    public abstract class AbstractFactory
    {
        #region Methods

        /// <summary>
        /// Create an instance of the specified type
        /// </summary>
        /// <typeparam name="T">
        /// The base type of the interface to create
        /// </typeparam>
        /// <param name="typeName">
        /// The type name of the implementation to create
        /// </param>
        /// <returns>
        /// The instance of the specified type or null if it fails
        /// </returns>
        protected static T Create<T>(string typeName) where T : class
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            T instance = null;
            try
            {
                Type type = Type.GetType(typeName);
                if (type != null)
                {
                    instance = Activator.CreateInstance(type) as T;
                }
            }
            catch (TargetInvocationException ex)
            {
                // TODO add proper logging
                Trace.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is AuthConfigurationException)
                    {
                        throw ex.InnerException;
                    }

                    throw new AuthConfigurationException(ex.InnerException.Message, ex.InnerException);
                }

                throw;
            }
            catch (Exception ex)
            {
                // TODO add proper logging
                Trace.WriteLine(ex.ToString());
                throw;
            }

            return instance;
        }

        #endregion
    }
}