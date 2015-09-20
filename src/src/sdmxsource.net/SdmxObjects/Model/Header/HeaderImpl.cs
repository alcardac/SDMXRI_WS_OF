// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The header impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Random;

    using Header = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.Header;
    using PartyType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.PartyType;
    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType;

    /// <summary>
    ///   The header impl.
    /// </summary>
    [Serializable]
    public class HeaderImpl : IHeader
    {
        #region Fields

        /// <summary>
        ///   The _additional attribtues.
        /// </summary>
        private readonly IDictionary<string, string> _additionalAttributes;

        /// <summary>
        ///   The _data provider reference.
        /// </summary>
        private IStructureReference _dataProviderReference;

        /// <summary>
        ///   The _dataset action.
        /// </summary>
        private DatasetAction _datasetAction;

        /// <summary>
        ///   The _dataset id.
        /// </summary>
        private string _datasetId;

        /// <summary>
        ///   The _embargo date.
        /// </summary>
        private readonly DateTime? _embargoDate;

        /// <summary>
        ///   The _extracted.
        /// </summary>
        private readonly DateTime? _extracted;

        /// <summary>
        ///   The _name.
        /// </summary>
        private readonly IList<ITextTypeWrapper> _name;

        /// <summary>
        ///   The _receiver.
        /// </summary>
        private readonly IList<IParty> _receiver;

        /// <summary>
        ///   The _reporting begin.
        /// </summary>
        private DateTime? _reportingBegin;

        /// <summary>
        ///   The _reporting end.
        /// </summary>
        private DateTime? _reportingEnd;

        /// <summary>
        ///   The _sender.
        /// </summary>
        private IParty _sender;

        /// <summary>
        ///   The _source.
        /// </summary>
        private readonly IList<ITextTypeWrapper> _source;

        /// <summary>
        ///   The _structure references.
        /// </summary>
        private readonly IList<IDatasetStructureReference> _structureReferences;

        /// <summary>
        ///   The _test.
        /// </summary>
        private bool _test;

        /// <summary>
        ///   The _id.
        /// </summary>
        private string _id;

        /// <summary>
        ///   The _prepared.
        /// </summary>
        private DateTime? _prepared;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderImpl"/> class.
        /// </summary>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="senderId">
        /// The sender id. 
        /// </param>
        public HeaderImpl(string id, string senderId)
        {
            this._additionalAttributes = new Dictionary<string, string>();
            this._name = new List<ITextTypeWrapper>();
            this._source = new List<ITextTypeWrapper>();
            this._receiver = new List<IParty>();
            this._structureReferences = new List<IDatasetStructureReference>();

            if (id == null)
            {
			   throw new ArgumentException("The ID may not be null");
		    }

		    if (Char.IsDigit(id, 0))
            {
			   throw new ArgumentException("An ID may not start with a digit!");
		    }

            this._id = id;
            this._sender = new PartyCore(null, senderId, null, null);
            this._prepared = DateTime.Now;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ATTRIBUTES               ////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderImpl"/> class. 
        ///   Minimal Constructor
        /// </summary>
        /// <param name="id">Id string
        /// </param>
        /// <param name="prepared">DateTime parameter
        /// </param>
        /// <param name="receiver">Reciever list
        /// </param>
        /// <param name="sender">Sender list
        /// </param>
        public HeaderImpl(string id, DateTime prepared, DateTime reportingBegin, DateTime reportingEnd, IList<IParty> receiver, IParty sender
            ,bool isTest)
            : this(
                null,
                null,
                null,
               null,
                id,
                null,
                null,
                null,
                prepared,
                reportingBegin,
                reportingEnd,
                null,
                null,
                receiver,
                sender,
                isTest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderImpl"/> class.
        /// </summary>
        /// <param name="additionalAttributes">
        /// The additional attribtues. 
        /// </param>
        /// <param name="structures">
        /// The structures. 
        /// </param>
        /// <param name="dataProviderReference">
        /// The data provider reference. 
        /// </param>
        /// <param name="datasetAction">
        /// The dataset action. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="datasetId">
        /// The dataset id. 
        /// </param>
        /// <param name="embargoDate">
        /// The embargo date. 
        /// </param>
        /// <param name="extracted">
        /// The extracted. 
        /// </param>
        /// <param name="prepared">
        /// The prepared. 
        /// </param>
        /// <param name="reportingBegin">
        /// The reporting begin. 
        /// </param>
        /// <param name="reportingEnd">
        /// The reporting end. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="source">
        /// The source. 
        /// </param>
        /// <param name="receiver">
        /// The receiver. 
        /// </param>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="test">
        /// The test. 
        /// </param>
        public HeaderImpl(
            IDictionary<string, string> additionalAttributes,
            IList<IDatasetStructureReference> structures,
            IStructureReference dataProviderReference,
            DatasetAction datasetAction,
            string id,
            string datasetId,
            DateTime? embargoDate,
            DateTime? extracted,
            DateTime? prepared,
            DateTime? reportingBegin,
            DateTime? reportingEnd,
            IList<ITextTypeWrapper> name,
            IList<ITextTypeWrapper> source,
            IList<IParty> receiver,
            IParty sender,
            bool test)
        {
            this._additionalAttributes = new Dictionary<string, string>();
            this._name = new List<ITextTypeWrapper>();
            this._source = new List<ITextTypeWrapper>();
            this._receiver = new List<IParty>();
            this._structureReferences = new List<IDatasetStructureReference>();
            if (additionalAttributes != null)
            {
                this._additionalAttributes = new Dictionary<string, string>(additionalAttributes);
            }

            if (structures != null)
            {
                this._structureReferences = new List<IDatasetStructureReference>(structures);
            }

            this._dataProviderReference = dataProviderReference;
            this._datasetAction = datasetAction;
            this._id = id;
            this._datasetId = datasetId;
            if (embargoDate != null)
            {
                this._embargoDate = embargoDate.Value;
            }

            if (extracted != null)
            {
                this._extracted = extracted.Value;
            }

            if (prepared != null)
            {
                this._prepared = prepared.Value;
            }

            if (reportingBegin != null)
            {
                this._reportingBegin = reportingBegin.Value;
            }

            if (reportingEnd != null)
            {
                this._reportingEnd = reportingEnd.Value;
            }

            if (name != null)
            {
                this._name = new List<ITextTypeWrapper>(name);
            }

            if (source != null)
            {
                this._source = new List<ITextTypeWrapper>(source);
            }

            if (receiver != null)
            {
                this._receiver = new List<IParty>(receiver);
            }

            this._sender = sender;
            this._test = test;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                ////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderImpl"/> class.
        /// </summary>
        /// <param name="headerType">
        /// The header type. 
        /// </param>
        public HeaderImpl(BaseHeaderType headerType)
        {
            this._additionalAttributes = new Dictionary<string, string>();
            this._name = new List<ITextTypeWrapper>();
            this._source = new List<ITextTypeWrapper>();
            this._receiver = new List<IParty>();
            this._structureReferences = new List<IDatasetStructureReference>();
            this._test = headerType.Test;
            if (headerType.DataProvider != null)
            {
                this._dataProviderReference = RefUtil.CreateReference(headerType.DataProvider);
            }

            if (headerType.DataSetAction != null)
            {
                switch (headerType.DataSetAction)
                {
                    case ActionTypeConstants.Append:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Append);
                        break;
                    case ActionTypeConstants.Replace:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Replace);
                        break;
                    case ActionTypeConstants.Delete:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Delete);
                        break;
                }
            }

            if (ObjectUtil.ValidCollection(headerType.DataSetID))
            {
                this._datasetId = headerType.DataSetID[0];
            }

            this._id = headerType.ID;
            if (headerType.EmbargoDate != null)
            {
                this._embargoDate = headerType.EmbargoDate.Value;
            }

            if (headerType.Extracted != null)
            {
                this._extracted = headerType.Extracted;
            }

            if (ObjectUtil.ValidCollection(headerType.Name))
            {
                foreach (Name tt in headerType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            var prepared = headerType.Prepared as DateTime?;
            if (prepared != null)
            {
                this._prepared = prepared;
            }

            if (ObjectUtil.ValidCollection(headerType.Receiver))
            {
                foreach (PartyType party in headerType.Receiver)
                {
                    this._receiver.Add(new PartyCore(party));
                }
            }

            if (headerType.ReportingBegin != null)
            {
                this._reportingBegin = DateUtil.FormatDate(headerType.ReportingBegin, true);
            }

            if (headerType.ReportingEnd != null)
            {
                this._reportingEnd = DateUtil.FormatDate(headerType.ReportingEnd, true);
            }

            if (headerType.Sender != null)
            {
                this._sender = new PartyCore(headerType.Sender);
            }

            if (ObjectUtil.ValidCollection(headerType.Source))
            {
                foreach (TextType textType in headerType.Source)
                {
                    this._source.Add(new TextTypeWrapperImpl(textType, null));
                }
            }

            if (ObjectUtil.ValidCollection(headerType.Structure))
            {
                foreach (PayloadStructureType payloadSt in headerType.Structure)
                {
                    this._structureReferences.Add(new DatasetStructureReferenceCore(payloadSt));
                }
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.0 SCHEMA            ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderImpl"/> class.
        /// </summary>
        /// <param name="headerType">
        /// The header type. 
        /// </param>
        public HeaderImpl(HeaderType headerType)
        {
            this._additionalAttributes = new Dictionary<string, string>();
            this._name = new List<ITextTypeWrapper>();
            this._source = new List<ITextTypeWrapper>();

            this._receiver = new List<IParty>();
            this._structureReferences = new List<IDatasetStructureReference>();
            this._test = headerType.Test;

            if (headerType.DataSetAction != null)
            {
                switch (headerType.DataSetAction)
                {
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.ActionTypeConstants.Append:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Append);
                        break;
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.ActionTypeConstants.Replace:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Replace);
                        break;
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.ActionTypeConstants.Delete:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Delete);
                        break;
                }
            }

            this._id = headerType.ID;
            this._datasetId = headerType.DataSetID;
            if (headerType.Extracted != null)
            {
                this._extracted = headerType.Extracted;
            }

            if (ObjectUtil.ValidCollection(headerType.Name))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType tt in headerType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            var prepared = headerType.Prepared as DateTime?;
            if (prepared != null)
            {
                this._prepared = prepared;
            }

            if (ObjectUtil.ValidCollection(headerType.Receiver))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.PartyType party in headerType.Receiver)
                {
                    this._receiver.Add(new PartyCore(party));
                }
            }

            if (headerType.ReportingBegin != null)
            {
                this._reportingBegin = DateUtil.FormatDate(headerType.ReportingBegin, true);
            }

            if (headerType.ReportingEnd != null)
            {
                this._reportingEnd = DateUtil.FormatDate(headerType.ReportingEnd, true);
            }

            if (ObjectUtil.ValidCollection(headerType.Sender))
            {
                this._sender = new PartyCore(headerType.Sender[0]);
            }

            if (ObjectUtil.ValidCollection(headerType.Source))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType textType in headerType.Source)
                {
                    this._source.Add(new TextTypeWrapperImpl(textType, null));
                }
            }

            if (!string.IsNullOrWhiteSpace(headerType.KeyFamilyAgency))
            {
                this._additionalAttributes.Add(Header.DsdAgencyRef, headerType.KeyFamilyAgency);
            }

            if (!string.IsNullOrWhiteSpace(headerType.KeyFamilyRef))
            {
                this._additionalAttributes.Add(Header.DsdRef, headerType.KeyFamilyRef);
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1.0 SCHEMA            ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderImpl"/> class.
        /// </summary>
        /// <param name="headerType">
        /// The header type. 
        /// </param>
        public HeaderImpl(Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.HeaderType headerType)
        {
            this._additionalAttributes = new Dictionary<string, string>();
            this._name = new List<ITextTypeWrapper>();
            this._source = new List<ITextTypeWrapper>();
            this._receiver = new List<IParty>();
            this._structureReferences = new List<IDatasetStructureReference>();
            this._test = headerType.Test;
            if (headerType.DataSetAction != null)
            {
                switch (headerType.DataSetAction)
                {
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.ActionTypeConstants.Update:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Replace);
                        break;
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.ActionTypeConstants.Delete:
                        this._datasetAction = DatasetAction.GetFromEnum(DatasetActionEnumType.Replace);
                        break;
                }
            }

            this._id = headerType.ID;
            this._datasetId = headerType.DataSetID;
            if (headerType.Extracted != null)
            {
                this._extracted = headerType.Extracted;
            }

            if (ObjectUtil.ValidCollection(headerType.Name))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType tt in headerType.Name)
                {
                    this._name.Add(new TextTypeWrapperImpl(tt, null));
                }
            }

            var prepared = headerType.Prepared as DateTime?;
            if (prepared != null)
            {
                this._prepared = prepared;
            }

            if (ObjectUtil.ValidCollection(headerType.Receiver))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.PartyType party in headerType.Receiver)
                {
                    this._receiver.Add(new PartyCore(party));
                }
            }

            if (headerType.ReportingBegin != null)
            {
                this._reportingBegin = DateUtil.FormatDate(headerType.ReportingBegin, true);
            }

            if (headerType.ReportingEnd != null)
            {
                this._reportingEnd = DateUtil.FormatDate(headerType.ReportingEnd, true);
            }

            if (headerType.Sender != null)
            {
                this._sender = new PartyCore(headerType.Sender);
            }

            if (ObjectUtil.ValidCollection(headerType.Source))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType textType in headerType.Source)
                {
                    this._source.Add(new TextTypeWrapperImpl(textType, null));
                }
            }

            if (!string.IsNullOrWhiteSpace(headerType.KeyFamilyAgency))
            {
                this._additionalAttributes.Add(Header.DsdAgencyRef, headerType.KeyFamilyAgency);
            }

            if (!string.IsNullOrWhiteSpace(headerType.KeyFamilyRef))
            {
                this._additionalAttributes.Add(Header.DsdRef, headerType.KeyFamilyRef);
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                         ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the action.
        /// </summary>
        public virtual DatasetAction Action
        {
            get
            {
                return this._datasetAction;
            }

            set
            {
                this._datasetAction = value;
            }
        }

        /// <summary>
        ///   Gets the additional attribtues.
        /// </summary>
        public virtual IDictionary<string, string> AdditionalAttribtues
        {
            get
            {
                return new Dictionary<string, string>(this._additionalAttributes);
            }
        }

        /// <summary>
        ///   Gets the data provider reference.
        /// </summary>
        public virtual IStructureReference DataProviderReference
        {
            get
            {
                return this._dataProviderReference;
            }

            set
            {
                if (value != null)
                {
                    if (value.TargetReference.EnumType != SdmxStructureEnumType.DataProvider)
                    {
                        throw new SdmxSemmanticException("Header.setDataProviderReference - structure type does not reference a data provider, it references a " + value.TargetReference.StructureType);
                    }
                    if (value.TargetUrn == null)
                    {
                        throw new SdmxSemmanticException("Header.setDataProviderReference - data provider reference incomplete");
                    }
                }
                this._dataProviderReference = value;
            }
        }

        /// <summary>
        ///   Gets or sets the dataset id.
        /// </summary>
        public virtual string DatasetId
        {
            get
            {
                return this._datasetId;
            }

            set
            {
                this._datasetId = value;
            }
        }

        /// <summary>
        ///   Gets the embargo date.
        /// </summary>
        public virtual DateTime? EmbargoDate
        {
            get
            {
                if (this._embargoDate != null)
                {
                    return this._embargoDate;
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets the extracted.
        /// </summary>
        public virtual DateTime? Extracted
        {
            get
            {
                if (this._extracted != null)
                {
                    return this._extracted;
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                this.Validate();
            }
        }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Name
        {
            get
            {
                return new List<ITextTypeWrapper>(this._name);
            }
        }

        /// <summary>
        ///   Gets the prepared.
        /// </summary>
        public virtual DateTime? Prepared
        {
            get
            {
                return this._prepared;
            }
        }

        /// <summary>
        ///   Gets the receiver.
        /// </summary>
        public virtual IList<IParty> Receiver
        {
            get
            {
                return new List<IParty>(this._receiver);
            }
        }

        /// <summary>
        ///   Gets or sets the reporting begin.
        /// </summary>
        public virtual DateTime? ReportingBegin
        {
            get
            {
                return this._reportingBegin;
            }

            set
            {
                this._reportingBegin = value;
            }
        }

        /// <summary>
        ///   Gets or sets the reporting end.
        /// </summary>
        public virtual DateTime? ReportingEnd
        {
            get
            {
                if (this._reportingEnd != null)
                {
                    return this._reportingEnd;
                }

                return null;
            }
            set
            {
                this._reportingEnd = value;
            }
        }

        /// <summary>
        ///   Gets or sets the sender.
        /// </summary>
        public virtual IParty Sender
        {
            get
            {
                return this._sender;
            }

            set
            {
                this._sender = value;
                this.Validate();
            }
        }

        /// <summary>
        ///   Gets the source.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Source
        {
            get
            {
                return new List<ITextTypeWrapper>(this._source);
            }
        }

        /// <summary>
        ///   Gets the structures.
        /// </summary>
        public virtual IList<IDatasetStructureReference> Structures
        {
            get
            {
                return new List<IDatasetStructureReference>(this._structureReferences);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether test.
        /// </summary>
        public virtual bool Test
        {
            get
            {
                return this._test;
            }

            set
            {
                this._test = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get additional attribtue.
        /// </summary>
        /// <param name="headerField">
        /// The header field. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        public virtual string GetAdditionalAttribtue(string headerField)
        {
            return this._additionalAttributes[headerField];
        }

        /// <summary>
        /// The get structure by id.
        /// </summary>
        /// <param name="structureId">
        /// The structure id. 
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetStructureReference"/> . 
        /// </returns>
        public virtual IDatasetStructureReference GetStructureById(string structureId)
        {
            foreach (IDatasetStructureReference currentStructure in this._structureReferences)
            {
                if (currentStructure.Id.Equals(structureId))
                {
                    return currentStructure;
                }
            }

            return null;
        }

        /// <summary>
        /// The has additional attribtue.
        /// </summary>
        /// <param name="headerField">
        /// The header field. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public virtual bool HasAdditionalAttribtue(string headerField)
        {
            return this._additionalAttributes.ContainsKey(headerField);
        }

        public void AddReciever(IParty recevier)
        {
            this._receiver.Add(recevier);
        }

        public void AddSource(ITextTypeWrapper source)
        {
            this._source.Add(source);
        }

        public void AddStructure(IDatasetStructureReference datasetStructureReference)
        {
            this._structureReferences.Add(datasetStructureReference);
        }

        public void AddName(ITextTypeWrapper name)
        {
            this._name.Add(name);
        }

        #endregion

        #region Methods


        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this._id))
            {
                this._id = RandomUtil.GenerateRandomString();
            }

            if (this._prepared == null)
            {
                this._prepared = DateTime.Now;
            }

            if (this._sender == null)
            {
                if (string.IsNullOrWhiteSpace(this._id))
                {
                    throw new SdmxSemmanticException("Header missing sender");
                }
            }
        }

        #endregion
    }
}