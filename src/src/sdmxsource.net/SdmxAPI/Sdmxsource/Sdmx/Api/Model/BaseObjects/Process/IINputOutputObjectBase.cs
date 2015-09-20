// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IINputOutputObjectBase.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;

    #endregion

    /// <summary>
    ///     The InputOutputObjectBase interface.
    /// </summary>
    public interface IInputOutputObjectBase : IAnnotableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the input output object.
        /// </summary>
        new IInputOutputObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the localID attribute is an optional identification for the input or output within the process.
        /// </summary>
        /// <value> </value>
        string LocalId { get; }

        /// <summary>
        ///     Gets the structure.
        /// </summary>
        IIdentifiableObject Structure { get; }

        #endregion
    }
}