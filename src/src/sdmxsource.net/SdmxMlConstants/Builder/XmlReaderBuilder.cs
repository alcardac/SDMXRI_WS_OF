// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlReaderBuilder.cs" company="Eurostat">
//   Date Created : 2014-07-16
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Build an XML reader using the <see cref="NameTableCache" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxXmlConstants.Builder
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Build an XML reader using the <see cref="NameTableCache" />
    /// </summary>
    public class XmlReaderBuilder : IXmlReaderBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the <see cref="XmlReader"/> from the specified stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="XmlReader"/>
        /// </returns>
        public XmlReader Build(Stream stream)
        {
            return XmlReader.Create(
                stream, 
                new XmlReaderSettings { IgnoreComments = true, IgnoreProcessingInstructions = true, IgnoreWhitespace = true, NameTable = NameTableCache.Instance.NameTable });
        }

        #endregion
    }
}