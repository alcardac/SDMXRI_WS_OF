// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureMapMutableObject.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;

    #endregion

    /// <summary>
    ///     The StructureMapMutableObject interface.
    /// </summary>
    public interface IStructureMapMutableObject : ISchemeMapMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the components.
        /// </summary>
        IList<IComponentMapMutableObject> Components { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether extension.
        /// </summary>
        bool Extension { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add component.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        void AddComponent(IComponentMapMutableObject component);

        #endregion
    }
}