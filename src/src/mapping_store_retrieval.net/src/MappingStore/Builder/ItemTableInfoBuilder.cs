// -----------------------------------------------------------------------
// <copyright file="ItemTableInfoBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-08
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Builds an <see cref="ItemTableInfo"/> 
    /// </summary>
    public class ItemTableInfoBuilder : IBuilder<ItemTableInfo, SdmxStructureEnumType>
    {
        /// <summary>
        /// Builds an <see cref="ItemTableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An <see cref="SdmxStructureEnumType"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="ItemTableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </returns>
        public ItemTableInfo Build(SdmxStructureEnumType buildFrom)
        {
            ItemTableInfo tableInfo = null;
            switch (buildFrom)
            {
                case SdmxStructureEnumType.Categorisation:
                    break;
                case SdmxStructureEnumType.CodeList:
                case SdmxStructureEnumType.Code:
                    tableInfo = CodeListConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                case SdmxStructureEnumType.Category:
                    tableInfo = CategorySchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                case SdmxStructureEnumType.Concept:
                    tableInfo = ConceptSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.DataProvider:
                case SdmxStructureEnumType.DataProviderScheme:
                    tableInfo = DataProviderSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.DataConsumer:
                case SdmxStructureEnumType.DataConsumerScheme:
                    tableInfo = DataConsumerSchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.Agency:
                case SdmxStructureEnumType.AgencyScheme:
                    tableInfo = AgencySchemeConstant.ItemTableInfo;
                    break;
                case SdmxStructureEnumType.OrganisationUnit:
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    tableInfo = OrganisationUnitSchemeConstant.ItemTableInfo;
                    break;
            }

            return tableInfo;
        }
    }
}