// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   SDMXObjects builder builds SDMXObjects from <see cref="XElement" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    using Structure = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.Structure;

    /// <summary>
    ///     SDMXObjects builder builds SDMXObjects from <see cref="XElement" />
    /// </summary>
    public interface ISdmxObjectsBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a <see cref="ISdmxObjects"/> from a V1 <paramref name="structuresDoc"/>.
        ///     <p/>
        ///     If the <paramref name="structuresDoc"/> contains no Structures then there will be an empty
        ///     <see cref="ISdmxObjects"/>
        ///     Object returned.
        ///     <p/>
        ///     If the <paramref name="structuresDoc"/> contains duplicate maintainable structures (defined by the same URN) - then an exception will be thrown
        /// </summary>
        /// <param name="structuresDoc">
        /// The structure message
        /// </param>
        /// <returns>
        /// a <see cref="ISdmxObjects"/> from a V1 <paramref name="structuresDoc"/>.
        /// </returns>
        ISdmxObjects Build(Structure structuresDoc);

        /// <summary>
        /// Builds a <see cref="ISdmxObjects"/> from a V2 <paramref name="structuresDoc"/>.
        ///     <p/>
        ///     If the <paramref name="structuresDoc"/> contains no Structures then there will be an empty
        ///     <see cref="ISdmxObjects"/>
        ///     Object returned.
        ///     <p/>
        ///     If the <paramref name="structuresDoc"/> contains duplicate maintainable structures (defined by the same URN) - then an exception will be thrown
        /// </summary>
        /// <param name="structuresDoc">
        /// The structure message
        /// </param>
        /// <returns>
        /// a <see cref="ISdmxObjects"/> from a V2 <paramref name="structuresDoc"/>.
        /// </returns>
        ISdmxObjects Build(Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.Structure structuresDoc);

        /// <summary>
        /// Builds a <see cref="ISdmxObjects"/> from a V2 <paramref name="registryInterface"/> QueryStructureRequest.
        ///     <p/>
        ///     If the <paramref name="registryInterface"/> contains no Structures then there will be an empty
        ///     <see cref="ISdmxObjects"/>
        ///     Object returned.
        ///     <p/>
        ///     If the <paramref name="registryInterface"/> contains duplicate maintainable structures (defined by the same URN) - then an exception will be thrown
        /// </summary>
        /// <param name="registryInterface">
        /// The registry interface message containing the QueryStructureRequest
        /// </param>
        /// <returns>
        /// a <see cref="ISdmxObjects"/> from a V2 <paramref name="registryInterface"/> QueryStructureRequest..
        /// </returns>
        ISdmxObjects Build(RegistryInterface registryInterface);

        /// <summary>
        /// Builds a <see cref="ISdmxObjects"/> from a V21 <paramref name="registryInterface"/>.
        ///     <p/>
        ///     If the <paramref name="registryInterface"/> contains no Structures then there will be an empty
        ///     <see cref="ISdmxObjects"/>
        ///     Object returned.
        ///     <p/>
        ///     If the <paramref name="registryInterface"/> contains duplicate maintainable structures (defined by the same URN) - then an exception will be thrown
        /// </summary>
        /// <param name="registryInterface">
        /// The registry interface message
        /// </param>
        /// <returns>
        /// a <see cref="ISdmxObjects"/> from a V2 <paramref name="registryInterface"/>.
        /// </returns>
        ISdmxObjects Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.RegistryInterface registryInterface);

        /// <summary>
        /// Builds a <see cref="ISdmxObjects"/> from a V21 <paramref name="structuresDoc"/>.
        ///     <p/>
        ///     If the <paramref name="structuresDoc"/> contains no Structures then there will be an empty
        ///     <see cref="ISdmxObjects"/>
        ///     Object returned.
        ///     <p/>
        ///     If the <paramref name="structuresDoc"/> contains duplicate maintainable structures (defined by the same URN) - then an exception will be thrown
        /// </summary>
        /// <param name="structuresDoc">
        /// The structure message
        /// </param>
        /// <returns>
        /// a <see cref="ISdmxObjects"/> from a V21 <paramref name="structuresDoc"/>.
        /// </returns>
        ISdmxObjects Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.Structure structuresDoc);

        #endregion
    }
}