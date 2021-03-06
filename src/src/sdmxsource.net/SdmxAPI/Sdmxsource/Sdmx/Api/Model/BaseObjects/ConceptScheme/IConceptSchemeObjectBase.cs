// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConceptSchemeObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;

    #endregion

    /// <summary>
    ///     A concept scheme contains a list of concepts
    /// </summary>
    public interface IConceptSchemeObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IConceptSchemeObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the concepts.
        /// </summary>
        ISet<IConceptObjectBase> Concepts { get; }

        #endregion
    }
}