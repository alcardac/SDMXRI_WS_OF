// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RdfDataWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Writer.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using RDFProvider.Constants;
    using RDFProvider.Retriever.Model;



    public class RDFDataWriterEngine : Writer, IRDFDataWriterEngine
    {
        #region Fields

        private readonly XmlWriter _writer;

        private readonly RDFNamespaces _namespaces;

        private string _dimensionAtObservation;

        private Action<string> _writeObservationMethod;

        private bool _startedDataSet;

        private bool _startedSeries;

        private bool _startedSeriesProv;

        private bool _startedProvSeries;

        private bool _startedSubSeries;

        private readonly List<KeyValuePair<string, string>> _componentVals = new List<KeyValuePair<string, string>>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDataWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public RDFDataWriterEngine(XmlWriter writer)            
            :base(writer,CreateDataNamespaces())
        {
            this._writer = writer;
            this._writeObservationMethod = this.RDFWriteObservation21;
            this._namespaces = CreateDataNamespaces();
        }


        #endregion

        #region Public Properties


        #endregion

        #region Properties

        protected  string DimensionAtObservation
        {
            get
            {
                return this._dimensionAtObservation;
            }
        }

        protected  string MessageElement
        {
            get
            {
                return RDFNameTableCache.GetElementName(RDFElementNameTable.RDF);
            }
        }

        protected RDFNamespaces Namespaces
        {
            get
            {
                return this._namespaces;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Close(params IFooterMessage[] footerMessages)
        {
            this.EndSeries();
            this.EndDataSet();

            this.CloseMessageTag();
        }

        public void StartDataset(IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, DataRetrievalInfoSeries info)
        {
            base.StartDataset(dataflow, dsd, header, info);
        }

        public void StartSeries(string values)
        {
            string[] val;
            val = (values.Split('/'));
            
            this.EndSeries();

                this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
                this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, values);
                this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.type);
                this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.Rdftype);
                this.WriteEndElement();
                this.WriteStartElement(this.Namespaces.QB, RDFElementNameTable.dataset);
                this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfDataset + "/" + val[4]);
                this.WriteEndElement();             

            this._startedSeries = true;
        }

        public void RDFWriterStrucInfo(string dataset, string struc)
        {
            this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, RDFConstants.Rdfqb + dataset);
            this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.type);
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfqbDataSet);
            this.WriteEndElement();
            this.WriteStartElement(this.Namespaces.QB, RDFElementNameTable.structure.ToString());
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfStructure + struc);
            this.WriteEndElement();
            this.WriteStartElement(this.Namespaces.DocTerms, RDFElementNameTable.identifier);
            this.RDFWriteString(dataset);
            this.WriteEndElement();
            this.WriteEndElement();
        }

        public void RDFWriteObservation(string obsConceptValue, string obsValue)
        {
            //string obsConcept = true ? this._dimensionAtObservation : ConceptRefUtil.GetConceptId(this.KeyFamily.TimeDimension.ConceptRef);
            string obsConcept = "TIME_PERIOD";
            this.RDFWriteObservation(obsConcept, obsConceptValue, obsValue);
        }

        public void RDFWriteObservation(string observationConceptId, string obsConceptValue, string primaryMeasureValue)
        {
            if (observationConceptId == null)
            {
                throw new ArgumentNullException("observationConceptId");
            }

            string obsValue = string.IsNullOrEmpty(primaryMeasureValue) ? this.DefaultObs : primaryMeasureValue;

            this._writeObservationMethod(obsConceptValue);

            if (!string.IsNullOrEmpty(obsValue))
            {
                this.WriteStartElement(this.Namespaces.Property, RDFElementNameTable.OBS_VALUE);
                this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.datatype, RDFConstants.RdfObsValue);
                this.RDFWriteString(obsValue);
                this.WriteEndElement();
                this.WriteStartElement(this.Namespaces.Property, RDFElementNameTable.OBS_STATUS);
                this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfObsStatus);
                this.WriteEndElement();
            }

        }

        public void RDFWriteString(string obsValue)
        {
            this.WriteString(obsValue);
        }

        public void WriteSeriesKeyValue(string key, string value, string version, string id)
        {
            this.WriteConceptValue(key, value, version, id);
        }

        #endregion

        #region Methods

        private static RDFNamespaces CreateDataNamespaces()
        {
            var namespaces = new RDFNamespaces();
            namespaces.Xsi = new NamespacePrefixPair(RDFConstants.XmlSchemaNS, RDFConstants.XmlSchemaPrefix);            
                namespaces.RDF = new NamespacePrefixPair(RDFConstants.RdfNs21, RDFPrefixConstants.RDF);
                namespaces.Property = new NamespacePrefixPair(RDFConstants.RdfProperty, RDFPrefixConstants.Property);
                namespaces.QB = new NamespacePrefixPair(RDFConstants.Rdfqb, RDFPrefixConstants.QB);
                namespaces.DocTerms = new NamespacePrefixPair(RDFConstants.RdfDcTerms, RDFPrefixConstants.DocTerms);
                namespaces.RDFs = new NamespacePrefixPair(RDFConstants.RdfS, RDFPrefixConstants.RDFs);
                namespaces.Owl = new NamespacePrefixPair(RDFConstants.RdfOwl, RDFPrefixConstants.Owl);
                namespaces.Skos = new NamespacePrefixPair(RDFConstants.RdfSkos, RDFPrefixConstants.Skos);
                namespaces.Xkos = new NamespacePrefixPair(RDFConstants.Rdfxkos, RDFPrefixConstants.Xkos);
                namespaces.XML = new NamespacePrefixPair(RDFConstants.XmlNs, RDFPrefixConstants.XML);
                namespaces.SdmxConcept = new NamespacePrefixPair(RDFConstants.RdfSdmxConcept, RDFPrefixConstants.SdmxConcept);
                namespaces.Prov = new NamespacePrefixPair(RDFConstants.RdfProv, RDFPrefixConstants.Prov);
                namespaces.SchemaLocation = string.Format(
                    CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs21);            

            return namespaces;
        }

        protected override void WriteFormatDataSet(IDatasetHeader header, DataRetrievalInfoSeries info)
        {
            if (this._startedDataSet)
            {
                return;
            }
            this._dimensionAtObservation = this.GetDimensionAtObservation(header);

            this.RDFWriteMessageTag(MessageElement, this.Namespaces);
            this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.RDF);
            this.RDFWriteProvenance(info.MappingSet.DataSet.Description.ToString(), RDFConstants.RdfDataset);

            this.RDFWriterStrucInfo(info.MappingSet.DataSet.Description.ToString(), info.MappingSet.Dataflow.Dsd.Id.ToString());

            this._startedDataSet = true;
        }

        protected void RDFWriteProvenance(string element, string type)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (type == null)
            {
                throw new ArgumentNullException("constant");
            }

            DateTime data = DateTime.Now;
            string provenanceID = data.ToString("yyyyMMddHHmmss");
            string label = "Transformed " + element + " data";

            this.StartSeriesProv(provenanceID, RDFConstants.RdfProvenance);
            this.RDFWriteGeneralTag(Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.about, RDFConstants.RdfActivity);

            this.RDFWriteDescriptionTag(this.Namespaces.RDFs, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, "en", label);
            this.RDFWriteElementWithStringTag(this.Namespaces.Prov, this.Namespaces.RDF, RDFElementNameTable.startedAtTime, RDFAttributeNameTable.datatype, data.ToString("yyyy-MM-ddTHH:mm:ssZ"), RDFConstants.RdfDateTime);
            this.StartProvSeries();
            this.StartSubSeries(type + element);
            this.RDFWriteGeneralTag(Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfEntity);
            this.RDFWriteElementWithStringTag(this.Namespaces.Prov, this.Namespaces.RDF, RDFElementNameTable.generatedAtTime, RDFAttributeNameTable.datatype, data.ToString("yyyy-MM-ddTHH:mm:ssZ"), RDFConstants.RdfDateTime);

            //Manca -> <prov:wasDerivedFrom rdf:resource="http://example.org/data/SEP_AMBIENTE_ACQ_ACQDOM.xml"/>

            this.RDFWriteGeneralTag(this.Namespaces.Prov, this.Namespaces.RDF, RDFElementNameTable.wasGeneratedBy, RDFAttributeNameTable.resource, RDFConstants.RdfProvenance + provenanceID);
            this.RDFWriteGeneralTag(this.Namespaces.DocTerms, RDFElementNameTable.issued, RDFAttributeNameTable.datatype, RDFConstants.RdfLicense);
            this.RDFWriteElementWithStringTag(this.Namespaces.DocTerms, this.Namespaces.RDF, RDFElementNameTable.issued, RDFAttributeNameTable.datatype, data.ToString("yyyy-MM-ddTHH:mm:ssZ"), RDFConstants.RdfDateTime);
            this.RDFWriteGeneralTag(this.Namespaces.DocTerms, RDFElementNameTable.creator, RDFAttributeNameTable.resource, RDFConstants.ISTAT);
            this.EndSubSeries();
            this.EndProvSeries();
            this.EndSeriesProv();

            provenanceID = null;
            label = null;
        }


        public void StartSeriesProv(string value, string constant)
        {
            EndSeriesProv();
            this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, constant + value);
            this._startedSeriesProv = true;
        }


        private void EndSeriesProv()
        {
            if (this._startedSeriesProv)
            {
                // in which case close it
                this.WriteEndElement();
                this._startedSeriesProv = false;
            }
        }

        private void EndSeries()
        {
            if (this._startedSeries)
            {
                // in which case close it
                this.WriteEndElement();
                this._startedSeries = false;
            }
        }

        public void StartSubSeries(string value)
        {
            EndSubSeries();
            this.WriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, value);
            this._startedSubSeries = true;
        }

        private void EndSubSeries()
        {
            if (this._startedSubSeries)
            {
                // in which case close it
                this.WriteEndElement();
                this._startedSubSeries = false;
            }
        }


        public void StartProvSeries()
        {
            EndProvSeries();
            this.WriteStartElement(this.Namespaces.Prov, RDFElementNameTable.generated);
            this._startedProvSeries = true;
        }

        private void EndProvSeries()
        {
            if (this._startedProvSeries)
            {
                // in which case close it
                this.WriteEndElement();
                this._startedProvSeries = false;
            }
        }



        protected void RDFWriteMessageTag(string element, RDFNamespaces namespaces)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            // <?xml version="1.0" encoding="UTF-8"?>
            if (!this.Wrapped)
            {

                // <RDFData
                this.WriteStartElement(this.Namespaces.RDF, element);

                // xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                this.WriteNamespaceDecl(this.Namespaces.Xsi);

                // rdf:Property
                this.WriteNamespaceDecl(this.Namespaces.Property);
                this.WriteNamespaceDecl(this.Namespaces.QB);
                this.WriteNamespaceDecl(this.Namespaces.Skos);
                this.WriteNamespaceDecl(this.Namespaces.Xkos);
                this.WriteNamespaceDecl(this.Namespaces.Owl);
                this.WriteNamespaceDecl(this.Namespaces.RDFs);
                this.WriteNamespaceDecl(this.Namespaces.DocTerms);
                this.WriteNamespaceDecl(this.Namespaces.SdmxConcept);
                this.WriteNamespaceDecl(this.Namespaces.Prov);
                
                string schemaLocation = this.Namespaces.SchemaLocation ?? string.Empty;
                string structureSpecific = string.Empty;

                if (!string.IsNullOrWhiteSpace(schemaLocation))
                {
                    this.WriteAttributeString(this.Namespaces.Xsi, RDFConstants.SchemaLocation, schemaLocation);
                }


            }
        }

        private void WriteConceptValue(string concept, string value, string version, string id)
        {
            this.WriteStartElement(this.Namespaces.Property, concept);
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfCode + version + "/" + id + "/" + value);
            this.WriteEndElement();
        }

        private void RDFWriteObservation21(string obsConcept)
        {
            //this.WriteStartElement(this.Namespaces.Property, this.DimensionAtObservation);
            this.WriteStartElement(this.Namespaces.Property, "TIME_PERIOD");
            this.WriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfTimePeriod + obsConcept);
            this.WriteEndElement();
        }

            protected virtual string GetDimensionAtObservation(IDatasetHeader header)
            {
                string dimensionAtObservation = DimensionObject.TimeDimensionFixedId;
                if (header != null && (header.DataStructureReference != null
                        && !string.IsNullOrWhiteSpace(header.DataStructureReference.DimensionAtObservation)))
                {
                    dimensionAtObservation = header.DataStructureReference.DimensionAtObservation;
                }

                return dimensionAtObservation;
            }



        public void RDFWriteGeneralTag(NamespacePrefixPair NS, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Constant)
        {
            this.WriteStartElement(NS, Element);
            this.WriteAttributeString(NS, Attribute, Constant);
            this.WriteEndElement();
        }

        public void RDFWriteGeneralTag(NamespacePrefixPair NS, NamespacePrefixPair NSAtt, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Constant)
        {
            this.WriteStartElement(NS, Element);
            this.WriteAttributeString(NSAtt, Attribute, Constant);
            this.WriteEndElement();
        }

        public void RDFWriteDescriptionTag(NamespacePrefixPair NS, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Locale, string Value)
        {
            this.WriteStartElement(NS, Element);
            this.WriteAttributeString(this.Namespaces.XML, Attribute, Locale);
            this.WriteString(Value);
            this.WriteEndElement();
        }

        public void RDFWriteElementWithStringTag(NamespacePrefixPair NS, NamespacePrefixPair NSAtt, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Text, string Constant)
        {
            this.WriteStartElement(NS, Element);
            this.WriteAttributeString(NSAtt, Attribute, Constant);
            this.WriteString(Text);
            this.WriteEndElement();
        }

        #endregion

    }
}
