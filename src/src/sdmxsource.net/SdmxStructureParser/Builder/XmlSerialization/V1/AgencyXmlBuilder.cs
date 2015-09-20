// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencyXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The agency xml bean builder.
    /// </summary>
    public class AgencyXmlBuilder : AbstractBuilder
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="AgencyXmlBuilder" /> class.
        /// </summary>
        static AgencyXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(AgencyXmlBuilder));
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
        /// The <see cref="AgencyType"/>.
        /// </returns>
        public AgencyType Build(IAgency buildFrom)
        {
            var builtObj = new AgencyType();
            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.id = buildFrom.Id;
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

            // FUNC Missing maintenance information
            return builtObj;
        }

        #endregion
    }
}