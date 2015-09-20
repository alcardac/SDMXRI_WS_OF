// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The Mutable Writer interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;

    /// <summary>
    ///     The Mutable Writer interface.
    /// </summary>
    public interface IMutableWriter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Write.the specified <paramref name="structure"/>
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        void Write(IMutableObjects structure);

        #endregion
    }
}