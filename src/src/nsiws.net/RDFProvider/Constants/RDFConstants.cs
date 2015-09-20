// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RDFConstants.cs" company="ISTAT">
//   Date Created : 2014-09-09
//   //  
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Constants
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    

    #endregion

    /// <summary>
    ///     The sdmx constants.
    /// </summary>
    public static class RDFConstants
    {
        #region Constants
       
        /// <summary>
        ///     The xml ns.
        /// </summary>
        public const string XmlNs = "http://www.w3.org/XML/1998/namespace";       
        public const string RdfNs21 = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        public const string RdfProperty = "http://datiopen.istat.it/property/";
        public const string RdfS = "http://www.w3.org/2000/01/rdf-schema#";
        public const string RdfOwl = "http://www.w3.org/2002/07/owl#";
        public const string RdfXsd = "http://www.w3.org/2001/XMLSchema#";
        public const string RdfDcTerms = "http://purl.org/dc/terms/";
        public const string RdfFoaf = "http://xmlns.com/foaf/0.1/";
        public const string RdfSkos = "http://www.w3.org/2004/02/skos/core#";
        public const string Rdfxkos = "http://purl.org/linked-data/xkos#";
        public const string RdfProv = "http://www.w3.org/ns/prov#";
        public const string Rdfqb = "http://purl.org/linked-data/cube#/";
        public const string RdfqbDataSet = "http://purl.org/linked-data/cube#dataset";
        public const string Rdfsdmx = "http://purl.org/linked-data/sdmx#";
        public const string Rdfsdmxattribute = "http://purl.org/linked-data/sdmx/2009/attribute#";
        public const string RdfDataset = "http://datiopen.istat.it/dataset/";
        public const string Rdftype = "http://purl.org/linked-data/cube#Observation";
        public const string RdfObsValue = "http://www.w3.org/2001/XMLSchema#decimal";
        public const string RdfObsStatus = "http://datiopen.istat.it/code/1.1/CL_FLAG/";
        public const string RdfTimePeriod = "http://reference.data.gov.uk/id/year/";
        public const string RdfCode = "http://datiopen.istat.it/code/";
        //public const string RdfDati = "http://datiopen.istat.it/code/";
        public const string RdfDati = "http://datiopen.istat.it/data/";
        public const string RdfStructure = "http://datiopen.istat.it/structure/";
        public const string RdfCatScheme = "http://datiopen.istat.it/categoryscheme/";
        public const string RdfCodelist = "http://purl.org/linked-data/sdmx#CodeList";
        public const string RdfConceptScheme = "http://www.w3.org/2004/02/skos/core#ConceptScheme";
        public const string RdfCategorySchema = "http://purl.org/linked-data/sdmx#CategorySchema";
        public const string RdfConceptURI = "http://www.w3.org/2004/02/skos/core#Concept";
        public const string Rdfclass = "http://datiopen.istat.it/class/";
        public const string Rdfconcept = "http://datiopen.istat.it/concept/";
        public const string RdfInteger = "http://www.w3.org/2001/XMLSchema#integer";
        public const string RdfSdmxCode = "http://purl.org/linked-data/sdmx/2009/code#";
        public const string RdfSdmxConcept = "http://purl.org/linked-data/sdmx/2009/concept#";
        public const string RdfSdmxDimension = "http://purl.org/linked-data/sdmx/2009/dimension#";
        public const string RdfSdmxMeasure = "http://purl.org/linked-data/sdmx/2009/measure#";
        public const string RdfSdmxMetadata = "http://purl.org/linked-data/sdmx/2009/metadata#";
        public const string RdfSdmxSubject = "http://purl.org/linked-data/sdmx/2009/subject#";
        public const string RdfComponentDimensionProperty = "http://purl.org/linked-data/cube#DimensionProperty";
        public const string RdfComponentCodedProperty = "http://purl.org/linked-data/cube#CodedProperty";
        public const string RdfComponentProperty = "http://www.w3.org/1999/02/22-rdf-syntax-ns#Property";
        public const string RdfComponentFrequencyRole = "http://purl.org/linked-data/sdmx#FrequencyRole";
        public const string RdfComponentAttributeProperty = "http://purl.org/linked-data/cube#AttributeProperty";
        public const string RdfComponentMeasureProperty = "http://purl.org/linked-data/cube#MeasureProperty";
        public const string RdfComponentConceptRole = "http://purl.org/linked-data/sdmx#ConceptRole";
        public const string RdfComponentPrimaryMeasureRole = "http://purl.org/linked-data/sdmx#PrimaryMeasureRole";
        public const string RdfComponentTimeRole = "http://purl.org/linked-data/sdmx#TimeRole";
        public const string RdfStructureType = "http://purl.org/linked-data/sdmx#DataStructureDefinition";
        public const string RdfisFinal = "http://www.w3.org/2001/XMLSchema#boolean";
        public const string RdfProvenance = "http://datiopen.istat.it/provenance/activity/";
        public const string RdfActivity = "http://www.w3.org/ns/prov#Activity";
        public const string RdfDateTime= "http://www.w3.org/2001/XMLSchema#dateTime";
        public const string RdfEntity = "http://www.w3.org/ns/prov#Entity";
        public const string RdfLicense = "http://creativecommons.org/publicdomain/zero/1.0/";

        public const string ISTAT = "Istituto Nazionale di Statistica";
        public const string LangAttribute = "lang";
        public const string NaN = "NaN";
        public const string SchemaLocation = "schemaLocation";
        public const string Xmlns = "xmlns";
        public const string XmlPrefix = "xml";
        public const string XmlSchemaNS = "http://www.w3.org/2001/XMLSchema-instance";
        public const string XmlSchemaPrefix = "xsi";


        #endregion

        #region Static Fields


        #endregion

        #region Public Properties

        public static IList<string> NamespacesRDF
        {
            get
            {
                IList<string> allNamespaces = new List<string>();
                allNamespaces.Add(RdfNs21);
                return allNamespaces;
            }
        }

        /*
         * Gets a list of all the SDMX version 2.1 namespaces 
         */



        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the version of the Schema for the given URI
        /// </summary>
        /// <param name="uri">The uri string
        /// </param>
        /// <returns>
        /// The Sdmx schema
        /// </returns>
        //public static SdmxSchema GetSchemaVersion(string uri)
        //{
        //    if (NamespacesRDF.Contains(uri))
        //    {
        //        return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.Rdf);
        //    }

        //    throw new ArgumentException("Unknown schema URI: " + uri);
        //}

        #endregion

        #region Methods

      

        #endregion

    }
}