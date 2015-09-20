// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds a v1 CategorySchemeType from a schema independent ICategorySchemeObject
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     Builds a v1 CategorySchemeType from a schema independent ICategorySchemeObject
    /// </summary>
    public class AttributeXmlBuilder : AbstractBuilder, IBuilder<AttributeType, IAttributeObject>
    {
        #region Constants

        /// <summary>
        ///     The time format id.
        /// </summary>
        private const string TimeformatId = "TimeFormat";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="AttributeXmlBuilder" /> class.
        /// </summary>
        static AttributeXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(AttributeXmlBuilder));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="AttributeType"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Unknown assignment status
        /// </exception>
        public virtual AttributeType Build(IAttributeObject buildFrom)
        {
            var builtObj = new AttributeType();
            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            string assignmentStatus = buildFrom.AssignmentStatus;
            if (!string.IsNullOrWhiteSpace(assignmentStatus))
            {
                switch (buildFrom.AssignmentStatus)
                {
                    case AssignmentStatusTypeConstants.Conditional:
                    case AssignmentStatusTypeConstants.Mandatory:
                        builtObj.assignmentStatus = buildFrom.AssignmentStatus;
                        break;
                    default:
                        throw new ArgumentException("Unknown assignment status: " + buildFrom.AssignmentStatus);
                }
            }

            string value = buildFrom.AttachmentGroup;
            if (!string.IsNullOrWhiteSpace(value))
            {
                var arr = new string[1];
                arr[0] = buildFrom.AttachmentGroup;
                builtObj.AttachmentGroup = arr;
            }

            if (buildFrom.AttachmentLevel != AttributeAttachmentLevel.Null)
            {
                switch (buildFrom.AttachmentLevel)
                {
                    case AttributeAttachmentLevel.DataSet:
                        builtObj.attachmentLevel = AttachmentLevelTypeConstants.DataSet;
                        break;
                    case AttributeAttachmentLevel.DimensionGroup:
                        builtObj.attachmentLevel = AttachmentLevelTypeConstants.Series;
                        break;
                    case AttributeAttachmentLevel.Observation:
                        builtObj.attachmentLevel = AttachmentLevelTypeConstants.Observation;
                        break;
                    case AttributeAttachmentLevel.Group:
                        builtObj.attachmentLevel = AttachmentLevelTypeConstants.Group;
                        break;
                    default:
                        throw new ArgumentException("Unknown attachment level: " + buildFrom.AttachmentLevel);
                }
            }

            if (buildFrom.HasCodedRepresentation())
            {
                builtObj.codelist = buildFrom.Representation.Representation.MaintainableReference.MaintainableId;
            }

            if (buildFrom.ConceptRef != null)
            {
                builtObj.concept = ConceptRefUtil.GetConceptId(buildFrom.ConceptRef);
            }

            if (buildFrom.Id.Equals(TimeformatId))
            {
                builtObj.isTimeFormat = true;
            }

            if (buildFrom.Representation != null && buildFrom.Representation.TextFormat != null)
            {
                var textFormatType = new TextFormatType();
                this.PopulateTextFormatType(textFormatType, buildFrom.Representation.TextFormat);
                builtObj.TextFormat = textFormatType;
            }

            return builtObj;
        }

        #endregion
    }
}