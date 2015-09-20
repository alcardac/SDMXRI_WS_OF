// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit structure builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.RegistryRequest
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

    using SubmitStructureRequestType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitStructureRequestType;

    /// <summary>
    ///     The submit structure builder.
    /// </summary>
    public class SubmitStructureBuilder
    {
        #region Fields

        /// <summary>
        ///     The structure xml bean builder.
        /// </summary>
        private readonly StructureXmlBuilder _structureXmlBuilder = new StructureXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build registry interface document.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildRegistryInterfaceDocument(ISdmxObjects buildFrom, DatasetAction action)
        {
            var rid = new RegistryInterface();
            RegistryInterfaceType rit = rid.Content;
            V21Helper.Header = rit;
            var structureRequestType = new SubmitStructureRequestType();
            rit.SubmitStructureRequest = structureRequestType;
            switch (action.EnumType)
            {
                case DatasetActionEnumType.Append:
                    structureRequestType.action = ActionTypeConstants.Append;
                    break;
                case DatasetActionEnumType.Replace:
                    structureRequestType.action = ActionTypeConstants.Replace;
                    break;
                case DatasetActionEnumType.Delete:
                    structureRequestType.action = ActionTypeConstants.Delete;
                    break;
                case DatasetActionEnumType.Information:
                    structureRequestType.action = ActionTypeConstants.Information;
                    break;
            }

            var structures = new Structures();
            structureRequestType.Structures = structures;
            this._structureXmlBuilder.PopulateStructureType(buildFrom, structures.Content);

            return rid;
        }

        #endregion
    }
}