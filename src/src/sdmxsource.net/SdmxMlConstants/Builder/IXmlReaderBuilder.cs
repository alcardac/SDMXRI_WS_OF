namespace Estat.Sri.SdmxXmlConstants.Builder
{
    using System.IO;
    using System.Xml;

    public interface IXmlReaderBuilder
    {
        /// <summary>
        /// Builds the <see cref="XmlReader"/> from the specified stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="XmlReader"/>
        /// </returns>
        XmlReader Build(Stream stream);
    }
}