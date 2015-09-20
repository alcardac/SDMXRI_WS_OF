// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Writer.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The base class for all SDMX-ML writers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Writer.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;
    

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using RDFProvider.Constants;
    using RDFProvider.Retriever.Model;

    public abstract class Writer 
    {
       #region Fields

        private IDataStructureObject _keyFamilyBean;

        private bool _startedDataSet;        

        private string _defaultObs = RDFConstants.NaN;

        private readonly bool _wrapped;

        private readonly XmlWriter _writer;

        #endregion

        #region Constructors and Destructors

        protected Writer()
        {
        }
        protected Writer(XmlWriter writer, RDFNamespaces namespaces)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this._writer = writer;                        
            this._wrapped = writer.WriteState != WriteState.Start;
        }

        #endregion

        #region Properties

        protected bool StartedDataSet
        {
            get
            {
                return this._startedDataSet;
            }

            set
            {
                this._startedDataSet = value;
            }
        }

        protected string DefaultObs
        {
            get
            {
                return this._defaultObs;
            }
        }

        protected IDataStructureObject KeyFamily
        {
            get
            {
                return this._keyFamilyBean;
            }

            set
            {
                this._keyFamilyBean = value;
            }
        }

        protected XmlWriter SdmxMLWriter
        {
            get
            {
                return this._writer;
            }
        }

        protected bool Wrapped
        {
            get
            {
                return this._wrapped;
            }
        }

        #endregion

        #region Methods

        protected void StartDataset(IDataflowObject dataflow, IDataStructureObject dataStructure, IDatasetHeader header,DataRetrievalInfoSeries info)
        {
                if (dataStructure == null)
                {
                    throw new ArgumentNullException("dataStructure");
                }

                this._keyFamilyBean = dataStructure;
                this.WriteFormatDataSet(header, info);
                this._startedDataSet = true;
            }

        protected abstract void WriteFormatDataSet(IDatasetHeader header, DataRetrievalInfoSeries info);

        protected void WriteNamespaceDecl(NamespacePrefixPair ns)
        {
            this.WriteAttributeString(RDFConstants.Xmlns, ns.Prefix, ns.NS);
        }

        protected void WriteAttributeString(NamespacePrefixPair namespacePrefixPair, string name, string value)
        {
            this._writer.WriteAttributeString(namespacePrefixPair.Prefix, name, namespacePrefixPair.NS, value);
        }

        protected void WriteAttributeString(NamespacePrefixPair namespacePrefixPair, RDFAttributeNameTable name, string value)
        {
            this.WriteAttributeString(namespacePrefixPair, RDFNameTableCache.GetAttributeName(name), value);
        }

        protected void WriteAttributeString(string prefix, string name, Uri value)
        {
            this._writer.WriteAttributeString(prefix, name, null, value.ToString());
        }        

        protected void WriteAttributeString(string prefix, string name, string value)
        {
            this._writer.WriteAttributeString(prefix, name, null, value);
        }        

        protected void WriteStartElement(NamespacePrefixPair ns, RDFElementNameTable element)
        {
            this.WriteStartElement(ns, RDFNameTableCache.GetElementName(element));
        }

        protected void WriteStartElement(NamespacePrefixPair ns, string element)
        {
            this._writer.WriteStartElement(ns.Prefix, element, ns.NS);
        }

        protected void WriteStartElement(string element, string ns)
        {
            this._writer.WriteStartElement(element, ns);
        }

        protected void WriteElement(NamespacePrefixPair namespacePrefix, string element, bool value)
        {
            this._writer.WriteElementString(
                namespacePrefix.Prefix, element, namespacePrefix.NS, XmlConvert.ToString(value));
        }

        protected void WriteString(string ObsValue)
        {
            this._writer.WriteString(ObsValue);
        }

        protected virtual void EndDataSet()
        {
            if (this._startedDataSet)
            {
                this.WriteEndElement();
                this._startedDataSet = false;
            }
        }

        protected void CloseMessageTag()
        {
            this.WriteEndElement();
            this.WriteEndDocument();
        }

        protected void WriteEndDocument()
        {
            if (this.Wrapped)
            {
                return;
            }
            this._writer.WriteEndDocument();
            this._writer.Flush();
        }

        protected void WriteEndElement()
        {
            this._writer.WriteEndElement();
        }

        #endregion
    }
}