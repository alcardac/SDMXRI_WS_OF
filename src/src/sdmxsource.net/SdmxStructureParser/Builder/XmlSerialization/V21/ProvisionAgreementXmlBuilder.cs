// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProvisionAgreementXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The provision agreement xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The provision agreement xml bean builder.
    /// </summary>
    public class ProvisionAgreementXmlBuilder : MaintainableAssembler, 
                                                IBuilder<ProvisionAgreementType, IProvisionAgreementObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="ProvisionAgreementType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IProvisionAgreementObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="ProvisionAgreementType"/>.
        /// </returns>
        public virtual ProvisionAgreementType Build(IProvisionAgreementObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new ProvisionAgreementType();

            // Populate it from inherited super
            this.AssembleMaintainable(builtObj, buildFrom);
            if (buildFrom.StructureUseage != null)
            {
                var structureUSeageType = new StructureUsageReferenceType();
                builtObj.StructureUsage = structureUSeageType;
                var structureUsageRefType = new StructureUsageRefType();
                structureUSeageType.SetTypedRef(structureUsageRefType);
                this.SetReference(structureUsageRefType, buildFrom.StructureUseage);
            }

            if (buildFrom.DataproviderRef != null)
            {
                var dataProviderRef = new DataProviderReferenceType();
                builtObj.DataProvider = dataProviderRef;
                var dataProviderRefType = new DataProviderRefType();
                dataProviderRef.SetTypedRef(dataProviderRefType);
                this.SetReference(dataProviderRefType, buildFrom.DataproviderRef);
            }

            return builtObj;
        }

        #endregion
    }
}