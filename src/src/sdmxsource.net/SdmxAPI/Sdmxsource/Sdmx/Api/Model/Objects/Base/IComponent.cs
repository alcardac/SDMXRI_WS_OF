// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponent.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The Component interface.
    /// </summary>
    public interface IComponent : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the concept reference, this method will not return null as concept reference is mandatory
        /// </summary>
        /// <value> </value>
        ICrossReference ConceptRef { get; }

        /// <summary>
        ///     Gets the representation for this component, returns null if there is no representation
        /// </summary>
        /// <value> </value>
        IRepresentation Representation { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the identifier component has coded representation
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasCodedRepresentation();

        #endregion
    }
}