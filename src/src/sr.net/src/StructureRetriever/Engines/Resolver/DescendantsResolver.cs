// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescendantsResolver.cs" company="Eurostat">
//   Date Created : 2013-09-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The descendants resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Engines.Resolver
{
    using System;
    using System.Collections.Generic;

    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The descendants resolver.
    /// </summary>
    internal class DescendantsResolver : ResolverBase, IResolver
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DescendantsResolver"/> class.
        /// </summary>
        /// <param name="crossReferenceManager">
        /// The cross reference manager.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="crossReferenceManager"/> is null.
        /// </exception>
        public DescendantsResolver(IAuthCrossReferenceMutableRetrievalManager crossReferenceManager)
            : base(crossReferenceManager)
        {
        }

        #endregion

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
            ResolveDescendants(mutableObjects, returnStub, this.CrossReferenceManager, allowedDataflows);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve children of the specified <paramref name="mutableObjects"/> using the specified parameters
        /// </summary>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <param name="returnStub">
        /// Set to true to return only stubs.
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        /// The cross reference mutable retrieval engine
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        internal static void ResolveDescendants(
            IMutableObjects mutableObjects, 
            bool returnStub, 
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            Func<IMaintainableMutableObject, IList<IMaintainableMutableObject>> reference =
                maintainableMutableObject => GetDescendantsReference(returnStub, crossReferenceMutableRetrievalManager, allowedDataflows, maintainableMutableObject);

            Resolve(mutableObjects, reference);
        }

        #endregion
    }
}