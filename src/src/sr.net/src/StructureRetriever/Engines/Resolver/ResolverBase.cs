// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolverBase.cs" company="Eurostat">
//   Date Created : 2013-09-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The resolver base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Engines.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Estat.Sdmxsource.Extension.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The resolver base class.
    /// </summary>
    public abstract class ResolverBase
    {
        #region Static Fields

        /// <summary>
        ///     The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ResolverBase));

        #endregion

        #region Fields

        /// <summary>
        ///     The _cross reference manager
        /// </summary>
        private readonly IAuthCrossReferenceMutableRetrievalManager _crossReferenceManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolverBase"/> class.
        /// </summary>
        /// <param name="crossReferenceManager">
        /// The cross reference manager.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="crossReferenceManager"/> is null.
        /// </exception>
        protected ResolverBase(IAuthCrossReferenceMutableRetrievalManager crossReferenceManager)
        {
            if (crossReferenceManager == null)
            {
                throw new ArgumentNullException("crossReferenceManager");
            }

            this._crossReferenceManager = crossReferenceManager;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the cross reference manager
        /// </summary>
        protected IAuthCrossReferenceMutableRetrievalManager CrossReferenceManager
        {
            get
            {
                return this._crossReferenceManager;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the descendants reference.
        /// </summary>
        /// <param name="returnStub">
        /// if set to <c>true</c> [return stub].
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        /// The cross reference mutable retrieval manager.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <param name="maintainableMutableObject">
        /// The maintainable mutable object.
        /// </param>
        /// <returns>
        /// The descendants in <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        protected internal static IList<IMaintainableMutableObject> GetDescendantsReference(
            bool returnStub, 
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager, 
            IList<IMaintainableRefObject> allowedDataflows, 
            IMaintainableMutableObject maintainableMutableObject)
        {
            var descendants = new List<IMaintainableMutableObject>();
            var stack = new Stack<IMaintainableMutableObject>();
            stack.Push(maintainableMutableObject);
            while (stack.Count > 0)
            {
                var descendant = stack.Pop();
                descendants.Add(descendant);
                var children = crossReferenceMutableRetrievalManager.GetCrossReferencedStructures(descendant, returnStub, allowedDataflows);
                for (int i = 0; i < children.Count; i++)
                {
                    stack.Push(children[i]);
                }
            }

            return descendants;
        }

        /// <summary>
        /// Gets the parents and siblings reference.
        /// </summary>
        /// <param name="returnStub">
        /// if set to <c>true</c> [return stub].
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        /// The cross reference mutable retrieval manager.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed dataflow.
        /// </param>
        /// <param name="maintainableMutableObject">
        /// The maintainable mutable object.
        /// </param>
        /// <returns>
        /// The parents and siblings in a <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        protected internal static IList<IMaintainableMutableObject> GetParentsAndSiblingsReference(
            bool returnStub, 
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager, 
            IList<IMaintainableRefObject> allowedDataflow, 
            IIdentifiableMutableObject maintainableMutableObject)
        {
            // get parents
            var parents = crossReferenceMutableRetrievalManager.GetCrossReferencingStructures(maintainableMutableObject, returnStub, allowedDataflow);
            var parentsAndSiblings = new List<IMaintainableMutableObject>(parents);
            foreach (var parent in parents)
            {
                // get siblinks
                var siblings = crossReferenceMutableRetrievalManager.GetCrossReferencedStructures(parent, returnStub, allowedDataflow);
                parentsAndSiblings.AddRange(siblings);
            }

            return parentsAndSiblings;
        }

        /// <summary>
        /// Resolve references of the specified <paramref name="mutableObjects"/> using the specified
        ///     <paramref name="getReference"/>
        /// </summary>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <param name="getReference">
        /// The method to retrieve cross reference
        /// </param>
        protected internal static void Resolve(IMutableObjects mutableObjects, Func<IMaintainableMutableObject, IList<IMaintainableMutableObject>> getReference)
        {
            var original = new HashSet<IMaintainableMutableObject>(mutableObjects.AllMaintainables);

            int count = 0;
            foreach (IMaintainableMutableObject resolveFor in original)
            {
                IList<IMaintainableMutableObject> referencingStructures = getReference(resolveFor);
                count += referencingStructures.Count;
                mutableObjects.AddIdentifiables(referencingStructures);
            }

            _log.InfoFormat(CultureInfo.InvariantCulture, "Found {0} references.", count);
        }

        #endregion
    }
}