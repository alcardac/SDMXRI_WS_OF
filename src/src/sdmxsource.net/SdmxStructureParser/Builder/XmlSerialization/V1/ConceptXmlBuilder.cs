// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The concept xml bean builder.
    /// </summary>
    public class ConceptXmlBuilder : AbstractBuilder, IBuilder<ConceptType, IConceptObject>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="ConceptXmlBuilder" /> class.
        /// </summary>
        static ConceptXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(ConceptXmlBuilder));
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
        /// The <see cref="ConceptType"/>.
        /// </returns>
        public virtual ConceptType Build(IConceptObject buildFrom)
        {
            var builtObj = new ConceptType();

            string str1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                builtObj.id = buildFrom.Id;
            }

            string str0 = buildFrom.MaintainableParent.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.agency = buildFrom.MaintainableParent.AgencyId;
            }

            string str2 = buildFrom.MaintainableParent.Version;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                builtObj.version = buildFrom.MaintainableParent.Version;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            return builtObj;
        }

        #endregion
    }
}