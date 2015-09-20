// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxMutableRegistrationPersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist.Mutable
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

    #endregion

    /// <summary>
    ///     Manages the persistence of registrations - this interface deals with mutable sdmxObjects, so extra validation is performed to
    ///     ensure the mutable @object conforms to the SDMX rules
    /// </summary>
    public interface ISdmxMutableRegistrationPersistenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Deletes a registration with the same urn of the passed in Registrations - if any registartions could not be found then an error will NOT be issued, the
        ///     registration will be ignored
        /// </summary>
        /// <param name="registration">The registration object. </param>
        void DeleteRegistration(IRegistrationMutableObject registration);

        /// <summary>
        /// Replaces any registrations against the provisions that the registrations specify
        /// </summary>
        /// <param name="registration"> The registration object. </param>
        void ReplaceRegistration(IRegistrationMutableObject registration);

        /// <summary>
        /// Stores the registration and returns a copy of the stored instance, in mutable form
        /// </summary>
        /// <param name="registrationMutableObject">The registration object. </param>
        /// <returns>
        /// The <see cref="IRegistrationMutableObject"/> .
        /// </returns>
        IRegistrationMutableObject SaveRegistration(IRegistrationMutableObject registrationMutableObject);

        #endregion
    }
}