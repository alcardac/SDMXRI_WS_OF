// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataStructureDefinitionXmlsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata structure definition xml beans builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///     The metadata structure definition xml beans builder.
    /// </summary>
    public class MetadataStructureDefinitionXmlsBuilder : AbstractBuilder, IBuilder<MetadataStructureDefinitionType, IMetadataStructureDefinitionObject>
    {
        /// <summary>
        /// The _report structure XML builder
        /// </summary>
        private readonly ReportStructureXmlBuilder _reportStructureXmlBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionXmlsBuilder"/> class.
        /// </summary>
        /// <param name="reportStructureXmlBuilder">The report structure XML builder.</param>
        public MetadataStructureDefinitionXmlsBuilder(ReportStructureXmlBuilder reportStructureXmlBuilder)
        {
            this._reportStructureXmlBuilder = reportStructureXmlBuilder ?? new ReportStructureXmlBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionXmlsBuilder"/> class.
        /// </summary>
        public MetadataStructureDefinitionXmlsBuilder()
            : this(null)
        {
        }

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="MetadataStructureDefinitionType"/> from <paramref name="buildFrom"/>.
        ///     NOT IMPLEMENTED.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="MetadataStructureDefinitionType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual MetadataStructureDefinitionType Build(IMetadataStructureDefinitionObject buildFrom)
        {
            var builtObj = new MetadataStructureDefinitionType();
            if (!string.IsNullOrWhiteSpace(buildFrom.AgencyId))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            if (!string.IsNullOrWhiteSpace(buildFrom.Id))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }
            else if (buildFrom.StructureUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }
            else if (buildFrom.ServiceUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }

            builtObj.urn = buildFrom.Urn;

            string str0 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.version = buildFrom.Version;
            }

            if (buildFrom.StartDate != null)
            {
                builtObj.validFrom = buildFrom.StartDate.Date;
            }

            if (buildFrom.EndDate != null)
            {
                builtObj.validTo = buildFrom.EndDate.Date;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                builtObj.Description = this.GetTextType(descriptions);
            }

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            if (buildFrom.IsFinal.IsSet())
            {
                builtObj.isFinal = buildFrom.IsFinal.IsTrue;
            }

            if (ObjectUtil.ValidCollection(buildFrom.MetadataTargets))
            {
                TargetIdentifiersType targetIdentifiers = new TargetIdentifiersType();
                builtObj.TargetIdentifiers = targetIdentifiers;

                // find the metadata target which has the superset of identifier components, it will act as FullTargetIdentifier
                int maxMetadataTargetSize = 0;
                int fullTargetPosition = 0;
                IMetadataTarget candidateFullTarget = null;
                var partialTargets = new PartialTargetIdentifierType[buildFrom.MetadataTargets.Count];

                for (int i = 0; i < buildFrom.MetadataTargets.Count; i++)
                {
                    var metadataTarget = buildFrom.MetadataTargets[i];
                    if (metadataTarget.KeyDescriptorValuesTarget != null || metadataTarget.ReportPeriodTarget != null || metadataTarget.ConstraintContentTarget != null
                        || metadataTarget.DataSetTarget != null)
                    {
                        throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "MSD contains Metadata Target content incompatible with SMDX v2.0 - please use SDMX v2.1");
                    }

                    if (metadataTarget.IdentifiableTarget.Count >= maxMetadataTargetSize)
                    {
                        maxMetadataTargetSize = metadataTarget.IdentifiableTarget.Count;
                        fullTargetPosition = i;
                        candidateFullTarget = metadataTarget;
                    }

                    partialTargets[i] = this.BuildPartialTargetIdentifier(metadataTarget);
                }

                targetIdentifiers.PartialTargetIdentifier.AddAll(partialTargets);
                targetIdentifiers.PartialTargetIdentifier.RemoveAt(fullTargetPosition);
                targetIdentifiers.FullTargetIdentifier = new FullTargetIdentifierType();
                this.PopulateFullTargetIdentifier(candidateFullTarget, targetIdentifiers.FullTargetIdentifier);
            }

            if (ObjectUtil.ValidCollection(buildFrom.ReportStructures))
            {
                foreach (var reportStructure in buildFrom.ReportStructures)
                {
                    builtObj.ReportStructure.Add(this._reportStructureXmlBuilder.Build(reportStructure));
                }
            }

            // needs to be last in .NET
            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            return builtObj;
        }

        #endregion

        /// <summary>
        /// Builds the partial target identifier.
        /// </summary>
        /// <param name="buildFrom">The build from.</param>
        /// <returns>
        /// The <see cref="PartialTargetIdentifierType"/>
        /// </returns>
        private PartialTargetIdentifierType BuildPartialTargetIdentifier(IMetadataTarget buildFrom)
        {
            var builtObj = new PartialTargetIdentifierType();
            if (!string.IsNullOrWhiteSpace(buildFrom.Id))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            if (buildFrom.Urn != null)
            {
                builtObj.urn = buildFrom.Urn;
            }

            var textType = new TextType();
            this.SetDefaultText(textType);
            builtObj.Name.Add(textType);

            if (ObjectUtil.ValidCollection(buildFrom.IdentifiableTarget))
            {
                foreach (var identifiableTarget in buildFrom.IdentifiableTarget)
                {
                    builtObj.IdentifierComponentRef.Add(identifiableTarget.Id);
                }
            }

            // needs to be last in .NET
            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            return builtObj;
        }

        private void PopulateFullTargetIdentifier(IMetadataTarget buildFrom, FullTargetIdentifierType builtObj)
        {
            if (!string.IsNullOrWhiteSpace(buildFrom.Id))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            if (buildFrom.Urn != null)
            {
                builtObj.urn = buildFrom.Urn;
            }

            var textType = new TextType();
            this.SetDefaultText(textType);
            builtObj.Name.Add(textType);

            if (ObjectUtil.ValidCollection(buildFrom.IdentifiableTarget))
            {
                foreach (var identifiableTarget in buildFrom.IdentifiableTarget)
                {
                    builtObj.IdentifierComponent.Add(this.buildIdentifierComponent(identifiableTarget));
                }
            }

            // needs to be last in .NET
            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        private IdentifierComponentType buildIdentifierComponent(IIdentifiableTarget buildFrom)
        {
            var builtObj = new IdentifierComponentType();

            if (!string.IsNullOrWhiteSpace(buildFrom.Id))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            if (buildFrom.Urn != null)
            {
                builtObj.urn = buildFrom.Urn;
            }

            var textType = new TextType();
            this.SetDefaultText(textType);
            builtObj.Name.Add(textType);

            if (buildFrom.ReferencedStructureType != null)
            {
                var targetObjectClass = buildFrom.ReferencedStructureType == SdmxStructureEnumType.TimeDimension
                                            ? SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension)
                                            : buildFrom.ReferencedStructureType;

                builtObj.TargetObjectClass = XmlobjectsEnumUtil.GetSdmxObjectIdType(targetObjectClass);
            }

            if (buildFrom.HasCodedRepresentation())
            {
                var representationSchemeTarget = new RepresentationSchemeType();
                builtObj.RepresentationScheme = representationSchemeTarget;

                var reference = buildFrom.Representation.Representation;
                if (reference != null)
                {
                    if (reference.HasMaintainableId())
                    {
                        representationSchemeTarget.representationScheme = reference.MaintainableId;
                    }

                    if (reference.HasAgencyId())
                    {
                        representationSchemeTarget.representationSchemeAgency = reference.AgencyId;
                    }

                    representationSchemeTarget.representationSchemeType1 = XmlobjectsEnumUtil.GetSdmxRepresentationSchemeType(reference.TargetReference);
                }
            }

            // needs to be last in .NET
            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            return builtObj;
        }
    }
}