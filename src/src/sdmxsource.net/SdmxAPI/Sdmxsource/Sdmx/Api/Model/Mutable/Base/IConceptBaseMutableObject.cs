// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConceptBaseMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    /// <summary>
    ///     The ConceptBaseMutableObject interface.
    /// </summary>
    public interface IConceptBaseMutableObject : INameableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether stand alone concept.
        /// </summary>
        bool StandaloneConcept { get; }

        #endregion
    }
}