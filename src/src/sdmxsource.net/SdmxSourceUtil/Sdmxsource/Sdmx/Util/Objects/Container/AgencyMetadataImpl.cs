// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencyMetadataImpl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects.Container
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// The agency metadata impl.
    /// </summary>
    [Serializable]
    public class AgencyMetadataImpl : IAgencyMetadata
    {
        #region Fields

        /// <summary>
        /// The _structure map.
        /// </summary>
        private readonly IDictionary<SdmxStructureEnumType, int> _structureMap;

        /// <summary>
        /// The _agency id.
        /// </summary>
        private string _agencyId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyMetadataImpl"/> class.
        /// </summary>
        public AgencyMetadataImpl()
        {
            this._structureMap = new Dictionary<SdmxStructureEnumType, int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyMetadataImpl"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="objects">
        /// The objects.
        /// </param>
        public AgencyMetadataImpl(string agencyId, ISdmxObjects objects)
        {
            this._structureMap = new Dictionary<SdmxStructureEnumType, int>();
            this._agencyId = agencyId;
            this._structureMap.Add(SdmxStructureEnumType.AgencyScheme, objects.GetAgenciesScheme(agencyId) == null ? 0 : 1);
            this._structureMap.Add(
                SdmxStructureEnumType.AttachmentConstraint, objects.GetAttachmentConstraints(agencyId).Count);
            this._structureMap.Add(
                SdmxStructureEnumType.ContentConstraint, objects.GetContentConstraintObjects(agencyId).Count);
            this._structureMap.Add(
                SdmxStructureEnumType.DataProviderScheme, objects.GetDataProviderScheme(agencyId) == null ? 0 : 1);
            this._structureMap.Add(
                SdmxStructureEnumType.DataConsumerScheme, objects.GetDataConsumerScheme(agencyId) == null ? 0 : 1);
            this._structureMap.Add(
                SdmxStructureEnumType.OrganisationUnitScheme, objects.GetOrganisationUnitSchemes(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.Categorisation, objects.GetCategorisations(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.Dataflow, objects.GetDataflows(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.MetadataFlow, objects.GetMetadataflows(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.CategoryScheme, objects.GetCategorySchemes(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.ConceptScheme, objects.GetConceptSchemes(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.CodeList, objects.GetCodelists(agencyId).Count);
            this._structureMap.Add(
                SdmxStructureEnumType.HierarchicalCodelist, objects.GetHierarchicalCodelists(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.Msd, objects.GetMetadataStructures(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.Dsd, objects.GetDataStructures(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.Process, objects.GetProcesses(agencyId).Count);
            this._structureMap.Add(
                SdmxStructureEnumType.ReportingTaxonomy, objects.GetReportingTaxonomys(agencyId).Count);
            this._structureMap.Add(SdmxStructureEnumType.StructureSet, objects.GetStructureSets(agencyId).Count);
            this._structureMap.Add(
                SdmxStructureEnumType.ProvisionAgreement, objects.GetProvisionAgreements(agencyId).Count);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the agency id.
        /// </summary>
        public string AgencyId
        {
            get
            {
                return this._agencyId;
            }

            set
            {
                this._agencyId = value;
            }
        }

        /// <summary>
        /// Gets or sets the number agency schemes.
        /// </summary>
        public int NumberAgencySchemes
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.AgencyScheme];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.AgencyScheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the number attachment constraint.
        /// </summary>
        public int NumberAttachmentConstraint
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.AttachmentConstraint];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.AttachmentConstraint, value);
            }
        }

        /// <summary>
        /// Gets or sets the number categorisations.
        /// </summary>
        public int NumberCategorisations
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.Categorisation];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.Categorisation, value);
            }
        }

        /// <summary>
        /// Gets or sets the number category schemes.
        /// </summary>
        public int NumberCategorySchemes
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.CategoryScheme];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.CategoryScheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the number codelists.
        /// </summary>
        public int NumberCodelists
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.CodeList];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.CodeList, value);
            }
        }

        /// <summary>
        /// Gets or sets the number concept schemes.
        /// </summary>
        public int NumberConceptSchemes
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.ConceptScheme];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.ConceptScheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the number content constraint bean.
        /// </summary>
        public int NumberContentConstraintObject
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.ContentConstraint];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.ContentConstraint, value);
            }
        }

        /// <summary>
        /// Gets or sets the number data consumer schemes.
        /// </summary>
        public int NumberDataConsumerSchemes
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.DataConsumerScheme];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.DataConsumerScheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the number data provider schemes.
        /// </summary>
        public int NumberDataProviderSchemes
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.DataProviderScheme];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.DataProviderScheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the number data structures.
        /// </summary>
        public int NumberDataStructures
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.Dsd];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.Dsd, value);
            }
        }

        /// <summary>
        /// Gets or sets the number dataflows.
        /// </summary>
        public int NumberDataflows
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.Dataflow];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.Dataflow, value);
            }
        }

        /// <summary>
        /// Gets or sets the number hierarchical codelists.
        /// </summary>
        public int NumberHierarchicalCodelists
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.HierarchicalCodelist];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.HierarchicalCodelist, value);
            }
        }

        /// <summary>
        /// Gets the number maintainables.
        /// </summary>
        public int NumberMaintainables
        {
            get
            {
                int i = 0;
                foreach (SdmxStructureEnumType currentMaint in Enum.GetValues(typeof(SdmxStructureEnumType)))
                {
                    i += this.GetNumberOfMaintainables(currentMaint);
                }

                return i;
            }

            //// TODO in java this doesn't do anything. 
            ////set
            ////{
            ////    // DO NOTHING - this is here for passing to external applications
            ////}
        }

        /// <summary>
        /// Gets or sets the number metadata structure definitions.
        /// </summary>
        public int NumberMetadataStructureDefinitions
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.Msd];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.Msd, value);
            }
        }

        /// <summary>
        /// Gets or sets the number metadataflows.
        /// </summary>
        public int NumberMetadataflows
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.MetadataFlow];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.MetadataFlow, value);
            }
        }

        /// <summary>
        /// Gets or sets the number organisation unit schemes.
        /// </summary>
        public int NumberOrganisationUnitSchemes
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.OrganisationUnitScheme];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.OrganisationUnitScheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the number processes.
        /// </summary>
        public int NumberProcesses
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.Process];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.Process, value);
            }
        }

        /// <summary>
        /// Gets or sets the number provisions.
        /// </summary>
        public int NumberProvisions
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.ProvisionAgreement];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.ProvisionAgreement, value);
            }
        }

        /// <summary>
        /// Gets or sets the number reporting taxonomies.
        /// </summary>
        public int NumberReportingTaxonomies
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.ReportingTaxonomy];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.ReportingTaxonomy, value);
            }
        }

        /// <summary>
        /// Gets or sets the number structure sets.
        /// </summary>
        public int NumberStructureSets
        {
            get
            {
                return this._structureMap[SdmxStructureEnumType.StructureSet];
            }

            set
            {
                this._structureMap.Add(SdmxStructureEnumType.StructureSet, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get number of maintainables.
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetNumberOfMaintainables(SdmxStructureEnumType structureType)
        {
            if (this._structureMap.ContainsKey(structureType))
            {
                return this._structureMap[structureType];
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structureType"></param>
        /// <param name="num"></param>
        public void SetStructure(SdmxStructureEnumType structureType, int num)
        {
            this._structureMap.Add(structureType, num);
        }

        #endregion
    }
}