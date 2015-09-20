// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Status.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The status of the <see cref="IQueryStructureResponseInfo" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    /// <summary>
    ///     The status of the <see cref="IQueryStructureResponseInfo" />.
    /// </summary>
    public enum Status
    {
        /// <summary>
        ///     The success.
        /// </summary>
        Success, 

        /// <summary>
        ///     The warning.
        /// </summary>
        Warning, 

        /// <summary>
        ///     The error.
        /// </summary>
        Error
    }
}