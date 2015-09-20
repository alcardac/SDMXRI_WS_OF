// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputOutputMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The InputOutputMutableObject interface.
    /// </summary>
    public interface IInputOutputMutableObject : IAnnotableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the local id.
        /// </summary>
        string LocalId { get; set; }

        /// <summary>
        ///     Gets or sets the structure reference.
        /// </summary>
        IStructureReference StructureReference { get; set; }

        #endregion
    }
}