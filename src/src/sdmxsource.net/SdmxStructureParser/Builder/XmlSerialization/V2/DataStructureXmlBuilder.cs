// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data structure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The data structure xml bean builder.
    /// </summary>
    public class DataStructureXmlBuilder : AbstractBuilder, IBuilder<KeyFamilyType, IDataStructureObject>
    {
        #region Fields

        /// <summary>
        ///     The attribute xml bean builder.
        /// </summary>
        private readonly AttributeXmlBuilder _attributeXmlBuilder = new AttributeXmlBuilder();

        /// <summary>
        ///     The cross sectional xml bean builder.
        /// </summary>
        private readonly CrossSectionalMeasureXmlBuilder _crossSectionalXmlBuilder =
            new CrossSectionalMeasureXmlBuilder();

        /// <summary>
        ///     The dimension xml bean builder.
        /// </summary>
        private readonly DimensionXmlBuilder _dimensionXmlBuilder = new DimensionXmlBuilder();

        /// <summary>
        ///     The group xml beans builder.
        /// </summary>
        private readonly GroupXmlsBuilder _groupXmlsBuilder = new GroupXmlsBuilder();

        /// <summary>
        ///     The primary measure xml bean builder.
        /// </summary>
        private readonly PrimaryMeasureXmlBuilder _primaryMeasureXmlBuilder = new PrimaryMeasureXmlBuilder();

        /// <summary>
        ///     The time dimension xml bean builder.
        /// </summary>
        private readonly TimeDimensionXmlBuilder _timeDimensionXmlBuilder = new TimeDimensionXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="KeyFamilyType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="KeyFamilyType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual KeyFamilyType Build(IDataStructureObject buildFrom)
        {
            var builtObj = new KeyFamilyType();
            string str2 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string str3 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str3))
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

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            if (buildFrom.IsFinal.IsSet())
            {
                builtObj.isFinal = buildFrom.IsFinal.IsTrue;
            }

            
            ComponentsType componentsType = new ComponentsType();
            if (buildFrom.PrimaryMeasure != null)
            {
                builtObj.Components = componentsType;
            }

            IList<IComponent> crossSectionalAttachDataSet = new List<IComponent>();
            IList<IComponent> crossSectionalAttachGroup = new List<IComponent>();
            IList<IComponent> crossSectionalAttachSection = new List<IComponent>();
            IList<IComponent> crossSectionalAttachObservation = new List<IComponent>();

            var crossSectionalDataStructureObject = buildFrom as ICrossSectionalDataStructureObject;
            if (crossSectionalDataStructureObject != null)
            {
                crossSectionalAttachDataSet = crossSectionalDataStructureObject.GetCrossSectionalAttachDataSet(false);
                crossSectionalAttachGroup = crossSectionalDataStructureObject.GetCrossSectionalAttachGroup(false);
                crossSectionalAttachSection = crossSectionalDataStructureObject.GetCrossSectionalAttachSection(false);
                crossSectionalAttachObservation = crossSectionalDataStructureObject.GetCrossSectionalAttachObservation();

                /* foreach */
                foreach (
                    ICrossSectionalMeasure currentMeasure in crossSectionalDataStructureObject.CrossSectionalMeasures)
                {
                    componentsType.CrossSectionalMeasure.Add(this._crossSectionalXmlBuilder.Build(currentMeasure));
                }
            }

            IList<IDimension> currentDims = buildFrom.GetDimensions(
                SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension);
            if (ObjectUtil.ValidCollection(currentDims))
            {
                /* foreach */
                foreach (IDimension currentDim in currentDims)
                {
                    DimensionType newDimension = this._dimensionXmlBuilder.Build(currentDim);
                    if (crossSectionalDataStructureObject != null && currentDim.MeasureDimension)
                    {
                        ICrossReference xsRef =
                            crossSectionalDataStructureObject.GetCodelistForMeasureDimension(currentDim.Id);
                        newDimension.codelist = xsRef.MaintainableReference.MaintainableId;
                        newDimension.codelistAgency = xsRef.MaintainableReference.AgencyId;
                        newDimension.codelistVersion = xsRef.MaintainableReference.Version;
                    }

                    if (crossSectionalAttachDataSet.Contains(currentDim))
                    {
                        newDimension.crossSectionalAttachDataSet = true;
                    }

                    if (crossSectionalAttachGroup.Contains(currentDim))
                    {
                        newDimension.crossSectionalAttachGroup = true;
                    }

                    if (crossSectionalAttachSection.Contains(currentDim))
                    {
                        newDimension.crossSectionalAttachSection = true;
                    }

                    if (crossSectionalAttachObservation.Contains(currentDim))
                    {
                        newDimension.crossSectionalAttachObservation = true;
                    }

                    componentsType.Dimension.Add(newDimension);
                }
            }

            if (buildFrom.TimeDimension != null)
            {
                TimeDimensionType newDimension0 = this._timeDimensionXmlBuilder.Build(buildFrom.TimeDimension);
                if (crossSectionalAttachDataSet.Contains(buildFrom.TimeDimension))
                {
                    newDimension0.crossSectionalAttachDataSet = true;
                }

                if (crossSectionalAttachGroup.Contains(buildFrom.TimeDimension))
                {
                    newDimension0.crossSectionalAttachGroup = true;
                }

                if (crossSectionalAttachSection.Contains(buildFrom.TimeDimension))
                {
                    newDimension0.crossSectionalAttachSection = true;
                }

                if (crossSectionalAttachObservation.Contains(buildFrom.TimeDimension))
                {
                    newDimension0.crossSectionalAttachObservation = true;
                }

                componentsType.TimeDimension = newDimension0;
            }

            IList<IGroup> currentGroups = buildFrom.Groups;
            if (ObjectUtil.ValidCollection(currentGroups))
            {
                /* foreach */
                foreach (IGroup currentGroup in currentGroups)
                {
                    componentsType.Group.Add(this._groupXmlsBuilder.Build(currentGroup));
                }
            }

            if (buildFrom.PrimaryMeasure != null)
            {
                componentsType.PrimaryMeasure = this._primaryMeasureXmlBuilder.Build(buildFrom.PrimaryMeasure);
            }

            IList<IAttributeObject> currentAttrs = buildFrom.Attributes;
            if (ObjectUtil.ValidCollection(currentAttrs))
            {
                /* foreach */
                foreach (IAttributeObject currentAttr in currentAttrs)
                {
                    AttributeType newAttribute = this._attributeXmlBuilder.Build(currentAttr);

                    if (currentAttr.AttachmentLevel == AttributeAttachmentLevel.DimensionGroup)
                    {
                        //If the group of dimensions is also a group, do not create the attribute;
                        IList<string> dimensionReferences = currentAttr.DimensionReferences;
                        foreach (IGroup grp in buildFrom.Groups)
                        {
                            if (grp.DimensionRefs.ContainsAll(dimensionReferences) &&
                                    dimensionReferences.ContainsAll(grp.DimensionRefs))
                            {
                                newAttribute.attachmentLevel = AttachmentLevelTypeConstants.Group;
                                newAttribute.AttachmentGroup.Add(grp.Id);
                                break;
                            }
                        }
                    }
                    if (crossSectionalAttachDataSet.Contains(currentAttr))
                    {
                        newAttribute.crossSectionalAttachDataSet = true;
                    }

                    if (crossSectionalAttachGroup.Contains(currentAttr))
                    {
                        newAttribute.crossSectionalAttachGroup = true;
                    }

                    if (crossSectionalAttachSection.Contains(currentAttr))
                    {
                        newAttribute.crossSectionalAttachSection = true;
                    }

                    if (crossSectionalAttachObservation.Contains(currentAttr))
                    {
                        newAttribute.crossSectionalAttachObservation = true;
                    }

                    if (crossSectionalDataStructureObject != null)
                    {
                        /* foreach */
                        foreach (ICrossSectionalMeasure crossSectionalMeasure in
                            crossSectionalDataStructureObject.GetAttachmentMeasures(currentAttr))
                        {
                            newAttribute.AttachmentMeasure.Add(crossSectionalMeasure.Id);
                        }
                    }

                    componentsType.Attribute.Add(newAttribute);
                }
            }

            return builtObj;
        }

        #endregion
    }
}