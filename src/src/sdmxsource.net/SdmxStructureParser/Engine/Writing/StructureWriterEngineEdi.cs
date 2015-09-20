// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWriterEngineEdi.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Not implemented.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Engine
{
    using System;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     Not implemented.
    /// </summary>
    public class StructureWriterEngineEdi : IStructureWriterEngine
    {
        ////private EdiParseManager editParseManager;
        #region Fields

        /// <summary>
        ///     The output stream.
        /// </summary>
        private Stream xout;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineEdi"/> class.
        /// </summary>
        /// <param name="xout">
        /// The output stream.
        /// </param>
        public StructureWriterEngineEdi(Stream xout)
        {
            this.xout = xout;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The write structure.
        /// </summary>
        /// <param name="bean">
        /// The maintainable object.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// SDMX EDI for structures is not implemented in this implementation.
        /// </exception>
        public virtual void WriteStructure(IMaintainableObject bean)
        {
            throw new NotImplementedException("SDMX EDI for structures is not implemented in this implementation.");

            ////ISdmxObjects beans = new SdmxObjectsImpl();
            ////beans.AddIdentifiable(bean);
            ////WriteStructures(beans);
        }

        /// <summary>
        /// The write structures.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// SDMX EDI for structures is not implemented in this implementation.
        /// </exception>
        public virtual void WriteStructures(ISdmxObjects beans)
        {
            throw new NotImplementedException("SDMX EDI for structures is not implemented in this implementation.");

            ////if (editParseManager == null) {
            ////    throw new Exception(
            ////            "Required dependancy 'structureReaderEngine' is null, StructureWriterEngineEdi is @Configurable and requires '<context:spring-configured />' to be set");
            ////}
            ////editParseManager.WriteToEDI(beans, xout);
        }

        #endregion
    }
}