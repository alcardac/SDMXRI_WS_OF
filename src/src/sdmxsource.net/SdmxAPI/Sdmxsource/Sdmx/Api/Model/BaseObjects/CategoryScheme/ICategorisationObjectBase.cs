// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorisationObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    #endregion

    /// <summary>
    ///     The CategorisationObjectBase interface.
    /// </summary>
    public interface ICategorisationObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new ICategorisationObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the category that is categorising the structure - this can not be null
        /// </summary>
        /// <value> </value>
        ICategoryObject Category { get; }

        /// <summary>
        ///     Gets a the structure that this is categorising - this can not be null
        /// </summary>
        /// <value> </value>
        IIdentifiableObject Structure { get; }

        #endregion
    }
}