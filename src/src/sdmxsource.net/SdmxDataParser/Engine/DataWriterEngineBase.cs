// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataWriterEngineBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using System;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     The data writer engine base.
    /// </summary>
    public abstract class DataWriterEngineBase : DataStreamWriterBase, IDataWriterEngine
    {
        #region Fields

        /// <summary>
        ///     The is cross sectional.
        /// </summary>
        private bool _isCrossSectional;

        /// <summary>
        ///     A value indicating whether the <see cref="StartGroup" /> has been called
        /// </summary>
        private bool _startedGroup;

        /// <summary>
        ///     The is flat
        /// </summary>
        private bool _isFlat;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The _footer
        /// </summary>
        private IFooterMessage[] _footer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterEngineBase"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        protected DataWriterEngineBase(XmlWriter writer, SdmxSchema schema)
            : base(writer, schema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterEngineBase"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        protected DataWriterEngineBase(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema)
            : base(writer, namespaces, schema)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether is cross sectional.
        /// </summary>
        protected bool IsCrossSectional
        {
            get
            {
                return this._isCrossSectional;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether is flat.
        /// </summary>
        protected bool IsFlat
        {
            get
            {
                return this._isFlat;
            }
        }

        /// <summary>
        /// Gets the footer message.
        /// </summary>
        /// <value>
        /// The footer message.
        /// </value>
        protected IFooterMessage[] FooterMessage
        {
            get
            {
                return this._footer;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Completes the XML document, closes off all the tags, closes any resources.
        ///     <b>NOTE</b> It is very important to close off a completed DataWriterEngine, as this ensures any output is written to the given location, and any resources are closed.  If this call
        ///     is not made, the output document may be incomplete.
        ///     Validates the following
        ///     <ul>
        ///         <li>The last series key or group key has been completed</li>
        ///     </ul>
        /// </summary>
        /// <param name="footer">
        /// The footer.
        /// </param>
        public void Close(params IFooterMessage[] footer)
        {
            this._footer = footer;
            this.Dispose();
        }

        /// <summary>
        /// Starts a dataset with the data conforming to the DSD
        /// </summary>
        /// <param name="dataflow">Optional. The dataflow can be provided to give extra information about the dataset.</param>
        /// <param name="dsd">The <see cref="IDataStructureObject" /> for which the dataset will be created</param>
        /// <param name="header">The <see cref="IDatasetHeader" /> of the dataset</param>
        /// <param name="annotations">Any additional annotations that are attached to the dataset, can be null if no annotations exist</param>
        /// <exception cref="System.ArgumentNullException">if the <paramref name="dsd" /> is null</exception>
        public override void StartDataset(IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, params IAnnotation[] annotations)
        {
            this.CheckDisposed();

            base.StartDataset(dataflow, dsd, header);
            if (dsd == null)
            {
                throw new ArgumentNullException("dsd");
            }

            switch (this.TargetSchema.EnumType)
            {
                case SdmxSchemaEnumType.VersionOne:
                case SdmxSchemaEnumType.VersionTwo:
                    WriterHelper.ValidateErrors(Validator.ValidateForCompact(dsd));
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    this._isCrossSectional =
                        DimensionObject.TimeDimensionFixedId.Equals(this.DimensionAtObservation);
                    this._isFlat = this.DimensionAtObservation.Equals(Api.Constants.DimensionAtObservation.GetFromEnum(DimensionAtObservationEnumType.All).Value);

                    if (!this._isFlat)
                    {
                        IDimension dimension = dsd.GetDimension(this.DimensionAtObservation);
                        if (dimension == null)
                        {
                            throw new ArgumentNullException(
                                "Can not start dataset.  The dimension at observation has been set to '" +
                                this.DimensionAtObservation + "', but this dimension does not exist in the datastructure '" +
                                dsd.Urn + "'");
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Starts a group with the given id, the subsequent calls to <c>writeGroupKeyValue</c> will write the id/values to this group.  After
        /// the group key is complete <c>writeAttributeValue</c> may be called to add attributes to this group.
        /// <p /><b>Example Usage</b>
        /// A group 'demo' is made up of 3 concepts (Country/Sex/Status), and has an attribute (Collection).
        /// <code>
        /// DataWriterEngine dre = //Create instance
        /// dre.StartGroup("demo");
        /// dre.WriteGroupKeyValue("Country", "FR");
        /// dre.WriteGroupKeyValue("Sex", "M");
        /// dre.WriteGroupKeyValue("Status", "U");
        /// dre.WriteAttributeValue("Collection", "A");
        /// </code>
        /// Any subsequent calls, for example to start a series, or to start a new group, will close off this exiting group.
        /// </summary>
        /// <param name="groupId">The Group ID</param>
        /// <param name="annotations">Annotations any additional annotations that are attached to the group, can be null if no annotations exist</param>
        public virtual void StartGroup(string groupId, params IAnnotation[] annotations)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not StartGroup, no call has been made to StartDataset");
            this._startedGroup = true;
        }

        /// <summary>
        /// Starts a new series, closes off any existing series keys or attribute/observations.
        /// </summary>
        /// <param name="annotations">Any additional annotations that are attached to the series, can be null if no annotations exist</param>
        public virtual void StartSeries(params IAnnotation[] annotations)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not StartSeries, no call has been made to StartDataset");
        }

        /// <summary>
        /// Writes an attribute value for the given id.
        ///     <p/>
        ///     If this method is called immediately after a <c>writeSeriesKeyValue</c> method call then it will write
        ///     the attribute at the series level.  If it is called after a <c>writeGroupKeyValue</c> it will write the attribute against the group.
        ///     <p/>
        ///     If this method is called immediately after a <c>writeObservation</c> method call then it will write the attribute at the observation level.
        /// </summary>
        /// <param name="id">
        /// the id of the given value for example 'OBS_STATUS'
        /// </param>
        /// <param name="valueRen">
        /// The attribute value.
        /// </param>
        public virtual void WriteAttributeValue(string id, string valueRen)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not WriteAttributeValue, no call has been made to StartDataset");
        }

        /// <summary>
        /// Writes a group key value, for example 'Country' is 'France'.  A group may contain multiple id/value pairs in the key, so this method may be called consecutively with an id / value for each key item.
        ///     <p/>
        ///     If this method is called consecutively multiple times and a duplicate id is passed in, then an exception will be thrown, as a group can only declare one value for a given id.
        ///     <p/>
        ///     The <c>startGroup</c> method must be called before calling this method to add the first id/value, as the WriterEngine needs to know what group to assign the id/values to.
        /// </summary>
        /// <param name="id">
        /// the id of the concept or dimension
        /// </param>
        /// <param name="valueRen">
        /// The attribute value.
        /// </param>
        public virtual void WriteGroupKeyValue(string id, string valueRen)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not WriteGroupKeyValue, no call has been made to StartDataset");
            if (!this._startedGroup)
            {
                throw new InvalidOperationException("Can not WriteGroupKeyValue, not in a group");
            }
        }

        /// <summary>
        /// Writes an observation, the observation concept is assumed to be that which has been defined to be at the observation level (as declared in the start dataset method DatasetHeaderObject).
        /// </summary>
        /// <param name="obsConceptValue">May be the observation time, or the cross section value </param>
        /// <param name="obsValue">The observation value - can be numerical</param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist</param>
        public virtual void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not WriteObservation, no call has been made to StartDataset");
        }

        /// <summary>
        /// Writes an Observation value against the current series.
        /// <p />
        /// The current series is determined by the latest writeKeyValue call,
        /// If this is a cross sectional dataset, then the <paramref name="obsConceptValue" /> is expected to be the cross sectional concept value - for example if this is cross sectional on Country the id may be "FR"
        /// If this is a time series dataset then the <paramref name="obsConceptValue" /> is expected to be the observation time - for example 2006-12-12
        /// <p />
        /// </summary>
        /// <param name="observationConceptId">the concept id for the observation, for example 'COUNTRY'.  If this is a Time Series, then the id will be DimensionBean.TIME_DIMENSION_FIXED_ID.</param>
        /// <param name="obsConceptValue">may be the observation time, or the cross section value</param>
        /// <param name="obsValue">the observation value - can be numerical</param>
        /// <param name="annotations">Any additional annotations that are attached to the observation, can be null if no annotations exist</param>
        public virtual void WriteObservation(string observationConceptId, string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not WriteObservation, no call has been made to StartDataset");
        }

        /// <summary>
        /// Writes an Observation value against the current series
        ///     <p/>
        ///     The date is formatted as a string, the format rules are determined by the TIME_FORMAT argument
        ///     <p/>
        /// </summary>
        /// <param name="obsTime">
        /// the time of the observation
        /// </param>
        /// <param name="obsValue">
        /// the observation value - can be numerical
        /// </param>
        /// <param name="sdmxSwTimeFormat">
        /// the time format to format the <paramref name="obsTime"/> in when converting to a string
        /// </param>
        /// <param name="annotations">
        /// The annotations.
        /// </param>
        public void WriteObservation(DateTime obsTime, string obsValue, TimeFormat sdmxSwTimeFormat, params IAnnotation[] annotations)
        {
            this.WriteObservation(DateUtil.FormatDate(obsTime, sdmxSwTimeFormat.EnumType), obsValue, annotations);
        }

        /// <summary>
        /// Writes a series key value.  This will have the effect of closing off any observation, or attribute values if they are any present
        ///     <p/>
        ///     If this method is called after calling writeAttribute or writeObservation, then the engine will start a new series.
        /// </summary>
        /// <param name="id">
        /// the id of the value for example 'Country'
        /// </param>
        /// <param name="keyValue">
        /// The key value.
        /// </param>
        public virtual void WriteSeriesKeyValue(string id, string keyValue)
        {
            this.CheckDisposed();
            this.CheckDataSet("Can not WriteSeriesKeyValue, no call has been made to StartDataset");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">This writer is disposed.</exception>
        protected void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("This writer is disposed.");
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposable)
        {
            if (!this._disposed)
            {
                if (disposable)
                {
                }
                
                this._disposed = true;
            }
        }

        /// <summary>
        /// The write footer.
        /// </summary>
        /// <param name="footer">The footer.</param>
        protected void WriteFooter(params IFooterMessage[] footer)
        {
            if (footer != null && footer.Length > 0 && this.IsTwoPointOne)
            {
                this.WriteStartElement(
                    new NamespacePrefixPair("http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", "footer"),
                           ElementNameTable.Footer);

                foreach (IFooterMessage currentFooter in footer)
                {
                    this.WriteStartElement(
                        new NamespacePrefixPair("http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", "footer"),
                          ElementNameTable.Message);

                    this.WriteAttributeString("code", currentFooter.Code);

                    this.WriteAttributeString("severity", currentFooter.Severity.ToString());

                    foreach (ITextTypeWrapper ttw in currentFooter.FooterText)
                    {
                        this.WriteTextType(this.Namespaces.Common, ttw, ElementNameTable.AnnotationText);
                    }

                    this.WriteEndElement(); // End Message
                }

                this.WriteEndElement(); // End footer
            }
        }

        #endregion
    }
}