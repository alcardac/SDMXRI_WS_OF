// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds a v2 AttributeType from a schema independent IAttributeObject
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     Builds a v2 AttributeType from a schema independent IAttributeObject
    /// </summary>
    public class AttributeXmlBuilder : AbstractBuilder, IBuilder<AttributeType, IAttributeObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="AttributeType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="AttributeType"/> from <paramref name="buildFrom"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Unknown assignment status
        ///     -or-
        ///     Unknown attachment level
        /// </exception>
        public virtual AttributeType Build(IAttributeObject buildFrom)
        {
            var builtObj = new AttributeType();
            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            string str0 = buildFrom.AssignmentStatus;
            if (!string.IsNullOrWhiteSpace(str0))
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
                    throw new ArgumentException("Unknown attachment level: " + buildFrom.AssignmentStatus);
            }

            if (buildFrom.HasCodedRepresentation())
            {
                IMaintainableRefObject maintRef = buildFrom.Representation.Representation.MaintainableReference;
                string str1 = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(str1))
                {
                    builtObj.codelist = maintRef.MaintainableId;
                }

                string str2 = maintRef.AgencyId;
                if (!string.IsNullOrWhiteSpace(str2))
                {
                    builtObj.codelistAgency = maintRef.AgencyId;
                }

                string str3 = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    builtObj.codelistVersion = maintRef.Version;
                }
            }

            if (buildFrom.ConceptRef != null)
            {
                IMaintainableRefObject maintRef0 = buildFrom.ConceptRef.MaintainableReference;
                string str2 = maintRef0.AgencyId;
                if (!string.IsNullOrWhiteSpace(str2))
                {
                    builtObj.conceptSchemeAgency = maintRef0.AgencyId;
                }

                string str3 = maintRef0.MaintainableId;
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    builtObj.conceptSchemeRef = maintRef0.MaintainableId;
                }

                string str4 = buildFrom.ConceptRef.ChildReference.Id;
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    builtObj.conceptRef = buildFrom.ConceptRef.ChildReference.Id;
                }

                string str1 = maintRef0.Version;
                if (!string.IsNullOrWhiteSpace(str1))
                {
                    builtObj.conceptVersion = maintRef0.Version;
                }
            }

            if (buildFrom.Representation != null && buildFrom.Representation.TextFormat != null)
            {
                var textFormatType = new TextFormatType();
                this.PopulateTextFormatType(textFormatType, buildFrom.Representation.TextFormat);
                builtObj.TextFormat = textFormatType;
            }

            if (buildFrom.Id.Equals("TimeFormat"))
            {
                builtObj.isTimeFormat = true;
            }

            return builtObj;
        }

        #endregion
    }
}