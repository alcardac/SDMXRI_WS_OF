// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableReader.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The Mutable Reader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine
{
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Util.Exception;

    /// <summary>
    ///     The Mutable Reader interface.
    /// </summary>
    public interface IMutableReader
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parses the reader opened against the stream containing the contents of a SDMX-ML Structure message or
        ///     RegistryInterface structure contents and populates the given IMutableObjects object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null
        /// </exception>
        /// <exception cref="ParseException">
        /// SDMX structure message parsing error
        /// </exception>
        /// <param name="reader">
        ///     The xml reader opened against the stream containing the structure contents
        /// </param>
        /// <param name="structure">
        ///     The IMutableObjects object to populate
        /// </param>
        IMutableObjects Read(XmlReader reader, IMutableObjects structure);

        #endregion
    }
}