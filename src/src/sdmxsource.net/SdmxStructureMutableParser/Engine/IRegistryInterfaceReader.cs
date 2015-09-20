// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistryInterfaceReader.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The RegistryInterfaceReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine
{
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Model;

    using Org.Sdmxsource.Sdmx.Util.Exception;

    /// <summary>
    /// The RegistryInterfaceReader interface.
    /// </summary>
    public interface IRegistryInterfaceReader
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parses the reader opened against the stream containing the contents of a SDMX-ML Registry message or
        ///     RegistryInterface structure contents and populates the given <see cref="IRegistryInfo"/> object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null
        /// </exception>
        /// <exception cref="ParseException">
        /// SDMX structure message parsing error
        /// </exception>
        /// <param name="registry">
        /// The <see cref="IRegistryInfo"/> object to populate
        /// </param>
        /// <param name="reader">
        /// The xml reader opened against the stream containing the structure contents
        /// </param>
        void Read(IRegistryInfo registry, XmlReader reader);

        #endregion
    }
}