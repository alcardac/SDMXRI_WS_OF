// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGroupObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     A group is a subset of the dimensionality of the full key.  It is purely to attach attributes.
    /// </summary>
    public interface IGroupObjectBase : IIdentifiableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the attachment constraint ref.
        /// </summary>
        ICrossReference AttachmentConstraintRef { get; }

        /// <summary>
        ///     Gets the dimension.
        /// </summary>
        IList<IDimensionObjectBase> Dimensions { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get dimension by id.
        /// </summary>
        /// <param name="conceptId">
        /// The concept id.
        /// </param>
        /// <returns>
        /// The <see cref="IDimensionObjectBase"/> .
        /// </returns>
        IDimensionObjectBase GetDimensionById(string conceptId);

        #endregion
    }
}