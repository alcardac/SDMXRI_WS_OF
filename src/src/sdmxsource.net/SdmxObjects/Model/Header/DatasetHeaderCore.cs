// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetHeaderBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataset header core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header
{
    using System;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///   The dataset header core.
    /// </summary>
    public class DatasetHeaderCore : IDatasetHeader
    {
        #region Fields

        /// <summary>
        ///   The action.
        /// </summary>
        private readonly DatasetAction _action;

        /// <summary>
        ///   The data provider ref.
        /// </summary>
        private readonly IMaintainableRefObject _dataProviderRef;

        /// <summary>
        ///   The dataset id.
        /// </summary>
        private readonly string _datasetId;

        /// <summary>
        ///   The idataset structure reference.
        /// </summary>
        private readonly IDatasetStructureReference _datasetStructureReference;

        /// <summary>
        ///   The publication period.
        /// </summary>
        private readonly string _publicationPeriod;

        /// <summary>
        ///   The publication year.
        /// </summary>
        private readonly int _publicationYear;

        /// <summary>
        ///   The reporting begin date.
        /// </summary>
        private readonly DateTime _reportingBeginDate;

        /// <summary>
        ///   The reporting end date.
        /// </summary>
        private readonly DateTime _reportingEndDate;

        /// <summary>
        ///   The valid from.
        /// </summary>
        private readonly DateTime _validFrom;

        /// <summary>
        ///   The valid to.
        /// </summary>
        private readonly DateTime _validTo;

        /// <summary>
        ///   The reporting year start date.
        /// </summary>
        private string reportingYearStartDate; // TODO WE NEED A MONTH DAY TYPE

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetHeaderCore"/> class. 
        ///   Minimal Constructor
        /// </summary>
        /// <param name="datasetId">Dataset id
        /// </param>
        /// <param name="action">Dataset action
        /// </param>
        /// <param name="datasetStructureReference">DatasetStructureReference object
        /// </param>
        public DatasetHeaderCore(
            string datasetId, DatasetAction action, IDatasetStructureReference datasetStructureReference)
        {
            this._action = DatasetAction.GetFromEnum(DatasetActionEnumType.Information);
            this._publicationYear = -1;
            this._datasetId = datasetId;
            if (action != null)
            {
                this._action = action;
            }

            this._datasetStructureReference = datasetStructureReference;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetHeaderCore"/> class.
        /// </summary>
        /// <param name="datasetId">
        /// The dataset id. 
        /// </param>
        /// <param name="action">
        /// The action. 
        /// </param>
        /// <param name="datasetStructureReference">
        /// The dataset structure reference agencySchemeMutable. 
        /// </param>
        /// <param name="dataProviderRef">
        /// The data provider ref. 
        /// </param>
        /// <param name="reportingBeginDate">
        /// The reporting begin date. 
        /// </param>
        /// <param name="reportingEndDate">
        /// The reporting end date. 
        /// </param>
        /// <param name="validFrom">
        /// The valid from. 
        /// </param>
        /// <param name="validTo">
        /// The valid to. 
        /// </param>
        /// <param name="publicationYear">
        /// The publication year. 
        /// </param>
        /// <param name="publicationPeriod">
        /// The publication period. 
        /// </param>
        /// <param name="reportingYearStartDate">
        /// The reporting year start date. 
        /// </param>
        public DatasetHeaderCore(
            string datasetId,
            DatasetAction action,
            IDatasetStructureReference datasetStructureReference,
            IMaintainableRefObject dataProviderRef,
            DateTime reportingBeginDate,
            DateTime reportingEndDate,
            DateTime validFrom,
            DateTime validTo,
            int publicationYear,
            string publicationPeriod,
            string reportingYearStartDate)
        {
            this._action = DatasetAction.GetFromEnum(DatasetActionEnumType.Information);
            this._publicationYear = -1;
            this._dataProviderRef = dataProviderRef;
            this._datasetStructureReference = datasetStructureReference;
            this._datasetId = datasetId;
            this._reportingBeginDate = reportingBeginDate;
            this._reportingEndDate = reportingEndDate;
            this._validFrom = validFrom;
            this._validTo = validTo;
            this._action = action;
            this._publicationYear = publicationYear;
            this._publicationPeriod = publicationPeriod;
            this.reportingYearStartDate = reportingYearStartDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetHeaderCore"/> class. 
        ///   Create an instance from a parser reading the dataset XML Node and the header
        /// </summary>
        /// <param name="parser">XML reader instance.
        /// </param>
        /// <param name="datasetHeader">Header object.
        /// </param>
        public DatasetHeaderCore(XmlReader parser, IHeader datasetHeader)
        {
            this._action = DatasetAction.GetFromEnum(DatasetActionEnumType.Information);
            this._publicationYear = -1;
            if (parser.GetAttribute("structureRef") != null)
            {
                string structureRef = parser.GetAttribute("structureRef");
                this._datasetStructureReference = GetStructureFromHeader(datasetHeader, structureRef);
                if (this._datasetStructureReference == null)
                {
                    throw new SdmxSemmanticException(
                        "Dataset references CategorisationStructure that is not defined in the Header of the message.  CategorisationStructure reference defined by Dataset is:"
                        + structureRef);
                }
            }
            else
            {
                this._datasetStructureReference = GenerateOrUseDefaultStructure(parser, datasetHeader);
            }

            if (parser.GetAttribute("action") != null)
            {
                this._action = DatasetAction.GetAction(parser.GetAttribute("action"));
            }
            else if (datasetHeader.Action != null)
            {
                this._action = datasetHeader.Action;
            }

            // if(parser.getAttributeValue(null, "dataProviderID") != null) {
            // datasetAttributes.setDataProviderId(parser.getAttributeValue(null, "dataProviderID"));
            // }
            // if(parser.getAttributeValue(null, "dataProviderSchemeAgencyId") != null) {
            // datasetAttributes.setDataProviderSchemeAgencyId(parser.getAttributeValue(null, "dataProviderSchemeAgencyId"));
            // }
            // if(parser.getAttributeValue(null, "dataProviderSchemeId") != null) {
            // datasetAttributes.setDataProviderSchemeId(parser.getAttributeValue(null, "dataProviderSchemeId"));
            // }
            // TODO use switch & parser.MoveToNextAttribute()
            if (parser.GetAttribute("datasetID") != null)
            {
                this._datasetId = parser.GetAttribute("datasetID");
            }

            if (parser.GetAttribute("publicationPeriod") != null)
            {
                this._publicationPeriod = parser.GetAttribute("publicationPeriod");
            }

            if (parser.GetAttribute("publicationYear") != null)
            {
                this._publicationYear = int.Parse(parser.GetAttribute("publicationYear"));
            }

            if (parser.GetAttribute("reportingBeginDate") != null)
            {
                this._reportingBeginDate = DateUtil.FormatDate(parser.GetAttribute("reportingBeginDate"), true);
            }

            if (parser.GetAttribute("reportingEndDate") != null)
            {
                this._reportingEndDate = DateUtil.FormatDate(parser.GetAttribute("reportingEndDate"), true);
            }

            if (parser.GetAttribute("validFromDate") != null)
            {
                this._validFrom = DateUtil.FormatDate(parser.GetAttribute("validFromDate"), true);
            }

            if (parser.GetAttribute("validToDate") != null)
            {
                this._validTo = DateUtil.FormatDate(parser.GetAttribute("validToDate"), true);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the action.
        /// </summary>
        public virtual DatasetAction Action
        {
            get
            {
                return this._action;
            }
        }

        /// <summary>
        ///   Gets the data provider reference.
        /// </summary>
        public virtual IMaintainableRefObject DataProviderReference
        {
            get
            {
                return this._dataProviderRef;
            }
        }

        /// <summary>
        ///   Gets the data structure reference.
        /// </summary>
        public virtual IDatasetStructureReference DataStructureReference
        {
            get
            {
                return this._datasetStructureReference;
            }
        }

        /// <summary>
        ///   Gets the dataset id.
        /// </summary>
        public virtual string DatasetId
        {
            get
            {
                return this._datasetId;
            }
        }

        /// <summary>
        ///   Gets the publication period.
        /// </summary>
        public virtual string PublicationPeriod
        {
            get
            {
                return this._publicationPeriod;
            }
        }

        /// <summary>
        ///   Gets the publication year.
        /// </summary>
        public virtual int PublicationYear
        {
            get
            {
                return this._publicationYear;
            }
        }

        /// <summary>
        ///   Gets the reporting begin date.
        /// </summary>
        public virtual DateTime ReportingBeginDate
        {
            get
            {
                return this._reportingBeginDate;
            }
        }

        /// <summary>
        ///   Gets the reporting end date.
        /// </summary>
        public virtual DateTime ReportingEndDate
        {
            get
            {
                return this._reportingEndDate;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether time series.
        /// </summary>
        public virtual bool Timeseries
        {
            get
            {
                if (this._datasetStructureReference == null)
                {
                    return true;
                }

                return this._datasetStructureReference.Timeseries;
            }
        }

        /// <summary>
        ///   Gets the valid from.
        /// </summary>
        public virtual DateTime ValidFrom
        {
            get
            {
                return this._validFrom;
            }
        }

        /// <summary>
        ///   Gets the valid to.
        /// </summary>
        public virtual DateTime ValidTo
        {
            get
            {
                return this._validTo;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The modify data structure reference.
        /// </summary>
        /// <param name="datasetStructureRef">
        /// The dataset structure reference agencySchemeMutable 0. 
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetHeader"/> . 
        /// </returns>
        public virtual IDatasetHeader ModifyDataStructureReference(
            IDatasetStructureReference datasetStructureRef)
        {
            return new DatasetHeaderCore(
                this.DatasetId,
                this.Action,
                datasetStructureRef,
                this.DataProviderReference,
                this.ReportingBeginDate,
                this.ReportingEndDate,
                this.ValidFrom,
                this.ValidTo,
                this.PublicationYear,
                this.PublicationPeriod,
                null); // TODO reportingYearStartDate
        }

        #endregion

        #region Methods

        /// <summary>
        /// The generate or use default structure.
        /// </summary>
        /// <param name="parser">
        /// The parser. 
        /// </param>
        /// <param name="datasetHeader">
        /// The dataset header. 
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetStructureReference"/> . 
        /// </returns>
        private static IDatasetStructureReference GenerateOrUseDefaultStructure(XmlReader parser, IHeader datasetHeader)
        {
            IStructureReference structureReference;
            IMaintainableRefObject dataflowReference = GetDataflowReference(parser);
            IMaintainableRefObject dsdReference = GetDsdReference(parser);
            if (dataflowReference != null)
            {
                structureReference = new StructureReferenceImpl(
                    dataflowReference, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            }
            else if (dsdReference != null)
            {
                structureReference = new StructureReferenceImpl(
                    dsdReference, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
            }
            else if (datasetHeader.Structures.Count == 1)
            {
                return datasetHeader.Structures[0];
            }
            else
            {
                return null;
            }

            return new DatasetStructureReferenceCore(
                "generated", structureReference, null, null, DimensionObject.TimeDimensionFixedId);
        }

        /// <summary>
        /// The get dataflow reference.
        /// </summary>
        /// <param name="parser">
        /// The parser. 
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableRefObject"/> . 
        /// </returns>
        private static IMaintainableRefObject GetDataflowReference(XmlReader parser)
        {
            string dataflowAgencyId = null;
            string dataflowId = null;

            if (parser.GetAttribute("dataflowAgencyID") != null)
            {
                dataflowAgencyId = parser.GetAttribute("dataflowAgencyID");
            }

            if (parser.GetAttribute("dataflowID") != null)
            {
                dataflowId = parser.GetAttribute("dataflowID");
            }

            if (!string.IsNullOrWhiteSpace(dataflowAgencyId))
            {
                return new MaintainableRefObjectImpl(dataflowAgencyId, dataflowId, MaintainableObject.DefaultVersion);
            }

            return null;
        }

        /// <summary>
        /// The get dsd reference.
        /// </summary>
        /// <param name="parser">
        /// The parser. 
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableRefObject"/> . 
        /// </returns>
        private static IMaintainableRefObject GetDsdReference(XmlReader parser)
        {
            if (parser.GetAttribute("keyFamilyURI") != null)
            {
                string dsdUri = parser.GetAttribute("keyFamilyURI");
                IStructureReference dsdRef = new StructureReferenceImpl(dsdUri);
                return dsdRef.MaintainableReference;
            }

            return null;
        }

        /// <summary>
        /// The get structure from header.
        /// </summary>
        /// <param name="header">
        /// The header. 
        /// </param>
        /// <param name="structureRef">
        /// The structure ref. 
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetStructureReference"/> . 
        /// </returns>
        private static IDatasetStructureReference GetStructureFromHeader(IHeader header, string structureRef)
        {
            if (header.Structures == null)
            {
                return null;
            }

            foreach (IDatasetStructureReference currentStructure in header.Structures)
            {
                if (currentStructure.Id.Equals(structureRef))
                {
                    return currentStructure;
                }
            }

            return null;
        }

        #endregion
    }
}