// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data structure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
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

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="DataStructureXmlBuilder" /> class.
        /// </summary>
        static DataStructureXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(DataStructureXmlBuilder));
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
        /// The <see cref="KeyFamilyType"/>.
        /// </returns>
        public virtual KeyFamilyType Build(IDataStructureObject buildFrom)
        {
            var builtObj = new KeyFamilyType();
            string str0 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.agency = buildFrom.AgencyId;
            }

            string str1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str1))
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

            string str2 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                builtObj.version = buildFrom.Version;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            var componentsType = new ComponentsType();
            builtObj.Components = componentsType;

            IList<IDimension> currentDims = buildFrom.GetDimensions(
                SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension);
            if (ObjectUtil.ValidCollection(currentDims))
            {
                /* foreach */
                foreach (IDimension currentDim in
                    currentDims)
                {
                    componentsType.Dimension.Add(this._dimensionXmlBuilder.Build(currentDim));
                }
            }

            if (buildFrom.TimeDimension != null)
            {
                componentsType.TimeDimension = this._timeDimensionXmlBuilder.Build(buildFrom.TimeDimension);
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
                    AttributeType attribute = _attributeXmlBuilder.Build(currentAttr);
                    if (currentAttr.AttachmentLevel == AttributeAttachmentLevel.DimensionGroup)
                    {
                        //If the group of dimensions is also a group, do not create the attribute;
                        IList<string> dimensionReferences = currentAttr.DimensionReferences;
                        foreach (IGroup grp in buildFrom.Groups)
                        {
                            if (dimensionReferences.ContainsAll(grp.DimensionRefs) &&
                                    grp.DimensionRefs.ContainsAll(dimensionReferences))
                            {
                                attribute.attachmentLevel = AttachmentLevelTypeConstants.Group;
                                attribute.AttachmentGroup.Add(grp.Id);
                                break;
                            }
                        }
                    }
                    componentsType.Attribute.Add(attribute);
                }
            }

            return builtObj;
        }

        #endregion
    }
}