// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingDataQueryParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The re using data query parsing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.DataQuery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The re using data query parsing manager.
    /// </summary>
    public static class ReUsingDataQueryParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            // 1. initialize the ISdmxObjectRetrievalManager we will use for retrieving Dataflow and DSD. 
            // Depending on the implementation, they could be retrieved from the registry or mapping store.
            // but in this example we used a simple implementation which retrieves them from files.
            IStructureParsingManager parsingManager = new StructureParsingManager();
            ISdmxObjects objects = new SdmxObjectsImpl();
            using (IReadableDataLocation location = new FileReadableDataLocation("ESTAT+STS+2.0.xml"))
            {
                objects.Merge(parsingManager.ParseStructures(location).GetStructureObjects(false));
            }

            using (IReadableDataLocation location = new FileReadableDataLocation("ESTAT+SSTSCONS_PROD_M+2.0.xml"))
            {
                objects.Merge(parsingManager.ParseStructures(location).GetStructureObjects(false));
            }

            ISdmxObjectRetrievalManager retrievalManager = new InMemoryRetrievalManager(objects);

            // 2. initialize the IDataQueryParseManager implementation. 
            IDataQueryParseManager parseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwo);

            // 3. Create a IReadableDataLocation. Since we work with files we use the FileReadableDataLocation implementation.
            IList<IDataQuery> buildDataQuery;
            using (IReadableDataLocation readable = new FileReadableDataLocation("query.xml"))
            {
                // 4. we call BuildDataQuery to process the query.xml and get a list of IDataQuery
                buildDataQuery = parseManager.BuildDataQuery(readable, retrievalManager);
            }

            // below we print to console the contents of each IDataQuery
            foreach (var dataQuery in buildDataQuery)
            {
                Console.WriteLine("Dataflow: {0}", dataQuery.Dataflow.Id);
                Console.WriteLine("DSD: {0}", dataQuery.DataStructure.Id);
                Console.WriteLine("Maximum number of observations (DefaultLimit): {0}", dataQuery.FirstNObservations);
                Console.WriteLine("Has selections: {0}", dataQuery.HasSelections());
                Console.WriteLine("(");
                foreach (var selectionGroup in dataQuery.SelectionGroups)
                {
                    if (selectionGroup.DateFrom != null)
                    {
                        Console.WriteLine("\tPeriod from {0}", selectionGroup.DateFrom);
                        Console.WriteLine(" AND ");
                    }

                    if (selectionGroup.DateTo != null)
                    {
                        Console.WriteLine("\tPeriod to {0}", selectionGroup.DateTo);
                        Console.WriteLine(" AND ");
                    }

                    foreach (var selection in selectionGroup.Selections)
                    {
                        var s = selection.HasMultipleValues ? string.Join(" OR ", selection.Values) : selection.Value;
                        Console.WriteLine("{0} = ( {1} )", selection.ComponentId, s);
                        Console.WriteLine(" AND ");
                    }
                }
            }
        }

        #endregion
    }
}