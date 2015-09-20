// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportStructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The report structure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The report structure xml bean builder.
    /// </summary>
    public class ReportStructureXmlBuilder : AbstractBuilder, IBuilder<ReportStructureType, IReportStructure>
    {
        #region Fields

        /// <summary>
        ///     The metadata attribute XML bean builder.
        /// </summary>
        private readonly MetadataAttributeXmlBuilder _metadataAttributeXmlBuilder;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportStructureXmlBuilder"/> class.
        /// </summary>
        public ReportStructureXmlBuilder()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportStructureXmlBuilder"/> class.
        /// </summary>
        /// <param name="metadataAttributeXmlBuilder">The metadata attribute XML builder.</param>
        public ReportStructureXmlBuilder(MetadataAttributeXmlBuilder metadataAttributeXmlBuilder)
        {
            this._metadataAttributeXmlBuilder = metadataAttributeXmlBuilder ?? new MetadataAttributeXmlBuilder();
        }

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="ReportStructureType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ReportStructureType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual ReportStructureType Build(IReportStructure buildFrom)
        {
            var builtObj = new ReportStructureType();
            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
            }

            if (buildFrom.TargetMetadatas != null && buildFrom.TargetMetadatas.Count > 0)
            {
                builtObj.target = buildFrom.TargetMetadatas [0];
            }

            var text = new TextType();
            builtObj.Name.Add(text);
            this.SetDefaultText(text);

            IList<IMetadataAttributeObject> metadataAttributes = buildFrom.MetadataAttributes;
            if (ObjectUtil.ValidCollection(metadataAttributes))
            {
                /* foreach */
                foreach (IMetadataAttributeObject currentMa in metadataAttributes)
                {
                    builtObj.MetadataAttribute.Add(this._metadataAttributeXmlBuilder.Build(currentMa));
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
    }
}