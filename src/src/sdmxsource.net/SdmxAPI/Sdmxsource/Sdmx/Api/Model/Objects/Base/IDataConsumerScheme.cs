// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataConsumerScheme.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Data Consumer Scheme
    /// </summary>
    public interface IDataConsumerScheme : IOrganisationScheme<IDataConsumer>
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IDataConsumerSchemeMutableObject MutableInstance { get; }

        #endregion
    }
}