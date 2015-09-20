// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsV2RegDocBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx beans v 2 registry interface doc builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    using StructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.StructureType;

    /// <summary>
    ///     The sdmx beans v 2 registry interface doc builder.
    /// </summary>
    public class SdmxObjectsV2RegDocBuilder : AbstractSdmxObjectsV2Builder, IBuilder<ISdmxObjects, RegistryInterface>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="ISdmxObjects"/> object from the specified <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// An <see cref="RegistryInterface"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="ISdmxObjects"/> object from the specified <paramref name="registryInterface"/>
        /// </returns>
        /// <exception cref="BuilderException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public virtual ISdmxObjects Build(RegistryInterface registryInterface)
        {
            RegistryInterfaceType rit = registryInterface.Content;
            ISdmxObjects beans = new SdmxObjectsImpl(new HeaderImpl(rit.Header));
            if (rit.SubmitStructureRequest != null)
            {
                if (rit.SubmitStructureRequest.Structure != null)
                {
                    return this.Build(rit.SubmitStructureRequest.Structure, beans);
                }
            }

            if (rit.QueryStructureResponse != null)
            {
                return this.Build(rit.QueryStructureResponse, beans);
            }

            return beans;
        }

        /// <summary>
        /// Builds an <see cref="ISdmxObjects"/> object from the specified <paramref name="structures"/>
        /// </summary>
        /// <param name="structures">
        /// An <see cref="StructureType"/> to build the output object from
        /// </param>
        /// <param name="beans">
        /// The SDMX objects 
        /// </param>
        /// <returns>
        /// an <see cref="ISdmxObjects"/> object from the specified <paramref name="structures"/>
        /// </returns>
        /// <exception cref="BuilderException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public ISdmxObjects Build(StructureType structures, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();

            // CATEGORY SCHEMES
            this.ProcessCategorySchemes(structures.CategorySchemes, beans);

            // CODELISTS
            this.ProcessCodelists(structures.CodeLists, beans, urns);

            // CONCEPT SCHEMES
            this.ProcessConceptSchemes(structures.Concepts, beans, urns);

            // DATAFLOWS
            this.ProcessDataflows(structures.Dataflows, beans);

            // HIERARCHICAL CODELISTS
            this.ProcessHierarchicalCodelists(structures.HierarchicalCodelists, beans, urns);

            // KEY FAMILIES
            this.ProcessKeyFamilies(structures.KeyFamilies, beans, urns);

            // METADATA FLOWS
            this.ProcessMetadataFlows(structures.Metadataflows, beans);

            // METADATASTRUCTURE DEFINITIONS
            ProcessMetadataStructureDefinitions(structures.MetadataStructureDefinitions, beans, urns);

            // ORGANISATION SCHEMES
            this.ProcessOrganisationSchemes(structures.OrganisationSchemes, beans, urns);

            this.ProcessProcesses(structures.Processes, beans, urns);

            this.ProcessReportingTaxonomies(structures.ReportingTaxonomies, beans, urns);

            this.ProcessStructureSets(structures.StructureSets, beans, urns);

            return beans;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds an <see cref="ISdmxObjects"/> object from the specified <paramref name="structures"/>
        /// </summary>
        /// <param name="structures">
        /// An <see cref="StructureType"/> to build the output object from
        /// </param>
        /// <param name="beans">
        /// The SDMX objects 
        /// </param>
        /// <returns>
        /// an <see cref="ISdmxObjects"/> object from the specified <paramref name="structures"/>
        /// </returns>
        /// <exception cref="BuilderException">
        /// - If anything goes wrong during the build process
        /// </exception>
        private ISdmxObjects Build(QueryStructureResponseType structures, ISdmxObjects beans)
        {
            var urns = new HashSet<Uri>();

            // CATEGORY SCHEMES
            this.ProcessCategorySchemes(structures.CategorySchemes, beans);

            // CODELISTS
            this.ProcessCodelists(structures.CodeLists, beans, urns);

            // CONCEPT SCHEMES
            this.ProcessConceptSchemes(structures.Concepts, beans, urns);

            // DATAFLOWS
            this.ProcessDataflows(structures.Dataflows, beans);

            // HIERARCHICAL CODELISTS
            this.ProcessHierarchicalCodelists(structures.HierarchicalCodelists, beans, urns);

            // KEY FAMILIES
            this.ProcessKeyFamilies(structures.KeyFamilies, beans, urns);

            // METADATA FLOWS
            this.ProcessMetadataFlows(structures.Metadataflows, beans);

            // METADATASTRUCTURE DEFINITIONS
            ProcessMetadataStructureDefinitions(structures.MetadataStructureDefinitions, beans, urns);

            // ORGANISATION SCHEMES
            this.ProcessOrganisationSchemes(structures.OrganisationSchemes, beans, urns);

            this.ProcessProcesses(structures.Processes, beans, urns);

            this.ProcessReportingTaxonomies(structures.ReportingTaxonomies, beans, urns);

            this.ProcessStructureSets(structures.StructureSets, beans, urns);

            return beans;
        }

        #endregion
    }
}