// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingQueryParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The re using query parsing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.StructureQuery
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The re using query parsing manager.
    /// </summary>
    public class ReUsingQueryParsingManager
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
            IQueryParsingManager manager = new QueryParsingManager(SdmxSchemaEnumType.VersionTwo);
            IQueryWorkspace workspace;
            using (IReadableDataLocation location = new FileReadableDataLocation("QueryStructureRequest.xml"))
            {
                workspace = manager.ParseQueries(location);
            }

            if (workspace != null)
            {
                Console.WriteLine("Is Resolve references : {0}", workspace.ResolveReferences);
                Console.WriteLine("Has Structure Queries : {0}", workspace.HasStructureQueries());
                if (workspace.HasStructureQueries())
                {
                    foreach (IStructureReference simpleStructureQuery in workspace.SimpleStructureQueries)
                    {
                        IMaintainableRefObject reference = simpleStructureQuery.MaintainableReference;
                        Console.WriteLine(
                            "Requested a {0}\n\tAgency ID: {1}\n\tId: {2}\n\tVersion: {3}", 
                            simpleStructureQuery.MaintainableStructureEnumType, 
                            reference.HasAgencyId() ? reference.AgencyId : "(none)", 
                            reference.HasMaintainableId() ? reference.MaintainableId : "(none)", 
                            reference.HasVersion() ? reference.Version : "(none)");
                    }
                }
            }
        }

        #endregion
    }
}