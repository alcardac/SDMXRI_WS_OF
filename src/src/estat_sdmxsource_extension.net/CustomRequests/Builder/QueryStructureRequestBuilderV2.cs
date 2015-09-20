// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureRequestBuilderV2.cs" company="Eurostat">
//   Date Created : 2013-03-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The query structure request builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Estat.Sri.CustomRequests.Constants;
    using Estat.Sri.CustomRequests.Model;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2;

    /// <summary>
    ///     The query structure request builder v 2.
    /// </summary>
    public class QueryStructureRequestBuilderV2 : IQueryStructureRequestBuilder<XDocument>
    {
        #region Constants

        /// <summary>
        ///     The unknown id.
        /// </summary>
        private const string UnknownId = "UNKNOWN";

        #endregion

        #region Fields

        /// <summary>
        ///     The _header xml builder.
        /// </summary>
        private readonly IBuilder<HeaderType, IHeader> _headerXmlBuilder;

        /// <summary>
        ///     The _header.
        /// </summary>
        private IHeader _header;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestBuilderV2"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        public QueryStructureRequestBuilderV2(IHeader header)
            : this(header, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestBuilderV2"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="headerXmlBuilder">
        /// The header xml builder.
        /// </param>
        public QueryStructureRequestBuilderV2(IHeader header, IBuilder<HeaderType, IHeader> headerXmlBuilder)
        {
            this._header = header;
            this._headerXmlBuilder = headerXmlBuilder ?? new StructureHeaderXmlBuilder();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds a StructureQuery that matches the passed in format
        /// </summary>
        /// <param name="query">
        ///     The list of Queries
        /// </param>
        /// <param name="resolveReferences">
        /// Set to <c>True</c> to resolve references.
        /// </param>
        /// <returns>
        /// The <see cref="QueryStructureRequestType"/>.
        /// </returns>
        public XDocument BuildStructureQuery(IEnumerable<IStructureReference> query, bool resolveReferences)
        {
            var requestType = new QueryStructureRequestType { resolveReferences = resolveReferences };
            foreach (var reference in query)
            {
                BuildStructureQuery(reference, requestType);
            }

            return this.BuildRegistryInterface(requestType);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a reference of type <typeparamref name="T"/> to <paramref name="output"/> from <paramref name="input"/>
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="setAgency">
        /// The method to run to set the agency id.
        /// </param>
        /// <param name="setId">
        /// The method to run to set the id.
        /// </param>
        /// <param name="setVersion">
        /// The method to run to set the version.
        /// </param>
        /// <param name="setUrn">
        /// The method to run to set the URN.
        /// </param>
        /// <typeparam name="T">
        /// The type of reference
        /// </typeparam>
        /// <returns>
        /// The newly created <typeparamref name="T"/> which was added to <paramref name="output"/>.
        /// </returns>
        private static T AddReference<T>(ICollection<T> output, IStructureReference input, Action<T, string> setAgency, Action<T, string> setId, Action<T, string> setVersion, Action<T, Uri> setUrn)
            where T : new()
        {
            var xsdClass = new T();
            var reference = input.MaintainableReference;
            bool identified = false;
            if (reference.HasAgencyId())
            {
                setAgency(xsdClass, reference.AgencyId);
                identified = true;
            }

            if (reference.HasMaintainableId())
            {
                setId(xsdClass, reference.MaintainableId);
                identified = true;
            }

            if (reference.HasVersion())
            {
                setVersion(xsdClass, reference.Version);
                identified = true;
            }

            if (!identified && input.HasMaintainableUrn())
            {
                setUrn(xsdClass, input.MaintainableUrn);
            }

            output.Add(xsdClass);

            return xsdClass;
        }

        /// <summary>
        /// Build constrainable from <paramref name="structureQuery"/> and add it to <paramref name="dataflowRefType"/>
        /// </summary>
        /// <param name="structureQuery">
        /// The structure query.
        /// </param>
        /// <param name="dataflowRefType">
        /// The dataflow ref type.
        /// </param>
        private static void BuildConstrainable(IStructureReference structureQuery, DataflowRefType dataflowRefType)
        {
            var constrainable = structureQuery as IConstrainableStructureReference;
            if (constrainable != null && constrainable.ConstraintObject != null)
            {
                dataflowRefType.Constraint = new ConstraintType { ConstraintID = constrainable.ConstraintObject.Id, ConstraintType1 = "Content" };
                BuildCubeRegion(constrainable.ConstraintObject.IncludedCubeRegion, dataflowRefType);
                BuildCubeRegion(constrainable.ConstraintObject.ExcludedCubeRegion, dataflowRefType);

                if (constrainable.ConstraintObject.ReferencePeriod != null)
                {
                    var referencePeriod = new ReferencePeriodType();
                    if (constrainable.ConstraintObject.ReferencePeriod.StartTime != null && constrainable.ConstraintObject.ReferencePeriod.StartTime.Date != null)
                    {
                        referencePeriod.startTime = constrainable.ConstraintObject.ReferencePeriod.StartTime.Date.Value;
                        dataflowRefType.Constraint.ReferencePeriod = referencePeriod;

                        if (constrainable.ConstraintObject.ReferencePeriod.EndTime != null && constrainable.ConstraintObject.ReferencePeriod.EndTime.Date != null)
                        {
                            referencePeriod.endTime = constrainable.ConstraintObject.ReferencePeriod.EndTime.Date.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build cube region from <paramref name="cubeRegionObject"/> and add it to <paramref name="dataflowRefType"/>
        /// </summary>
        /// <param name="cubeRegionObject">
        /// The cube Region Object.
        /// </param>
        /// <param name="dataflowRefType">
        /// The dataflow ref type.
        /// </param>
        private static void BuildCubeRegion(ICubeRegion cubeRegionObject, DataflowRefType dataflowRefType)
        {
            if (cubeRegionObject != null)
            {
                var cubeRegion = new CubeRegionType { isIncluded = true };
                dataflowRefType.Constraint.CubeRegion.Add(cubeRegion);
                foreach (var keyValuese in cubeRegionObject.KeyValues)
                {
                    BuildMember(keyValuese, cubeRegion);
                }
            }
        }

        /// <summary>
        /// Build member from <paramref name="keyValuese"/> and add it to <paramref name="cubeRegion"/>
        /// </summary>
        /// <param name="keyValuese">
        /// The key values.
        /// </param>
        /// <param name="cubeRegion">
        /// The cube region.
        /// </param>
        private static void BuildMember(IKeyValues keyValuese, CubeRegionType cubeRegion)
        {
            var member = new MemberType { ComponentRef = keyValuese.Id, isIncluded = true };
            cubeRegion.Member.Add(member);
            foreach (var value in keyValuese.Values)
            {
                switch (value)
                {
                    case SpecialValues.DummyMemberValue:
                        break;
                    default:
                        member.MemberValue.Add(new MemberValueType { Value = value });
                        break;
                }
            }
        }

        /// <summary>
        /// The build structure query.
        /// </summary>
        /// <param name="structureQuery">
        /// The structure query.
        /// </param>
        /// <param name="requestType">
        /// The request type.
        /// </param>
        private static void BuildStructureQuery(IStructureReference structureQuery, QueryStructureRequestType requestType)
        {
            switch (structureQuery.MaintainableStructureEnumType.EnumType)
            {
                case SdmxStructureEnumType.CodeList:
                    AddReference(
                        requestType.CodelistRef, structureQuery, (type, s) => type.AgencyID = s, (type, s) => type.CodelistID = s, (type, s) => type.Version = s, (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    AddReference(
                        requestType.HierarchicalCodelistRef, 
                        structureQuery, 
                        (type, s) => type.AgencyID = s, 
                        (type, s) => type.HierarchicalCodelistID = s, 
                        (type, s) => type.Version = s, 
                        (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    AddReference(
                        requestType.CategorySchemeRef, 
                        structureQuery, 
                        (type, s) => type.AgencyID = s, 
                        (type, s) => type.CategorySchemeID = s, 
                        (type, s) => type.Version = s, 
                        (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    AddReference(
                        requestType.ConceptSchemeRef, 
                        structureQuery, 
                        (type, s) => type.AgencyID = s, 
                        (type, s) => type.ConceptSchemeID = s, 
                        (type, s) => type.Version = s, 
                        (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.Dsd:
                    AddReference(
                        requestType.KeyFamilyRef, structureQuery, (type, s) => type.AgencyID = s, (type, s) => type.KeyFamilyID = s, (type, s) => type.Version = s, (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.Dataflow:
                    {
                        var dataflowRefType = AddReference(
                            requestType.DataflowRef, structureQuery, (type, s) => type.AgencyID = s, (type, s) => type.DataflowID = s, (type, s) => type.Version = s, (type, uri) => type.URN = uri);
                        BuildConstrainable(structureQuery, dataflowRefType);
                    }

                    break;
                case SdmxStructureEnumType.Msd:
                    AddReference(
                        requestType.MetadataStructureRef, 
                        structureQuery, 
                        (type, s) => type.AgencyID = s, 
                        (type, s) => type.MetadataStructureID = s, 
                        (type, s) => type.Version = s, 
                        (type, uri) => type.URN = uri);
                    break;

                case SdmxStructureEnumType.MetadataFlow:
                    AddReference(
                        requestType.MetadataflowRef, structureQuery, (type, s) => type.AgencyID = s, (type, s) => type.MetadataflowID = s, (type, s) => type.Version = s, (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.Process:
                    AddReference(requestType.ProcessRef, structureQuery, (type, s) => type.AgencyID = s, (type, s) => type.ProcessID = s, (type, s) => type.Version = s, (type, uri) => type.URN = uri);
                    break;
                case SdmxStructureEnumType.StructureSet:
                    AddReference(
                        requestType.StructureSetRef, structureQuery, (type, s) => type.AgencyID = s, (type, s) => type.StructureSetID = s, (type, s) => type.Version = s, (type, uri) => type.URN = uri);
                    break;
            }
        }

        /// <summary>
        /// The build registry interface.
        /// </summary>
        /// <param name="requestType">
        /// The request type.
        /// </param>
        /// <returns>
        /// The <see cref="XDocument"/>.
        /// </returns>
        private XDocument BuildRegistryInterface(QueryStructureRequestType requestType)
        {
            var registry = new RegistryInterface();

            if (this._header == null)
            {
                this._header = new HeaderImpl(UnknownId, UnknownId);
            }

            registry.Content.Header = this._headerXmlBuilder.Build(this._header);
            
            registry.Content.QueryStructureRequest = requestType;

            return new XDocument(registry.Untyped);
        }

        #endregion
    }
}