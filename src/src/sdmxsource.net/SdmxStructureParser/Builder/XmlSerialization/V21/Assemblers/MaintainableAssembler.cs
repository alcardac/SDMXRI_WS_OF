// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The maintainable bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The maintainable bean assembler.
    /// </summary>
    public class MaintainableAssembler : NameableAssembler
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble maintainable.
        /// </summary>
        /// <param name="builtObj">
        /// The object to populate.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        public void AssembleMaintainable(MaintainableType builtObj, IMaintainableObject buildFrom)
        {
            // Populate it from inherited super
            this.AssembleNameable(builtObj, buildFrom);

            // Populate it using this class's specifics
            string str1 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string str0 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.version = buildFrom.Version;
            }

            if (buildFrom.StartDate != null)
            {
                builtObj.validFrom = buildFrom.StartDate.Date;
            }

            if (buildFrom.EndDate != null)
            {
                builtObj.validTo = buildFrom.EndDate.Date;
            }

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            if (buildFrom.IsFinal.IsSet())
            {
                builtObj.isFinal = buildFrom.IsFinal.IsTrue;
            }

            if (buildFrom.StructureUrl != null)
            {
                builtObj.structureURL = buildFrom.StructureUrl;
            }

            if (buildFrom.ServiceUrl != null)
            {
                builtObj.serviceURL = buildFrom.ServiceUrl;
            }
        }

        #endregion
    }
}