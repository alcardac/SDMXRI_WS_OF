// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///     The dimension xml bean builder.
    /// </summary>
    public class DimensionXmlBuilder : AbstractBuilder, IBuilder<DimensionType, IDimension>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="DimensionXmlBuilder" /> class.
        /// </summary>
        static DimensionXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(DimensionXmlBuilder));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DimensionType"/>.
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
                builtObj.codelist = buildFrom.Representation.Representation.MaintainableReference.MaintainableId;
            }

            if (buildFrom.ConceptRef != null)
            {
                builtObj.concept = ConceptRefUtil.GetConceptId(buildFrom.ConceptRef);
            }

            if (buildFrom.FrequencyDimension)
            {
                builtObj.isFrequencyDimension = buildFrom.FrequencyDimension;
            }

            if (buildFrom.MeasureDimension)
            {
                builtObj.isMeasureDimension = buildFrom.MeasureDimension;
            }

            return builtObj;
        }

        #endregion
    }
}