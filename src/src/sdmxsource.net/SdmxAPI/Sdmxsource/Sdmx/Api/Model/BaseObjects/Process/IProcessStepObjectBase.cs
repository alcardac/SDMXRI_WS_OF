// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessStepObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Process
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;

    #endregion

    /// <summary>
    ///     The ProcessStepObjectBase interface.
    /// </summary>
    public interface IProcessStepObjectBase : INameableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the computation.
        /// </summary>
        IComputationObject Computation { get; }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        IList<IInputOutputObjectBase> Input { get; }

        /// <summary>
        ///     Gets the output.
        /// </summary>
        IList<IInputOutputObjectBase> Output { get; }

        /// <summary>
        ///     Gets the process step.
        /// </summary>
        new IProcessStepObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the process steps.
        /// </summary>
        IList<IProcessStepObjectBase> ProcessSteps { get; }

        /// <summary>
        ///     Gets the transitions.
        /// </summary>
        IList<ITransition> Transitions { get; }

        #endregion
    }
}