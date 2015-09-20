// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeRefMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The CodeRefMutableObject interface.
    /// </summary>
    public interface ICodeRefMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the code id.
        /// </summary>
        string CodeId { get; set; }

        /// <summary>
        ///     Gets or sets the code reference.
        /// </summary>
        IStructureReference CodeReference { get; set; }

        /// <summary>
        ///     Gets the code refs.
        /// </summary>
        IList<ICodeRefMutableObject> CodeRefs { get; }

        /// <summary>
        ///     Gets or sets the codelist alias ref.
        /// </summary>
        string CodelistAliasRef { get; set; }

        /// <summary>
        ///     Gets or sets the level reference.
        /// </summary>
        string LevelReference { get; set; }

        /// <summary>
        ///     Gets or sets the valid from.
        /// </summary>
        DateTime? ValidFrom { get; set; }

        /// <summary>
        ///     Gets or sets the valid to.
        /// </summary>
        DateTime? ValidTo { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add code ref.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref.
        /// </param>
        void AddCodeRef(ICodeRefMutableObject codeRef);

        #endregion
    }
}