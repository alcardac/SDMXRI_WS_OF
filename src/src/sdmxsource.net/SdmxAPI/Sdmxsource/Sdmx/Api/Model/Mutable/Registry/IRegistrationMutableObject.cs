// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationMutableObject.cs" company="Eurostat">
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

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The RegistrationMutableObject interface.
    /// </summary>
    public interface IRegistrationMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the data source.
        /// </summary>
        IDataSourceMutableObject DataSource { get; set; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the RegistrationObject.
        /// </summary>
        /// <value> </value>
        new IRegistrationObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets or sets the index attributes.
        /// </summary>
        TertiaryBool IndexAttributes { get; set; }

        /// <summary>
        ///     Gets or sets the index dataset.
        /// </summary>
        TertiaryBool IndexDataset { get; set; }

        /// <summary>
        ///     Gets or sets the index reporting period.
        /// </summary>
        TertiaryBool IndexReportingPeriod { get; set; }

        /// <summary>
        ///     Gets or sets the index time series.
        /// </summary>
        TertiaryBool IndexTimeseries { get; set; }

        /// <summary>
        ///     Gets or sets the last updated.
        /// </summary>
        DateTime? LastUpdated { get; set; }

        /// <summary>
        ///     Gets or sets the provision agreement ref.
        /// </summary>
        IStructureReference ProvisionAgreementRef { get; set; }

        /// <summary>
        ///     Gets or sets the valid from.
        /// </summary>
        DateTime? ValidFrom { get; set; }

        /// <summary>
        ///     Gets or sets the valid to.
        /// </summary>
        DateTime? ValidTo { get; set; }

        #endregion
    }
}