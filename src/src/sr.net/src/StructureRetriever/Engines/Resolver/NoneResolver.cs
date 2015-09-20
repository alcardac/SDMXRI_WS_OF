// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoneResolver.cs" company="Eurostat">
//   Date Created : 2013-09-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The none reference resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Engines.Resolver
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The none reference resolver.
    /// </summary>
    public class NoneResolver : IResolver
    {
        #region Public Methods and Operators

        /// <summary>
        /// Resolves the references of the specified mutable objects.
        /// </summary>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <param name="returnStub">
        /// if set to <c>true</c> return stub references.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        public void ResolveReferences(IMutableObjects mutableObjects, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            //// do nothing
        }

        #endregion
    }
}