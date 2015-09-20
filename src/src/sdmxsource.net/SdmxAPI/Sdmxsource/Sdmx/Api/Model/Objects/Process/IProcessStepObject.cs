// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessStepObject.cs" company="Eurostat">
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
    ///     Represents an SDMX Process Step
    /// </summary>
    public interface IProcessStepObject : INameableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the computation.
        /// </summary>
        IComputationObject Computation { get; }

        /// <summary>
        ///     Gets the inputs of this Process Step as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if no inputs exist
        /// </summary>
        /// <value> </value>
        IList<IInputOutputObject> Input { get; }

        /// <summary>
        ///     Gets the outputs of this process step as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if no outputs exist
        /// </summary>
        /// <value> </value>
        IList<IInputOutputObject> Output { get; }

        /// <summary>
        ///     Gets the child Process Steps as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if no child process steps exist
        /// </summary>
        /// <value> </value>
        IList<IProcessStepObject> ProcessSteps { get; }

        /// <summary>
        ///     Gets TransitionObjects as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if no ITransition exist
        /// </summary>
        /// <value> </value>
        IList<ITransition> Transitions { get; }

        #endregion
    }
}