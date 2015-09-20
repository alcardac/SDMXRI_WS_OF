// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The dimension core.
    /// </summary>
    /// <remarks>
    /// In this implementation the coded representation Time dimension is allowed.
    /// It is a convention requested by Eurostat to allow coded TimeDimensions. 
    /// See <c>CITnet</c> JIRA ticket SDMXRI-22:
    /// <a href="https://webgate.ec.europa.eu/CITnet/jira/browse/SDMXRI-22">SDMXRI-22</a>
    /// Also the original ticket in AGILIS JIRA: <a href="http://www.agilis-sa.gr:8070/browse/SODIHD-1209">SODIHD-1209</a>
    /// </remarks>
    [Serializable]
    public class DimensionCore : ComponentCore, IDimension
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(DimensionCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The concept role.
        /// </summary>
        private readonly IList<ICrossReference> conceptRole;

        /// <summary>
        ///   The freq dimension.
        /// </summary>
        private readonly bool freqDimension;

        /// <summary>
        ///   The measure dimension.
        /// </summary>
        private readonly bool measureDimension;

        /// <summary>
        ///   The position.
        /// </summary>
        private readonly int position;

        /// <summary>
        ///   The time dimension.
        /// </summary>
        private readonly bool timeDimension;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DimensionCore(IDimensionMutableObject itemMutableObject, int position0, IDimensionList parent)
            : base(itemMutableObject, parent)
        {
            this.conceptRole = new List<ICrossReference>();
            try
            {
                this.position = position0;
                this.measureDimension = itemMutableObject.MeasureDimension;
                this.timeDimension = itemMutableObject.TimeDimension;
                if (itemMutableObject.ConceptRole != null)
                {
                    foreach (IStructureReference currentConceptRole in itemMutableObject.ConceptRole)
                    {
                        this.conceptRole.Add(new CrossReferenceImpl(this, currentConceptRole));
                    }
                }

                this.ValidateDimension();
            }
            catch (Exception e)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), e);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(DimensionType dimension, IDimensionList parent, int position0)
            : base(dimension, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension), parent)
        {
            this.conceptRole = new List<ICrossReference>();
            if (dimension.position.HasValue)
            {
                this.position = dimension.position.Value;
            }
            else
            {
                this.position = position0;
            }

            if (ObjectUtil.ValidCollection(dimension.ConceptRole))
            {
                foreach (ConceptReferenceType conceptReference in dimension.ConceptRole)
                {
                    this.conceptRole.Add(RefUtil.CreateReference(this, conceptReference));
                }
            }

            this.ValidateDimension();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(MeasureDimensionType dimension, IDimensionList parent, int position0)
            : base(dimension, dimension.GetTypedLocalRepresentation<MeasureDimensionRepresentationType>(), SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDimension), parent)
        {
            this.conceptRole = new List<ICrossReference>();
            this.measureDimension = true;
            this.position = dimension.position.HasValue ? dimension.position.Value : position0;

            if (ObjectUtil.ValidCollection(dimension.ConceptRole))
            {
                foreach (ConceptReferenceType conceptReference in dimension.ConceptRole)
                {
                    this.conceptRole.Add(RefUtil.CreateReference(this, conceptReference));
                }
            }

            this.ValidateDimension();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(TimeDimensionType dimension, IDimensionList parent, int position0)
            : base(dimension, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeDimension), parent)
        {
            this.conceptRole = new List<ICrossReference>();
            this.timeDimension = true;
            if (dimension.position.HasValue)
            {
                this.position = dimension.position.Value;
            }
            else
            {
                this.position = position0;
            }

            if (ObjectUtil.ValidCollection(dimension.ConceptRole))
            {
                foreach (ConceptReferenceType conceptReference in dimension.ConceptRole)
                {
                    this.conceptRole.Add(RefUtil.CreateReference(this, conceptReference));
                }
            }

            this.ValidateDimension();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.DimensionType dimension, 
            IDimensionList parent, 
            int position0)
            : base(
                dimension, 
                GetDimensionType(dimension), 
                dimension.Annotations, 
                dimension.TextFormat, 
                dimension.codelistAgency, 
                dimension.codelist, 
                dimension.codelistVersion, 
                dimension.conceptSchemeAgency, 
                dimension.conceptSchemeRef, 
                GetConceptSchemeVersion(dimension), 
                dimension.conceptAgency, 
                dimension.conceptRef, 
                parent)
        {
            this.conceptRole = new List<ICrossReference>();
            if (parent.MaintainableParent is ICrossSectionalDataStructureObject)
            {
                this.measureDimension = dimension.isMeasureDimension;
            }

            this.position = position0;
            this.freqDimension = dimension.isFrequencyDimension;
            this.ValidateDimension();
        }

        /// <summary>
        /// Return the specified <paramref name="dimension"/> type.
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/>.
        /// </returns>
        private static SdmxStructureType GetDimensionType(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.DimensionType dimension)
        {
            return SdmxStructureType.GetFromEnum(dimension.isMeasureDimension ? SdmxStructureEnumType.MeasureDimension : SdmxStructureEnumType.Dimension);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TimeDimensionType dimension, 
            IDimensionList parent, 
            int position0)
            : base(
                dimension, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeDimension), 
                dimension.Annotations, 
                dimension.TextFormat, 
                dimension.codelistAgency, 
                dimension.codelist, 
                dimension.codelistVersion, 
                dimension.conceptSchemeAgency, 
                dimension.conceptSchemeRef, 
                GetConceptSchemeVersion(dimension), 
                dimension.conceptAgency, 
                dimension.conceptRef, 
                parent)
        {
            this.conceptRole = new List<ICrossReference>();
            this.timeDimension = true;
            this.position = position0;
            this.ValidateDimension();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.DimensionType dimension, 
            IDimensionList parent, 
            int position0)
            : base(
                dimension, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension), 
                dimension.Annotations, 
                dimension.codelist, 
                dimension.concept, 
                parent)
        {
            this.conceptRole = new List<ICrossReference>();
            this.measureDimension = false;
            this.position = position0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionCore"/> class.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="position0">
        /// The position 0. 
        /// </param>
        public DimensionCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.TimeDimensionType dimension, 
            IDimensionList parent, 
            int position0)
            : base(
                dimension, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension), 
                dimension.Annotations, 
                dimension.codelist, 
                dimension.concept, 
                parent)
        {
            this.conceptRole = new List<ICrossReference>();
            this.timeDimension = true;
            this.position = position0;
            this.ValidateDimension();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the concept role.
        /// </summary>
        public virtual IList<ICrossReference> ConceptRole
        {
            get
            {
                return new List<ICrossReference>(this.conceptRole);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether frequency dimension.
        /// </summary>
        public virtual bool FrequencyDimension
        {
            get
            {
                if (this.freqDimension)
                {
                    return true;
                }

                return this.Id.Equals("FREQ");
            }
        }

        /// <summary>
        ///   Gets a value indicating whether measure dimension.
        /// </summary>
        public virtual bool MeasureDimension
        {
            get
            {
                return this.measureDimension;
            }
        }

        /// <summary>
        ///   Gets the position.
        /// </summary>
        public virtual int Position
        {
            get
            {
                return this.position;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether time dimension.
        /// </summary>
        public virtual bool TimeDimension
        {
            get
            {
                return this.timeDimension;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other. 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public virtual int CompareTo(IDimension other)
        {
            if (other.Position == this.Position)
            {
                if (!other.Equals(this))
                {
                    throw new SdmxSemmanticException(
                        "Two dimensions (" + this.Id + " & " + other.Id + ") can not share the same dimension position : "
                        + this.Position);
                }
            }

            return (other.Position > this.Position) ? -1 : +1;
        }

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
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IDimension)sdmxObject;
                if (this.measureDimension != that.MeasureDimension)
                {
                    return false;
                }

                if (this.timeDimension != that.TimeDimension)
                {
                    return false;
                }

                if (this.Position != that.Position)
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.conceptRole, that.ConceptRole))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get parent ids.
        /// </summary>
        /// <param name="includeDifferentTypes">
        /// The include different types. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        protected internal override IList<string> GetParentIds(bool includeDifferentTypes)
        {
            IList<string> returnList = new List<string>();
            returnList.Add(this.Id);
            return returnList;
        }

        /// <summary>
        /// Returns concept scheme version. It tries to detect various conventions
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        /// <returns>
        /// The concept scheme version; otherwise null
        /// </returns>
        private static string GetConceptSchemeVersion(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TimeDimensionType dimension)
        {
            if (!string.IsNullOrWhiteSpace(dimension.conceptVersion))
            {
                return dimension.conceptVersion;
            }

            if (!string.IsNullOrWhiteSpace(dimension.ConceptSchemeVersionEstat))
            {
                return dimension.ConceptSchemeVersionEstat;
            }

            var extDimension = dimension as Org.Sdmx.Resources.SdmxMl.Schemas.V20.extension.structure.TimeDimensionType;
            if (extDimension != null && !string.IsNullOrWhiteSpace(extDimension.conceptSchemeVersion))
            {
                return extDimension.conceptSchemeVersion;
            }

            return null;
        }

        /// <summary>
        /// Returns concept scheme version. It tries to detect various conventions
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        /// <returns>
        /// The concept scheme version; otherwise null
        /// </returns>
        private static string GetConceptSchemeVersion(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.DimensionType dimension)
        {
            if (!string.IsNullOrWhiteSpace(dimension.conceptVersion))
            {
                return dimension.conceptVersion;
            }

            if (!string.IsNullOrWhiteSpace(dimension.ConceptSchemeVersionEstat))
            {
                return dimension.ConceptSchemeVersionEstat;
            }

            var extDimension = dimension as Org.Sdmx.Resources.SdmxMl.Schemas.V20.extension.structure.DimensionType;
            if (extDimension != null && !string.IsNullOrWhiteSpace(extDimension.conceptSchemeVersion))
            {
                return extDimension.conceptSchemeVersion;
            }

            return null;
        }

        /// <summary>
        ///   The validate dimension.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void ValidateDimension()
        {
            // FUNC 2.1 validate only the allowed text format attributes have been set
            if (this.timeDimension)
            {
                this.Id = DimensionObject.TimeDimensionFixedId;
                if (this.HasCodedRepresentation())
                {
                    // This is exception is comment out because it was requested by Eurostat to allow coded TimeDimensions. 
                    // See CITnet JIRA ticket SDMXRI-22:
                    // https://webgate.ec.europa.eu/CITnet/jira/browse/SDMXRI-22
                    // Also the original ticket in Agilis JIRA: http://www.agilis-sa.gr:8070/browse/SODIHD-1209
                    // Also read the documentation at  sdmx_maintenance > 2013 > QTM 2013 budget Framework Contract 60402.2012.001-2012.112 LOT2 > 2013.433-SC000030-FC112Lot2-SDMX_tools_corrective_and_evolutive_maintenance-QTM > QTM-2 > Deliverables 
                    // CIRCABC link: https://circabc.europa.eu/w/browse/91272b61-b03a-431f-ae91-75db31e324cf
                    // This is part of QTM-4/2014. 
                    // based on the above. DO NOT UNCOMMENT the following line when sync with SdmxSource.Java:
                    // throw new SdmxSemmanticException("Time Dimensions can not have coded representation");
                    _log.Warn("Time Dimensions can not have coded representation.");
                }
                else if (this.LocalRepresentation == null || this.LocalRepresentation.TextFormat == null)
                {
                    IRepresentationMutableObject rep = new RepresentationMutableCore();
                    rep.TextFormat = new TextFormatMutableCore();
                    this.LocalRepresentation = new RepresentationCore(rep, this);
                }
                else if (this.LocalRepresentation.TextFormat.TextType != null)
                {
                    if (!this.LocalRepresentation.TextFormat.TextType.IsValidTimeDimensionTextType)
                    {
                        // STRIP THE INVALID TEXT FORMAT
                        _log.Warn(
                            "Invalid Text Format found on Time Dimensions, removing Text Format : "
                            + this.LocalRepresentation.TextFormat.TextType);
                        IRepresentationMutableObject representationMutableCore = new RepresentationMutableCore();
                        representationMutableCore.TextFormat = new TextFormatMutableCore();
                        this.LocalRepresentation = new RepresentationCore(representationMutableCore, this);
                    }
                }
            }
            else
            {
                if (this.Id.Equals(DimensionObject.TimeDimensionFixedId))
                {
                    throw new SdmxSemmanticException(
                        "Only the Time Dimensions can have the id: "
                        + DimensionObject.TimeDimensionFixedId);
                }
            }

            if (this.measureDimension)
            {
                base.StructureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDimension);

                if (this.Representation == null || this.Representation.Representation == null)
                {
                    throw new SdmxSemmanticException("Measure Dimensions missing representation");
                }

                if (this.MaintainableParent is ICrossSectionalDataStructureObject)
                {
                    // Ignore this
                }
                else
                {
                    if (this.Representation.Representation.TargetReference.EnumType
                        != SdmxStructureEnumType.ConceptScheme)
                    {
                        throw new SdmxSemmanticException(
                            "Measure Dimensions representation must reference a concept scheme, currently it references a "
                            + this.Representation.Representation.TargetReference.GetType());
                    }
                }
            }

            if (this.measureDimension && this.timeDimension)
            {
                throw new SdmxSemmanticException("Dimensions can not be both a measure dimension and a time dimension");
            }
        }

        #endregion
    }
}