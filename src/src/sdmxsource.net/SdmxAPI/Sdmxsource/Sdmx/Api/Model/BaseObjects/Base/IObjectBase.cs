// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ObjectBase interface.
    /// </summary>
    public interface IObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the object that was used to build this object base.
        /// </summary>
        ISdmxObject BuiltFrom { get; }

        /// <summary>
        /// Gets the composite objects that were used to build this object base.
        /// </summary>
        /// <value>
        /// The composite objects.
        /// </value>
        ISet<IMaintainableObject> CompositeObjects { get; }

        #endregion
    }
}