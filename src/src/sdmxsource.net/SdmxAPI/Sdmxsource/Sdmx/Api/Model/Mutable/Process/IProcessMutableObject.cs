// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessMutableObject.cs" company="Eurostat">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;

    #endregion

    /// <summary>
    ///     The ProcessMutableObject interface.
    /// </summary>
    public interface IProcessMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IProcessObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets the process steps.
        /// </summary>
        IList<IProcessStepMutableObject> ProcessSteps { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add process step.
        /// </summary>
        /// <param name="processStep">
        /// The process step.
        /// </param>
        void AddProcessStep(IProcessStepMutableObject processStep);

        #endregion
    }
}