// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvisionAgreementObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Provision Agreement
    /// </summary>
    public interface IProvisionAgreementObject : IMaintainableObject, IConstrainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a reference to the data provider that this provision agreement is for.
        ///     <p />
        ///     This reference is mandatory and will never return <c>null</c>
        /// </summary>
        /// <value> </value>
        ICrossReference DataproviderRef { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IProvisionAgreementMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets the reference to the flow, this will either be a dataflow or metadataflow reference.
        ///     <p />
        ///     This reference is mandatory and will never return <c>null</c>
        /// </summary>
        /// <value> </value>
        ICrossReference StructureUseage { get; }

        #endregion
    }
}