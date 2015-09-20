// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransitionMutableObject.cs" company="Eurostat">
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

    #endregion

    /// <summary>
    ///     The TransitionMutableObject interface.
    /// </summary>
    public interface ITransitionMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the condition.
        /// </summary>
        /// <remarks>No setter in .NET </remarks>
        IList<ITextTypeWrapperMutableObject> Conditions { get; }

        /// <summary>
        ///     Gets or sets the local id.
        /// </summary>
        string LocalId { get; set; }

        /// <summary>
        ///     Gets or sets the target step.
        /// </summary>
        string TargetStep { get; set; }

        #endregion
    }
}