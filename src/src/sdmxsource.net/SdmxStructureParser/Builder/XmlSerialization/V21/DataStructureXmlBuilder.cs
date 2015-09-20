// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data structure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.query;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;
    using Org.Sdmxsource.Sdmx.Util.Objects.Annotation;
    using Org.Sdmxsource.Util;

    using AttributeType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.AttributeType;
    using DimensionList = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.DimensionList;
    using DimensionType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.DimensionType;
    using MeasureList = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MeasureList;
    using PrimaryMeasure = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.PrimaryMeasure;

    /// <summary>
    ///     The data structure XML bean builder.
    /// </summary>
    /// <remarks>
    /// In this implementation the coded representation Time dimension is replaced with an annotation.
    /// It is a convention requested by Eurostat to allow coded TimeDimensions. 
    /// See <c>CITnet</c> JIRA ticket SDMXRI-22:
    /// <a href="https://webgate.ec.europa.eu/CITnet/jira/browse/SDMXRI-22">SDMXRI-22</a>
    /// Also the original ticket in AGILIS JIRA: <a href="http://www.agilis-sa.gr:8070/browse/SODIHD-1209">SODIHD-1209</a>
    /// </remarks>
    public class DataStructureXmlBuilder : MaintainableAssembler, IBuilder<DataStructureType, IDataStructureObject>
    {
        #region Fields

        /// <summary>
        ///     The attribute xml assembler.
        /// </summary>
        private readonly AttributeXmlAssembler _attributeXmlAssembler = new AttributeXmlAssembler();

        /// <summary>
        ///     The dimension xml assembler.
        /// </summary>
        private readonly DimensionXmlAssembler _dimensionXmlAssembler = new DimensionXmlAssembler();

        /// <summary>
        ///     The group xml assembler.
        /// </summary>
        private readonly GroupXmlAssembler _groupXmlAssembler = new GroupXmlAssembler();

        /// <summary>
        ///     The measure dimension xml assembler.
        /// </summary>
        private readonly MeasureDimensionXmlAssembler _measureDimensionXmlAssembler = new MeasureDimensionXmlAssembler();

        /// <summary>
        ///     The primary measure xml assembler.
        /// </summary>
        private readonly PrimaryMeasureXmlAssembler _primaryMeasureXmlAssembler = new PrimaryMeasureXmlAssembler();

        /// <summary>
        ///     The time dimension xml assembler.
        /// </summary>
        private readonly TimeDimensionXmlAssembler _timeDimensionXmlAssembler = new TimeDimensionXmlAssembler();

        /// <summary>
        /// The annotation builder
        /// </summary>
        private static readonly IAnnotationBuilder<IMaintainableRefObject> _annotationBuilder = new CodedTimeDimensionAnnotationBuilder<AnnotationMutableCore>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DataStructureType"/>.
        /// </returns>
        public virtual DataStructureType Build(IDataStructureObject buildFrom)
        {
            var builtObj = new DataStructureType();
            var normalizedDataStructure = AddDefaultTimeDimensionTextFormat(NormalizeForSdmxV21(buildFrom));
            this.AssembleMaintainable(builtObj, normalizedDataStructure);

            DataStructureComponents components = null;

            IList<IDimension> dimensions = normalizedDataStructure.GetDimensions();
            if (dimensions.Count > 0)
            {
                components = new DataStructureComponents();
                builtObj.DataStructureComponents = components;
                var dimensionList = new DimensionList();
                components.DimensionList = dimensionList;
                this.ProcessDimensionList(dimensionList.Content, normalizedDataStructure.DimensionList);
            }

            if (ObjectUtil.ValidCollection(normalizedDataStructure.Groups))
            {
                if (components == null)
                {
                    components = new DataStructureComponents();
                    builtObj.DataStructureComponents = components;
                }

                /* foreach */
                foreach (IGroup currentGroup in normalizedDataStructure.Groups)
                {
                    var item = new Group();
                    components.Group.Add(item);
                    this._groupXmlAssembler.Assemble(item.Content, currentGroup);
                }
            }

            if (normalizedDataStructure.Attributes.Count > 0)
            {
                if (components == null)
                {
                    components = new DataStructureComponents();
                    builtObj.DataStructureComponents = components;
                }

                var contents = new AttributeListType();
                components.AttributeList = new AttributeList(contents);

                this.ProcessAttributeList(contents, normalizedDataStructure.AttributeList);
            }

            if (normalizedDataStructure.PrimaryMeasure != null)
            {
                if (components == null)
                {
                    components = new DataStructureComponents();
                    builtObj.DataStructureComponents = components;
                }

                var content = new MeasureListType();
                components.MeasureList = new MeasureList(content);
                this.ProcessMeasureList(content, normalizedDataStructure.MeasureList);
            }

            return builtObj;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the default time dimension text format.
        /// </summary>
        /// <param name="buildFrom">The build from.</param>
        /// <returns>The <see cref="IDataStructureObject" />.</returns>
        /// <remarks>This is needed for matching the output of <c>SdmxSource.Java</c>.</remarks>
        private static IDataStructureObject AddDefaultTimeDimensionTextFormat(IDataStructureObject buildFrom)
        {
            if (buildFrom.TimeDimension == null)
            {
                return buildFrom;
            }

            var mutable = buildFrom.MutableInstance;
            var timeDimension = mutable.GetDimension(DimensionObject.TimeDimensionFixedId);
            if (!buildFrom.TimeDimension.HasCodedRepresentation())
            {
                if (buildFrom.TimeDimension.Representation == null)
                {
                    timeDimension.Representation = new RepresentationMutableCore();
                }

                if (timeDimension.Representation.TextFormat == null)
                {
                    timeDimension.Representation.TextFormat = new TextFormatMutableCore();
                }

                if (timeDimension.Representation.TextFormat.TextType == null)
                {
                    timeDimension.Representation.TextFormat.TextType = TextType.GetFromEnum(TextEnumType.ObservationalTimePeriod);
                }
            }

            return mutable.ImmutableInstance;
        }

        /// <summary>
        /// Normalizes the specified <paramref name="buildFrom" /> for SDMX V21.
        /// </summary>
        /// <param name="buildFrom">The original DSD.</param>
        /// <returns>
        /// The <see cref="IDataStructureObject" />
        /// </returns>
        /// <remarks>
        /// In this implementation the coded representation Time dimension is replaced with an annotation.
        /// Convention requested by Eurostat to allow coded TimeDimensions. 
        /// See <c>CITnet</c> JIRA ticket SDMXRI-22:
        /// <a href="https://webgate.ec.europa.eu/CITnet/jira/browse/SDMXRI-22">SDMXRI-22</a>
        /// Also the original ticket in AGILIS JIRA: <a href="http://www.agilis-sa.gr:8070/browse/SODIHD-1209">SODIHD-1209</a>
        /// Also read the documentation at <c> sdmx_maintenance, 2013, QTM 2013 budget Framework Contract 60402.2012.001-2012.112 LOT2, 2013.433-SC000030-FC112Lot2-SDMX_tools_corrective_and_evolutive_maintenance-QTM, QTM-2, Deliverables </c>
        /// CIRCABC link: <a href="https://circabc.europa.eu/w/browse/91272b61-b03a-431f-ae91-75db31e324cf">CIRCABC</a>
        /// This is part of QTM-4/2014. 
        /// </remarks>
        private static IDataStructureObject NormalizeForSdmxV21(IDataStructureObject buildFrom)
        {
            if (buildFrom.TimeDimension == null || !buildFrom.TimeDimension.HasCodedRepresentation())
            {
                return buildFrom;
            }

            var mutable = buildFrom.MutableInstance;
            var timeDimension = mutable.GetDimension(DimensionObject.TimeDimensionFixedId);
            var codelistRef = timeDimension.Representation.Representation;
            timeDimension.Representation.Representation = null;
            if (timeDimension.Representation.TextFormat == null)
            {
                timeDimension.Representation = null;
            }

            timeDimension.AddAnnotation(_annotationBuilder.Build(codelistRef));

            return mutable.ImmutableInstance;
        }

        /// <summary>
        /// The process attribute list.
        /// </summary>
        /// <param name="attributeListType">
        /// The attribute list type.
        /// </param>
        /// <param name="attributeList">
        /// The attribute list.
        /// </param>
        private void ProcessAttributeList(AttributeListType attributeListType, IAttributeList attributeList)
        {
            this.AssembleIdentifiable(attributeListType, attributeList);
            if (attributeList.Attributes != null)
            {
                /* foreach */
                foreach (IAttributeObject currentAttribute in attributeList.Attributes)
                {
                    var content = new AttributeType();
                    attributeListType.Attribute.Add(new Attribute(content));
                    this._attributeXmlAssembler.Assemble(content, currentAttribute);
                }
            }
        }

        /// <summary>
        /// The process dimension list.
        /// </summary>
        /// <param name="dimensionListType">
        /// The dimension list type.
        /// </param>
        /// <param name="dimensionList">
        /// The dimension list.
        /// </param>
        private void ProcessDimensionList(DimensionListType dimensionListType, IDimensionList dimensionList)
        {
            this.AssembleIdentifiable(dimensionListType, dimensionList);
            if (dimensionList.Dimensions != null)
            {
                /* foreach */
                foreach (IDimension currentDimension in dimensionList.Dimensions)
                {
                    if (currentDimension.MeasureDimension)
                    {
                        var content = new MeasureDimensionType();
                        dimensionListType.MeasureDimension.Add(new MeasureDimension(content));
                        this._measureDimensionXmlAssembler.Assemble(content, currentDimension);
                    }
                    else if (currentDimension.TimeDimension)
                    {
                        var content = new TimeDimensionType();
                        dimensionListType.TimeDimension.Add(new TimeDimension(content));
                        this._timeDimensionXmlAssembler.Assemble(content, currentDimension);
                    }
                    else
                    {
                        var content = new DimensionType();
                        dimensionListType.Dimension.Add(new Dimension(content));
                        this._dimensionXmlAssembler.Assemble(content, currentDimension);
                    }
                }
            }
        }

        /// <summary>
        /// The process measure list.
        /// </summary>
        /// <param name="measureListType">
        /// The measure list type.
        /// </param>
        /// <param name="measureList">
        /// The measure list.
        /// </param>
        private void ProcessMeasureList(MeasureListType measureListType, IMeasureList measureList)
        {
            this.AssembleIdentifiable(measureListType, measureList);
            if (measureList.PrimaryMeasure != null)
            {
                var content = new PrimaryMeasureType();
                measureListType.Component.Add(new PrimaryMeasure(content));
                this._primaryMeasureXmlAssembler.Assemble(content, measureList.PrimaryMeasure);
            }
        }

        #endregion
    }
}