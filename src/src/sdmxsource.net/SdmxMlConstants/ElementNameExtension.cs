// -----------------------------------------------------------------------
// <copyright file="ElementNameExtension.cs" company="Eurostat">
//   Date Created : 2014-05-20
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.SdmxXmlConstants
{
    using System.Xml;

    /// <summary>
    /// The element name extension.
    /// </summary>
    public static class ElementNameExtension
    {
        /// <summary>
        /// Determines whether the <paramref name="localName"/> is the specified <paramref name="element"/>
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="localName">The element local name as an object.</param>
        /// <returns><c>true</c> if the <paramref name="localName"/> is the specified <paramref name="element"/>; otherwise false.</returns>
        public static bool Is(this ElementNameTable element, object localName)
        {
            return NameTableCache.IsElement(localName, element);
        }

        /// <summary>
        /// Get a string representation of the <paramref name="element"/> 
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The string representation of the <paramref name="element"/> 
        /// </returns>
        public static string FastToString(this ElementNameTable element)
        {
            return NameTableCache.GetElementName(element);
        }

        /// <summary>
        /// Get a string representation of the <paramref name="attribute"/> 
        /// </summary>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        /// <returns>
        /// The string representation of the <paramref name="attribute"/> 
        /// </returns>
        public static string FastToString(this AttributeNameTable attribute)
        {
            return NameTableCache.GetAttributeName(attribute);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        /// <returns>
        /// The attribute value if it exists; otherwise null.
        /// </returns>
        public static string GetAttribute(this XmlReader reader, AttributeNameTable attribute)
        {
            return reader.GetAttribute(attribute.FastToString());
        }
    }
}