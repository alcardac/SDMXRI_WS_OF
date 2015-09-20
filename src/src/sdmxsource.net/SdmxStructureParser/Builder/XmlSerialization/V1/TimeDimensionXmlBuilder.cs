// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time dimension xml bean builder.
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
    ///     The time dimension xml bean builder.
    /// </summary>
    public class TimeDimensionXmlBuilder : AbstractBuilder, IBuilder<TimeDimensionType, IDimension>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="TimeDimensionXmlBuilder" /> class.
        /// </summary>
        static TimeDimensionXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(TimeDimensionXmlBuilder));
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
        /// The <see cref="TimeDimensionType"/>.
        /// </returns>
        public virtual TimeDimensionType Build(IDimension buildFrom)
        {
            var builtObj = new TimeDimensionType();

            if (buildFrom.HasCodedRepresentation())
            {
                builtObj.codelist = buildFrom.Representation.Representation.MaintainableReference.MaintainableId;
            }

            if (buildFrom.ConceptRef != null)
            {
                builtObj.concept = ConceptRefUtil.GetConceptId(buildFrom.ConceptRef);
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