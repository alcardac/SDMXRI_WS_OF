// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataflowObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    #endregion

    /// <summary>
    ///     A dataflow is a component that represents the collection of reporting data over time.
    /// </summary>
    public interface IDataflowObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IDataflowObject BuiltFrom { get; }

        /// <summary>
        ///     Gets a Data Structure for this dataflow.
        /// </summary>
        /// <value> </value>
        IDataStructureObjectBase DataStructure { get; }

        #endregion
    }
}