// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time dimension xml bean builder.
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
    ///     The time dimension xml bean builder.
    /// </summary>
    public class TimeDimensionXmlBuilder : AbstractBuilder, IBuilder<TimeDimensionType, IDimension>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="StructureSetType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The source SDMX Object.
        /// </param>
        /// <returns>
        /// The <see cref="StructureSetType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual TimeDimensionType Build(IDimension buildFrom)
        {
            var builtObj = new TimeDimensionType();

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.HasCodedRepresentation())
            {
                IMaintainableRefObject maintRef = buildFrom.Representation.Representation.MaintainableReference;
                string value2 = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    builtObj.codelist = maintRef.MaintainableId;
                }

                string value1 = maintRef.AgencyId;
                if (!string.IsNullOrWhiteSpace(value1))
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

                string value3 = maintRef0.AgencyId;
                if (!string.IsNullOrWhiteSpace(value3))
                {
                    builtObj.conceptSchemeAgency = maintRef0.AgencyId;
                }

                string value1 = maintRef0.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    builtObj.conceptSchemeRef = maintRef0.MaintainableId;
                }

                string value = buildFrom.ConceptRef.ChildReference.Id;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builtObj.conceptRef = buildFrom.ConceptRef.ChildReference.Id;
                }

                string value2 = maintRef0.Version;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    builtObj.conceptVersion = maintRef0.Version;
                }

                if (buildFrom.Representation != null && buildFrom.Representation.TextFormat != null)
                {
                    var textFormatType = new TextFormatType();
                    this.PopulateTextFormatType(textFormatType, buildFrom.Representation.TextFormat);
                    builtObj.TextFormat = textFormatType;
                }
            }

            return builtObj;
        }

        #endregion
    }
}