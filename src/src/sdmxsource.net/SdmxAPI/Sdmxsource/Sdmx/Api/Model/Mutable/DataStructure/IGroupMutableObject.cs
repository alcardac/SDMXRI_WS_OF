// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGroupMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The GroupMutableObject interface.
    /// </summary>
    public interface IGroupMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the attachment constraint ref.
        /// </summary>
        IStructureReference AttachmentConstraintRef { get; set; }

        /// <summary>
        ///     Gets the dimension ref.
        /// </summary>
        IList<string> DimensionRef { get; }

        #endregion
    }
}