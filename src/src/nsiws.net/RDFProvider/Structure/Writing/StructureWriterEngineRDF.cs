// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWritingEngineRDF.cs" company="ISTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writing engine for RDF.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Structure.Writing
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
  
    using RDFProvider.Constants;
    using System.Globalization;

    using Xml.Schema.Linq;
    using System.Collections.Generic;    
    using RDFProvider.Structure.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
   


    public class StructureWriterEngineRDF : IStructureRDFWriterEngine
    {
        #region Fields

        private string _namespace;

        private RDFProvider.Constants.NamespacePrefixPair _dataSetNS;  

        private readonly XmlWriter _writer;

        private SchemaLocationWriter schemaLocationWriter;

        private bool _wrapped;

        private readonly RDFNamespaces _namespaces;

        private bool _startedSeries;

        private bool _startedProvSeries;

        private bool _startedSubSeries;

        private bool _startedKeySeries;

        private bool _startedSeeAlso;

        private bool _startedComponent;

        private bool _startedComponentSpecification;

        private bool _startedComponentDimension;

        private bool _startedComponentMeasure;

        private bool _startedComponentAttribute;

        private bool _startedComponentConcept;

        private bool _startedSubDesc;

        private bool _startedMajorSeries;

        private bool _startedCodelistChild;

        #endregion

        #region Constructors and Destructors

        public StructureWriterEngineRDF(Stream outputStream)
        {
        }

        public StructureWriterEngineRDF(XmlWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }


            this._namespaces = CreateDataNamespaces();
            this._writer = writer;
            this._wrapped = writer.WriteState != WriteState.Start;
        }

        public StructureWriterEngineRDF(XmlWriter writer, RDFNamespaces namespaces, SdmxSchema schema)            
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }
            this._namespaces = namespaces;
            this._writer = writer;
            this._wrapped = writer.WriteState != WriteState.Start;
        }

        #endregion

        #region Public Properties 

        protected string MessageElement
        {
            get
            {
                return RDFNameTableCache.GetElementName(RDFElementNameTable.RDF);
            }
        }

        public string Namespace
        {
            get
            {
                return this._namespace;
            }

            set
            {
                this._namespace = value;
            }
        }

        #endregion

        #region OpenClose Methods

        public void StartMajorSeries(string value, string constant)
        {
            EndMajorSeries();
            this.RDFWriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, constant + value);
            this._startedMajorSeries = true;
        }

        private void EndMajorSeries()
        {
            if (this._startedMajorSeries)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedMajorSeries = false;
            }
        }

        public void StartProvSeries()
        {
            EndProvSeries();
            this.RDFWriteStartElement(this.Namespaces.Prov, RDFElementNameTable.generated);            
            this._startedProvSeries = true;
        }

        private void EndProvSeries()
        {
            if (this._startedProvSeries)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedProvSeries = false;
            }
        }

        public void StartSeries(string value, string constant)
        {
            EndSeries();
            this.RDFWriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, constant + value);
            this._startedSeries = true;
        }


        private void EndSeries()
        {
            if (this._startedSeries)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedSeries = false;
            }
        }

        public void StartCodelistChildSeries()
        {
            EndCodelistChildSeries();
            this.RDFWriteStartElement(this.Namespaces.Xkos, RDFElementNameTable.isPartOf);            
            this._startedCodelistChild = true;
        }

        private void EndCodelistChildSeries()
        {
            if (this._startedCodelistChild)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedCodelistChild = false;
            }
        }

        public void StartSubDesc(string value, string constant)
        {
            EndSubDesc();
            this.RDFWriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, constant + value);
            this._startedSubDesc = true;
        }

        private void EndSubDesc()
        {
            if (this._startedSubDesc)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedSubDesc = false;
            }
        }

        public void StartSeeAlso()
        {
            EndSeeAlso();
            this.RDFWriteStartElement(this.Namespaces.RDFs, RDFElementNameTable.seeAlso);
            this._startedSeeAlso = true;
        }

        private void EndSeeAlso()
        {
            if (this._startedSeeAlso)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedSeeAlso = false;
            }
        }

        public void StartKeySeries()
        {
            EndKeySeries();
            this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.hasTopConcept);
            this._startedKeySeries = true;
        }

        private void EndKeySeries()
        {
            if (this._startedKeySeries)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedKeySeries = false;
            }
        }

        public void StartSubSeries(string value)
        {
            EndSubSeries();
            this.RDFWriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, value);
            this._startedSubSeries = true;
        }

        private void EndSubSeries()
        {
            if (this._startedSubSeries)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedSubSeries = false;
            }
        }

        public void StartComponent()
        {
            EndComponent();
            this.RDFWriteStartElement(this.Namespaces.QB, RDFElementNameTable.component);
            this._startedComponent = true;
        }

        private void EndComponent()
        {
            if (this._startedComponent)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedComponent = false;
            }
        }

        public void StartComponentSpecification()
        {
            EndComponentSpecification();
            this.RDFWriteStartElement(this.Namespaces.QB, RDFElementNameTable.ComponentSpecification);
            this._startedComponentSpecification = true;
        }

        private void EndComponentSpecification()
        {
            if (this._startedComponentSpecification)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedComponentSpecification = false;
            }
        }

        public void StartComponentDimension()
        {
            EndComponentDimension();
            this.RDFWriteStartElement(this.Namespaces.QB, RDFElementNameTable.dimension);
            this._startedComponentDimension = true;
        }

        private void EndComponentDimension()
        {
            if (this._startedComponentDimension)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedComponentDimension = false;
            }
        }

        public void StartComponentMeasure()
        {
            EndComponentMeasure();
            this.RDFWriteStartElement(this.Namespaces.QB, RDFElementNameTable.measure);
            this._startedComponentMeasure = true;
        }

        private void EndComponentMeasure()
        {
            if (this._startedComponentMeasure)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedComponentMeasure = false;
            }
        }

        public void StartComponentAttribute()
        {
            EndComponentAttribute();
            this.RDFWriteStartElement(this.Namespaces.QB, RDFElementNameTable.attribute);
            this._startedComponentAttribute = true;
        }

        private void EndComponentAttribute()
        {
            if (this._startedComponentAttribute)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedComponentAttribute = false;
            }
        }

        public void StartComponentConcept()
        {
            EndComponentConcept();
            this.RDFWriteStartElement(this.Namespaces.QB, RDFElementNameTable.concept);
            this._startedComponentConcept = true;
        }

        private void EndComponentConcept()
        {
            if (this._startedComponentConcept)
            {
                // in which case close it
                this.RDFWriteEndElement();
                this._startedComponentConcept = false;
            }
        }

        #endregion

        #region Methods

        private static RDFNamespaces CreateDataNamespaces()
        {
            var namespaces = new RDFNamespaces
            {
                Xsi =
                    new NamespacePrefixPair(
                    RDFConstants.XmlSchemaNS, RDFConstants.XmlSchemaPrefix)
            };
                    //Inicializar todos os namespaces que vao ser utilizados
                    namespaces.RDF = new NamespacePrefixPair(RDFConstants.RdfNs21, RDFPrefixConstants.RDF);
                    namespaces.Property = new NamespacePrefixPair(RDFConstants.RdfProperty, RDFPrefixConstants.Property);
                    namespaces.QB = new NamespacePrefixPair(RDFConstants.Rdfqb, RDFPrefixConstants.QB);
                    namespaces.DocTerms = new NamespacePrefixPair(RDFConstants.RdfDcTerms, RDFPrefixConstants.DocTerms);
                    namespaces.RDFs = new NamespacePrefixPair(RDFConstants.RdfS, RDFPrefixConstants.RDFs);
                    namespaces.Owl = new NamespacePrefixPair(RDFConstants.RdfOwl, RDFPrefixConstants.Owl);
                    namespaces.Skos = new NamespacePrefixPair(RDFConstants.RdfSkos, RDFPrefixConstants.Skos);
                    namespaces.Xkos = new NamespacePrefixPair(RDFConstants.Rdfxkos, RDFPrefixConstants.Xkos);
                    namespaces.XML = new NamespacePrefixPair(RDFConstants.XmlNs, RDFPrefixConstants.XML);
                    namespaces.Prov = new NamespacePrefixPair(RDFConstants.RdfProv, RDFPrefixConstants.Prov);
                    namespaces.SdmxConcept = new NamespacePrefixPair(RDFConstants.RdfSdmxConcept, RDFPrefixConstants.SdmxConcept);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs21);      

            return namespaces;
        }

        protected void RDFWriteNamespaceDecl(NamespacePrefixPair ns)
        {
            this.RDFWriteAttributeString(RDFConstants.Xmlns, ns.Prefix, ns.NS);
        }

        public void RDFDataStructure(ISdmxObjects dataStructure)
        {
            this.WriteMessageTag();
           
            ISet<IDataStructureObject> dataStructures = dataStructure.DataStructures;
            if (dataStructures.Count > 0)
            {
                // Corpo
                foreach (IDataStructureObject dataStructuresBean in dataStructures)
                {
                    this.RDFWriteProvenance(dataStructuresBean.Id, RDFConstants.RdfStructure);
                    string URIDesc = dataStructuresBean.Version + "/" + dataStructuresBean.Id;
                    this.StartMajorSeries(URIDesc, RDFConstants.RdfStructure);

                    //Scrive la radice della DataStructure 
                    //<rdf:resource="http://purl.org/linked-data/sdmx#DataStructureDefinition" />
                    this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfStructureType);

                    //<rdf:resource="http://purl.org/linked-data/sdmx#DataStructureDefinition" />
                    this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfStructureType);
                    //dsi
                    //<sdmx-concept:dsi>
                    this.RDFWriteElementWithStringTag(this.Namespaces.SdmxConcept, RDFElementNameTable.dsi, dataStructuresBean.Id);
                    //
                    //mAgency
                    //<sdmx-concept:mAgency>
                    this.RDFWriteElementWithStringTag(this.Namespaces.SdmxConcept, RDFElementNameTable.mAgency, dataStructuresBean.AgencyId);
                    //
                    //Version
                    //<owl:versionInfo
                    this.RDFWriteElementWithStringTag(this.Namespaces.Owl, RDFElementNameTable.versionInfo, dataStructuresBean.Version);
                    //
                    //isFinal
                    //<sdmx-concept:isFinal
                    this.RDFWriteElementAttributeWithStringTag(this.Namespaces.SdmxConcept, this.Namespaces.RDF, RDFElementNameTable.isFinal, RDFAttributeNameTable.datatype, RDFConstants.RdfisFinal, dataStructuresBean.IsFinal.IsTrue.ToString());

                    var prefLabel = dataStructuresBean.Names;

                    //<skos:prefLabel
                    foreach (var item in prefLabel)
                    {
                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                    }

                    var definition = dataStructuresBean.Descriptions;

                    //<skos:definition
                    foreach (var item in definition)
                    {
                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.definition, RDFAttributeNameTable.lang, item.Locale, item.Value);
                    }

                    // Prima parte      : Dimension
                    var dimensions = dataStructuresBean.DimensionList;
                    if (!(dimensions == null))
                    {
                        foreach (var item in dimensions.Dimensions)
                        {
                            this.StartComponent();
                            this.StartComponentSpecification();

                            //<qb:componentProperty/>
                            this.RDFWriteGeneralTag(this.Namespaces.QB, RDFElementNameTable.componentProperty, RDFAttributeNameTable.resource, RDFConstants.RdfProperty + item.Id);

                            //<qb:dimension>
                            this.StartComponentDimension();
                            //<rdf:Description
                            this.StartSeries(item.Id, RDFConstants.RdfProperty);

                            //<rdf:type rdf:resource="http://purl.org/linked-data/cube#DimensionProperty"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentDimensionProperty);

                            //<rdf:type rdf:resource="http://purl.org/linked-data/cube#CodedProperty"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentCodedProperty);

                            //<rdf:type rdf:resource="http://www.w3.org/1999/02/22-rdf-syntax-ns#Property"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentProperty);

                            //<qb:component
                            this.StartComponentConcept();

                            ICrossReference Ref = item.ConceptRef;
                            foreach (ICrossReference Sec in Ref.ReferencedFrom.CrossReferences)
                            {
                                this.StartSubDesc(Sec.MaintainableReference.Version + "/" + Sec.MaintainableReference.MaintainableId + "/" + item.Id, RDFConstants.Rdfconcept);
                            }

                            if (item.FrequencyDimension)
                            {
                                //<rdf:type rdf:resource="http://purl.org/linked-data/sdmx#FrequencyRole"/>
                                this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentFrequencyRole);
                                //
                            }
                            else if (item.TimeDimension)
                            {
                                //<rdf:type rdf:resource="http://purl.org/linked-data/sdmx#TimeRole"/>
                                this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentTimeRole);
                            }
                            else
                            {
                                //<rdf:type rdf:resource="http://purl.org/linked-data/sdmx#ConceptRole"/>
                                this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentConceptRole);
                            }

                            this.EndSubDesc();

                            this.EndComponentConcept();

                            foreach (ICrossReference Sec in Ref.ReferencedFrom.CrossReferences)
                            {
                                if (item.TimeDimension)
                                {
                                    foreach (IConceptSchemeObject ConceptSchema in dataStructure.ConceptSchemes)
                                    {
                                        foreach (IConceptObject Concept in ConceptSchema.Items)
                                        {
                                            if (Concept.Id == Sec.FullId)
                                            {
                                                foreach (var Descs in Concept.Names)
                                                {
                                                    this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, Descs.Locale, Descs.Value);
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    foreach (var Terc in Sec.ReferencedFrom.Composites)
                                    {
                                        foreach (var Qua in Terc.CrossReferences)
                                        {
                                            string Codelist = Qua.MaintainableReference.Version + "/" + Qua.MaintainableReference.MaintainableId;
                                            //<qb:codelist
                                            this.RDFWriteGeneralTag(this.Namespaces.QB, RDFElementNameTable.codeList, RDFAttributeNameTable.resource, RDFConstants.RdfCode + Codelist);
                                            //rdfs:range
                                            this.RDFWriteGeneralTag(this.Namespaces.RDFs, RDFElementNameTable.range, RDFAttributeNameTable.resource, RDFConstants.Rdfclass + Codelist);
                                            foreach (ICodelistObject Codelists in dataStructure.Codelists)
                                            {
                                                if (Codelists.AsReference.MaintainableReference.MaintainableId == Qua.MaintainableReference.MaintainableId)
                                                {
                                                    foreach (var Descs in Codelists.Names)
                                                    {
                                                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, Descs.Locale, Descs.Value);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            this.EndSeries();
                            this.EndComponentDimension();

                            //<qb:order
                            this.RDFWriteElementAttributeWithStringTag(this.Namespaces.QB, this.Namespaces.XML, RDFElementNameTable.order, RDFAttributeNameTable.datatype, RDFConstants.RdfInteger, item.Position.ToString());

                            this.EndComponentSpecification();
                            this.EndComponent();
                        }
                    }                    

                    // Seconda parte    : Measure
                    var measure = dataStructuresBean.PrimaryMeasure;
                    if (!(measure == null))
                    {
                        this.StartComponent();
                        this.StartComponentSpecification();

                        //<qb:componentProperty/>
                        this.RDFWriteGeneralTag(this.Namespaces.QB, RDFElementNameTable.componentProperty, RDFAttributeNameTable.resource, RDFConstants.RdfProperty + measure.Id);

                        //<qb:Measure>
                        this.StartComponentMeasure();
                        //<rdf:Description
                        this.StartSeries(measure.Id, RDFConstants.RdfProperty);

                        //<rdf:type rdf:resource="http://purl.org/linked-data/cube#MeasureProperty"/>
                        this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentMeasureProperty);

                        //<rdf:type rdf:resource="http://purl.org/linked-data/cube#CodedProperty"/>
                        this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentCodedProperty);

                        //<rdf:type rdf:resource="http://www.w3.org/1999/02/22-rdf-syntax-ns#Property"/>
                        this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentProperty);

                        //<qb:component
                        this.StartComponentConcept();

                        ICrossReference Refmeasure = measure.ConceptRef;
                        foreach (ICrossReference Sec in Refmeasure.ReferencedFrom.CrossReferences)
                        {
                            this.StartSubDesc(Sec.MaintainableReference.Version + "/" + Sec.MaintainableReference.MaintainableId + "/" + measure.Id, RDFConstants.Rdfconcept);
                        }

                        //<rdf:resource="http://purl.org/linked-data/sdmx#ConceptRole"/>
                        this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentPrimaryMeasureRole);

                        this.EndSubDesc();

                        this.EndComponentConcept();

                        foreach (ICrossReference Sec in Refmeasure.ReferencedFrom.CrossReferences)
                        {
                            foreach (var Terc in Sec.ReferencedFrom.CrossReferences)
                            {
                                foreach (IConceptSchemeObject ConceptSchemas in dataStructure.ConceptSchemes)
                                {
                                    if (ConceptSchemas.AsReference.MaintainableReference.MaintainableId == Terc.MaintainableReference.MaintainableId)
                                    {
                                        foreach (var MeasureItem in ConceptSchemas.Items)
                                        {
                                            if (Terc.FullId == MeasureItem.Id)
                                            {
                                                foreach (var Descs in MeasureItem.Names)
                                                {
                                                    this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, Descs.Locale, Descs.Value);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        this.EndSeries();
                        this.EndComponentMeasure();

                        //<qb:componentAttachment
                        this.RDFWriteGeneralTag(this.Namespaces.QB, RDFElementNameTable.ComponentAttachment, RDFAttributeNameTable.resource, RDFConstants.RdfComponentConceptRole);

                        this.EndComponentSpecification();
                        this.EndComponent();

                    }                    

                    // Terza            : Attribute 
                    var attributes = dataStructuresBean.AttributeList;
                    if (!(attributes == null))
                    {
                        foreach (var item in attributes.Attributes)
                        {
                            this.StartComponent();
                            this.StartComponentSpecification();

                            //<qb:componentProperty/>
                            this.RDFWriteGeneralTag(this.Namespaces.QB, RDFElementNameTable.componentProperty, RDFAttributeNameTable.resource, RDFConstants.RdfProperty + item.Id);

                            //<qb:Attribute>
                            this.StartComponentAttribute();
                            //<rdf:Description
                            this.StartSeries(item.Id, RDFConstants.RdfProperty);

                            //<rdf:type rdf:resource="http://purl.org/linked-data/cube#AttributeProperty"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentAttributeProperty);

                            //<rdf:type rdf:resource="http://purl.org/linked-data/cube#CodedProperty"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentCodedProperty);

                            //<rdf:type rdf:resource="http://www.w3.org/1999/02/22-rdf-syntax-ns#Property"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentProperty);

                            //<qb:component
                            this.StartComponentConcept();

                            ICrossReference Ref = item.ConceptRef;
                            foreach (ICrossReference Sec in Ref.ReferencedFrom.CrossReferences)
                            {
                                this.StartSubDesc(Sec.MaintainableReference.Version + "/" + Sec.MaintainableReference.MaintainableId + "/" + item.Id, RDFConstants.Rdfconcept);
                            }
                            //<rdf:resource="http://purl.org/linked-data/sdmx#ConceptRole"/>
                            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfComponentConceptRole);

                            this.EndSubDesc();

                            this.EndComponentConcept();

                            foreach (ICrossReference Sec in Ref.ReferencedFrom.CrossReferences)
                            {
                                foreach (var Terc in Sec.ReferencedFrom.Composites)
                                {
                                    foreach (var Qua in Terc.CrossReferences)
                                    {
                                        foreach (ICodelistObject Codelists in dataStructure.Codelists)
                                        {
                                            if (Codelists.AsReference.MaintainableReference.MaintainableId == Qua.MaintainableReference.MaintainableId)
                                            {
                                                foreach (var Descs in Codelists.Names)
                                                {
                                                    this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, Descs.Locale, Descs.Value);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            this.EndSeries();
                            this.EndComponentAttribute();

                            //<qb:componentAttachment
                            this.RDFWriteGeneralTag(this.Namespaces.QB, RDFElementNameTable.ComponentAttachment, RDFAttributeNameTable.resource, RDFConstants.RdfComponentConceptRole);

                            this.EndComponentSpecification();
                            this.EndComponent();
                        }
                    }
                    this.EndMajorSeries();
                }

                
            }
        }

        public void RDFConceptScheme(ISdmxObjects conceptSchema)
        {
            this.WriteMessageTag();
            //Qui si scrive il conceptScheme
            ISet<IConceptSchemeObject> conceptSchems = conceptSchema.ConceptSchemes;
            if (conceptSchems.Count > 0)
            {
                foreach (IConceptSchemeObject conceptSchemsBean in conceptSchems)
                {
                    this.RDFWriteProvenance(conceptSchemsBean.Id, RDFConstants.Rdfconcept);
                    string URIDesc = conceptSchemsBean.Version + "/" + conceptSchemsBean.Id;
                    this.StartSeries(URIDesc, RDFConstants.Rdfconcept);

                    //Scrive la radice della conceptScheme 
                    //<rdf:type rdf:resource="http://www.w3.org/2004/02/skos/core#ConceptScheme"/>
                    this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfConceptScheme);
                    //
                    //Version
                    //<owl:versionInfo
                    this.RDFWriteElementWithStringTag(this.Namespaces.Owl, RDFElementNameTable.versionInfo, conceptSchemsBean.Version);
                    //
                    //Nome conceptScheme
                    //<skos:notation
                    this.RDFWriteElementWithStringTag(this.Namespaces.Skos, RDFElementNameTable.notation, conceptSchemsBean.Id);
                    //
                    var lang = conceptSchemsBean.Names;
                    //Scrive le descrizione                    
                    //<skos:prefLabel
                    foreach (var item in lang)
                    {
                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                    }
                    //
                    //Scrive itens                    
                    IList<IConceptObject> concepts = conceptSchemsBean.Items;
                    if (concepts.Count > 0)
                    {
                        foreach (IConceptObject conceptBean in concepts)
                        {
                            //<skos:hasTopConcept>
                            this.StartKeySeries();
                            string valuehasTopConcept = RDFConstants.Rdfconcept + URIDesc + "/" + conceptBean.Id;
                            //<rdf:Description
                            this.StartSubSeries(valuehasTopConcept);
                            //Description dati comune
                            this.RDFWritehasTopConceptElements(URIDesc, conceptBean.Id, RDFConstants.Rdfconcept, RDFConstants.Rdfconcept, false, null);
                            var names = conceptBean.Names;
                            foreach (var item in names)
                            {
                                this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                            }
                            //</rdf:Description
                            this.EndSubSeries();
                            //</skos:hasTopConcept>
                            this.EndKeySeries();
                        }
                    }
                    this.EndSeries();
                }
            }
        }

        public void RDFCodelist(ISdmxObjects codelist)
        {
            this.WriteMessageTag();            
            //Qui si scrive il codelist
            ISet<ICodelistObject> codelists = codelist.Codelists;
            if (codelists.Count > 0)
            {
                foreach (ICodelistObject codelistBean in codelists)
                {
                    this.RDFWriteProvenance(codelistBean.Id, RDFConstants.RdfCode);
                    string URIDesc = codelistBean.Version + "/" + codelistBean.Id;
                    this.StartSeries(URIDesc, RDFConstants.RdfDati);

                    //Scrive la radice della CodeList 
                    //<rdf:type rdf:resource="http://purl.org/linked-data/sdmx#CodeList"/>
                    this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfCodelist);

                    //<rdf:type rdf:resource="http://www.w3.org/2004/02/skos/core#ConceptScheme"/>
                    this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfConceptScheme);

                    //Scrive seeAlso
                    string seeAlso = codelistBean.Version + "/" + codelistBean.Id;
                    this.StartSeeAlso();

                    //seeAlso Description
                    this.StartSubSeries(RDFConstants.Rdfclass + seeAlso);
                    this.RDFWriteSeeAlsoElements(seeAlso);
                    //<skos:prefLabel....                    
                    var lang = codelistBean.Names;
                    foreach (var item in lang)
                    {
                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                    }
                    //</skos:prefLabel>....                    
                    this.EndSubSeries();
                    //seeAlso Description
                    this.EndSeeAlso();
                    //Finisce seeAlso
                    //
                    //Version
                    //<owl:versionInfo
                    this.RDFWriteElementWithStringTag(this.Namespaces.Owl, RDFElementNameTable.versionInfo, codelistBean.Version);
                    //
                    //Nome Codelist
                    //<skos:notation
                    this.RDFWriteElementWithStringTag(this.Namespaces.Skos, RDFElementNameTable.notation, codelistBean.Id);
                    //
                    //Scrive le descrizione
                    //<skos:prefLabel
                    foreach (var item in lang)
                    {
                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                    }
                    //
                    //Scrive itens                    
                    IList<ICode> codes = codelistBean.Items;
                    if (codes.Count > 0)
                    {
                        foreach (ICode codeBean in codes)
                        {
                            //<skos:hasTopConcept>
                            this.StartKeySeries();
                            string valuehasTopConcept = RDFConstants.RdfDati + URIDesc + "/" + codeBean.Id;

                            string ParentCodeString;
                            if (!(codeBean.ParentCode == null))
                            {
                                ParentCodeString = RDFConstants.RdfDati + URIDesc + "/" + codeBean.ParentCode;
                            }
                            else
                            {
                                ParentCodeString = null;
                            }                            
                            //<rdf:Description
                            this.StartSubSeries(valuehasTopConcept);
                            //Description dati comune
                            this.RDFWritehasTopConceptElements(URIDesc, codeBean.Id, RDFConstants.RdfDati, RDFConstants.RdfDati, true, ParentCodeString);
                            var names = codeBean.Names;
                            foreach (var item in names)
                            {
                                this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                            }
                            //</rdf:Description
                            this.EndSubSeries();
                            //</skos:hasTopConcept>
                            this.EndKeySeries();
                        }
                    }
                    this.EndSeries();
                }
            }
        }

        public void RDFCategorySchemeTree(ISdmxObjects categoryscheme)
        {
            this.WriteMessageTag();

            foreach (ICategorySchemeObject categorySchemesBean in categoryscheme.CategorySchemes)
            {
                //this.RDFWriteProvenance(categorySchemesBean.Id, RDFConstants.RdfStructure);
                this.RDFWriteProvenance(categorySchemesBean.Id, RDFConstants.RdfCatScheme);
                string URIDesc = categorySchemesBean.Version + "/" + categorySchemesBean.Id;
                this.StartSeries(URIDesc, RDFConstants.RdfCatScheme);

                //Scrive la radice delle categoryScheme 
                //<rdf:type rdf:resource="http://purl.org/linked-data/sdmx#CategoryScheme"/>
                this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfCategorySchema);

                //<rdf:type rdf:resource="http://www.w3.org/2004/02/skos/core#ConceptScheme"/>
                this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfConceptScheme);

                //Scrive seeAlso
                string seeAlso = categorySchemesBean.Version + "/" + categorySchemesBean.Id;
                this.StartSeeAlso();

                //seeAlso Description
                this.StartSubSeries(RDFConstants.Rdfclass + seeAlso);
                this.RDFWriteSeeAlsoElements(seeAlso);
                //<skos:prefLabel....                    
                var lang = categorySchemesBean.Names;
                foreach (var item in lang)
                {
                    this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                }
                //</skos:prefLabel>....                    
                this.EndSubSeries();
                //seeAlso Description
                this.EndSeeAlso();
                //Finisce seeAlso
                //
                //Version
                //<owl:versionInfo
                this.RDFWriteElementWithStringTag(this.Namespaces.Owl, RDFElementNameTable.versionInfo, categorySchemesBean.Version);
                //
                //Nome
                //<skos:notation
                this.RDFWriteElementWithStringTag(this.Namespaces.Skos, RDFElementNameTable.notation, categorySchemesBean.Id);
                //
                //AgencyID
                this.RDFWriteElementWithStringTag(this.Namespaces.SdmxConcept, RDFElementNameTable.mAgency, categorySchemesBean.AgencyId);
                //
                //URN
                this.RDFWriteElementWithStringTag(this.Namespaces.DocTerms, RDFElementNameTable.identifier, categorySchemesBean.Urn.AbsoluteUri);
                //               
                foreach (var category in categorySchemesBean.Items)
                {
                    //this.RDFWriteProvenance(category.Id, RDFConstants.RdfStructure);
                    //this.StartSeries(URIDesc + "/" + category.Id, RDFConstants.RdfStructure);
                    this.StartSeries(URIDesc + "/" + category.Id, RDFConstants.RdfCatScheme);
                    //Scrive la lista dei dataflow
                    this.RDFCategorySchemeDataFlowList(categoryscheme, category.Id);
                            
                    //<skos:inScheme
                    this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.inScheme);
                    //this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfStructure + URIDesc);
                    this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfCatScheme + URIDesc);
                    this.RDFWriteEndElement();

                    var label = category.Names;
                         
                    foreach (var item in label)
                    {
                        this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                    }

                    this.RDFWriteElementWithStringTag(this.Namespaces.Skos, RDFElementNameTable.notation, category.Id);
                    this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfConceptURI);

                    this.RDFDataFlowItemList(categoryscheme, category.Id, URIDesc, category.Id);
                    this.EndSeries();

                    foreach (var categoryChild in category.Items)
                    {
                        //this.RDFWriteProvenance(category.Id + "." + categoryChild.Id, RDFConstants.RdfStructure);
                        //this.StartSeries(URIDesc + "/" + category.Id + "." + categoryChild.Id, RDFConstants.RdfStructure);
                        this.StartSeries(URIDesc + "/" + category.Id + "." + categoryChild.Id, RDFConstants.RdfCatScheme);
                        //Scrive la lista dei dataflow                        
                        this.RDFCategorySchemeDataFlowList(categoryscheme, categoryChild.Id);

                        //<skos:inScheme
                        this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.inScheme);
                        //this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfStructure + URIDesc);
                        this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfCatScheme + URIDesc);
                        this.RDFWriteEndElement();

                        var label2 = categoryChild.Names;
                            
                        foreach (var item in label2)
                        {
                            this.RDFWriteDescriptionTag(this.Namespaces.Skos, RDFElementNameTable.prefLabel, RDFAttributeNameTable.lang, item.Locale, item.Value);
                        }

                        this.RDFWriteElementWithStringTag(this.Namespaces.Skos, RDFElementNameTable.notation, categoryChild.Id);
                        this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfConceptURI);

                        this.RDFDataFlowItemList(categoryscheme, categoryChild.Id, URIDesc, category.Id + "." + categoryChild.Id);
                        this.EndSeries();
                    }
                }                                                                               
                this.EndSeries();
            }            
        }

        public void RDFCategorySchemeDataFlowList(ISdmxObjects categoryscheme,string category)
        {
            foreach (ICategorisationObject Categorisations in categoryscheme.Categorisations)
            {
                foreach (IDataflowObject dataflowBean in categoryscheme.Dataflows)
                {
                    if (category == Categorisations.CategoryReference.FullId && Categorisations.StructureReference.MaintainableId == dataflowBean.Id)
                    {
                        this.RDFWriteGeneralTag(this.Namespaces.Skos, this.Namespaces.RDF, RDFElementNameTable.generalizes, RDFAttributeNameTable.resource, RDFConstants.RdfDataset + dataflowBean.DataStructureRef.Version + "/" + dataflowBean.DataStructureRef.MaintainableId);
                    }
                }
            }
        }

        public void RDFDataFlowItemList(ISdmxObjects categoryscheme, string category, string URIDesc,string categoryString)
        {
            foreach (ICategorisationObject categorisation in categoryscheme.Categorisations)
            {
                foreach (IDataflowObject dataflowBean in categoryscheme.Dataflows)
                {
                    if (category == categorisation.CategoryReference.ChildReference.Id && categorisation.StructureReference.MaintainableId == dataflowBean.Id)
                    {
                        this.StartSeries(URIDesc + "/" + dataflowBean.DataStructureRef.MaintainableId, RDFConstants.RdfDataset);                        
                        //<xkos:specializes
                        this.RDFWriteGeneralTag(this.Namespaces.Xkos, this.Namespaces.RDF, RDFElementNameTable.specializes, RDFAttributeNameTable.resource, RDFConstants.RdfDataset + URIDesc + "/" + categoryString);                        
                        //<skos:inScheme
                        this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.inScheme);
                        this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfCatScheme + URIDesc);
                        //this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, RDFConstants.RdfStructure + URIDesc);
                        this.RDFWriteEndElement();
                        //<skos:notation
                        this.RDFWriteElementWithStringTag(this.Namespaces.Skos, RDFElementNameTable.notation, dataflowBean.DataStructureRef.MaintainableId);
                        //
                        //AgencyID
                        this.RDFWriteElementWithStringTag(this.Namespaces.SdmxConcept, RDFElementNameTable.mAgency, dataflowBean.AgencyId);
                        //
                        //Version
                        //<owl:versionInfo
                        this.RDFWriteElementWithStringTag(this.Namespaces.Owl, RDFElementNameTable.versionInfo, dataflowBean.DataStructureRef.Version);
                        //
                        this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfqbDataSet);
                        this.EndSeries();
                    }
                }
            }
        }

        public void RDFWriteGeneralTag(NamespacePrefixPair NS, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Constant)
        {
            this.RDFWriteStartElement(NS, Element);
            this.RDFWriteAttributeString(NS, Attribute, Constant);
            this.RDFWriteEndElement();
        }

        public void RDFWriteGeneralTag(NamespacePrefixPair NS, NamespacePrefixPair NSAtt, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Constant)
        {
            this.RDFWriteStartElement(NS, Element);
            this.RDFWriteAttributeString(NSAtt, Attribute, Constant);
            this.RDFWriteEndElement();
        }

        public void RDFWriteDescriptionTag(NamespacePrefixPair NS, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Locale, string Value)
        {
            this.RDFWriteStartElement(NS, Element);
            this.RDFWriteAttributeString(this.Namespaces.XML, Attribute, Locale);
            this.RDFWriteString(Value);
            this.RDFWriteEndElement();
        }

        public void RDFWriteElementWithStringTag(NamespacePrefixPair NS, RDFElementNameTable Element, string Text)
        {
            this.RDFWriteStartElement(NS, Element);
            this.RDFWriteString(Text);
            this.RDFWriteEndElement();
        }

        public void RDFWriteElementWithStringTag(NamespacePrefixPair NS, NamespacePrefixPair NSAtt, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Text, string Constant)
        {
            this.RDFWriteStartElement(NS, Element);
            this.RDFWriteAttributeString(NSAtt, Attribute, Constant);
            this.RDFWriteString(Text);
            this.RDFWriteEndElement();
        }

        public void RDFWriteElementAttributeWithStringTag(NamespacePrefixPair ElementNS, NamespacePrefixPair AttributeNS, RDFElementNameTable Element, RDFAttributeNameTable Attribute, string Constant, string Value)
        {
            this.RDFWriteStartElement(ElementNS, Element);
            this.RDFWriteAttributeString(AttributeNS, Attribute, Constant);
            this.RDFWriteString(Value);
            this.RDFWriteEndElement();
        }

        public void RDFWritehasTopConceptElements(string URIDesc, string Name, string topConceptOf, string inScheme, bool element, string ParentCodeString)
        {
            //<rdf:type rdf:resource="http://purl.org/linked-data/sdmx#Concept"/>
            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.Rdfsdmx + "Concept");

            //<rdf:type rdf:resource="http://www.w3.org/2004/02/skos/core#Concept"/>
            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfSkos + "Concept");

            //Finora solo Codelist scrive qualcosa.
            if (element)
            {
                //<rdf:type rdf:resource="http://dati.istat.it/class/1.0/CL_AMBIENTE_INDICATOR"/>
                this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.Rdfclass + URIDesc);

            }
            //<skos:topConceptOf            
            this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.topConceptOf);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, topConceptOf + URIDesc);
            this.RDFWriteEndElement();

            //<skos:inScheme
            this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.inScheme);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, inScheme + URIDesc);
            this.RDFWriteEndElement();

            //If a codelist have higher levels, writes here
            if (!(ParentCodeString == null))
            {
                this.RDFWriteCodelistChild(inScheme + URIDesc + "/" + Name, ParentCodeString );    
            }            

            //skos:notation>
            this.RDFWriteStartElement(this.Namespaces.Skos, RDFElementNameTable.notation);
            this.RDFWriteString(Name);
            this.RDFWriteEndElement();
        }

        public void RDFWriteSeeAlsoElements(string seeAlso)
        {
            //<rdf:type rdf:resource="http://www.w3.org/2000/01/rdf-schema#Class"/>
            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfS + "Class");

            //<rdf:type rdf:resource="http://www.w3.org/2002/07/owl#Class"/>
            this.RDFWriteGeneralTag(this.Namespaces.RDF, RDFElementNameTable.type, RDFAttributeNameTable.resource, RDFConstants.RdfOwl + "Class");

            //<rdfs:subClassOf rdf:resource="http://www.w3.org/2004/02/skos/core#Concept"/>
            this.RDFWriteGeneralTag(this.Namespaces.RDFs, RDFElementNameTable.subClassOf, RDFAttributeNameTable.resource, RDFConstants.RdfSkos + "Concept");

            //<rdfs:seeAlso rdf:resource="http://dati.istat.it/code/1.0/CL_AMBIENTE_INDICATOR"/>
            this.RDFWriteGeneralTag(this.Namespaces.RDFs, RDFElementNameTable.seeAlso, RDFAttributeNameTable.resource, RDFConstants.RdfDati + seeAlso);

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
                

            this.StartSeries(provenanceID, RDFConstants.RdfProvenance);
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
            this.EndSeries();


            
            provenanceID = null;
            label = null;

        
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
                this.RDFWriteStartDocument();
                this.Wrapped = true;

                // <Data
                this.RDFWriteStartElement(this.Namespaces.RDF, element);            
                
                this._dataSetNS = this.Namespaces.RDF;

                this.RDFWriteNamespaceDecl(this._dataSetNS);

                // xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                this.RDFWriteNamespaceDecl(this.Namespaces.Xsi);

                // rdf:Property
                this.RDFWriteNamespaceDecl(this.Namespaces.Property);
                this.RDFWriteNamespaceDecl(this.Namespaces.QB);
                this.RDFWriteNamespaceDecl(this.Namespaces.Skos);
                this.RDFWriteNamespaceDecl(this.Namespaces.Xkos);
                this.RDFWriteNamespaceDecl(this.Namespaces.Owl);
                this.RDFWriteNamespaceDecl(this.Namespaces.RDFs);
                this.RDFWriteNamespaceDecl(this.Namespaces.DocTerms);
                this.RDFWriteNamespaceDecl(this.Namespaces.SdmxConcept);
                this.RDFWriteNamespaceDecl(this.Namespaces.Prov);

                // xsi:schemaLocation="http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message SDMXMessage.xsd">
                string schemaLocation = this.Namespaces.SchemaLocation ?? string.Empty;
                string structureSpecific = string.Empty;
                if (!string.IsNullOrWhiteSpace(structureSpecific))
                {
                    schemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} {1}", schemaLocation, structureSpecific);
                }

                if (!string.IsNullOrWhiteSpace(schemaLocation))
                {
                    this.RDFWriteAttributeString(this.Namespaces.Xsi, RDFConstants.SchemaLocation, schemaLocation);
                }
            }
        }

        protected void WriteMessageTag()
        {
            this.RDFWriteMessageTag(this.MessageElement, this.Namespaces);
        }

        protected void RDFWriteCodelistChild(string URIDesc, string ParentCodeString)
        {
            this.StartCodelistChildSeries();
            this.RDFWriteStartElement(this.Namespaces.RDF, RDFElementNameTable.Description);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.about, ParentCodeString);
            this.RDFWriteStartElement(this.Namespaces.Xkos, RDFElementNameTable.hasPart);
            this.RDFWriteAttributeString(this.Namespaces.RDF, RDFAttributeNameTable.resource, URIDesc);
            this.RDFWriteEndElement();
            this.RDFWriteEndElement();
            this.EndCodelistChildSeries();
        }

        #endregion

        #region RDF

        public virtual void RDFWriteStructures(ISdmxObjects beans)
        {
            this.RDFStructure(beans);
        }

        public bool Wrapped
        {
            get
            {
                return this._wrapped;
            }
            set
            {
                this._wrapped = value;
            }
        }

        protected RDFNamespaces Namespaces
        {
            get
            {
                return this._namespaces;
            }
        }
        // Principale quello che fa tutta la strutura
        public virtual void RDFStructure(ISdmxObjects beans)
        {
            if (beans.DataStructures.Count > 0)
            {
                this.RDFDataStructure(beans);
            }

            if (beans.ConceptSchemes.Count > 0)
            {
                this.RDFConceptScheme(beans);
            }

            if (beans.Codelists.Count > 0)
            {
                this.RDFCodelist(beans);
            }

            if (beans.CategorySchemes.Count > 0)
            {
                this.RDFCategorySchemeTree(beans);
            }

        }

        public void RDFWriteAttributeString(NamespacePrefixPair namespacePrefixPair, RDFAttributeNameTable name, string value)
        {
            this._writer.WriteAttributeString(namespacePrefixPair.Prefix, RDFNameTableCache.GetAttributeName(name), namespacePrefixPair.NS, value);
        }

        public void RDFWriteAttributeString(NamespacePrefixPair namespacePrefixPair, string name, string value)
        {
            this._writer.WriteAttributeString(namespacePrefixPair.Prefix, name, namespacePrefixPair.NS, value);
        }

        public void RDFWriteAttributeString(string prefix, string name, string value)
        {
            this._writer.WriteAttributeString(prefix, name, null, value);
        }

        public void RDFWriteAttributeString(string name, string value)
        {
            this._writer.WriteAttributeString(name, value);
        }

        public void RDFWriteString(string ObsValue)
        {
            this._writer.WriteString(ObsValue);
        }

        public void RDFWriteStartElement(NamespacePrefixPair ns, RDFElementNameTable element)
        {
            this._writer.WriteStartElement(ns.Prefix, RDFNameTableCache.GetElementName(element), ns.NS);
        }

        public void RDFWriteStartElement(NamespacePrefixPair ns, string element)
        {
            this._writer.WriteStartElement(ns.Prefix, element, ns.NS);
        }

        public void RDFWriteStartElement(string prefix, RDFElementNameTable element)
        {
            this._writer.WriteStartElement(prefix, RDFNameTableCache.GetElementName(element), null);
        }

        public void RDFWriteStartElement(string element)
        {
            this._writer.WriteStartElement(element);
        }

        public void RDFWriteElement(NamespacePrefixPair namespacePrefix, RDFElementNameTable element, string value)
        {
            this._writer.WriteElementString(namespacePrefix.Prefix, RDFNameTableCache.GetElementName(element), null, value);
        }

        public void RDFWriteEndDocument()
        {
            if (this.Wrapped)
            {
                return;
            }

            this._writer.WriteEndDocument();
            this._writer.Flush();
        }

        public void RDFWriteStartDocument()
        {
            if (this.Wrapped)
            {
                return;
            }
            this._writer.WriteStartDocument();
        }

        public void RDFWriteEndElement()
        {
            this._writer.WriteEndElement();
        }        
        
        #endregion


    }
}