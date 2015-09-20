// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefixConstants.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The prefix constants prefix.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxXmlConstants
{
    /// <summary>
    /// The prefix constants.
    /// </summary>
    public static class PrefixConstants
    {
        #region Constants

        /// <summary>
        /// The common prefix.
        /// </summary>
        public const string Common = "common";

        /// <summary>
        /// The generic prefix.
        /// </summary>
        public const string Generic = "generic";

        /// <summary>
        /// The message prefix.
        /// </summary>
        public const string Message = "message";

        /// <summary>
        /// The query prefix.
        /// </summary>
        public const string Query = "query";

        /// <summary>
        /// The registry prefix.
        /// </summary>
        public const string Registry = "registry";

        /// <summary>
        /// The structure prefix.
        /// </summary>
        public const string Structure = "structure";

        /// <summary>
        /// The dataset data structure specific prefix. Note this depends on the <c>DSD</c> and applies to all SDMX versions.
        /// </summary>
        public const string DataSetStructureSpecific = "ns";

        /// <summary>
        /// The structure specific prefix. Note this about <c>http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific</c>
        /// </summary>
        public const string StructureSpecific21 = "ss";

        #endregion
    }
}