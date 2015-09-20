// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelStatus.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Current REL status
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Constants
{
    using System;

    /// <summary>
    ///     Current REL status
    /// </summary>
    [Flags]
    internal enum RelStatus
    {
        /// <summary>
        ///     The none.
        /// </summary>
        None = 0, 

        /// <summary>
        ///     The data set.
        /// </summary>
        DataSet = 1, 

        /// <summary>
        ///     The sibling.
        /// </summary>
        Sibling = 2, 

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