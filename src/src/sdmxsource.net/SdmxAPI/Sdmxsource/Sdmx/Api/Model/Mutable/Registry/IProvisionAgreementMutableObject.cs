// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvisionAgreementMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The ProvisionAgreementMutableObject interface.
    /// </summary>
    public interface IProvisionAgreementMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the dataprovider ref.
        /// </summary>
        IStructureReference DataproviderRef { get; set; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IProvisionAgreementObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets or sets the structure usage.
        /// </summary>
        IStructureReference StructureUsage { get; set; }

        #endregion
    }
}