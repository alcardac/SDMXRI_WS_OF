// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencySchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The agency scheme xml bean builder.
    /// </summary>
    public class AgencySchemeXmlBuilder : IBuilder<OrganisationSchemeType, IAgencyScheme>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemeType"/> .
        /// </returns>
        public virtual OrganisationSchemeType Build(IAgencyScheme buildFrom)
        {
            // TODO Auto-generated method stub
            return null;
        }

        #endregion
    }
}