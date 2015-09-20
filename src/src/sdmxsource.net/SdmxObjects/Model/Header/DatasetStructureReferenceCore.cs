// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetStructureReferenceBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataset structure reference core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;

    using PrimaryMeasure = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.PrimaryMeasure;

    /// <summary>
    ///   The dataset structure reference core.
    /// </summary>
    [Serializable]
    public class DatasetStructureReferenceCore : IDatasetStructureReference
    {
        #region Fields

        /// <summary>
        ///   The service url.
        /// </summary>
        private readonly Uri _serviceUrl;

        /// <summary>
        ///   The structure reference.
        /// </summary>
        private readonly IStructureReference _structureReference;

        /// <summary>
        ///   The structure url.
        /// </summary>
        private readonly Uri _structureUrl;

        /// <summary>
        ///   The dimension at observation.
        /// </summary>
        private string _dimensionAtObservation;

        /// <summary>
        ///   The id.
        /// </summary>
        private string _id;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetStructureReferenceCore"/> class. 
        ///   Minimal Constructor
        /// </summary>
        /// <param name="structureReference">CategorisationStructure reference object
        /// </param>
        public DatasetStructureReferenceCore(IStructureReference structureReference)
        {
            this._dimensionAtObservation = DimensionObject.TimeDimensionFixedId;

            this._structureReference = structureReference;
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetStructureReferenceCore"/> class.
        /// </summary>
        /// <param name="id0">
        /// The id 0. 
        /// </param>
        /// <param name="structureReference1">
        /// The structure reference 1. 
        /// </param>
        /// <param name="serviceUrl2">
        /// The service url 2. 
        /// </param>
        /// <param name="structureUrl3">
        /// The structure url 3. 
        /// </param>
        /// <param name="dimensionAtObservation4">
        /// The dimension at observation 4. 
        /// </param>
        public DatasetStructureReferenceCore(
            string id0,
            IStructureReference structureReference1,
            Uri serviceUrl2,
            Uri structureUrl3,
            string dimensionAtObservation4)
        {
            this._dimensionAtObservation = DimensionObject.TimeDimensionFixedId;
            this._id = id0;
            this._structureReference = structureReference1;
            this._serviceUrl = serviceUrl2;
            this._structureUrl = structureUrl3;
            if (!string.IsNullOrWhiteSpace(dimensionAtObservation4))
            {
                this._dimensionAtObservation = dimensionAtObservation4;
            }

            this.Validate();
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////////
        // ////////////BUILD FROM V2.1 XMLStreamReader    ///////////////////////////////////////////////////////
        // ///////////////////////////////////////////////////////////////////////////////////////////////////
        // public DatasetStructureReferenceCore(XMLStreamReader reader) {
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA            ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetStructureReferenceCore"/> class.
        /// </summary>
        /// <param name="payloadSt">
        /// The payload st. 
        /// </param>
        public DatasetStructureReferenceCore(PayloadStructureType payloadSt)
        {
            this._dimensionAtObservation = DimensionObject.TimeDimensionFixedId;
            if (payloadSt.ProvisionAgrement != null)
            {
                this._structureReference = RefUtil.CreateReference(payloadSt.ProvisionAgrement);
            }
            else if (payloadSt.StructureUsage != null)
            {
                this._structureReference = RefUtil.CreateReference(payloadSt.StructureUsage);
            }
            else if (payloadSt.Structure != null)
            {
                this._structureReference = RefUtil.CreateReference(payloadSt.Structure);
            }

            this._id = payloadSt.structureID;
            this._serviceUrl = payloadSt.serviceURL;
            this._structureUrl = payloadSt.structureURL;
            this._dimensionAtObservation = payloadSt.dimensionAtObservation.ToString();
            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the dimension at observation.
        /// </summary>
        public string DimensionAtObservation
        {
            get
            {
                return this._dimensionAtObservation;
            }
        }

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public string Id
        {
            get
            {
                return this._id;
            }
        }

        /// <summary>
        ///   Gets the service url.
        /// </summary>
        public Uri ServiceUrl
        {
            get
            {
                return this._serviceUrl;
            }
        }

        /// <summary>
        ///   Gets the structure reference.
        /// </summary>
        public IStructureReference StructureReference
        {
            get
            {
                return this._structureReference;
            }
        }

        /// <summary>
        ///   Gets the structure url.
        /// </summary>
        public Uri StructureUrl
        {
            get
            {
                return this._structureUrl;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether time series.
        /// </summary>
        public bool Timeseries
        {
            get
            {
                return this._dimensionAtObservation.Equals(DimensionObject.TimeDimensionFixedId);
            }
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //////////VALIDATE                        /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this._id))
            {
                this._id = Guid.NewGuid().ToString();
            }

            if (this._structureReference == null)
            {
                throw new SdmxSemmanticException("Header 'CategorisationStructure' missing ObjectStructure Reference");
            }

            if (this._dimensionAtObservation == null)
            {
                this._dimensionAtObservation = DimensionObject.TimeDimensionFixedId;
            }
        }

        #endregion
    }
}