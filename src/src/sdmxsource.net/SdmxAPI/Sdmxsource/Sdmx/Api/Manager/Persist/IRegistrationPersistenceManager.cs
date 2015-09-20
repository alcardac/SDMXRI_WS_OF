// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationPersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     Manages the persistence of registrations
    /// </summary>
    public interface IRegistrationPersistenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Deletes a registration with the same urn of the passed in Registrations - if any registrations could not be found then an error will NOT be issued, the
        ///     registration will be ignored
        /// </summary>
        /// <param name="registration">The registration. </param>
        void DeleteRegistration(IRegistrationObject registration);

        /// <summary>
        /// Persists the Registration, treated as DatasetAction.REPLACE
        ///     <p/>
        ///     REPLACE action will replace any registration that matches the URN of the passed in Registration
        /// </summary>
        /// <param name="registration">The registration. </param>
        void ReplaceRegistration(IRegistrationObject registration);

        /// <summary>
        /// Persists the Registration, treated as DatasetAction.APPEND
        ///     <p/>
        ///     Will not overwrite a Registration if one already exists with the same URN
        /// </summary>
        /// <param name="registration">The registration.  </param>
        void SaveRegistration(IRegistrationObject registration);

        #endregion
    }
}