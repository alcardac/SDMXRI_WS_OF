// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Registry
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The RegistrationObjectBase interface.
    /// </summary>
    public interface IRegistrationObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the BuidlFrom object
        /// </summary>
        new IRegistrationObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the data source.
        /// </summary>
        IDataSource DataSource { get; }

        /// <summary>
        ///     Gets the index attribtues.
        /// </summary>
        TertiaryBool IndexAttribtues { get; }

        /// <summary>
        ///     Gets the index dataset.
        /// </summary>
        TertiaryBool IndexDataset { get; }

        /// <summary>
        ///     Gets the index reporting period.
        /// </summary>
        TertiaryBool IndexReportingPeriod { get; }

        /// <summary>
        ///     Gets the index time series.
        /// </summary>
        TertiaryBool IndexTimeSeries { get; }

        /// <summary>
        ///     Gets the last updated.
        /// </summary>
        ISdmxDate LastUpdated { get; }

        /// <summary>
        ///     Gets the provision agreement that this registration is referencing
        /// </summary>
        /// <value> </value>
        IProvisionAgreementObjectBase ProvisionAgreement { get; }

        /// <summary>
        ///     Gets the valid from.
        /// </summary>
        ISdmxDate ValidFrom { get; }

        /// <summary>
        ///     Gets the valid to.
        /// </summary>
        ISdmxDate ValidTo { get; }

        #endregion
    }
}