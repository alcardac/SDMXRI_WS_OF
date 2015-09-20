// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryMeasureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The primary measure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The primary measure xml bean builder.
    /// </summary>
    public class PrimaryMeasureXmlBuilder : AbstractBuilder, IBuilder<PrimaryMeasureType, IPrimaryMeasure>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="PrimaryMeasureType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="PrimaryMeasureType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual PrimaryMeasureType Build(IPrimaryMeasure buildFrom)
        {
            var builtObj = new PrimaryMeasureType();

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.HasCodedRepresentation())
            {
                IMaintainableRefObject maintRef = buildFrom.Representation.Representation.MaintainableReference;
                string value1 = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    builtObj.codelist = maintRef.MaintainableId;
                }

                string value2 = maintRef.AgencyId;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    builtObj.codelistAgency = maintRef.AgencyId;
                }

                string value = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builtObj.codelistVersion = maintRef.Version;
                }
            }

            if (buildFrom.ConceptRef != null)
            {
                IMaintainableRefObject maintRef0 = buildFrom.ConceptRef.MaintainableReference;

                string value = maintRef0.AgencyId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builtObj.conceptSchemeAgency = maintRef0.AgencyId;
                }

                string value1 = maintRef0.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    builtObj.conceptSchemeRef = maintRef0.MaintainableId;
                }

                string value2 = buildFrom.ConceptRef.ChildReference.Id;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    builtObj.conceptRef = buildFrom.ConceptRef.ChildReference.Id;
                }

                string value3 = maintRef0.Version;
                if (!string.IsNullOrWhiteSpace(value3))
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

            return builtObj;
        }

        #endregion
    }
}