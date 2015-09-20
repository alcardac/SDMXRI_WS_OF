// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProvisionAgreementXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The provision agreement xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The provision agreement xml bean builder.
    /// </summary>
    public class ProvisionAgreementXmlBuilder : AbstractBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly ProvisionAgreementXmlBuilder _instance = new ProvisionAgreementXmlBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="ProvisionAgreementXmlBuilder" /> class from being created.
        /// </summary>
        private ProvisionAgreementXmlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static ProvisionAgreementXmlBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="ProvisionAgreementType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ProvisionAgreementType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public ProvisionAgreementType Build(IProvisionAgreementObject buildFrom)
        {
            var builtObj = new ProvisionAgreementType();

            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
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

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
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

            if (buildFrom.StructureUseage != null)
            {
                if (buildFrom.StructureUseage.TargetReference.EnumType == SdmxStructureEnumType.Dataflow)
                {
                    DataflowRefType dataflowRef = builtObj.DataflowRef = new DataflowRefType();
                    this.PopulateDataflowRef(buildFrom.StructureUseage, dataflowRef);
                }
                else if (buildFrom.StructureUseage.TargetReference.EnumType == SdmxStructureEnumType.MetadataFlow)
                {
                    MetadataflowRefType metadataflowRef = builtObj.MetadataflowRef = new MetadataflowRefType();
                    this.PopulateMetadataflowRef(buildFrom.StructureUseage, metadataflowRef);
                }
            }

            if (buildFrom.DataproviderRef != null)
            {
                DataProviderRefType dataProviderRef = builtObj.DataProviderRef = new DataProviderRefType();
                this.PopulateDataproviderRef(buildFrom.DataproviderRef, dataProviderRef);
            }

            return builtObj;
        }

        #endregion
    }
}