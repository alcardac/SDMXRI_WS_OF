// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemaValidationManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   TODO
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxDataParser.Manager
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion


    /// <summary>
    /// TODO
    /// </summary>
    public interface ISchemaValidationManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates a DataSet against the schema generated from the DataStructureSuperBean and referenced Codelists
        /// <p/>
        /// Expects the data to be in SDMX-ML format
        /// </summary>
        /// <param name="dataset">
        /// The readable data location
        /// </param>
        /// <param name="keyFamily">
        /// The key family
        /// </param>
        /// throws SdmxSemmanticException if the data is not valid against the schema
        /// throws SdmxSyntaxException if the data is not SDMX-ML
        void ValidateDatasetAgainstSchema(IReadableDataLocation dataset, 
                                          DataStructureSuperBean keyFamily);

        #endregion
    }
}
