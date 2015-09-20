// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputOutputObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     An InputOutputObject defines an input OR an output to a ProcessStep.
    ///     <p />
    ///     The InputOutputObject has a local id, and references any SDMX Identifiable Structure
    /// </summary>
    /// <seealso cref="T:Org.Sdmxsource.Sdmx.Api.Model.Objects.Process.IProcessStepObject" />
    public interface IInputOutputObject : IAnnotableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the localID attribute is an optional identification for the input or output within the process.
        /// </summary>
        /// <value> </value>
        string LocalId { get; }

        /// <summary>
        ///     Gets the reference to the input / output structure.  This can refernece any identifiable structure and will not be null.
        /// </summary>
        /// <value> </value>
        ICrossReference StructureReference { get; }

        #endregion
    }
}