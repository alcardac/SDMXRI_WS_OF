// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAgencyMetadata.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    ///     The AgencyMetadata interface.
    /// </summary>
    public interface IAgencyMetadata
    {
        #region Public Properties

        /// <summary>
        ///     Gets the agency id.
        /// </summary>
        string AgencyId { get; }

        /// <summary>
        ///     Gets the number agency schemes.
        /// </summary>
        int NumberAgencySchemes { get; }

        /// <summary>
        ///     Gets the number attachment constraint.
        /// </summary>
        int NumberAttachmentConstraint { get; }

        /// <summary>
        ///     Gets the number categorisations.
        /// </summary>
        int NumberCategorisations { get; }

        /// <summary>
        ///     Gets the number category schemes.
        /// </summary>
        int NumberCategorySchemes { get; }

        /// <summary>
        ///     Gets the number codelists.
        /// </summary>
        int NumberCodelists { get; }

        /// <summary>
        ///     Gets the number concept schemes.
        /// </summary>
        int NumberConceptSchemes { get; }

        /// <summary>
        ///     Gets the number content constraint Object.
        /// </summary>
        int NumberContentConstraintObject { get; }

        /// <summary>
        ///     Gets the number data consumer schemes.
        /// </summary>
        int NumberDataConsumerSchemes { get; }

        /// <summary>
        ///     Gets the number data provider schemes.
        /// </summary>
        int NumberDataProviderSchemes { get; }

        /// <summary>
        ///     Gets the number data structures.
        /// </summary>
        int NumberDataStructures { get; }

        /// <summary>
        ///     Gets the number dataflows.
        /// </summary>
        int NumberDataflows { get; }

        /// <summary>
        ///     Gets the number hierarchical codelists.
        /// </summary>
        int NumberHierarchicalCodelists { get; }

        /// <summary>
        ///     Gets the number maintainable.
        /// </summary>
        int NumberMaintainables { get; }

        /// <summary>
        ///     Gets the number metadata structure definitions.
        /// </summary>
        int NumberMetadataStructureDefinitions { get; }

        /// <summary>
        ///     Gets the number metadataflows.
        /// </summary>
        int NumberMetadataflows { get; }

        /// <summary>
        ///     Gets the number organisation unit schemes.
        /// </summary>
        int NumberOrganisationUnitSchemes { get; }

        /// <summary>
        ///     Gets the number processes.
        /// </summary>
        int NumberProcesses { get; }

        /// <summary>
        ///     Gets the number provisions.
        /// </summary>
        int NumberProvisions { get; }

        /// <summary>
        ///     Gets the number reporting taxonomies.
        /// </summary>
        int NumberReportingTaxonomies { get; }

        /// <summary>
        ///     Gets the number structure sets.
        /// </summary>
        int NumberStructureSets { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get number of maintainable.
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> .
        /// </returns>
        int GetNumberOfMaintainables(SdmxStructureEnumType structureType);

        #endregion
    }
}