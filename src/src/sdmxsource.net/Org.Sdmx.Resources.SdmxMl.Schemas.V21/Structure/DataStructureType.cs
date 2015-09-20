// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureType.cs" company="EUROSTAT">
//   EUPL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    /// <summary>
    /// <para>
    /// DataStructureType describes the structure of a data structure definition. A data structure definition is defined as a collection of metadata concepts, their structure and usage when used to collect or disseminate data.
    /// </para>
    /// </summary>
    public partial class DataStructureType
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the DataStructureComponents. DataStructureComponents defines the grouping of the sets of metadata concepts that have a defined structural role in the data structure definition. Note that for any component or group defined in a data structure definition, its id must be unique. This applies to the identifiers explicitly defined by the components as well as those inherited from the concept identity of a component. For example, if two dimensions take their identity from concepts with same identity (regardless of whether the concepts exist in different schemes) one of the dimensions must be provided a different explicit identifier. Although there are XML schema constraints to help enforce this, these only apply to explicitly assigned identifiers. Identifiers inherited from a concept from which a component takes its identity cannot be validated against this constraint. Therefore, systems processing data structure definitions will have to perform this check outside of the XML validation. There are also three reserved identifiers in a data structure definition; OBS_VALUE, TIME_PERIOD, and REPORTING_PERIOD_START_DAY. These identifiers may not be used outside of their respective defintions (PrimaryMeasure, TimeDimension, and ReportingYearStartDay). This applies to both the explicit identifiers that can be assigned to the components or groups as well as an identifier inherited by a component from its concept identity. For example, if an ordinary dimension (i.e. not the time dimension) takes its concept identity from a concept with the identifier TIME_PERIOD, that dimension must provide a different explicit identifier.
        /// </summary>
        public DataStructureComponents DataStructureComponents
        {
            get
            {
                return (DataStructureComponents)this.Grouping;
            }

            set
            {
                this.Grouping = value;
            }
        }

        #endregion
    }
}