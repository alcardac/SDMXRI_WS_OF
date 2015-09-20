// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvisionRetrievalManager.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     Manages the retrieval of provision agreements using simple reference of structures that directly reference a provision
    /// </summary>
    public interface IProvisionRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the provision agreement that the registration is referencing
        /// </summary>
        /// <param name="registration">The Registration Object
        /// </param>
        /// <returns>
        /// The <see cref="IProvisionAgreementObject"/> .
        /// </returns>
        IProvisionAgreementObject GetProvision(IRegistrationObject registration);

        /// <summary>
        /// Gets a list of provisions that match the structure reference.
        ///     <p/>
        ///     The structure reference can either be referencing a Provision structure, a Data or MetadataFlow, or a DataProvider.
        /// </summary>
        /// <param name="provisionRef">
        /// The provision Ref.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProvisionAgreementObject}"/> .
        /// </returns>
        ISet<IProvisionAgreementObject> GetProvisions(IStructureReference provisionRef);

        /// <summary>
        /// Gets all the provision Agreements that are referencing the given dataflow
        /// </summary>
        /// <param name="dataflow">Dataflow Object
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProvisionAgreementObject}"/> .
        /// </returns>
        ISet<IProvisionAgreementObject> GetProvisions(IDataflowObject dataflow);

        /// <summary>
        /// Gets all the provision Agreements that are referencing the given metadataflow
        /// </summary>
        /// <param name="metadataflow">
        /// The metadataflow.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IProvisionAgreementObject}"/> .
        /// </returns>
        ISet<IProvisionAgreementObject> GetProvisions(IMetadataFlow metadataflow);

        #endregion
    }
}