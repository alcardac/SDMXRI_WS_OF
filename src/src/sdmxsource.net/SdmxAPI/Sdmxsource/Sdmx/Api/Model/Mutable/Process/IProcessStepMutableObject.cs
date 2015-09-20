// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessStepMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ProcessStepMutableObject interface.
    /// </summary>
    public interface IProcessStepMutableObject : INameableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the computation.
        /// </summary>
        IComputationMutableObject Computation { get; set; }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        IList<IInputOutputMutableObject> Input { get; }

        /// <summary>
        ///     Gets the output.
        /// </summary>
        IList<IInputOutputMutableObject> Output { get; }

        /// <summary>
        ///     Gets the process steps.
        /// </summary>
        IList<IProcessStepMutableObject> ProcessSteps { get; }

        /// <summary>
        ///     Gets the transitions.
        /// </summary>
        IList<ITransitionMutableObject> Transitions { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add process step.
        /// </summary>
        /// <param name="processStep">
        /// The process step.
        /// </param>
        void AddProcessStep(IProcessStepMutableObject processStep);

        /// <summary>
        /// The add transition.
        /// </summary>
        /// <param name="transition">
        /// The transition.
        /// </param>
        void AddTransition(ITransitionMutableObject transition);

        #endregion
    }
}