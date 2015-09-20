// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorisationMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The CategorisationMutableObject interface.
    /// </summary>
    public interface ICategorisationMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the reference to the category that this giving the semmantic
        /// </summary>
        IStructureReference CategoryReference { get; set; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICategorisationObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets or sets the reference to the structure that this is categorising
        /// </summary>
        IStructureReference StructureReference { get; set; }

        #endregion
    }
}