// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableInfoBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-06
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The table info builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The table info builder.
    /// </summary>
    public class TableInfoBuilder : IBuilder<TableInfo, SdmxStructureEnumType>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="TableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An <see cref="SdmxStructureEnumType"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="TableInfo"/> from the specified <paramref name="buildFrom"/>
        /// </returns>
        public TableInfo Build(SdmxStructureEnumType buildFrom)
        {
            TableInfo tableInfo = null;
            switch (buildFrom)
            {
                case SdmxStructureEnumType.Categorisation:
                    tableInfo = CategorisationConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.CodeList:
                    tableInfo = CodeListConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    tableInfo = CategorySchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    tableInfo = ConceptSchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.Dataflow:
                    tableInfo = DataflowConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.Dsd:
                    tableInfo = DsdConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    tableInfo = HclConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    tableInfo = OrganisationUnitSchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.AgencyScheme:
                    tableInfo = AgencySchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    tableInfo = DataConsumerSchemeConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    tableInfo = DataProviderSchemeConstant.TableInfo;
                    break;

                case SdmxStructureEnumType.StructureSet:
                    tableInfo = StructureSetConstant.TableInfo;
                    break;
                case SdmxStructureEnumType.ContentConstraint:
                    tableInfo = ContentConstraintConstant.TableInfo;
                    break;
            }

            return tableInfo;
        }

        #endregion
    }
}
