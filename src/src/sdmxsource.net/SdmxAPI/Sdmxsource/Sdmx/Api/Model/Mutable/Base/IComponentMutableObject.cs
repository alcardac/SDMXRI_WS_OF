// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The ComponentMutableObject interface.
    /// </summary>
    public interface IComponentMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the concept ref.
        /// </summary>
        IStructureReference ConceptRef { get; set; }

        /// <summary>
        ///     Gets or sets the representation.
        /// </summary>
        IRepresentationMutableObject Representation { get; set; }

        #endregion
    }
}