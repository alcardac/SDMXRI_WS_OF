// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx objects builder implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    using Structure = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.Structure;

    /// <summary>
    ///     The sdmx objects builder implementation.
    /// </summary>
    public class SdmxObjectsBuilder : ISdmxObjectsBuilder
    {
        #region Fields

        /// <summary>
        ///     The sdmx beans v1.0 builder.
        /// </summary>
        private readonly SdmxObjectsV1Builder _sdmxObjectsV1Builder = new SdmxObjectsV1Builder();

        /// <summary>
        ///     The sdmx beans v2.1 builder.
        /// </summary>
        private readonly SdmxObjectsV21Builder _sdmxObjectsV21Builder = new SdmxObjectsV21Builder();

        /// <summary>
        ///     The sdmx beans v2.0 registry builder.
        /// </summary>
        private readonly SdmxObjectsV2RegDocBuilder _sdmxObjectsV2RegDocBuilder = new SdmxObjectsV2RegDocBuilder();

        /// <summary>
        ///     The sdmx beans v2.0 structure builder.
        /// </summary>
        private readonly SdmxObjectsV2StrucDocBuilder _sdmxObjectsV2StrucDocBuilder = new SdmxObjectsV2StrucDocBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build from Version 1.0 Structure Document
        /// </summary>
        /// <param name="structuresDoc">
        /// The structures Doc.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects Build(Structure structuresDoc)
        {
            return this._sdmxObjectsV1Builder.Build(structuresDoc);
        }

        /// <summary>
        /// Build from a Registry Document, this can be either a submit structure request, or a query structure response
        /// </summary>
        /// <param name="rid">
        /// The registryInterface.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects Build(RegistryInterface rid)
        {
            return this._sdmxObjectsV2RegDocBuilder.Build(rid);
        }

        /// <summary>
        /// Build beans from a v2.0 Structure Document
        /// </summary>
        /// <param name="structuresDoc">
        /// The structures Doc.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects Build(Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.Structure structuresDoc)
        {
            return this._sdmxObjectsV2StrucDocBuilder.Build(structuresDoc);
        }

        /// <summary>
        /// Build beans from a v2.0 Registry Document
        /// </summary>
        /// <param name="registryInterface">
        /// The registry Doc.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects Build(
            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface registryInterface)
        {
            return this._sdmxObjectsV21Builder.Build(registryInterface);
        }

        /// <summary>
        /// Build beans from a v2.1 Structure Document
        /// </summary>
        /// <param name="structuresDoc">
        /// The structures Doc.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.Structure structuresDoc)
        {
            return this._sdmxObjectsV21Builder.Build(structuresDoc);
        }

        #endregion
    }
}