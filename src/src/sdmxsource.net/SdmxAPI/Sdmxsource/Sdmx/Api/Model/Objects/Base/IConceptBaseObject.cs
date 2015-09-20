// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConceptBaseObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    /// <summary>
    ///     To be extended by concept or standalone concept.
    ///     <p />
    ///     Concept base contains the common methods and inheritance tree for both concepts maintained within schcmes (<c>IConceptObject</c>),
    ///     and those maintained outside of schemes (<c>StandAloneConceptObject</c>)
    /// </summary>
    public interface IConceptBaseObject : INameableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the concept is maintained outside of a scheme, if this is true then
        ///     the concept will be a <c>IMaintainableObject</c>
        /// </summary>
        /// <value> </value>
        bool StandAloneConcept { get; }

        #endregion
    }
}