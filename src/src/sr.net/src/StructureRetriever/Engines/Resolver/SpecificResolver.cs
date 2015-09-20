// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecificResolver.cs" company="Eurostat">
//   Date Created : 2013-09-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The specific resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Engines.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The specific resolver.
    /// </summary>
    internal class SpecificResolver : ResolverBase, IResolver
    {
        #region Fields

        /// <summary>
        ///     The _structure types
        /// </summary>
        private readonly SdmxStructureType[] _structureTypes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificResolver"/> class.
        /// </summary>
        /// <param name="crossReferenceManager">
        /// The cross reference manager.
        /// </param>
        /// <param name="structureTypes">
        /// The specific object structure Types.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="crossReferenceManager"/> is null.
        /// </exception>
        public SpecificResolver(IAuthCrossReferenceMutableRetrievalManager crossReferenceManager, params SdmxStructureType[] structureTypes)
            : base(crossReferenceManager)
        {
            this._structureTypes = structureTypes;
        }

        #endregion

        #region Enums

        /// <summary>
        ///     The relation type
        /// </summary>
        private enum RelationType
        {
            /// <summary>
            ///     The parent
            /// </summary>
            Parent, 

            /// <summary>
            ///     The descendant
            /// </summary>
            Descendant
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
            ResolveSpecific(mutableObjects, this._structureTypes, returnStub, this.CrossReferenceManager, allowedDataflows);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve the specified <paramref name="specificStructureReferences"/> only reference from
        ///     <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <param name="specificStructureReferences">
        /// The specific structure reference.
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        /// The cross Reference Mutable Retrieval Manager.
        /// </param>
        /// <param name="allowedDataflow">
        /// The allowed Dataflow.
        /// </param>
        internal static void ResolveSpecific(
            IMutableObjects mutableObjects, 
            IList<SdmxStructureType> specificStructureReferences, 
            bool returnStub, 
            IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager, 
            IList<IMaintainableRefObject> allowedDataflow)
        {
            Func<IMaintainableMutableObject, IList<IMaintainableMutableObject>> reference = maintainableMutableObject =>
                {
                    var referencedStructures = new List<IMaintainableMutableObject>();
                    foreach (var specificStructureType in specificStructureReferences)
                    {
                        referencedStructures.AddRange(GetSpecificObjects(crossReferenceMutableRetrievalManager, maintainableMutableObject, specificStructureType, returnStub, allowedDataflow));
                    }

                    return referencedStructures;
                };

            Resolve(mutableObjects, reference);
        }

        /// <summary>
        /// Returns the method that retrieves the 
        /// </summary>
        /// <param name="crossReferenceManager">
        /// The cross reference manager.
        /// </param>
        /// <param name="returnStub">
        /// The return stub.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <param name="relationNode">
        /// The relation node.
        /// </param>
        /// <returns>
        /// The function that returns 
        /// </returns>
        private static Func<IMaintainableMutableObject, IList<IMaintainableMutableObject>> GetFunc(
            IAuthCrossReferenceMutableRetrievalManager crossReferenceManager, 
            bool returnStub, 
            IList<IMaintainableRefObject> allowedDataflows, 
            RelationNode relationNode)
        {
            Func<IMaintainableMutableObject, IList<IMaintainableMutableObject>> func = x => new IMaintainableMutableObject[0];
            var destinationType = SdmxStructureType.GetFromEnum(relationNode.DestType);
            switch (relationNode.RelationType)
            {
                case RelationType.Parent:
                    func = x => crossReferenceManager.GetCrossReferencingStructures(x, returnStub, allowedDataflows, destinationType);
                    break;
                case RelationType.Descendant:
                    func = x => crossReferenceManager.GetCrossReferencedStructures(x, returnStub, allowedDataflows, destinationType);
                    break;
            }

            return func;
        }

        /// <summary>
        /// Returns specific structures of the specified type <paramref name="specificStructureType"/> related to the <paramref name="maintainable"/>
        /// </summary>
        /// <param name="crossReferenceManager">
        /// The cross reference manager.
        /// </param>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <param name="specificStructureType">
        /// The specific structure type.
        /// </param>
        /// <param name="returnStub">
        /// The return stub.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IMaintainableMutableObject}"/>.
        /// </returns>
        private static IEnumerable<IMaintainableMutableObject> GetSpecificObjects(
            IAuthCrossReferenceMutableRetrievalManager crossReferenceManager, 
            IMaintainableMutableObject maintainable, 
            BaseConstantType<SdmxStructureEnumType> specificStructureType, 
            bool returnStub, 
            IList<IMaintainableRefObject> allowedDataflows)
        {
            var specificObjects = new List<IMaintainableMutableObject>();
            var relationNodes = RelationBuilder.Build(maintainable.StructureType.EnumType, specificStructureType.EnumType);
            var stack = new Stack<IMaintainableMutableObject>();
            foreach (var rootNode in relationNodes)
            {
                ProcessNode(crossReferenceManager, maintainable, specificStructureType, returnStub, allowedDataflows, rootNode, specificObjects, stack);

                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    var currentNode = RelationBuilder.Build(current.StructureType.EnumType, specificStructureType.EnumType).FirstOrDefault();
                    if (currentNode != null)
                    {
                        ProcessNode(crossReferenceManager, current, specificStructureType, returnStub, allowedDataflows, currentNode, specificObjects, stack);
                    }
                }
            }

            return specificObjects;
        }

        /// <summary>
        /// Processes the node.
        /// </summary>
        /// <param name="crossReferenceManager">The cross reference manager.</param>
        /// <param name="maintainable">The maintainable.</param>
        /// <param name="specificStructureType">Type of the specific structure.</param>
        /// <param name="returnStub">if set to <c>true</c> [return stub].</param>
        /// <param name="allowedDataflows">The allowed dataflows.</param>
        /// <param name="rootNode">The root node.</param>
        /// <param name="specificObjects">The specific objects.</param>
        /// <param name="stack">The stack.</param>
        private static void ProcessNode(
            IAuthCrossReferenceMutableRetrievalManager crossReferenceManager, 
            IMaintainableMutableObject maintainable, 
            BaseConstantType<SdmxStructureEnumType> specificStructureType, 
            bool returnStub, 
            IList<IMaintainableRefObject> allowedDataflows, 
            RelationNode rootNode, 
            ICollection<IMaintainableMutableObject> specificObjects, 
            Stack<IMaintainableMutableObject> stack)
        {
            var isTarget = rootNode.DestType == specificStructureType.EnumType;
            bool getStub;
            Action<IMaintainableMutableObject> action;
            if (isTarget)
            {
                getStub = returnStub;
                action = specificObjects.Add;
            }
            else
            {
                getStub = rootNode.DestType != SdmxStructureEnumType.Categorisation;
                action = stack.Push;
            }

            var rootFunc = GetFunc(crossReferenceManager, getStub, allowedDataflows, rootNode);
            
            foreach (var maintainableMutableObject in rootFunc(maintainable))
            {
                action(maintainableMutableObject);
            }
        }

        #endregion

        /// <summary>
        /// The relation builder.
        /// </summary>
        private static class RelationBuilder
        {
            #region Static Fields

            /// <summary>
            /// The _categorisation to category scheme.
            /// </summary>
            private static readonly RelationNode _categorySchemeToCategorisation;

            /// <summary>
            /// The _categorisation to dataflow.
            /// </summary>
            private static readonly RelationNode _categorisationToDataflow;

            /// <summary>
            /// The _code list to DSD.
            /// </summary>
            private static readonly RelationNode _codeListToDsd;

            /// <summary>
            /// The _concept scheme to DSD.
            /// </summary>
            private static readonly RelationNode _conceptSchemeToDsd;

            /// <summary>
            /// The _dataflow to categorisation.
            /// </summary>
            private static readonly RelationNode _dataflowToCategorisation;

            /// <summary>
            /// The _dataflow to DSD.
            /// </summary>
            private static readonly RelationNode _dataflowToDsd;

            /// <summary>
            /// The DSD to dataflow.
            /// </summary>
            private static readonly RelationNode _dsdToDataflow;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes static members of the <see cref="RelationBuilder"/> class.
            /// </summary>
            static RelationBuilder()
            {
                _dataflowToDsd = new RelationNode(RelationType.Descendant, SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.Dsd);
                _categorisationToDataflow = new RelationNode(RelationType.Descendant, SdmxStructureEnumType.Categorisation, SdmxStructureEnumType.Dataflow);
                _categorySchemeToCategorisation = new RelationNode(RelationType.Parent, SdmxStructureEnumType.CategoryScheme, SdmxStructureEnumType.Categorisation);
                _dataflowToCategorisation = new RelationNode(RelationType.Parent, SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.Categorisation);
                _dsdToDataflow = new RelationNode(RelationType.Parent, SdmxStructureEnumType.Dsd, SdmxStructureEnumType.Dataflow);
                _codeListToDsd = new RelationNode(RelationType.Parent, SdmxStructureEnumType.CodeList, SdmxStructureEnumType.Dsd);
                _conceptSchemeToDsd = new RelationNode(RelationType.Parent, SdmxStructureEnumType.ConceptScheme, SdmxStructureEnumType.Dsd);
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// Returns the list of next hope's for getting from <paramref name="source"/> to <paramref name="destination"/>
            /// </summary>
            /// <param name="source">
            /// The source.
            /// </param>
            /// <param name="destination">
            /// The destination.
            /// </param>
            /// <returns>
            /// The <see cref="IEnumerable{RelationNode}"/>.
            /// </returns>
            public static IEnumerable<RelationNode> Build(SdmxStructureEnumType source, SdmxStructureEnumType destination)
            {
                switch (source)
                {
                    case SdmxStructureEnumType.Categorisation:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.CategoryScheme:
                            case SdmxStructureEnumType.Dataflow:
                                return new[] { new RelationNode(RelationType.Descendant, SdmxStructureEnumType.Categorisation, destination) };
                            case SdmxStructureEnumType.Dsd:
                            case SdmxStructureEnumType.CodeList:
                            case SdmxStructureEnumType.ConceptScheme:
                                return new[] { _categorisationToDataflow };
                        }

                        break;
                    case SdmxStructureEnumType.CategoryScheme:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.Categorisation:
                            case SdmxStructureEnumType.CategoryScheme:
                            case SdmxStructureEnumType.Dataflow:
                            case SdmxStructureEnumType.Dsd:
                            case SdmxStructureEnumType.CodeList:
                            case SdmxStructureEnumType.ConceptScheme:
                                return new[] { _categorySchemeToCategorisation };
                        }

                        break;

                    case SdmxStructureEnumType.Dataflow:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.Categorisation:
                            case SdmxStructureEnumType.CategoryScheme:
                            case SdmxStructureEnumType.Dataflow:
                                return new[] { _dataflowToCategorisation };
                            case SdmxStructureEnumType.Dsd:
                            case SdmxStructureEnumType.CodeList:
                            case SdmxStructureEnumType.ConceptScheme:
                                return new[] { _dataflowToDsd };
                        }

                        break;

                    case SdmxStructureEnumType.Dsd:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.Categorisation:
                            case SdmxStructureEnumType.CategoryScheme:
                            case SdmxStructureEnumType.Dataflow:
                            case SdmxStructureEnumType.Dsd:
                                return new[] { _dsdToDataflow };
                            case SdmxStructureEnumType.CodeList:
                            case SdmxStructureEnumType.ConceptScheme:
                                return new[] { new RelationNode(RelationType.Descendant, SdmxStructureEnumType.Dsd, destination) };
                        }

                        break;

                    case SdmxStructureEnumType.CodeList:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.Categorisation:
                            case SdmxStructureEnumType.CategoryScheme:
                            case SdmxStructureEnumType.Dataflow:
                            case SdmxStructureEnumType.Dsd:
                            case SdmxStructureEnumType.ConceptScheme:
                                return new[] { _codeListToDsd };
                            case SdmxStructureEnumType.CodeList:
                                return new[]
                                           {
                                               _codeListToDsd, new RelationNode(RelationType.Parent, source, SdmxStructureEnumType.HierarchicalCodelist)
                                           };
                            case SdmxStructureEnumType.HierarchicalCodelist:
                                return new[] { new RelationNode(RelationType.Parent, source, destination) };
                        }

                        break;
                    case SdmxStructureEnumType.ConceptScheme:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.Categorisation:
                            case SdmxStructureEnumType.CategoryScheme:
                            case SdmxStructureEnumType.Dataflow:
                            case SdmxStructureEnumType.Dsd:
                            case SdmxStructureEnumType.CodeList:
                            case SdmxStructureEnumType.ConceptScheme:
                                return new[] { _conceptSchemeToDsd };
                        }

                        break;
                    case SdmxStructureEnumType.HierarchicalCodelist:
                        switch (destination)
                        {
                            case SdmxStructureEnumType.CodeList:
                                return new[] { new RelationNode(RelationType.Descendant, source, destination) };
                        }

                        break;
                }

                return new RelationNode[0];
            }

            #endregion
        }

        /// <summary>
        /// The relation node.
        /// </summary>
        private class RelationNode
        {
            #region Fields

            /// <summary>
            ///     The _destination type
            /// </summary>
            private readonly SdmxStructureEnumType _destType;

            /// <summary>
            ///     The _relation type
            /// </summary>
            private readonly RelationType _relationType;

            /// <summary>
            ///     The _source type
            /// </summary>
            private readonly SdmxStructureEnumType _sourceType;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="RelationNode"/> class.
            /// </summary>
            /// <param name="relationType">
            /// Type of the relation.
            /// </param>
            /// <param name="sourceType">
            /// Type of the source.
            /// </param>
            /// <param name="destType">
            /// Type of the destination.
            /// </param>
            public RelationNode(RelationType relationType, SdmxStructureEnumType sourceType, SdmxStructureEnumType destType)
            {
                this._destType = destType;
                this._relationType = relationType;
                this._sourceType = sourceType;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the destination type
            /// </summary>
            public SdmxStructureEnumType DestType
            {
                get
                {
                    return this._destType;
                }
            }

            /// <summary>
            ///     Gets the relation type
            /// </summary>
            public RelationType RelationType
            {
                get
                {
                    return this._relationType;
                }
            }

            /// <summary>
            ///     Gets the source type
            /// </summary>
            public SdmxStructureEnumType SourceType
            {
                get
                {
                    return this._sourceType;
                }
            }

            #endregion
        }
    }
}