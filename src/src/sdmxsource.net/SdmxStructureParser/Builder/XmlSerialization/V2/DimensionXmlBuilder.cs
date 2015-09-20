// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The dimension xml bean builder.
    /// </summary>
    public class DimensionXmlBuilder : AbstractBuilder, IBuilder<DimensionType, IDimension>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="DimensionType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DimensionType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual DimensionType Build(IDimension buildFrom)
        {
            var builtObj = new DimensionType();

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.HasCodedRepresentation())
            {
                IMaintainableRefObject maintRef = buildFrom.Representation.Representation.MaintainableReference;
                if (!string.IsNullOrWhiteSpace(maintRef.MaintainableId))
                {
                    builtObj.codelist = maintRef.MaintainableId;
                }

                if (!string.IsNullOrWhiteSpace(maintRef.AgencyId))
                {
                    builtObj.codelistAgency = maintRef.AgencyId;
                }

                if (!string.IsNullOrWhiteSpace(maintRef.Version))
                {
                    builtObj.codelistVersion = maintRef.Version;
                }
            }

            if (buildFrom.ConceptRef != null)
            {
                IMaintainableRefObject maintainableRef = buildFrom.ConceptRef.MaintainableReference;
                if (!string.IsNullOrWhiteSpace(maintainableRef.AgencyId))
                {
                    builtObj.conceptSchemeAgency = maintainableRef.AgencyId;
                }

                if (!string.IsNullOrWhiteSpace(maintainableRef.MaintainableId))
                {
                    builtObj.conceptSchemeRef = maintainableRef.MaintainableId;
                }

                if (!string.IsNullOrWhiteSpace(buildFrom.ConceptRef.ChildReference.Id))
                {
                    builtObj.conceptRef = buildFrom.ConceptRef.ChildReference.Id;
                }

                if (!string.IsNullOrWhiteSpace(maintainableRef.Version))
                {
                    builtObj.conceptVersion = maintainableRef.Version;
                }
            }

            if (buildFrom.FrequencyDimension)
            {
                builtObj.isFrequencyDimension = buildFrom.FrequencyDimension;
            }

            if (buildFrom.MeasureDimension)
            {
                builtObj.isMeasureDimension = buildFrom.MeasureDimension;
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