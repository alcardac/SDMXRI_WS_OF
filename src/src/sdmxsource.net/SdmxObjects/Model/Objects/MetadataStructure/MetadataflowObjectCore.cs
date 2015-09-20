// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataflowBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadataflow object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    using MetadataflowType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MetadataflowType;
    using MetadataStructureRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataStructureRefType;

    /// <summary>
    ///   The metadataflow object core.
    /// </summary>
    [Serializable]
    public class MetadataflowObjectCore : MaintainableObjectCore<IMetadataFlow, IMetadataFlowMutableObject>, 
                                          IMetadataFlow
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(MetadataflowObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The metadata structure ref.
        /// </summary>
        private readonly ICrossReference metadataStructureRef;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataflowObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public MetadataflowObjectCore(IMetadataFlowMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            Log.Debug("Building IMetadataFlow from Mutable Object");
            if (itemMutableObject.MetadataStructureRef != null)
            {
                this.metadataStructureRef = new CrossReferenceImpl(this, itemMutableObject.MetadataStructureRef);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IMetadataFlow Built " + this.Urn);
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataflowObjectCore"/> class.
        /// </summary>
        /// <param name="metadataflow">
        /// The sdmxObject. 
        /// </param>
        public MetadataflowObjectCore(MetadataflowType metadataflow)
            : base(metadataflow, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow))
        {
            Log.Debug("Building IMetadataFlow from 2.1 SDMX");
            var dataStructureReferenceType = metadataflow.GetStructure<MetadataStructureReferenceType>();
            if (!metadataflow.isExternalReference)
            {
                if (dataStructureReferenceType != null)
                {
                    this.metadataStructureRef = RefUtil.CreateReference(this, dataStructureReferenceType);
                }
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IMetadataFlow Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataflowObjectCore"/> class.
        /// </summary>
        /// <param name="metadataflow">
        /// The sdmxObject. 
        /// </param>
        public MetadataflowObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataflowType metadataflow)
            : base(
                metadataflow, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow), 
                metadataflow.validTo, 
                metadataflow.validFrom, 
                metadataflow.version, 
                CreateTertiary(metadataflow.isFinal), 
                metadataflow.agencyID, 
                metadataflow.id, 
                metadataflow.uri, 
                metadataflow.Name, 
                metadataflow.Description, 
                CreateTertiary(metadataflow.isExternalReference), 
                metadataflow.Annotations)
        {
            Log.Debug("Building IMetadataFlow from 2.0 SDMX");
            if (metadataflow.MetadataStructureRef != null)
            {
                MetadataStructureRefType xref = metadataflow.MetadataStructureRef;
                if (xref.URN != null)
                {
                    this.metadataStructureRef = new CrossReferenceImpl(this, xref.URN);
                }
                else
                {
                    this.metadataStructureRef = new CrossReferenceImpl(
                        this, 
                        xref.MetadataStructureAgencyID, 
                        xref.MetadataStructureID, 
                        xref.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));
                }
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IMetadataFlow Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataflowObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The sdmxObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private MetadataflowObjectCore(IMetadataFlow agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            Log.Debug("Stub IMetadataFlow Built");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the metadata structure ref.
        /// </summary>
        public virtual ICrossReference MetadataStructureRef
        {
            get
            {
                return this.metadataStructureRef;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IMetadataFlowMutableObject MutableInstance
        {
            get
            {
                return new MetadataflowMutableCore(this);
            }
        }

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IMetadataFlow)sdmxObject;
                if (!this.Equivalent(this.metadataStructureRef, that.MetadataStructureRef))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IMetadataFlow"/> . 
        /// </returns>
        public override IMetadataFlow GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new MetadataflowObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion
    }
}