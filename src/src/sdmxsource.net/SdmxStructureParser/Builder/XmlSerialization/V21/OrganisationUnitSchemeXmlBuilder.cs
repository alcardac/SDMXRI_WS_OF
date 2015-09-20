// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnitSchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation unit scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    using OrganisationUnit = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.OrganisationUnit;

    /// <summary>
    ///     The organisation unit scheme xml bean builder.
    /// </summary>
    public class OrganisationUnitSchemeXmlBuilder : ItemSchemeAssembler, 
                                                    IBuilder<OrganisationUnitSchemeType, IOrganisationUnitSchemeObject>
    {
        private OrganisationXmlAssembler _organisationXmlAssembler = new OrganisationXmlAssembler();

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="OrganisationUnitSchemeType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IOrganisationUnitSchemeObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationUnitSchemeType"/>.
        /// </returns>
        public virtual OrganisationUnitSchemeType Build(IOrganisationUnitSchemeObject buildFrom)
        {
            var returnType = new OrganisationUnitSchemeType();
            this.AssembleItemScheme(returnType, buildFrom);
            if (buildFrom.Items != null)
            {
                /* foreach */
                foreach (IOrganisationUnit item in buildFrom.Items)
                {
                    var organisationUnitType = new OrganisationUnit();
                    returnType.Organisation.Add(organisationUnitType);
                    _organisationXmlAssembler.Assemble(organisationUnitType, item);
                    this.AssembleNameable(organisationUnitType.Content, item);
                    if (item.HasParentUnit)
                    {
                        LocalItemReferenceType parent = new LocalAgencyReferenceType();
                        parent.Ref = new LocalOrganisationUnitRefType();
                        parent.Ref.id = item.ParentUnit;
                        organisationUnitType.SetTypedParent(parent);
                    }
                }
            }

            return returnType;
        }

        #endregion
    }
}