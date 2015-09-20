// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransition.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Process
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represented a transition between process steps
    /// </summary>
    public interface ITransition : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the condition.
        /// </summary>
        IList<ITextTypeWrapper> Condition { get; }

        /// <summary>
        ///     Gets the local id.
        /// </summary>
        string LocalId { get; }

        /// <summary>
        ///     Gets the target step.
        /// </summary>
        IProcessStepObject TargetStep { get; }

        #endregion
    }
}