// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameTableCache.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The name table cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxXmlConstants
{
    using System;

    /// <summary>
    ///     The name table cache.
    /// </summary>
    public sealed class NameTableCache
    {
        #region Static Fields

        /// <summary>
        ///     The singleton instance
        /// </summary>
        private static readonly NameTableCache _instance = new NameTableCache();

        #endregion

        #region Fields

        /// <summary>
        ///     An containing the <see cref="System.Xml.NameTable" /> string references and are indexed
        ///     by enumerations <see cref="ElementNameTable" /> and <see cref="AttributeNameTable" />
        /// </summary>
        private readonly object[] _elementNameCache;

        /// <summary>
        ///     A name table object that stores all the SDMX-ML elements
        /// </summary>
        private readonly ConcurrentNameTable _nameTable;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="NameTableCache" /> class from being created.
        ///     Initialize name table and element name cache object array both containing the same object references.
        ///     The name table would be used in XmlReaderSettings and the elementNameCache object array
        ///     would be used with <see cref="NameTableCache.IsElement" /> to atomically compare element local names.
        /// </summary>
        private NameTableCache()
        {
            this._nameTable = new ConcurrentNameTable();
            string[] elementNames = Enum.GetNames(typeof(ElementNameTable));
            string[] attributeNames = Enum.GetNames(typeof(AttributeNameTable));
            this._elementNameCache = new object[elementNames.Length + attributeNames.Length];

            for (int i = 0; i < elementNames.Length; i++)
            {
                this._elementNameCache[i] = this._nameTable.Add(elementNames[i]);
            }

            int count = elementNames.Length;

            for (int i = 0; i < attributeNames.Length; i++)
            {
                this._elementNameCache[count++] = this._nameTable.Add(attributeNames[i]);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the singleton instance
        /// </summary>
        public static NameTableCache Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        ///     Gets the name table object that stores all the SDMX-ML elements
        /// </summary>
        public ConcurrentNameTable NameTable
        {
            get
            {
                return this._nameTable;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the <paramref name="attributeName"/> name string
        /// </summary>
        /// <param name="attributeName">
        /// The element name
        /// </param>
        /// <returns>
        /// The atomized string of the <paramref name="attributeName"/>
        /// </returns>
        public static string GetAttributeName(AttributeNameTable attributeName)
        {
            return (string)_instance._elementNameCache[(int)attributeName];
        }

        /// <summary>
        /// Gets the <paramref name="elementName"/> name string
        /// </summary>
        /// <param name="elementName">
        /// The element name
        /// </param>
        /// <returns>
        /// The atomized string of the <paramref name="elementName"/>
        /// </returns>
        public static string GetElementName(ElementNameTable elementName)
        {
            return (string)_instance._elementNameCache[(int)elementName];
        }

        /// <summary>
        /// Checks if the given <paramref name="localName"/> equals to the given <paramref name="attributeName"/>.
        /// </summary>
        /// <param name="localName">
        /// Objectified string containing the current local name
        /// </param>
        /// <param name="attributeName">
        /// The element name to check against
        /// </param>
        /// <returns>
        /// True if the <paramref name="localName"/> is <paramref name="attributeName"/>
        /// </returns>
        public static bool IsAttribute(object localName, AttributeNameTable attributeName)
        {
            return ReferenceEquals(localName, _instance._elementNameCache[(int)attributeName]);
        }

        /// <summary>
        /// Checks if the given <paramref name="localName"/> equals to the given <paramref name="elementName"/>.
        /// </summary>
        /// <param name="localName">
        /// Objectified string containing the current local name
        /// </param>
        /// <param name="elementName">
        /// The element name to check against
        /// </param>
        /// <returns>
        /// True if the <paramref name="localName"/> is <paramref name="elementName"/>
        /// </returns>
        public static bool IsElement(object localName, ElementNameTable elementName)
        {
            object objB = _instance._elementNameCache[(int)elementName];
            return ReferenceEquals(localName, objB);
        }

        #endregion
    }
}