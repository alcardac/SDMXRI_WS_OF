// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencySchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The agency scheme xml bean builder.
    /// </summary>
    public class AgencySchemeXmlBuilder : ItemSchemeAssembler, IBuilder<AgencySchemeType, IAgencyScheme>
    {
        /// <summary>
        /// The organisation xml assembler.
        /// </summary>
        private readonly OrganisationXmlAssembler organisationXmlAssembler = new OrganisationXmlAssembler(); //TODO: In java the field is not instantieted

        #region Public Methods and Operators

        /// <summary>
        /// Builds and returns the
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="AgencySchemeType"/>.
        /// </returns>
        public virtual AgencySchemeType Build(IAgencyScheme buildFrom)
        {
            var returnType = new AgencySchemeType();
            this.AssembleItemScheme(returnType, buildFrom);
            if (buildFrom.Items != null)
            {
                foreach (IAgency currentBean in buildFrom.Items)
                {
                    var agencyType = new Agency();
                    returnType.Item.Add(agencyType);
                    organisationXmlAssembler.Assemble(agencyType, currentBean);
                    this.AssembleNameable(agencyType.Content, currentBean);
                }
               
            }

            return returnType;
        }

        #endregion
    }
}