// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataflowXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadataflow xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The metadataflow xml bean builder.
    /// </summary>
    public class MetadataflowXmlBuilder : MaintainableAssembler, IBuilder<MetadataflowType, IMetadataFlow>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="MetadataflowType"/>.
        /// </returns>
        public virtual MetadataflowType Build(IMetadataFlow buildFrom)
        {
            // Create outgoing build
            var builtObj = new MetadataflowType();

            // Populate it from inherited super
            this.AssembleMaintainable(builtObj, buildFrom);

            // Populate it using this class's specifics
            if (buildFrom.MetadataStructureRef != null)
            {
                var refType = new MetadataStructureRefType();
                var metadataStructureReferenceType = new MetadataStructureReferenceType();
                builtObj.SetStructure(metadataStructureReferenceType);
                metadataStructureReferenceType.SetTypedRef(refType);
                this.SetReference(refType, buildFrom.MetadataStructureRef);
            }

            return builtObj;
        }

        #endregion
    }
}