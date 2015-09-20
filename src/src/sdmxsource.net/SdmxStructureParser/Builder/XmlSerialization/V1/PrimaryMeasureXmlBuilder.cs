// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryMeasureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The primary measure xml bean builder.
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
    ///     The primary measure xml bean builder.
    /// </summary>
    public class PrimaryMeasureXmlBuilder : AbstractBuilder, IBuilder<PrimaryMeasureType, IPrimaryMeasure>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="PrimaryMeasureXmlBuilder" /> class.
        /// </summary>
        static PrimaryMeasureXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(PrimaryMeasureXmlBuilder));
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
        /// The <see cref="PrimaryMeasureType"/>.
        /// </returns>
        public virtual PrimaryMeasureType Build(IPrimaryMeasure buildFrom)
        {
            var builtObj = new PrimaryMeasureType();

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.ConceptRef != null)
            {
                builtObj.concept = ConceptRefUtil.GetConceptId(buildFrom.ConceptRef);
            }

            return builtObj;
        }

        #endregion
    }
}