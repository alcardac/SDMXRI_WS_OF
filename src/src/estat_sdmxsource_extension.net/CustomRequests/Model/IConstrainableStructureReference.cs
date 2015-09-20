// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstrainableStructureReference.cs" company="Eurostat">
//   Date Created : 2013-03-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A structure reference that can include a <see cref="IContentConstraintObject" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///     A structure reference that can include a <see cref="IContentConstraintObject" />
    /// </summary>
    public interface IConstrainableStructureReference : IStructureReference
    {
        #region Public Properties

        /// <summary>
        ///     Gets the content constraint that this structure reference can hold.
        /// </summary>
        IContentConstraintObject ConstraintObject { get; }

        #endregion
    }
}