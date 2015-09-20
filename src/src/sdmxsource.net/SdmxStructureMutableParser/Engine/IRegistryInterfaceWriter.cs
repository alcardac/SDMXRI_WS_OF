// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistryInterfaceWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The RegistryInterfaceWriterV2 interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine
{
    using Estat.Sri.SdmxStructureMutableParser.Model;

    /// <summary>
    ///     The RegistryInterfaceWriterV2 interface.
    /// </summary>
    public interface IRegistryInterfaceWriter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Write the specified <paramref name="registry"/>
        /// </summary>
        /// <param name="registry">
        /// The <see cref="IRegistryInfo"/> object
        /// </param>
        void Write(IRegistryInfo registry);

        #endregion
    }
}