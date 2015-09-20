// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     Manages the retrieval of registrations by using simple structures that directly reference a registration
    /// </summary>
    public interface IRegistrationRetrievalManager
    {
        #region Public Properties

        /// <summary>
        ///     Gets all the registrations.  Gets an empty set if no registrations exist.
        /// </summary>
        /// <value> </value>
        ISet<IRegistrationObject> AllRegistrations { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the registration that matches the maintainable reference.  Gets null if no registrations match the criteria.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="IRegistrationObject"/> .
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// if more then one match is found for the reference (this is only possible if the reference does not uniquely identify a registration, this is only possible if the reference does not have all three parameters populated (agencyId, id and version)
        /// </exception>
        IRegistrationObject GetRegistration(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the registrations that match the maintainable reference.  
        /// Gets an empty set if no registrations exist that match the criteria.
        /// </summary>
        /// <param name="xref"> The xref object. </param>
        /// <returns> A set containing the registration objects.</returns>
        ISet<IRegistrationObject> GetRegistrations(IMaintainableRefObject xref);

        /// <summary>
        /// Gets all the registrations against the provision references
        ///     <p/>
        ///     The structure reference can either be referencing a Provision structure, a Data or MetdataFlow, or a DataProvider.
        /// </summary>
        /// <param name="structureRef">
        /// The provision Refs.
        /// </param>
        /// <returns>  A set containing the registration objects.</returns>
        ISet<IRegistrationObject> GetRegistrations(IStructureReference structureRef);

        /// <summary>
        /// Gets all the registrations against the provision
        /// </summary>
        /// <param name="provision">
        /// - the provision to return the registration for
        /// </param>
        /// <returns>  A set containing the registration objects.
        /// </returns>
        ISet<IRegistrationObject> GetRegistrations(IProvisionAgreementObject provision);

        #endregion
    }
}