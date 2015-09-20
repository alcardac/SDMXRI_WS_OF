// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attributeObject core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using AttributeType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.AttributeType;
    using PrimaryMeasure = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.PrimaryMeasure;

    /// <summary>
    ///   The attributeObject core.
    /// </summary>
    [Serializable]
    public class AttributeObjectCore : ComponentCore, IAttributeObject
    {
        #region Fields

        /// <summary>
        ///   The assignment status.
        /// </summary>
        private readonly string assignmentStatus;

        /// <summary>
        ///   The attachment group.
        /// </summary>
        private readonly string attachmentGroup;

        /// <summary>
        ///   The concept roles.
        /// </summary>
        private readonly IList<ICrossReference> conceptRoles;

        /// <summary>
        ///   The dimension reference.
        /// </summary>
        private readonly IList<string> dimensionReferences;

        /// <summary>
        ///   The attachment level.
        /// </summary>
        private AttributeAttachmentLevel attachmentLevel;

        /// <summary>
        ///   The primary measure reference.
        /// </summary>
        private string primaryMeasureReference;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeObjectCore"/> class.
        /// </summary>
        /// <param name="attribute">
        /// The attributeObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public AttributeObjectCore(IAttributeMutableObject attribute, IAttributeList parent)
            : base(attribute, parent)
        {
            this.dimensionReferences = new List<string>();
            this.conceptRoles = new List<ICrossReference>();
            try
            {
                this.attachmentLevel = attribute.AttachmentLevel;
                this.assignmentStatus = attribute.AssignmentStatus;
                this.attachmentGroup = attribute.AttachmentGroup;
                if (attribute.DimensionReferences != null)
                {
                    this.dimensionReferences = new List<string>(attribute.DimensionReferences);
                }

                if (attribute.ConceptRoles != null)
                {
                    foreach (IStructureReference currentConceptRole in attribute.ConceptRoles)
                    {
                        this.conceptRoles.Add(new CrossReferenceImpl(this, currentConceptRole));
                    }
                }

                this.primaryMeasureReference = attribute.PrimaryMeasureReference;
                this.ValidateAttributeAttributes();
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), th);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeObjectCore"/> class.
        /// </summary>
        /// <param name="attribute">
        /// The attributeObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public AttributeObjectCore(AttributeType attribute, IAttributeList parent)
            : base(attribute, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute), parent)
        {
            this.dimensionReferences = new List<string>();
            this.conceptRoles = new List<ICrossReference>();
            if (attribute.AttributeRelationship != null)
            {
                AttributeRelationshipType attRelationShip = attribute.AttributeRelationship;
                if (attRelationShip.Group != null)
                {
                    // FUNC - this can also be attached to a dimension list
                    this.attachmentGroup = RefUtil.CreateLocalIdReference(attRelationShip.Group);
                    this.attachmentLevel = AttributeAttachmentLevel.Group;
                }
                else if (ObjectUtil.ValidCollection(attRelationShip.Dimension))
                {
                    // DEFAULT TO DIMENSION GROUP
                    this.attachmentLevel = AttributeAttachmentLevel.DimensionGroup;

                    foreach (LocalDimensionReferenceType dim in attRelationShip.Dimension)
                    {
                        this.dimensionReferences.Add(RefUtil.CreateLocalIdReference(dim));
                    }

                    var parentDsd = (IDataStructureObject)this.MaintainableParent;

                    foreach (IDimension currentDimension in parentDsd.GetDimensions())
                    {
                        if (this.dimensionReferences.Contains(currentDimension.Id))
                        {
                            if (currentDimension.TimeDimension)
                            {
                                // REFERENCING THE TIME DIMENSION THEREFOR OBSERVATION LEVEL
                                this.attachmentLevel = AttributeAttachmentLevel.Observation;
                                break;
                            }
                        }
                    }

                    if (ObjectUtil.ValidCollection(attRelationShip.AttachmentGroup))
                    {
                        this.attachmentGroup = RefUtil.CreateLocalIdReference(attRelationShip.AttachmentGroup.First());
                        this.attachmentLevel = AttributeAttachmentLevel.Group;
                    }
                }
                else if (attRelationShip.PrimaryMeasure != null)
                {
                    this.primaryMeasureReference = RefUtil.CreateLocalIdReference(attRelationShip.PrimaryMeasure);
                    this.attachmentLevel = AttributeAttachmentLevel.Observation;
                }
                else
                {
                    this.attachmentLevel = AttributeAttachmentLevel.DataSet;
                }
            }

            this.assignmentStatus = attribute.assignmentStatus;
            if (attribute.ConceptRole != null)
            {
                foreach (ConceptReferenceType conceptRef in attribute.ConceptRole)
                {
                    this.conceptRoles.Add(RefUtil.CreateReference(this, conceptRef));
                }
            }

            try
            {
                this.ValidateAttributeAttributes();
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), th);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeObjectCore"/> class.
        /// </summary>
        /// <param name="attribute">
        /// The attributeObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public AttributeObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.AttributeType attribute, IAttributeList parent)
            : base(
                attribute, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute), 
                attribute.Annotations, 
                attribute.TextFormat, 
                attribute.codelistAgency, 
                attribute.codelist, 
                attribute.codelistVersion, 
                attribute.conceptSchemeAgency, 
                attribute.conceptSchemeRef,
                GetConceptSchemeVersion(attribute), 
                attribute.conceptAgency, 
                attribute.conceptRef, 
                parent)
        {
            this.dimensionReferences = new List<string>();
            this.conceptRoles = new List<ICrossReference>();

            if (attribute.AttachmentGroup != null)
            {
                if (attribute.AttachmentGroup.Count > 1)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.Unsupported, "Attribute with more then one group attachment");
                }

                if (attribute.AttachmentGroup.Count == 1)
                {
                    this.attachmentGroup = attribute.AttachmentGroup[0];
                }
            }

            AttributeAttachmentLevel attachmentLevel0 = default(AttributeAttachmentLevel) /* was: null */;
            var parentDsd = (IDataStructureObject)this.MaintainableParent;

            switch (attribute.attachmentLevel)
            {
                case AttachmentLevelTypeConstants.DataSet:
                    attachmentLevel0 = AttributeAttachmentLevel.DataSet;
                    break;
                case AttachmentLevelTypeConstants.Group:
                    attachmentLevel0 = AttributeAttachmentLevel.Group;
                    break;
                case AttachmentLevelTypeConstants.Observation:
                    attachmentLevel0 = AttributeAttachmentLevel.Observation;
                    this.primaryMeasureReference = parentDsd.PrimaryMeasure.Id;
                    break;
                case AttachmentLevelTypeConstants.Series:
                    attachmentLevel0 = AttributeAttachmentLevel.DimensionGroup;
                    {
                        foreach (IDimension currentDimension in parentDsd.GetDimensions(SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension))
                        {
                            if (!this.dimensionReferences.Contains(currentDimension.Id))
                            {
                                this.dimensionReferences.Add(currentDimension.Id);
                            }
                        }
                    }

                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Attachment:" + attribute.attachmentLevel);
            }

            this.attachmentLevel = attachmentLevel0;
            this.assignmentStatus = attribute.assignmentStatus;

            try
            {
                this.ValidateAttributeAttributes();
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), th);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeObjectCore"/> class.
        /// </summary>
        /// <param name="attribute">
        /// The attributeObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public AttributeObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.AttributeType attribute, IAttributeList parent)
            : base(
                attribute, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute), 
                attribute.Annotations, 
                attribute.codelist, 
                attribute.concept, 
                parent)
        {
            this.dimensionReferences = new List<string>();
            this.conceptRoles = new List<ICrossReference>();

            if (attribute.AttachmentGroup != null)
            {
                if (attribute.AttachmentGroup.Count > 1)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.Unsupported, "Attribute with more then one group attachment");
                }

                if (attribute.AttachmentGroup.Count == 1)
                {
                    this.attachmentGroup = attribute.AttachmentGroup[0];
                }
            }

            AttributeAttachmentLevel attachmentLevel0 = default(AttributeAttachmentLevel);
            var parentDsd = (IDataStructureObject)this.MaintainableParent;

            switch (attribute.attachmentLevel)
            {
                case Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.AttachmentLevelTypeConstants.DataSet:
                    attachmentLevel0 = AttributeAttachmentLevel.DataSet;
                    break;
                case Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.AttachmentLevelTypeConstants.Group:
                    attachmentLevel0 = AttributeAttachmentLevel.Group;
                    break;
                case Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.AttachmentLevelTypeConstants.Observation:
                    attachmentLevel0 = AttributeAttachmentLevel.Observation;
                    this.primaryMeasureReference = parentDsd.PrimaryMeasure.Id;
                    break;
                case Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.AttachmentLevelTypeConstants.Series:
                    attachmentLevel0 = AttributeAttachmentLevel.DimensionGroup;
                    {
                        foreach (IDimension currentDimension in parentDsd.GetDimensions(SdmxStructureEnumType.Dimension))
                        {
                            if (!this.dimensionReferences.Contains(currentDimension.Id))
                            {
                                this.dimensionReferences.Add(currentDimension.Id);
                            }
                        }
                    }

                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Attachment:" + attribute.attachmentLevel);
            }

            this.attachmentLevel = attachmentLevel0;
            this.assignmentStatus = attribute.assignmentStatus;

            try
            {
                this.ValidateAttributeAttributes();
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this.ToString(), th);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the assignment status.
        /// </summary>
        public virtual string AssignmentStatus
        {
            get
            {
                return this.assignmentStatus;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the attachment group.
        /// </summary>
        public virtual string AttachmentGroup
        {
            get
            {
                return this.attachmentGroup;
            }
        }

        /// <summary>
        ///   Gets the attachment level.
        /// </summary>
        public virtual AttributeAttachmentLevel AttachmentLevel
        {
            get
            {
                return this.attachmentLevel;
            }
        }

        /// <summary>
        ///   Gets the concept roles.
        /// </summary>
        public virtual IList<ICrossReference> ConceptRoles
        {
            get
            {
                return new List<ICrossReference>(this.conceptRoles);
            }
        }

        /// <summary>
        ///   Gets the dimension reference.
        /// </summary>
        public virtual IList<string> DimensionReferences
        {
            get
            {
                return this.dimensionReferences;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether mandatory.
        /// </summary>
        public virtual bool Mandatory
        {
            get
            {
                return this.assignmentStatus.Equals("Mandatory");
            }
        }

        /// <summary>
        ///   Gets the primary measure reference.
        /// </summary>
        public virtual string PrimaryMeasureReference
        {
            get
            {
                return this.primaryMeasureReference;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether time format.
        /// </summary>
        public virtual bool TimeFormat
        {
            get
            {
                return this.Id.Equals("TIME_FORMAT");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
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
                var that = (IAttributeObject)sdmxObject;
                if (!ObjectUtil.Equivalent(this.attachmentLevel, that.AttachmentLevel))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.assignmentStatus, that.AssignmentStatus))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.attachmentGroup, that.AttachmentGroup))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.dimensionReferences, that.DimensionReferences))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.conceptRoles, that.ConceptRoles))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.primaryMeasureReference, that.PrimaryMeasureReference))
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
        ///   The validate attributeObject attributes.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void ValidateAttributeAttributes()
        {
            if (this.attachmentLevel == AttributeAttachmentLevel.Null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredAttribute, this.StructureType, "attachmentLevel");
            }

            if (this.attachmentLevel == AttributeAttachmentLevel.Observation)
            {
                if (!ObjectUtil.ValidCollection(this.dimensionReferences))
                {
                    // Ensure that the primary measure reference is set if the dimension references are empty
                    this.primaryMeasureReference = PrimaryMeasure.FixedId;
                }
                else
                {
                    var parentDsd = (IDataStructureObject)this.MaintainableParent;
                    IDimension timeDimension = parentDsd.TimeDimension;
                    if (timeDimension == null)
                    {
                        // This is not an observation attributeObject as it does not include the time dimension
                        this.attachmentLevel = AttributeAttachmentLevel.DimensionGroup;
                    }
                    else
                    {
                        string timeDimensionId = timeDimension.Id;
                        if (!this.dimensionReferences.Contains(timeDimensionId))
                        {
                            // This is not an observation attributeObject as it does not include the time dimension
                            this.attachmentLevel = AttributeAttachmentLevel.DimensionGroup;
                        }
                    }
                }
            }

            if (this.assignmentStatus == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredAttribute, this.StructureType, "assignmentStatus");
            }
            //TODO: Check err messages
            if (string.IsNullOrWhiteSpace(this.attachmentGroup) && this.attachmentLevel == AttributeAttachmentLevel.Group)
            {
                throw new SdmxSemmanticException(
                    "Attribute has specified attachment level specified as Group, but specifies the attachment groups. Either change the attachment level to group, or remove the attachment groups off the attributeObject : "
                    + this.Id);
            }

            if (!string.IsNullOrWhiteSpace(this.attachmentGroup) && this.attachmentLevel != AttributeAttachmentLevel.Group)
            {
                throw new SdmxSemmanticException(
                    "Attribute specifies an attachment group of '" + this.attachmentGroup
                    + "', but the the attachement level is not set to GROUP, it is set to  '" + this.attachmentLevel
                    + "'.  Either remove the attributeObject's reference to the '" + this.attachmentGroup
                    + "' or change the attributeObject's attachment level to GROUP");
            }

            if (this.attachmentLevel == AttributeAttachmentLevel.DimensionGroup)
            {
                if (!ObjectUtil.ValidCollection(this.dimensionReferences))
                {
                    throw new SdmxSemmanticException(
                        "Attribute specifies attachment level of 'Dimensions Group' but does not specify any dimensions");
                }
            }

            foreach (ICrossReference conceptRole in conceptRoles)
            {
                if (conceptRole.TargetReference != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept))
                {
                    throw new SdmxSemmanticException("Illegal concept role '" + conceptRole.TargetUrn + "'.  Concept Role must reference a concept.");
                }
            }
        }


        /// <summary>
        /// Returns concept scheme version. It tries to detect various conventions
        /// </summary>
        /// <param name="attributeType">
        /// The attribute Type.
        /// </param>
        /// <returns>
        /// The concept scheme version; otherwise null
        /// </returns>
        private static string GetConceptSchemeVersion(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.AttributeType attributeType)
        {
            if (!string.IsNullOrWhiteSpace(attributeType.conceptVersion))
            {
                return attributeType.conceptVersion;
            }

            if (!string.IsNullOrWhiteSpace(attributeType.ConceptSchemeVersionEstat))
            {
                return attributeType.ConceptSchemeVersionEstat;
            }

            var extDimension = attributeType as Org.Sdmx.Resources.SdmxMl.Schemas.V20.extension.structure.AttributeType;
            if (extDimension != null && !string.IsNullOrWhiteSpace(extDimension.conceptSchemeVersion))
            {
                return extDimension.conceptSchemeVersion;
            }

            return null;
        }


        #endregion
    }
}