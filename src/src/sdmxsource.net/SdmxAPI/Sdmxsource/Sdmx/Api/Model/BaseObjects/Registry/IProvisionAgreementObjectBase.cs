// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvisionAgreementObjectBase.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The ProvisionAgreementObjectBase interface.
    /// </summary>
    public interface IProvisionAgreementObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        /// <value> the IProvisionAgreementObject that was the constructor of this Super @object. </value>
        new IProvisionAgreementObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the data provider @object that this provision agreement references
        /// </summary>
        /// <value> </value>
        IDataProvider DataProvider { get; }

        /// <summary>
        ///     Gets the dataflow super @object that this provision agreement references
        /// </summary>
        /// <value> </value>
        IDataflowObjectBase DataflowObjectBase { get; }

        #endregion
    }
}