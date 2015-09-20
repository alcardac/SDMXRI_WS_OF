// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalMeasureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross sectional measure xml bean builder.
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
    ///     The cross sectional measure xml bean builder.
    /// </summary>
    public class CrossSectionalMeasureXmlBuilder : AbstractBuilder, 
                                                   IBuilder<CrossSectionalMeasureType, ICrossSectionalMeasure>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CrossSectionalMeasureType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CrossSectionalMeasureType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual CrossSectionalMeasureType Build(ICrossSectionalMeasure buildFrom)
        {
            var builtObj = new CrossSectionalMeasureType();

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.HasCodedRepresentation())
            {
                IMaintainableRefObject maintRef = buildFrom.Representation.Representation.MaintainableReference;
                string str0 = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(str0))
                {
                    builtObj.codelist = maintRef.MaintainableId;
                }

                string str2 = maintRef.AgencyId;
                if (!string.IsNullOrWhiteSpace(str2))
                {
                    builtObj.codelistAgency = maintRef.AgencyId;
                }

                string str1 = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(str1))
                {
                    builtObj.codelistVersion = maintRef.Version;
                }
            }

            if (buildFrom.ConceptRef != null)
            {
                IMaintainableRefObject conceptScheme = buildFrom.ConceptRef.MaintainableReference;
                string value1 = conceptScheme.AgencyId;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    builtObj.conceptSchemeAgency = conceptScheme.AgencyId;
                }

                string value = conceptScheme.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builtObj.conceptSchemeRef = conceptScheme.MaintainableId;
                }

                string conceptRef = buildFrom.ConceptRef.ChildReference.Id;
                if (!string.IsNullOrWhiteSpace(conceptRef))
                {
                    builtObj.conceptRef = conceptRef;
                }

                // TODO in ESTAT version of SDMX v2.0 we set concept scheme version.
                string value2 = conceptScheme.Version;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    builtObj.conceptVersion = conceptScheme.Version;
                }
            }

            builtObj.code = buildFrom.Code;
            builtObj.measureDimension = buildFrom.MeasureDimension;
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