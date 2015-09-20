// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The constraint bean assembler.
    /// </summary>
    public class ConstraintAssembler : MaintainableAssembler, IAssembler<ConstraintType, IConstraintObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(ConstraintType assembleInto, IConstraintObject assembleFrom)
        {
            this.AssembleMaintainable(assembleInto, assembleFrom);

            if (assembleFrom.ConstraintAttachment != null)
            {
                // TODO check if ContentConstraintAttachmentType is the correct type.
                // In java they seem to use the abstract type ConstraintAttachmentType (which is not abstract in Xmls).
                ConstraintAttachmentType constraintAttachment =
                    assembleInto.ConstraintAttachment = new ContentConstraintAttachmentType();

                this.BuildConstraintAttachment(constraintAttachment, assembleFrom.ConstraintAttachment);
            }

            DataKeySetType type = null;

            if (assembleFrom.IncludedSeriesKeys != null)
            {
                type = new DataKeySetType();
                assembleInto.DataKeySet.Add(type);

                BuildDataKeySet(type, assembleFrom.IncludedSeriesKeys, true);
            }

            if (assembleFrom.ExcludedSeriesKeys != null)
            {
                if (type == null)
                {
                    type = new DataKeySetType();
                    assembleInto.DataKeySet.Add(type);
                }

                BuildDataKeySet(type, assembleFrom.ExcludedSeriesKeys, true);
            }

            MetadataKeySetType metadataType = null;
            if (assembleFrom.IncludedMetadataKeys != null)
            {
                metadataType = new MetadataKeySetType();
                assembleInto.MetadataKeySet.Add(metadataType);
                BuildDataKeySet(metadataType, assembleFrom.IncludedMetadataKeys, true);
            }

            if (assembleFrom.ExcludedMetadataKeys != null)
            {
                if (metadataType == null)
                {
                    metadataType = new MetadataKeySetType();
                    assembleInto.MetadataKeySet.Add(metadataType);
                }
                BuildDataKeySet(metadataType, assembleFrom.ExcludedMetadataKeys, false);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build <paramref name="outputKeySetType"/> from <paramref name="input"/>.
        /// </summary>
        /// <param name="outputKeySetType">
        /// The output key set type.
        /// </param>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="included">
        /// The included.
        /// </param>
        private static void BuildDataKeySet(KeySetType outputKeySetType, IConstraintDataKeySet input, bool included)
        {
            outputKeySetType.isIncluded = included;

            /* foreach */
            foreach (IConstrainedDataKey dataKey in input.ConstrainedDataKeys)
            {
                var keyType = new DataKeyType();
                outputKeySetType.Key.Add(keyType);

                foreach (IKeyValue kv in dataKey.KeyValues)
                {
                    var componentValueSetType = new DataKeyValueType();
                    keyType.KeyValue.Add(componentValueSetType);
                    componentValueSetType.id = kv.Concept;
                    var simpleValueType = new SimpleValueType();
                    componentValueSetType.Value.Add(simpleValueType);
                    simpleValueType.TypedValue = kv.Code;
                }
            }
        }

        /// <summary>
        /// Build constraint attachment.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bean">
        /// The bean.
        /// </param>
        private void BuildConstraintAttachment(ConstraintAttachmentType type, IConstraintAttachment bean)
        {
            if (bean.DataOrMetadataSetReference != null)
            {
                var refType = new SetReferenceType();
                if (bean.DataOrMetadataSetReference.IsDataSetReference)
                {
                    type.DataSet.Add(refType);
                }
                else
                {
                    type.MetadataSet.Add(refType);
                }

                refType.ID = bean.DataOrMetadataSetReference.SetId;
                refType.DataProvider = new DataProviderReferenceType();
                var dataProviderRefType = new DataProviderRefType();
                refType.DataProvider.SetTypedRef(dataProviderRefType);
                this.SetReference(dataProviderRefType, bean.DataOrMetadataSetReference.DataSetReference);
            }

            if (bean.DataSources != null)
            {
                /* foreach */
                foreach (IDataSource ds in bean.DataSources)
                {
                    if (ds.SimpleDatasource)
                    {
                        if (ds.DataUrl != null)
                        {
                            type.SimpleDataSource.Add(ds.DataUrl);
                        }
                    }
                    else
                    {
                        var dataSourceType = new QueryableDataSourceType();
                        type.QueryableDataSource.Add(dataSourceType);
                        if (ds.DataUrl != null)
                        {
                            dataSourceType.DataURL = ds.DataUrl;
                        }

                        dataSourceType.isRESTDatasource = ds.RESTDatasource;
                        dataSourceType.isWebServiceDatasource = ds.WebServiceDatasource;
                        if (ds.WadlUrl != null)
                        {
                            dataSourceType.WADLURL = ds.WadlUrl;
                        }

                        if (ds.WsdlUrl != null)
                        {
                            dataSourceType.WSDLURL = ds.WsdlUrl;
                        }
                    }
                }
            }

            ICrossReference attachment = null;
            var attached = SdmxStructureEnumType.Null;

            /* foreach */
            foreach (ICrossReference currentRef in bean.StructureReference)
            {
                switch (currentRef.TargetReference.EnumType)
                {
                    case SdmxStructureEnumType.DataProvider:
                        if (attached == SdmxStructureEnumType.Null)
                        {
                            attached = currentRef.TargetReference.EnumType;
                            attachment = currentRef;
                        }

                        break;
                    case SdmxStructureEnumType.Dsd:
                        if (attached == SdmxStructureEnumType.Null || attached == SdmxStructureEnumType.DataProvider)
                        {
                            attached = currentRef.TargetReference.EnumType;
                            attachment = currentRef;
                        }

                        break;
                    case SdmxStructureEnumType.Msd:
                        break;
                    case SdmxStructureEnumType.Dataflow:
                        if (attached == SdmxStructureEnumType.Null || attached == SdmxStructureEnumType.DataProvider
                            || attached == SdmxStructureEnumType.Dsd)
                        {
                            attached = currentRef.TargetReference.EnumType;
                            attachment = currentRef;
                        }

                        break;
                    case SdmxStructureEnumType.MetadataFlow:
                        if (attached == SdmxStructureEnumType.Null || attached == SdmxStructureEnumType.DataProvider)
                        {
                            attached = currentRef.TargetReference.EnumType;
                            attachment = currentRef;
                        }

                        break;
                    case SdmxStructureEnumType.ProvisionAgreement:
                        attached = currentRef.TargetReference.EnumType;
                        attachment = currentRef;
                        break;
                    default:
                        continue;
                }
                AddAttachment(type, attached, attachment);
            }
        }

        /// <summary>
        /// Adds constraint attachment.
        /// </summary>
        /// <param name="type">
        /// The constraint attachment type.
        /// </param>
        /// <param name="attachedTo">
        /// The type of the destination to attach to.
        /// </param>
        /// <param name="attachment">
        /// The attachment.
        /// </param>
        private void AddAttachment(ConstraintAttachmentType type, SdmxStructureEnumType attachedTo, ICrossReference attachment)
        {
            switch (attachedTo)
            {
                case SdmxStructureEnumType.DataProvider:
                    {
                        var dataStructure = new DataProviderReferenceType();
                        type.DataProvider = dataStructure;
                        var refBaseType = new DataProviderRefType();
                        dataStructure.SetTypedRef(refBaseType);
                        this.SetReference(refBaseType, attachment);
                    }

                    break;
                case SdmxStructureEnumType.Dsd:
                    {
                        var dataStructure = new DataStructureReferenceType();
                        type.DataStructure.Add(dataStructure);
                        var refBaseType = new DataStructureRefType();
                        dataStructure.SetTypedRef(refBaseType);
                        this.SetReference(refBaseType, attachment);
                    }

                    break;
                case SdmxStructureEnumType.Msd:
                    {
                        var dataStructure = new MetadataStructureReferenceType();
                        type.MetadataStructure.Add(dataStructure);
                        var refBaseType = new MetadataStructureRefType();
                        dataStructure.SetTypedRef(refBaseType);
                        this.SetReference(refBaseType, attachment);
                    }

                    break;
                case SdmxStructureEnumType.Dataflow:
                    {
                        var dataStructure = new DataflowReferenceType();
                        type.Dataflow.Add(dataStructure);
                        var refBaseType = new DataflowRefType();
                        dataStructure.SetTypedRef(refBaseType);
                        this.SetReference(refBaseType, attachment);
                    }

                    break;
                case SdmxStructureEnumType.MetadataFlow:
                    {
                        var dataStructure = new MetadataflowReferenceType();
                        type.Metadataflow.Add(dataStructure);
                        var refBaseType = new MetadataflowRefType();
                        dataStructure.SetTypedRef(refBaseType);
                        this.SetReference(refBaseType, attachment);
                    }

                    break;
                case SdmxStructureEnumType.ProvisionAgreement:
                    {
                        var dataStructure = new ProvisionAgreementReferenceType();
                        type.ProvisionAgreement.Add(dataStructure);
                        var refBaseType = new ProvisionAgreementRefType();
                        dataStructure.SetTypedRef(refBaseType);
                        this.SetReference(refBaseType, attachment);
                    }

                    break;
            }
        }

        #endregion
    }
}