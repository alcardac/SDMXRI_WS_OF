// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentLevel.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Attribute attachment level
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Constants
{
    /// <summary>
    ///     Attribute attachment level
    /// </summary>
    internal enum AttachmentLevel
    {
        /// <summary>
        ///     No level
        /// </summary>
        None = 0, 

        /// <summary>
        ///     The data set.
        /// </summary>
        DataSet = 1, 

        /// <summary>
        ///     The group.
        /// </summary>
        Group = 4, 

        /// <summary>
        ///     The series.
        /// </summary>
        Series = 4, 

        /// <summary>
        ///     The observation.
        /// </summary>
        Observation = 5
    }
}