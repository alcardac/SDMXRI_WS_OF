// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderSchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data provider scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The data provider scheme xml bean builder.
    /// </summary>
    public class DataProviderSchemeXmlBuilder : ItemSchemeAssembler,
                                                IBuilder<DataProviderSchemeType, IDataProviderScheme>
    {
        /// <summary>
        /// The organisation XML assembler
        /// </summary>
        private readonly OrganisationXmlAssembler _organisationXmlAssembler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeXmlBuilder"/> class.
        /// </summary>
        public DataProviderSchemeXmlBuilder()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeXmlBuilder"/> class.
        /// </summary>
        /// <param name="organisationXmlAssembler">The organisation XML assembler.</param>
        public DataProviderSchemeXmlBuilder(OrganisationXmlAssembler organisationXmlAssembler)
        {
            this._organisationXmlAssembler = organisationXmlAssembler ?? new OrganisationXmlAssembler();
        }

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DataProviderSchemeType"/>.
        /// </returns>
        public virtual DataProviderSchemeType Build(IDataProviderScheme buildFrom)
        {
            var returnType = new DataProviderSchemeType();

            if (buildFrom.Partial)
                returnType.isPartial = true;

            this.AssembleItemScheme(returnType, buildFrom);
            if (buildFrom.Items != null)
            {
                /* foreach */
                foreach (IDataProvider item in buildFrom.Items)
                {
                    var dataProviderType = new DataProvider();
                    returnType.Item.Add(dataProviderType);
                    _organisationXmlAssembler.Assemble(dataProviderType, item);
                    this.AssembleNameable(dataProviderType.Content, item);
                }
            }

            return returnType;
        }

        #endregion
    }
}