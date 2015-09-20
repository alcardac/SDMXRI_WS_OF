// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConsumerSchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data consumer scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The data consumer scheme xml bean builder.
    /// </summary>
    public class DataConsumerSchemeXmlBuilder : ItemSchemeAssembler, 
                                                IBuilder<DataConsumerSchemeType, IDataConsumerScheme>
    {
        private OrganisationXmlAssembler _organisationXmlAssembler = new OrganisationXmlAssembler();

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DataConsumerSchemeType"/>.
        /// </returns>
        public virtual DataConsumerSchemeType Build(IDataConsumerScheme buildFrom)
        {
            var returnType = new DataConsumerSchemeType();
            this.AssembleItemScheme(returnType, buildFrom);
            if (buildFrom.Items != null)
            {
                /* foreach */
                foreach (IDataConsumer dataConsumer in buildFrom.Items)
                {
                    var dataProviderType = new DataConsumer();
                    returnType.Item.Add(dataProviderType);
                    _organisationXmlAssembler.Assemble(dataProviderType, dataConsumer);
                    this.AssembleNameable(dataProviderType.Content, dataConsumer);
                }
            }

            return returnType;
        }

        #endregion
    }
}