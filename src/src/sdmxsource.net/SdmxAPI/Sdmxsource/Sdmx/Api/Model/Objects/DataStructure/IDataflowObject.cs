// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataflowObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Dataflow
    /// </summary>
    public interface IDataflowObject : IStructureUsage, IConstrainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Data Structure that this dataflow is cross referencing, this will not return null.
        /// </summary>
        /// <value> </value>
        ICrossReference DataStructureRef { get; }

        /// <summary>
        ///     Gets a representation of itself in a Object which can be modified, modifications to the mutable Object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IDataflowMutableObject MutableInstance { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub Object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <returns>
        /// The <see cref="IDataflowObject"/> .
        /// </returns>
        /// <param name="actualLocation">
        /// the Uri indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        new IDataflowObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}