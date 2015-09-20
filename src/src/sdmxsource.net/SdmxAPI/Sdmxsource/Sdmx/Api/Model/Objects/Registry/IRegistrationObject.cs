// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The RegistrationObject interface.
    /// </summary>
    public interface IRegistrationObject : IMaintainableObject, IConstrainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the data source.
        /// </summary>
        IDataSource DataSource { get; }

        /// <summary>
        ///     Gets the indexAttributes, if true, indicates that the registry must index the range of actual (present) values
        ///     for each attribute of the data set or the presence of the metadata attributes of the metadata set (as indicated in the set's structure definition).
        ///     <p />
        ///     The default value is false.
        /// </summary>
        /// <value> </value>
        TertiaryBool IndexAttribtues { get; }

        /// <summary>
        ///     Gets the indexDataSet, if true, indicates that the registry must index the range of actual (present)
        ///     values for each dimension of the data set or identifier component of the metadata set (as indicated in the set's structure definition).
        ///     <p />
        ///     The index will create a Cube Region Constraint
        ///     <p />
        ///     The default value is false.
        /// </summary>
        /// <value> </value>
        TertiaryBool IndexDataset { get; }

        /// <summary>
        ///     Gets the indexReportingPeriod, if true, indicates that the registry must index the time period ranges for which data is present for the data source.
        ///     <p />
        ///     The default value is false, and the attribute will always be assumed false when the provision agreement references a metadata flow.
        /// </summary>
        /// <value> </value>
        TertiaryBool IndexReportingPeriod { get; }

        /// <summary>
        ///     Gets the indexTimeSeries, if true, indicates that the registry must index all time series for the registered data.
        ///     <p />
        ///     The index will create a Keyset Constraint
        ///     <p />
        ///     The default value is false, and the attribute will always be assumed false when the provision agreement references a metadata flow.
        /// </summary>
        /// <value> </value>
        TertiaryBool IndexTimeseries { get; }

        /// <summary>
        ///     Gets a value indicating whether the one of the index getters is set to true
        /// </summary>
        /// <value> </value>
        bool Indexed { get; }

        /// <summary>
        ///     Gets when the registration was last updated.
        /// </summary>
        ISdmxDate LastUpdated { get; }

        /// <summary>
        ///     Gets a representation of itself in a Object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the RegistrationMutableObject.
        /// </summary>
        /// <value> </value>
        new IRegistrationMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets the provision agreement ref.
        /// </summary>
        ICrossReference ProvisionAgreementRef { get; }

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