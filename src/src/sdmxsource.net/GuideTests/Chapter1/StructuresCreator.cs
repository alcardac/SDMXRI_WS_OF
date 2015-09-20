// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuresCreator.cs" company="Eurostat">
//   Date Created : 2014-04-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Description of Class1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GuideTests.Chapter1
{
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///    Creating Structures example
    /// </summary>
    public class StructuresCreator
    {
        #region Public Methods and Operators


        /// <summary>
        /// Builds the agency scheme.
        /// </summary>
        /// <returns>
        /// The <see cref="IAgencyScheme"/>.
        /// </returns>
        public IAgencyScheme BuildAgencyScheme()
        {
            IAgencyScheme defScheme = AgencySchemeCore.CreateDefaultScheme();
            IAgencySchemeMutableObject mutableDefScheme = defScheme.MutableInstance;

            mutableDefScheme.CreateItem("SDMXSOURCE", "Sdmx Source");
            return mutableDefScheme.ImmutableInstance;
        }


        /// <summary>
        /// Builds the concept scheme.
        /// </summary>
        /// <returns>
        /// The <see cref="IConceptSchemeObject"/>.
        /// </returns>
        public IConceptSchemeObject BuildConceptScheme()
        {
            IConceptSchemeMutableObject conceptSchemeMutable = new ConceptSchemeMutableCore();
            conceptSchemeMutable.AgencyId = "SDMXSOURCE";
            conceptSchemeMutable.Id = "CONCEPTS";
            conceptSchemeMutable.Version = "1.0";
            conceptSchemeMutable.AddName("en", "Web Service Concepts");

            conceptSchemeMutable.CreateItem("COUNTRY", "Country");
            conceptSchemeMutable.CreateItem("INDICATOR", "World Developement Indicators");
            conceptSchemeMutable.CreateItem("TIME", "Time");
            conceptSchemeMutable.CreateItem("OBS_VALUE", "Observation Value");

            return conceptSchemeMutable.ImmutableInstance;
        }

        /// <summary>
        /// Builds the country codelist.
        /// </summary>
        /// <returns>
        /// The <see cref="ICodelistObject" />.
        /// </returns>
        public ICodelistObject BuildCountryCodelist()
        {
            ICodelistMutableObject codelistMutable = new CodelistMutableCore();
            codelistMutable.AgencyId = "SDMXSOURCE";
            codelistMutable.Id = "CL_COUNTRY";
            codelistMutable.Version = "1.0";
            codelistMutable.AddName("en", "Country");

            codelistMutable.CreateItem("UK", "United Kingdom");
            codelistMutable.CreateItem("FR", "France");
            codelistMutable.CreateItem("DE", "Germany");

            return codelistMutable.ImmutableInstance;
        }

        /// <summary>
        /// Builds the data structure.
        /// </summary>
        /// <returns>
        /// The <see cref="IDataStructureObject" />.
        /// </returns>
        public IDataStructureObject BuildDataStructure()
        {
            IDataStructureMutableObject dsd = new DataStructureMutableCore();
            dsd.AgencyId = "SDMXSOURCE";
            dsd.Id = "WDI";
            dsd.AddName("en", "World Development Indicators");

            dsd.AddDimension(CreateConceptReference("COUNTRY"), CreateCodelistReference("CL_COUNTRY"));
            dsd.AddDimension(CreateConceptReference("INDICATOR"), CreateCodelistReference("CL_INDICATOR"));
            IDimensionMutableObject timeDim = dsd.AddDimension(CreateConceptReference("TIME"), null);
            timeDim.TimeDimension = true;
            dsd.AddPrimaryMeasure(CreateConceptReference("OBS_VALUE"));

            return dsd.ImmutableInstance;
        }

        /// <summary>
        /// Builds a dataflow that refernces a dsd
        /// </summary>
        /// <param name="id">
        /// the id of the dataflow
        /// </param>
        /// <param name="name">
        /// the english name of the dataflow
        /// </param>
        /// <param name="dsd">
        /// the data structure that is being referenced by the dsd.
        /// </param>
        /// <returns>
        /// the newly created dataflow.
        /// </returns>
        public IDataflowObject BuildDataflow(string id, string name, IDataStructureObject dsd)
        {
            IDataflowMutableObject dataflow = new DataflowMutableCore();
            dataflow.AgencyId = "SDMXSOURCE";
            dataflow.Id = id;
            dataflow.AddName("en", name);

            dataflow.DataStructureRef = dsd.AsReference;

            return dataflow.ImmutableInstance;
        }

        /// <summary>
        /// The build indicator codelist.
        /// </summary>
        /// <returns>
        /// The <see cref="ICodelistObject"/>.
        /// </returns>
        public ICodelistObject BuildIndicatorCodelist()
        {
            ICodelistMutableObject codelistMutable = new CodelistMutableCore();
            codelistMutable.AgencyId = "SDMXSOURCE";
            codelistMutable.Id = "CL_INDICATOR";
            codelistMutable.Version = "1.0";
            codelistMutable.AddName("en", "World Developement Indicators");

            ICodeMutableObject code;
            codelistMutable.CreateItem("E", "Environment");
            code = codelistMutable.CreateItem("E_A", "Agriculture land");
            code.ParentCode = "E";
            code = codelistMutable.CreateItem("E_P", "Population");
            code.ParentCode = "E";

            codelistMutable.CreateItem("H", "HEALTH");
            code = codelistMutable.CreateItem("H_B", "Birth Rate");
            code.ParentCode = "H";
            code = codelistMutable.CreateItem("H_C", "Children (0-14) living with HIV");
            code.ParentCode = "H";

            return codelistMutable.ImmutableInstance;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create codelist reference.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        private static IStructureReference CreateCodelistReference(string id)
        {
            return new StructureReferenceImpl("SDMXSOURCE", id, "1.0", SdmxStructureEnumType.CodeList);
        }

        /// <summary>
        /// The create concept reference.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        private static IStructureReference CreateConceptReference(string id)
        {
            return new StructureReferenceImpl("SDMXSOURCE", "CONCEPTS", "1.0", SdmxStructureEnumType.Concept, id);
        }

        #endregion
    }
}