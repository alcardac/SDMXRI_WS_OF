// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataStructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata structure xml codelistRef builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;
    using Org.Sdmxsource.Util;

    using DataSetTarget = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.DataSetTarget;
    using IdentifiableObjectTarget = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.IdentifiableObjectTarget;
    using MetadataAttribute = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MetadataAttribute;
    using MetadataStructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MetadataStructureType;
    using MetadataTarget = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MetadataTarget;
    using ReportPeriodTarget = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ReportPeriodTarget;
    using ReportStructure = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ReportStructure;

    /// <summary>
    ///     The metadata structure xml codelistRef builder.
    /// </summary>
    public class MetadataStructureXmlBuilder : MaintainableAssembler, 
                                               IBuilder<MetadataStructureType, IMetadataStructureDefinitionObject>
    {
        #region Fields

        /// <summary>
        ///     The component assembler.
        /// </summary>
        private readonly ComponentAssembler<IdentifiableObjectRepresentationType> _componentAssembler =
            new ComponentAssembler<IdentifiableObjectRepresentationType>();

        /// <summary>
        ///     The data type builder.
        /// </summary>
        private readonly DataTypeBuilder _dataTypeBuilder = new DataTypeBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="MetadataStructureType"/>.
        /// </returns>
        public virtual MetadataStructureType Build(IMetadataStructureDefinitionObject buildFrom)
        {
            var builtObj = new MetadataStructureType();
            this.AssembleMaintainable(builtObj, buildFrom);
            if (ObjectUtil.ValidCollection(buildFrom.MetadataTargets)
                || ObjectUtil.ValidCollection(buildFrom.ReportStructures))
            {
                builtObj.MetadataStructureComponents = new MetadataStructureComponents();
                if (buildFrom.MetadataTargets != null)
                {
                    /* foreach */
                    foreach (IMetadataTarget currentMetadataTarget in buildFrom.MetadataTargets)
                    {
                        var metadataTarget = new MetadataTarget();
                        builtObj.MetadataStructureComponents.MetadataTarget.Add(metadataTarget);
                        this.AssembleMetadataTargetType(metadataTarget.Content, currentMetadataTarget);
                    }
                }

                if (buildFrom.ReportStructures != null)
                {
                    /* foreach */
                    foreach (IReportStructure currentBean in buildFrom.ReportStructures)
                    {
                        var reportStructure = new ReportStructure();
                        builtObj.MetadataStructureComponents.ReportStructure.Add(reportStructure);
                        this.AssembleReportStructure(reportStructure.Content, currentBean);
                    }
                }
            }

            return builtObj;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The assemble constraint content target.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleConstraintContentTarget(ConstraintContentTargetType type, IIdentifiableObject bean)
        {
            this.AssembleTarget<ConstraintRepresentationType, ConstraintTextFormatType>(
                type, bean, DataTypeConstants.AttachmentConstraintReference);
        }

        /// <summary>
        /// The assemble data set target.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleDataSetTarget(DataSetTargetType type, IDataSetTarget bean)
        {
            this.AssembleTarget<DataSetRepresentationType, DataSetTextFormatType>(
                type, bean, DataTypeConstants.DataSetReference);
        }

        /// <summary>
        /// The assemble identifiable object target.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleIdentifiableObjectTarget(IdentifiableObjectTargetType type, IIdentifiableTarget bean)
        {
            this._componentAssembler.AssembleComponent(type, bean);
            type.objectType = XmlobjectsEnumUtil.BuildV21(bean.ReferencedStructureType);
        }

        /// <summary>
        /// The assemble key descriptor values target.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleKeyDescriptorValuesTarget(
            KeyDescriptorValuesTargetType type, IKeyDescriptorValuesTarget bean)
        {
            this.AssembleTarget<KeyDescriptorValuesRepresentationType, KeyDescriptorValuesTextFormatType>(
                type, bean, DataTypeConstants.KeyValues);
        }

        /// <summary>
        /// The assemble metadata attributes.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleMetadataAttributes(MetadataAttributeType type, IMetadataAttributeObject bean)
        {
            this._componentAssembler.AssembleComponent(type, bean);
            if (bean.MinOccurs != null)
            {
                type.minOccurs = bean.MinOccurs.Value;
            }

            if (bean.MaxOccurs != null)
            {
                type.maxOccurs = bean.MaxOccurs.Value;
            }
            else
            {
                type.maxOccurs = "unbounded";
            }

            if (bean.Presentational.IsSet())
            {
                type.isPresentational = bean.Presentational.IsTrue;
            }

            if (bean.MetadataAttributes != null)
            {
                this.AssembleMetadataAttributes(type, bean.MetadataAttributes);
            }
        }

        /// <summary>
        /// The assemble metadata attributes.
        /// </summary>
        /// <param name="metadaAttributeType">
        /// The metadata attribute type.
        /// </param>
        /// <param name="metadataAttributes">
        /// The metadata attributes.
        /// </param>
        private void AssembleMetadataAttributes(
            MetadataAttributeType metadaAttributeType, IEnumerable<IMetadataAttributeObject> metadataAttributes)
        {
            /* foreach */
            foreach (IMetadataAttributeObject currentMa in metadataAttributes)
            {
                var metadataAttribute = new MetadataAttribute();
                metadaAttributeType.MetadataAttribute.Add(metadataAttribute);
                this.AssembleMetadataAttributes(metadataAttribute.Content, currentMa);
            }
        }

        /// <summary>
        /// The assemble metadata target type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleMetadataTargetType(MetadataTargetType type, IMetadataTarget bean)
        {
            this.AssembleIdentifiable(type, bean);
            if (bean.DataSetTarget != null)
            {
                var content = new DataSetTargetType();
                type.DataSetTarget.Add(new DataSetTarget(content));
                this.AssembleDataSetTarget(content, bean.DataSetTarget);
            }

            if (bean.KeyDescriptorValuesTarget != null)
            {
                var content = new KeyDescriptorValuesTargetType();
                type.KeyDescriptorValuesTarget.Add(new KeyDescriptorValuesTarget(content));
                this.AssembleKeyDescriptorValuesTarget(content, bean.KeyDescriptorValuesTarget);
            }

            if (bean.ReportPeriodTarget != null)
            {
                var content = new ReportPeriodTargetType();
                type.ReportPeriodTarget.Add(new ReportPeriodTarget(content));
                this.AssembleReportPeriodTarget(content, bean.ReportPeriodTarget);
            }

            if (bean.ConstraintContentTarget != null)
            {
                var content = new ConstraintContentTargetType();
                type.ConstraintContentTarget.Add(new ConstraintContentTarget(content));
                this.AssembleConstraintContentTarget(content, bean.ConstraintContentTarget);
            }

            if (bean.IdentifiableTarget != null)
            {
                /* foreach */
                foreach (IIdentifiableTarget currentBean in bean.IdentifiableTarget)
                {
                    var content = new IdentifiableObjectTargetType();
                    type.IdentifiableObjectTarget.Add(new IdentifiableObjectTarget(content));
                    this.AssembleIdentifiableObjectTarget(content, currentBean);
                }
            }
        }

        /// <summary>
        /// The assemble report period target.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleReportPeriodTarget(ReportPeriodTargetType type, IReportPeriodTarget bean)
        {
            this.AssembleIdentifiable(type, bean);

            var reportPeriodRepresentationType = new ReportPeriodRepresentationType();
            type.LocalRepresentation = reportPeriodRepresentationType;
            TextFormatType textFormatType = reportPeriodRepresentationType.AddNewTextFormatType();
            if (bean.TextType != null)
            {
                textFormatType.textType = this._dataTypeBuilder.Build(bean.TextType);
            }

            if (bean.StartTime != null)
            {
                textFormatType.startTime = bean.StartTime.DateInSdmxFormat;
            }

            if (bean.EndTime != null)
            {
                textFormatType.endTime = bean.EndTime.DateInSdmxFormat;
            }
        }

        /// <summary>
        /// The assemble report structure.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        private void AssembleReportStructure(ReportStructureType type, IReportStructure bean)
        {
            this.AssembleIdentifiable(type, bean);
            if (bean.MetadataAttributes != null)
            {
                /* foreach */
                foreach (IMetadataAttributeObject currentMa in bean.MetadataAttributes)
                {
                    var content = new MetadataAttributeType();
                    type.Component.Add(new MetadataAttribute(content));
                    this.AssembleMetadataAttributes(content, currentMa);
                }
            }

            if (bean.TargetMetadatas != null)
            {
                /* foreach */
                foreach (string metadataTarget in bean.TargetMetadatas)
                {
                    var localMetadataTargetReferenceType = new LocalMetadataTargetReferenceType();
                    type.MetadataTarget.Add(localMetadataTargetReferenceType);
                    localMetadataTargetReferenceType.SetTypedRef(new LocalMetadataTargetRefType { id = metadataTarget });
                }
            }
        }

        /// <summary>
        /// Assemble target.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The codelistRef.
        /// </param>
        /// <param name="dataType">
        /// The <see cref="Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.DataType"/> value
        /// </param>
        /// <typeparam name="TR">
        /// The local representation type
        /// </typeparam>
        /// <typeparam name="TT">
        /// The TextFormatType type
        /// </typeparam>
        private void AssembleTarget<TR, TT>(ComponentType type, IIdentifiableObject bean, string dataType)
            where TR : RepresentationType, new() where TT : TextFormatType, new()
        {
            this.AssembleIdentifiable(type, bean);
            type.LocalRepresentation = new TR { TextFormat = new TT { textType = dataType } };
        }

        #endregion
    }
}