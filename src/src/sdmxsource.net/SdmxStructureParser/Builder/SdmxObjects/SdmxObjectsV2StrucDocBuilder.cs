// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsV2StrucDocBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx beans v 2 structure builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     The sdmx beans v 2 structure builder.
    /// </summary>
    public class SdmxObjectsV2StrucDocBuilder : AbstractSdmxObjectsV2Builder, IBuilder<ISdmxObjects, Structure>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="ISdmxObjects"/> object from the specified <paramref name="structuresDoc"/>
        /// </summary>
        /// <param name="structuresDoc">
        /// An <see cref="Structure"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="ISdmxObjects"/> object from the specified <paramref name="structuresDoc"/>
        /// </returns>
        /// <exception cref="BuilderException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public virtual ISdmxObjects Build(Structure structuresDoc)
        {
            var urns = new HashSet<Uri>();
            StructureType structures = structuresDoc.Content;

            var beans = new SdmxObjectsImpl(new HeaderImpl(structures.Header));

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