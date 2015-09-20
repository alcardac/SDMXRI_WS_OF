// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The dataflow xml bean builder.
    /// </summary>
    public class DataflowXmlBuilder : MaintainableAssembler, IBuilder<DataflowType, IDataflowObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="DataflowType"/>.
        /// </returns>
        public virtual DataflowType Build(IDataflowObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new DataflowType();

            // Populate it from inherited super
            this.AssembleMaintainable(builtObj, buildFrom);

            // Populate it using this class's specifics
            if (buildFrom.DataStructureRef != null)
            {
                var dataStructureReferenceType = new DataStructureReferenceType();
                var dataStructureRefType = new DataStructureRefType();
                dataStructureReferenceType.SetTypedRef(dataStructureRefType);
                builtObj.SetStructure(dataStructureReferenceType);

                this.SetReference(dataStructureRefType, buildFrom.DataStructureRef);
            }

            return builtObj;
        }

        #endregion
    }
}