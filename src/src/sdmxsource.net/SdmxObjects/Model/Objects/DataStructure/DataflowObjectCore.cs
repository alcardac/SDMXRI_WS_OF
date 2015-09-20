// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    using DataflowType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.DataflowType;

    /// <summary>
    ///   The dataflow object core.
    /// </summary>
    [Serializable]
    public class DataflowObjectCore : MaintainableObjectCore<IDataflowObject, IDataflowMutableObject>, IDataflowObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataflowObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The key family ref.
        /// </summary>
        private readonly ICrossReference keyFamilyRef;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public DataflowObjectCore(IDataflowMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            Log.Debug("Building IDataflowObject from Mutable Object");
            if (itemMutableObject.DataStructureRef != null)
            {
                this.keyFamilyRef = new CrossReferenceImpl(this, itemMutableObject.DataStructureRef);
            }
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {

                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IDataflowObject Built " + base.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowObjectCore"/> class.
        /// </summary>
        /// <param name="dataflow">
        /// The sdmxObject. 
        /// </param>
        public DataflowObjectCore(DataflowType dataflow)
            : base(dataflow, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
        {
            Log.Debug("Building IDataflowObject from 2.1 SDMX");
            var dataStructureReferenceType = dataflow.GetStructure<DataStructureReferenceType>();
            if (dataStructureReferenceType != null)
            {
                this.keyFamilyRef = RefUtil.CreateReference(this, dataStructureReferenceType);
            }
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {

                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IDataflowObject Built " + base.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowObjectCore"/> class.
        /// </summary>
        /// <param name="dataflow">
        /// The sdmxObject. 
        /// </param>
        public DataflowObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.DataflowType dataflow)
            : base(
                dataflow, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow), 
                dataflow.validTo, 
                dataflow.validFrom, 
                dataflow.version, 
                CreateTertiary(dataflow.isFinal), 
                dataflow.agencyID, 
                dataflow.id, 
                dataflow.uri, 
                dataflow.Name, 
                dataflow.Description, 
                CreateTertiary(dataflow.isExternalReference), 
                dataflow.Annotations)
        {
            Log.Debug("Building IDataflowObject from 2.0 SDMX");
            if (dataflow.KeyFamilyRef != null)
            {
                KeyFamilyRefType familyRef = dataflow.KeyFamilyRef;
                if (familyRef.URN != null)
                {
                    this.keyFamilyRef = new CrossReferenceImpl(this, familyRef.URN);
                }
                else
                {
                    this.keyFamilyRef = new CrossReferenceImpl(
                        this, 
                        familyRef.KeyFamilyAgencyID, 
                        familyRef.KeyFamilyID, 
                        familyRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
                }
            }
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {

                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IDataflowObject Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowObjectCore"/> class.
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
        private DataflowObjectCore(IDataflowObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            Log.Debug("Stub IDataflowObject Built");
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the cross referenced constrainables.
        /// </summary>
        public virtual IList<ICrossReference> CrossReferencedConstrainables
        {
            get
            {
                IList<ICrossReference> returnList = new List<ICrossReference>();
                if (keyFamilyRef != null)
                {
                    returnList.Add(this.DataStructureRef);
                }
                return returnList;
            }
        }

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public override sealed Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        /// <summary>
        ///   Gets the data structure ref.
        /// </summary>
        public virtual ICrossReference DataStructureRef
        {
            get
            {
                return this.keyFamilyRef;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IDataflowMutableObject MutableInstance
        {
            get
            {
                return new DataflowMutableCore(this);
            }
        }

        #endregion

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
                var that = (IDataflowObject)sdmxObject;
                if (!this.Equivalent(this.keyFamilyRef, that.DataStructureRef))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        private void Validate()
        {
            //// back port from 0.9.18
            if (!this.IsExternalReference.IsTrue)
            {
                if (keyFamilyRef == null)
                {
                    throw new SdmxSemmanticException("Dataflow must reference a Data Structure Definition");
                }
            }
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
        /// The <see cref="IDataflowObject"/> . 
        /// </returns>
        public override IDataflowObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new DataflowObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion
    }
}