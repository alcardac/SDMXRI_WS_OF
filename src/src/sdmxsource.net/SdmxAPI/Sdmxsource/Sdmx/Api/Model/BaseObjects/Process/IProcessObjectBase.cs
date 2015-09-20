// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessObjectBase.cs" company="Eurostat">
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
    ///     The ProcessObjectBase interface.
    /// </summary>
    public interface IProcessObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the process.
        /// </summary>
        IProcessObject Process { get; }

        /// <summary>
        ///     Gets the process steps.
        /// </summary>
        IList<IProcessStepObjectBase> ProcessSteps { get; }

        #endregion
    }
}