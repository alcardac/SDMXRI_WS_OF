// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentConstraintXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The content constraint xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The content constraint xml bean builder.
    /// </summary>
    public class ContentConstraintXmlBuilder : ConstraintAssembler, 
                                               IBuilder<ContentConstraintType, IContentConstraintObject>
    {
        #region Fields

        /// <summary>
        ///     The time range xml assembler.
        /// </summary>
        private readonly TimeRangeXmlAssembler _timeRangeXmlAssembler = new TimeRangeXmlAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ContentConstraintType"/>.
        /// </returns>
        public virtual ContentConstraintType Build(IContentConstraintObject buildFrom)
        {
            var returnType = new ContentConstraintType();
            this.Assemble(returnType, buildFrom);
            returnType.type = buildFrom.IsDefiningActualDataPresent
                                  ? ContentConstraintTypeCodeTypeConstants.Actual
                                  : ContentConstraintTypeCodeTypeConstants.Allowed;

            if (buildFrom.IncludedCubeRegion != null)
            {
                var cubeRegionType = new CubeRegionType();
                returnType.CubeRegion.Add(cubeRegionType);
                this.BuildCubeRegion(cubeRegionType, buildFrom.IncludedCubeRegion, true);
            }

            if (buildFrom.ExcludedCubeRegion != null)
            {
                var cubeRegionType = new CubeRegionType();
                returnType.CubeRegion.Add(cubeRegionType);
                this.BuildCubeRegion(cubeRegionType, buildFrom.ExcludedCubeRegion, false);
            }

            // FUNC 2.1 Metadata Target Region
            if (buildFrom.ReleaseCalendar != null)
            {
                IReleaseCalendar calBean = buildFrom.ReleaseCalendar;
                var calType = new ReleaseCalendarType();
                returnType.ReleaseCalendar = calType;
                calType.Offset = calBean.Offset;
                calType.Periodicity = calBean.Periodicity;
                calType.Tolerance = calBean.Tolerance;
            }

            if (buildFrom.ReferencePeriod != null)
            {
                IReferencePeriod refPeriodBean = buildFrom.ReferencePeriod;
                var refPeriodType = new ReferencePeriodType();
                returnType.ReferencePeriod = refPeriodType;
                if (refPeriodBean.StartTime != null && refPeriodBean.StartTime.Date != null)
                {
                    refPeriodType.startTime = refPeriodBean.StartTime.Date.Value;
                }

                if (refPeriodBean.EndTime != null && refPeriodBean.EndTime.Date != null)
                {
                    refPeriodType.endTime = refPeriodBean.EndTime.Date.Value;
                }
            }

            if (buildFrom.MetadataTargetRegion != null)
            {
                var metadataTargetRegionType = new MetadataTargetRegionType();
                returnType.MetadataTargetRegion.Add(metadataTargetRegionType);

                IMetadataTargetRegion metadataTargetRegion = buildFrom.MetadataTargetRegion;
                BuildMetadataTargetRegion(metadataTargetRegionType, metadataTargetRegion);
            }

            return returnType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build metadata target region.
        /// </summary>
        /// <param name="metadataTargetRegionType">
        /// The metadata target region type.
        /// </param>
        /// <param name="metadataTargetRegion">
        /// The metadata target region.
        /// </param>
        private void BuildMetadataTargetRegion(
            MetadataTargetRegionType metadataTargetRegionType, IMetadataTargetRegion metadataTargetRegion)
        {
            metadataTargetRegionType.include = metadataTargetRegion.IsInclude;
            metadataTargetRegionType.Report = metadataTargetRegion.Report;
            metadataTargetRegionType.MetadataTarget = metadataTargetRegion.MetadataTarget;

            foreach (IKeyValues keyValues in metadataTargetRegion.Attributes)
            {
                ComponentValueSetType componentValue = new MetadataAttributeValueSetType();
                metadataTargetRegionType.Attribute.Add(componentValue);

                BuildKeyValues(componentValue, keyValues);
            }
            foreach (IMetadataTargetKeyValues keyValues in metadataTargetRegion.Key)
            {
                var componentValue = new MetadataKeyValueType();
                metadataTargetRegionType.KeyValue.Add(componentValue);

                BuildKeyValues(componentValue, keyValues);
                foreach (IDataSetReference dataSetReference in keyValues.DatasetReferences)
                {
                    var setReference = new SetReferenceType();
                    componentValue.DataSet.Add(setReference);
                    setReference.ID = dataSetReference.DatasetId;
                    setReference.DataProvider = new DataProviderReferenceType { Ref = new DataProviderRefType() };

                    this.SetReference(setReference.DataProvider.Ref, dataSetReference.DataProviderReference);
                }
            }
        }


        /// <summary>
        /// The build cube region.
        /// </summary>
        /// <param name="cubeRegionType">
        /// The cube region type.
        /// </param>
        /// <param name="cubeRegionBean">
        /// The cube region bean.
        /// </param>
        /// <param name="isIncluded">
        /// The is included.
        /// </param>
        private void BuildCubeRegion(CubeRegionType cubeRegionType, ICubeRegion cubeRegionBean, bool isIncluded)
        {
            cubeRegionType.include = isIncluded;

            foreach (IKeyValues currentKv in cubeRegionBean.KeyValues)
            {
                var cubeRegionKeyType = new CubeRegionKeyType();
                cubeRegionType.KeyValue.Add(cubeRegionKeyType);
                cubeRegionKeyType.id = currentKv.Id;
                if (currentKv.TimeRange != null)
                {
                    this._timeRangeXmlAssembler.Assemble(
                        cubeRegionKeyType.TimeRange = new TimeRangeValueType(), currentKv.TimeRange);
                }

                foreach (string valueren in currentKv.Values)
                {
                    var simpleValueType = new SimpleValueType();
                    cubeRegionKeyType.Value.Add(simpleValueType);
                    simpleValueType.TypedValue = valueren;
                    if (currentKv.IsCascadeValue(valueren))
                    {
                        simpleValueType.cascadeValues = true;
                    }
                }
            }

            /* foreach */
            foreach (IKeyValues values in cubeRegionBean.AttributeValues)
            {
                /*var attributeValueSetType = new AttributeValueSetType();
                cubeRegionType.Attribute.Add(attributeValueSetType);
                attributeValueSetType.id = currentKv0.Id;
                if (currentKv0.TimeRange != null)
                {
                    this._timeRangeXmlAssembler.Assemble(
                        attributeValueSetType.TimeRange = new TimeRangeValueType(), currentKv0.TimeRange);
                }

                
                foreach (string value2 in currentKv0.Values)
                {
                    var simpleValueType3 = new SimpleValueType();
                    attributeValueSetType.Value.Add(simpleValueType3);
                    simpleValueType3.TypedValue = value2;
                    if (currentKv0.IsCascadeValue(value2))
                    {
                        simpleValueType3.cascadeValues = true;
                    }
                }*/
                var attributeValueSetType = new AttributeValueSetType();
                cubeRegionType.Attribute.Add(attributeValueSetType);

                BuildKeyValues(attributeValueSetType, values);
            }
        }

        private void BuildKeyValues(ComponentValueSetType cvst, IKeyValues keyValues)
        {
            cvst.id = keyValues.Id;

            if (keyValues.TimeRange != null)
            {
                this._timeRangeXmlAssembler.Assemble(cvst.TimeRange = new TimeRangeValueType(), keyValues.TimeRange);
            }

            foreach (string value in keyValues.Values)
            {
                var simpleValueType = new SimpleValueType();
                cvst.Value.Add(simpleValueType);
                simpleValueType.TypedValue = value;
                if (keyValues.IsCascadeValue(value))
                {
                    simpleValueType.cascadeValues = true;
                }
            }


        }

        #endregion
    }
}